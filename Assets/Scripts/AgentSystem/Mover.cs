using Assets.Scripts.BuildingSystem;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using static Assets.Scripts.AgentSystem.Pathfinding;

namespace Assets.Scripts.AgentSystem
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private bool _mouseMovement = true;


        public Transform MovePoint;

        //private int2 _areaSize;
        private readonly int _range = 50;
        private NativeList<int2> _resultPath;
        //private List<int2> _resultPath;

        private Vector3Int _startPoint;
        private int _pathIndex = -1;
        private JobHandle _jobHandle;
        //private Task<IEnumerable<int2>> _pathfindingTask;

        private void Awake()
        {
            //_areaSize = new int2(range * 2, range * 2);

            _resultPath = new NativeList<int2>(Allocator.Persistent);
            //_resultPath = new List<int2>();
        }
        // Start is called before the first frame update
        void Start()
        {
            MovePoint.parent = null;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, MovePoint.position, _moveSpeed * Time.deltaTime);

            if(_mouseMovement)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Do not place any object if mouse is over a UI object
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        Vector2 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                        Vector3Int endPoint = GameManager.Instance.GridLayout.LocalToCell(screenPoint);
                        ////Debug.Log("End: " + endPoint);

                        _startPoint = GameManager.Instance.GridLayout.WorldToCell(MovePoint.position);
                        ////_startPoint = GameManager.Instance.GridLayout.WorldToCell(transform.position);
                        ////Debug.Log("Start: " + _startPoint);

                        _pathIndex = -1;
                        PathfindingJob job = CreatePathfindingJob(_startPoint, endPoint, _range, _resultPath);
                        _resultPath.Clear();
                        _jobHandle = job.Schedule();
                        //_resultPath.Clear();
                        //_pathfindingTask = Pathfind(_startPoint, endPoint, _range);
                    }
                }
            }



            if (_jobHandle.IsCompleted/*_pathfindingTask != null && _pathfindingTask.IsCompleted*/) //TODO: can do better
            {
                if (_resultPath.Length > 0 && _pathIndex < 0)
                {
                    //recieved new path
                    _pathIndex = _resultPath.Length - 1;
                }


                //If the point hasn't been reached then skip getting additional input
                if (Vector3.Distance(transform.position, MovePoint.position) <= 0.05f)
                {
                    if (_pathIndex >= 0)
                    {
                        //set a new point to walk towards
                        var cellPos = ConvertToMapPosition(_startPoint, _resultPath[_pathIndex]);
                        //Debug.Log("Moving towards: " + cellPos);
                        _pathIndex--;
                        var newPos = GameManager.Instance.GridLayout.CellToWorld(cellPos);

                        var lowerTile = GameManager.Instance.TilemapGround.GetTile(cellPos);
                        var upperTile = GameManager.Instance.TilemapSurface.GetTile(cellPos + new Vector3Int(0, 0, 1));

                        if (lowerTile != null)
                        {
                            if (((IMapElement)lowerTile).Walkable)
                            {
                                if (upperTile == null)
                                {
                                    MovePoint.position = newPos;
                                }
                                else if (((IMapElement)upperTile).CanWalkThrough)
                                {
                                    MovePoint.position = newPos;
                                }
                            }
                        }

                        //path completed
                        if (_pathIndex < 0)
                        {
                            _resultPath.Clear();
                        }
                    }

                }
            }
            
            
            
        }

        private Vector3Int ConvertToMapPosition(Vector3Int startPoint, int2 pos)
        {
            return new Vector3Int(pos.x, pos.y) + startPoint - new Vector3Int(_range + 1, _range + 1, 0);
        }

        private void OnDestroy()
        {
            _resultPath.Dispose();
        }

    }
}



//if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
//{
//    var cellPos = GameManager.Instance.GridLayout.WorldToCell(movePoint.position) + new Vector3Int((int)Input.GetAxisRaw("Horizontal"), 0, 0);
//    var newPos = GameManager.Instance.GridLayout.CellToWorld( cellPos );

//    var lowerTile = GameManager.Instance.TilemapGround.GetTile(cellPos);
//    var upperTile = GameManager.Instance.TilemapSurface.GetTile(cellPos + new Vector3Int(0, 0, 1));

//    if(lowerTile != null)
//    {
//        if ( ((IMapElement)lowerTile).Walkable )
//        {
//            if(upperTile == null)
//            {
//                movePoint.position = newPos;
//            }
//            else if ( ((IMapElement)upperTile).CanWalkThrough )
//            {
//                movePoint.position = newPos;
//            }
//        }
//    }

//}

//if ( Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f )
//{
//    var cellPos = GameManager.Instance.GridLayout.WorldToCell(movePoint.position) + new Vector3Int(0, (int)Input.GetAxisRaw("Vertical"), 0);
//    var newPos = GameManager.Instance.GridLayout.CellToWorld(cellPos);

//    var lowerTile = GameManager.Instance.TilemapGround.GetTile(cellPos);
//    var upperTile = GameManager.Instance.TilemapSurface.GetTile(cellPos + new Vector3Int(0, 0, 1));

//    if (lowerTile != null)
//    {
//        if (((IMapElement)lowerTile).Walkable)
//        {
//            if (upperTile == null)
//            {
//                movePoint.position = newPos;
//            }
//            else if (((IMapElement)upperTile).CanWalkThrough)
//            {
//                movePoint.position = newPos;
//            }
//        }
//    }
//}