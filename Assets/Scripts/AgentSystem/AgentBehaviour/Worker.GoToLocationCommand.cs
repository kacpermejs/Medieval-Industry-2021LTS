using UnityEngine;
using Assets.Scripts.AgentSystem.Movement;

namespace Assets.Scripts.AgentSystem.AgentBehaviour
{

    public partial class Worker
    {
        public class GoToLocationCommand : WorkerCommandBase
        {
            public Vector3 Position;

            public GoToLocationCommand(Vector3 position = default, Worker worker = null)
            {
                TargetWorker = worker;
                Position = position;
            }

            public override WorkerCommandBase Clone()
            {
                return new GoToLocationCommand(Position, TargetWorker);
            }

            public override void Execute()
            {
                base.Execute();
                Mover mover = TargetWorker.GetComponent<Mover>();

                Vector3Int cellPos = GameManager.ConvertToGridPosition(Position);

                var moveCommand = new Mover.MoveCommand(mover, cellPos);
                moveCommand.OnExecutionEnded += ExecutionEnded;

                TargetWorker.GetComponent<Mover>().AddCommand(moveCommand);
            }
        }

    }
}
