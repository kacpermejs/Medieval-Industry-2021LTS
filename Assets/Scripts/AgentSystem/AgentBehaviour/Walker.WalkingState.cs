using BuildingSystem;
using Pathfinding;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using System;

namespace AgentSystem
{



    public partial class Walker
    {
        public class WalkingState : WalkerStateBase
        {
            Vector3Int targetPosition;
            Action callback;



            //private WalkerComandBase _command;
            private NativeList<int2> _resultPath;

            //Pathfinding
            private Vector3Int _startPoint;
            private int _pathIndex = -1;
            private JobHandle _jobHandle;
            //[SerializeField, ReadOnlyInspector] private bool _busy;
            private bool _pathIsUpToDate;


            public WalkingState(Vector3Int targetPositionProvider, Action callback)
            {
                this.targetPosition = targetPositionProvider;
                this.callback = callback;
            }

            public override void EnterState(Walker obj)
            {
                _resultPath = new NativeList<int2>(Allocator.Persistent);
                PathfindingManager.OnWalkableArrayChanged += HandleMapChanges;
                SchedulePathfinding(obj, targetPosition);
            }

            public override void UpdateState(Walker obj)
            {
                if (_jobHandle.IsCompleted)
                {
                    _pathIsUpToDate = true;
                    _jobHandle.Complete();
                }
                if (_pathIsUpToDate)
                {
                    FollowThePath(obj);
                }
                else
                {
                    if (_jobHandle.IsCompleted)
                    {
                        _pathIsUpToDate = true;
                        _jobHandle.Complete();
                        //Set to default
                        _pathIndex = -1;
                        _resultPath.Clear();
                        //Execute again
                        SchedulePathfinding(obj, targetPosition);
                    }
                }
            }
            public override void ExitState(Walker obj)
            {
                PathfindingManager.OnWalkableArrayChanged -= HandleMapChanges;
                _resultPath.Dispose();
            }

            public bool SchedulePathfinding(Walker obj, Vector3Int targetPoint)
            {

                _startPoint = MapManager.Instance.GridLayout.LocalToCell(obj.MovePoint.position - new Vector3(0, Walker.Y_OFFSET, Walker.Z_OFFSET));

                _pathIndex = -1;
                _resultPath.Clear();

                PathfindingJob job = PathfindingManager.CreatePathfindingJob(_startPoint, targetPoint, _resultPath);

                _jobHandle = job.Schedule();

                return true;
            }

            private void HandleMapChanges()
            {
                _pathIsUpToDate = false;
            }

            private void FollowThePath(Walker walker)
            {
                var walkerTransform = walker.transform;
                var walkerMovePointTransform = walker.MovePoint;

                //Move towards Waypoint position
                var vec = MapManager.Instance.TilemapGround.layoutGrid.LocalToCell(walkerTransform.position + new Vector3(0f, Walker.Y_OFFSET, 0f));
                var tileBelow = MapManager.Instance.TilemapGround.GetTile(vec + new Vector3Int(0, 0, -1));
                float slowDownfactor = ((IMapElement)tileBelow).WalkingSpeedFactor;

                walkerTransform.position = Vector3.MoveTowards(walkerTransform.position, walkerMovePointTransform.position, (walker.MoveSpeed * slowDownfactor) * Time.deltaTime);
                //Check if path calculation finished
                if (_jobHandle.IsCompleted)
                {
                    _jobHandle.Complete();


                    if (Vector3.Distance(walkerTransform.position, walkerMovePointTransform.position) <= 0.05f)//Point reached
                    {
                        if (_resultPath.Length > 0 && _pathIndex < 0)
                        {
                            //recieved new path
                            _pathIndex = _resultPath.Length - 1;
                        }
                        else if (_resultPath.Length <= 0)
                        {
                            callback();
                            walker.SwitchState(new IdleState());
                        }
                        if (_pathIndex >= 0)
                        {

                            //set a new point to walk towards
                            var cellPos = PathfindingManager.ConvertToTilemapCoordinates(_resultPath[_pathIndex]);
                            var newPos = MapManager.Instance.GridLayout.CellToWorld(cellPos) + new Vector3(0, Walker.Y_OFFSET, Walker.Z_OFFSET);

                            var lowerTile = MapManager.Instance.TilemapGround.GetTile(cellPos + new Vector3Int(0, 0, -1));
                            var upperTile = MapManager.Instance.TilemapGround.GetTile(cellPos);

                            if (lowerTile != null)
                            {
                                if (((IMapElement)lowerTile).Walkable)
                                {
                                    if (upperTile == null)
                                    {
                                        walkerMovePointTransform.position = newPos;
                                    }
                                    else if (((IMapElement)upperTile).CanWalkThrough)
                                    {
                                        walkerMovePointTransform.position = newPos;
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

        }
    }
}


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