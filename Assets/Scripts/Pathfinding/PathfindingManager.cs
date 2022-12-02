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

        private NativeArray<bool> _walkableArray;

        #region Public Accesors

        public static NativeArray<bool> WalkableArray { get => Instance._walkableArray; }
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
            _walkableArray = new NativeArray<bool>(MAP_X_SIZE * MAP_Y_SIZE, Allocator.Persistent);
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

        public static Vector3Int ConvertToTilemapCoordinates(int2 vec)
        {
            return new Vector3Int(vec.x + ZeroPointOffset.x, vec.y + ZeroPointOffset.y);
        }

        public static Vector3Int ConvertToArrayCoordinates(Vector3Int vec)
        {
            return new Vector3Int(vec.x - ZeroPointOffset.x, vec.y - ZeroPointOffset.y);
        }

        public static PathfindingJob CreatePathfindingJob(Vector3Int startPos, Vector3Int endPos, bool closestAvaliable, NativeList<int2> path)
        {

            int2 areaSize = new int2(MAP_X_SIZE, MAP_Y_SIZE);

            Vector3Int convertedCoordinates = ConvertToArrayCoordinates(startPos);
            int2 start = new int2(convertedCoordinates.x, convertedCoordinates.y);

            convertedCoordinates = ConvertToArrayCoordinates(endPos);
            int2 end = new int2(convertedCoordinates.x, convertedCoordinates.y);


            path.Clear();

            return new PathfindingJob
            {
                start = start,
                end = end,
                areaSize = areaSize,
                closestAvaliable = closestAvaliable,
                walkableArray = WalkableArray,
                path = path,
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

                    bool value = false;
                    Vector3Int vec = ConvertToTilemapCoordinates(new int2(x, y));

                    if (GameManager.Instance.TilemapGround.GetTile(vec + new Vector3Int(0, 0, -1)) != null)
                    {
                        if (((IMapElement)GameManager.Instance.TilemapGround.GetTile(vec + new Vector3Int(0, 0, -1))).Walkable)
                        {
                            
                            if (GameManager.Instance.TilemapGround.GetTile(vec) == null)
                            {
                                value = true;
                            }
                            else if (((IMapElement)GameManager.Instance.TilemapGround.GetTile(vec)).CanWalkThrough)
                            {
                                value = true;
                            }
                            if(GameManager.Instance.TilemapColliders.GetTile(vec) != null)
                            {
                                if (((IMapElement)GameManager.Instance.TilemapColliders.GetTile(vec)).CanWalkThrough)
                                {
                                    value = true;
                                }
                                else
                                {
                                    value = false;
                                }
                            }
                            
                        }
                    }
                    


                    _walkableArray[x + y * MAP_X_SIZE] = value;

                }
            }
            
        }


        #endregion


    }
}