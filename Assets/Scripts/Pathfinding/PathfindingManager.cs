using System;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Assets.Scripts.BuildingSystem;
using System.Collections;
using Assets.Scripts.Utills;

namespace Assets.Scripts.Pathfinding
{
    public class PathfindingManager : SingletoneBase<PathfindingManager>
    {
        public static readonly int MAP_X_SIZE = 256;
        public static readonly int MAP_Y_SIZE = 256;

        private NativeArray<int> _walkableArray;

        

        #region Public Accesors

        public static NativeArray<int> WalkableArray { get => Instance._walkableArray; }
        public static Vector3Int ZeroPointOffset { get; private set; }

        #endregion

        #region Events and Handlers

        public static event Action OnWalkableArrayChanged;

        #endregion

        #region UnityMethods

        private void Awake()
        {
            var x = Instance;
        }

        private void Start()
        {
            //TODO: later it will be chunked
            _walkableArray = new NativeArray<int>(MAP_X_SIZE * MAP_Y_SIZE, Allocator.Persistent);
            ZeroPointOffset = new Vector3Int(-MAP_X_SIZE / 2, -MAP_Y_SIZE / 2);

            GameManager.OnMapChanged += MapChangedHandler;

            
            //TODO: This is temporary solution to make sure that update happens after the map has been loaded
            StartCoroutine(UpdateWalkableArrayLate());
        }

        private void MapChangedHandler(BoundsInt area)
        {
            StartCoroutine(UpdateWalkableArrayLate(area));
        }

        private void OnDestroy()
        {
            WalkableArray.Dispose();
        }

        #endregion

        #region PublicMethods

        [BurstCompile]
        public static int CalculateIndex(int x, int y, int gridWidth) => x + y * gridWidth;

        [BurstCompile]
        public static int CalculateIndex(int2 pos, int gridWidth) => pos.x + pos.y * gridWidth;

        public static Vector3Int ConvertToTilemapCoordinates(int2 vec)
        {
            return new Vector3Int(vec.x + ZeroPointOffset.x, vec.y + ZeroPointOffset.y);
        }

        public static Vector3Int ConvertToArrayCoordinates(Vector3Int vec)
        {
            return new Vector3Int(vec.x - ZeroPointOffset.x, vec.y - ZeroPointOffset.y);
        }

        public static PathfindingJob CreatePathfindingJob(Vector3Int startPos, Vector3Int endPos, NativeList<int2> path)
        {

            int2 areaSize = new int2(MAP_X_SIZE, MAP_Y_SIZE);

            Vector3Int convertedCoordinates = ConvertToArrayCoordinates(startPos);
            int2 start = new int2(convertedCoordinates.x, convertedCoordinates.y);

            convertedCoordinates = ConvertToArrayCoordinates(endPos);
            int2 end = new int2(convertedCoordinates.x, convertedCoordinates.y);


            path.Clear();

            return new PathfindingJob
            {
                Start = start,
                End = end,
                AreaSize = areaSize,
                WalkableArray = WalkableArray,
                Path = path,
            };
        }

        #endregion

        #region PrivateMethods


        /// <summary>
        /// Coroutine used to update a native array of walkable tiles one frame after its called
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateWalkableArrayLate(BoundsInt? area = null)
        {
            yield return null;
            UpdateWalkableArray(area);// TODO: Change to a job with pathfinding depending on it
            yield return null;
            OnWalkableArrayChanged?.Invoke();
        }

        private void UpdateWalkableArray(BoundsInt? area = null)
        {
            int minX, minY, maxX, maxY;
            if (area != null)
            {
                minX = ConvertToArrayCoordinates(area.Value.min).x;
                minY = ConvertToArrayCoordinates(area.Value.min).y;
                maxX = ConvertToArrayCoordinates(area.Value.max).x;
                maxY = ConvertToArrayCoordinates(area.Value.max).y;
            }
            else
            {
                minX = 0;
                minY = 0;
                maxX = MAP_X_SIZE;
                maxY = MAP_Y_SIZE;
            }


            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    bool canWalkThere = false;

                    Vector3Int vec = ConvertToTilemapCoordinates(new int2(x, y));

                    var tileBelow = GameManager.Instance.TilemapGround.GetTile(vec + new Vector3Int(0, 0, -1));


                    if (tileBelow != null)
                    {
                        if (((IMapElement)tileBelow).Walkable)
                        {
                            var tileAbove = GameManager.Instance.TilemapGround.GetTile(vec);

                            if (tileAbove == null)
                            {
                                canWalkThere = true;
                            }
                            else if ( ((IMapElement)tileAbove).CanWalkThrough )
                            {
                                canWalkThere = true;
                            }

                            if( tileAbove != null)
                            {
                                if (((IMapElement)tileAbove).CanWalkThrough)
                                {
                                    canWalkThere = true;
                                }
                                else
                                {
                                    canWalkThere = false;
                                }
                            }

                            tileAbove = GameManager.Instance.TilemapColliders.GetTile(vec);

                            if (tileAbove == null)
                            {
                                canWalkThere = true;
                            }
                            else if (((IMapElement)tileAbove).CanWalkThrough)
                            {
                                canWalkThere = true;
                            }

                            if (tileAbove != null)
                            {
                                if (((IMapElement)tileAbove).CanWalkThrough)
                                {
                                    canWalkThere = true;
                                }
                                else
                                {
                                    canWalkThere = false;
                                }
                            }

                        }
                    }
                    
                    int value = (canWalkThere ? (int)(((IMapElement)tileBelow).WalkingSpeedFactor * 100f) : 0);

                    _walkableArray[x + y * MAP_X_SIZE] = value;

                }
            }
            
        }


        #endregion


    }
}