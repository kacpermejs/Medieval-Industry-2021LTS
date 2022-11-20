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

namespace Assets.Scripts.AgentSystem.Movement
{


    public partial class Mover : AIBehaviourInvoker
    {
        public const float Y_OFFSET = 0.22f;
        public const float Z_OFFSET = 1;
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private bool _mouseMovement = true;

        [SerializeField] private Transform MovePoint;

        private Queue<Command> _commandQueue = new Queue<Command>();
        private NativeList<int2> _resultPath;


        //Scheduling Pathfinding calculations
        private Vector3Int _startPoint;
        private int _pathIndex = -1;
        private JobHandle _jobHandle;
        [SerializeField, ReadOnlyInspector] private bool _busy;


        //public event EventHandler OnArrivedAtDestination;

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
            transform.position = Vector3.MoveTowards(transform.position, MovePoint.position, _moveSpeed * Time.deltaTime);

            if (_jobHandle.IsCompleted)
            {
                _jobHandle.Complete();
            }

            if (!_busy)
            {
                if (_commandQueue.Count > 0)
                {
                    ExecuteCommand();
                    _busy = true;
                }
            }
            if (_commandQueue.Count > 0)
            {
                if (_commandQueue.Peek() is MoveCommand)
                {
                    //Alter the Waypoint if it has been reached
                    FollowThePath();
                }
                else if (_commandQueue.Peek() is HoldCommand)
                {
                    //Nothing to do
                    //Command is served by coroutine
                }
            }
            // Schedule pathfinding by clicking the mouse on the map
            if (_mouseMovement && GameManager.Instance.GameState == GameState.Default)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Do not place any object if mouse is over a UI object
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        Vector2 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                        Vector3Int endPoint = GameManager.Instance.GridLayout.LocalToCell(screenPoint);

                        var manualMoveCommand = ScriptableObject.CreateInstance<MoveCommand>();
                        manualMoveCommand.CreateCommand(this, this, endPoint, priority: -1);

                        AddCommand(manualMoveCommand);

                    }
                }
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
            _commandQueue.Dequeue().OnExecutionEnded();
            //Debug.Log("Coroutine Ended");
            _busy = false;

        }

        

        public void AddCommand(Command command)
        {
            _commandQueue.Enqueue(command);
        }

        private void ExecuteCommand()
        {
            _commandQueue.Peek().Execute();
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
                        _commandQueue.Dequeue().OnExecutionEnded();
                        _busy = false;
                        return;
                    }
                    if (_pathIndex >= 0)
                    {
                        
                        //set a new point to walk towards
                        var cellPos = PathfindingManager.ConvertToTilemapCoordinates(_resultPath[_pathIndex]);
                        var newPos = GameManager.Instance.GridLayout.CellToWorld(cellPos) + new Vector3(0, Y_OFFSET, Z_OFFSET);

                        var lowerTile = GameManager.Instance.TilemapGround.GetTile(cellPos + new Vector3Int(0, 0, -1));
                        var upperTile = GameManager.Instance.TilemapGround.GetTile(cellPos);

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
                        }

                    }

                }
                
            }
            
        }
        
        public bool SchedulePathfinding(Vector3Int targetPoint, bool comeNextTo = false)
        {

            _startPoint = GameManager.Instance.GridLayout.LocalToCell(MovePoint.position - new Vector3(0, Y_OFFSET, Z_OFFSET));
            
            _pathIndex = -1;
            _resultPath.Clear();

            PathfindingJob job = PathfindingManager.CreatePathfindingJob(_startPoint, targetPoint, comeNextTo, _resultPath);
            
            _jobHandle = job.Schedule();

            return true;
        }

        private void HandleMapChanges()
        {
            _jobHandle.Complete();
            //if agent is walking
            if(_busy)
            {
                Command currentCommand;
                if(_commandQueue.TryPeek(out currentCommand))
                {
                    if (currentCommand is MoveCommand)
                    {
                        //Set to default
                        _pathIndex = -1;
                        _resultPath.Clear();
                        //Execute again
                        currentCommand.Execute();
                    }
                }

            }
            //Recalculate path
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