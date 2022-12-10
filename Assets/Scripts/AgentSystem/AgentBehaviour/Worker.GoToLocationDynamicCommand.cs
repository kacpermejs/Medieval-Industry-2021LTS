using Assets.Scripts.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Assets.Scripts.AgentSystem.AgentBehaviour
{

    public partial class Worker
    {
        public class GoToLocationDynamicCommand : GoToLocationCommand
        {
            public Transform TargetObjectTransform;

            public GoToLocationDynamicCommand(Transform transform, Worker worker = null)
            {
                TargetWorker = worker;
                TargetObjectTransform = transform;
            }

            public override WorkerCommandBase Clone()
            {
                return new GoToLocationDynamicCommand(TargetObjectTransform, TargetWorker);
            }

            public override void Execute()
            {
                Position = TargetObjectTransform.position;
                base.Execute();

            }

            /*public static List<Vector3Int> SuitableOffsets(Func<Vector3> targetLocationProvider)
            {

                var targetCell = GameManager.ConvertToGridPosition(targetLocationProvider());

                Vector3Int[] neighbourOffsetArray = new Vector3Int[8];

                neighbourOffsetArray[0] = new Vector3Int( 0,  1); //North
                neighbourOffsetArray[1] = new Vector3Int( 1,  1); //NorthEast
                neighbourOffsetArray[2] = new Vector3Int( 1,  0); //East
                neighbourOffsetArray[3] = new Vector3Int( 1, -1); //SouthEast
                neighbourOffsetArray[4] = new Vector3Int( 0, -1); //South
                neighbourOffsetArray[5] = new Vector3Int(-1, -1); //SouthWest
                neighbourOffsetArray[6] = new Vector3Int(-1,  0); //West
                neighbourOffsetArray[7] = new Vector3Int(-1,  1); //NorthWest

                return neighbourOffsetArray.Where(
                    (e) => 
                    {
                        var vec = targetCell + e;
                        var arrayVec = PathfindingManager.ConvertToArrayCoordinates(vec);
                        return PathfindingManager.WalkableArray[arrayVec.x + arrayVec.y * PathfindingManager.MAP_X_SIZE] > 0;
                    }).ToList();
            }*/
        }



    }
}
