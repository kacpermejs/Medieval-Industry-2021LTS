using Assets.Scripts.AgentSystem;
using Assets.Scripts.JobSystem;
using Assets.Scripts.AgentSystem.Movement;
using UnityEngine;
using Assets.Scripts.AgentSystem.AgentBehaviour;

namespace Assets.Scripts.PlaceableObjectBehaviour.Workplace
{
    /*public partial class Workplace
    {
        [System.Serializable]
        public class GoToTaskAreaCommand : WorkplaceCommandBase
        {
            public override void Execute()
            {
                base.Execute();
                Mover mover = worker.GetComponent<Mover>();

                Vector3Int resourceCellPos = workplace.GetTaskPosition();

                Mover.MoveCommand moveCommand = new Mover.MoveCommand();
                moveCommand.CreateCommand(this, mover, resourceCellPos, comeNextTo: false);
                moveCommand.ExecutionFinishedEvent.AddListener(OnExecutionEnded);

                worker.GetComponent<Mover>().AddCommand(moveCommand);

            }
        }
    }*/
}
