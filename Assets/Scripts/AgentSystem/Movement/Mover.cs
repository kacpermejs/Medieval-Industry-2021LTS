using Assets.Scripts.BuildingSystem;
using Assets.Scripts.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Utills;
using Assets.Scripts.GameStates;

namespace Assets.Scripts.AgentSystem.Movement
{
    public partial class Mover : MonoBehaviour
    {
        public const float Y_OFFSET = 0.22f;
        public const float Z_OFFSET = 1;
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private bool _mouseMovement = false;

        [SerializeField] private Transform MovePoint;

        private MoverComandBase _command;
        private NativeList<int2> _resultPath;

        //Pathfinding
        private Vector3Int _startPoint;
        private int _pathIndex = -1;
        private JobHandle _jobHandle;
        [SerializeField, ReadOnlyInspector] private bool _busy;

        #region UnityMethods


        private void Awake()
        {
            _resultPath = new NativeList<int2>(Allocator.Persistent);
        }

        // Start is called before the first frame update
        void Start()
        {
            MovePoint.parent = null;
            PathfindingManager.OnWalkableArrayChanged += HandleMapChanges;
        }

        // Update is called once per frame
        void Update()
        {
            //Move towards Waypoint position
            var vec = MapManager.Instance.TilemapGround.layoutGrid.LocalToCell(transform.position + new Vector3(0f, Y_OFFSET, 0f));
            var tileBelow = MapManager.Instance.TilemapGround.GetTile(vec + new Vector3Int(0, 0, -1));
            float slowDownfactor = ((IMapElement)tileBelow).WalkingSpeedFactor;

            

            transform.position = Vector3.MoveTowards(transform.position, MovePoint.position, (_moveSpeed * slowDownfactor) * Time.deltaTime);
            FollowThePath();


            /*if (!_busy && _command != null)
            {
                _command = null;
            }*/
 
            /*if (_mouseMovement)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Do not place any object if mouse is over a UI object
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        Vector2 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                        Vector3Int endPoint = GameManager.ConvertToGridPosition(screenPoint);

                        var manualMoveCommand = new MoveCommand(this, endPoint);

                        AddCommand(manualMoveCommand);
                        StartCoroutine(ExecuteCommand(true));

                    }
                }
            }*/
            if (!_busy && _command != null)
            {
                StartCoroutine(ExecuteCommand(false));
            }
            
        }
        private void OnDestroy()
        {
            _resultPath.Dispose();
        }

        #endregion

        public IEnumerator HoldForSeconds(int seconds)
        {
            var temp = _moveSpeed;
            _moveSpeed = 0;

            yield return new WaitForSeconds(seconds);

            _moveSpeed = temp;
            _busy = false;
        }

        

        public void AddCommand(MoverComandBase command)
        {
            _command?.ExecutionEnded();
            _command = command;
        }

        private IEnumerator ExecuteCommand(bool force)
        {
            if(!_busy || force)
            {
                _busy = true;
                yield return new WaitForEndOfFrame();
                _command.Execute();
            }
        }

        private void FollowThePath()
        {
            //Check if path calculation finished
            if (_jobHandle.IsCompleted)
            {
                _jobHandle.Complete();
                

                if (Vector3.Distance(transform.position, MovePoint.position) <= 0.05f)//Point reached
                {
                    if (_resultPath.Length > 0 && _pathIndex < 0)
                    {
                        //recieved new path
                        _pathIndex = _resultPath.Length - 1;
                    }
                    else if (_resultPath.Length <= 0)
                    {
                        _busy = false;
                        return;
                    }
                    if (_pathIndex >= 0)
                    {
                        
                        //set a new point to walk towards
                        var cellPos = PathfindingManager.ConvertToTilemapCoordinates(_resultPath[_pathIndex]);
                        var newPos = MapManager.Instance.GridLayout.CellToWorld(cellPos) + new Vector3(0, Y_OFFSET, Z_OFFSET);

                        var lowerTile = MapManager.Instance.TilemapGround.GetTile(cellPos + new Vector3Int(0, 0, -1));
                        var upperTile = MapManager.Instance.TilemapGround.GetTile(cellPos);

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
                        _pathIndex--;

                        if (_pathIndex < 0) //path completed
                        {
                            _resultPath.Clear();
                            _command.ExecutionEnded();
                            _command = null;
                        }

                    }

                }
                
            }
            
        }
        
        public bool SchedulePathfinding(Vector3Int targetPoint)
        {

            _startPoint = MapManager.Instance.GridLayout.LocalToCell(MovePoint.position - new Vector3(0, Y_OFFSET, Z_OFFSET));
            
            _pathIndex = -1;
            _resultPath.Clear();

            PathfindingJob job = PathfindingManager.CreatePathfindingJob(_startPoint, targetPoint, _resultPath);
            
            _jobHandle = job.Schedule();

            return true;
        }

        private void HandleMapChanges()
        {
            _jobHandle.Complete();
            //if agent is walking
            if(_busy)
            {
                if(_command is MoveCommand)
                {
                    
                    //Set to default
                    _pathIndex = -1;
                    _resultPath.Clear();
                    //Execute again
                    StartCoroutine(ExecuteCommand(true)); //Recalculate path
                    
                }

            }
        }

        

        
    }
}