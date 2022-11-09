using System;
using UnityEngine;

namespace Assets.Scripts.AgentSystem
{

    /// <summary>
    /// Agent will follow the path starting the next frame from execution (not including the waiting queue)
    /// </summary>
    [CreateAssetMenu]
    public class MoveCommand : Command
    {
        [HideInInspector]
        public Mover Mover;
        [HideInInspector]
        public Vector3Int Destination;
        [HideInInspector]
        public Action Callback = null;
            
        public int Range = 50;
        public bool ComeNextTo = false;
        public float SlowDownFactor = 1f;

        public void CreateCommand(Mover mover, Vector3Int destination, Action callback = null, int range = 50, bool comeNextTo = false, int priority = 20, float slowDownFactor = 1f)
        {
            Mover = mover;
            Destination = destination;
            Callback = callback;
            Range = range;
            ComeNextTo = comeNextTo;
            Priority = priority;
            SlowDownFactor = slowDownFactor;
        }

        public override void Execute()
        {
            //Agent will follow the path starting the next frame from execution (not including the waiting queue)
            Mover.SchedulePathfinding(Destination, Range, ComeNextTo);
        }

        public override void OnExecutionEnded()
        {
            base.OnExecutionEnded();
            Callback?.Invoke();
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