/*using Assets.Scripts.BuildingSystem;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.AgentSystem
{
    public class AgentManager : MonoBehaviour
    {
        public readonly int MAP_X_SIZE = 1024;
        public readonly int MAP_Y_SIZE = 1024;

        private NativeArray<bool> _walkableArray;
        private Vector3Int _zeroPointOffset;

        public NativeArray<bool> WalkableArray { get => _walkableArray; }
        public Vector3Int ZeroPointOffset { get => _zeroPointOffset; }

        #region UnityMethods

        public static AgentManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _walkableArray = new NativeArray<bool>(MAP_X_SIZE * MAP_Y_SIZE, Allocator.Persistent);
            _zeroPointOffset = new Vector3Int(-MAP_X_SIZE / 2, -MAP_Y_SIZE / 2);
            
        }

        private void Update()
        {
            UpdateArray();
            foreach (var item in _walkableArray)
            {
                if (item == true)
                {
                    Debug.Log("Found");
                    break;
                }
            }
        }

        private void OnDestroy()
        {
            _walkableArray.Dispose();
        }

        #endregion

        private void UpdateArray()
        {
            
            for (int x = 0; x < MAP_X_SIZE; x++)
            {
                for (int y = 0; y < MAP_Y_SIZE; y++)
                {

                    bool value = false;
                    Vector3Int vec = new Vector3Int(x, y) - ZeroPointOffset;
                    if (GameManager.Instance.TilemapGround.GetTile(vec) != null)
                    {
                        if (((IMapElement)GameManager.Instance.TilemapGround.GetTile(vec)).Walkable)
                        {
                            if (GameManager.Instance.TilemapSurface.GetTile(vec + new Vector3Int(0, 0, 1)) == null)
                            {
                                value = true;
                            }
                            else if (((IMapElement)GameManager.Instance.TilemapSurface.GetTile(vec + new Vector3Int(0, 0, 1))).CanWalkThrough)
                            {
                                value = true;
                            }
                        }
                    }


                    _walkableArray[x + y * MAP_X_SIZE] = value;

                }
            }
        }

        public Vector3Int ConvertToArrayCoordinates(Vector3Int vec)
        {
            return new Vector3Int(vec.x - ZeroPointOffset.x, vec.y - ZeroPointOffset.y);
        }
    }
}

*/