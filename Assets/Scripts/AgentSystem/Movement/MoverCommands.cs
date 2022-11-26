using System;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;

namespace Assets.Scripts.AgentSystem.Movement
{
    public partial class Mover
    {
        public abstract class MoverComandBase : ICommand
        {
            public Mover Mover;
            
            public event Action OnExecutionEnded;
            public abstract void Execute();

            internal void ExecutionEnded()
            {
                OnExecutionEnded?.Invoke();
            }
        }

        /// <summary>
        /// Agent will follow the path starting the next frame from execution (not including the waiting queue)
        /// </summary>
        public class MoveCommand : MoverComandBase
        {
            public Vector3Int Destination;
            public bool ComeNextTo = false;
            public float SlowDownFactor = 1f;

            public MoveCommand(Mover mover, Vector3Int destination, bool comeNextTo = false, float slowDownFactor = 1f)
            {
                Mover = mover;

                Destination = destination;
                ComeNextTo = comeNextTo;
                SlowDownFactor = slowDownFactor;
            }

            public override void Execute()
            {
                //Agent will follow the path starting the next frame from execution (not including the waiting queue)
                Mover.SchedulePathfinding(Destination, ComeNextTo);
            }
        }

        public class HoldCommand : MoverComandBase
        {
            public int Seconds;
            public override void Execute()
            {
                Mover.StartCoroutine(Mover.HoldForSeconds(Seconds));
            }
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