using Assets.Scripts.AgentSystem;
using Assets.Scripts.JobSystem;
using Assets.Scripts.AgentSystem.Movement;
using UnityEngine;
using Assets.Scripts.AgentSystem.AgentBehaviour;

namespace Assets.Scripts.PlaceableObjectBehaviour.Workplace
{
    /*public partial class Workplace
    {
        [CreateAssetMenu(menuName = "WorkplaceCommands/DoTask")]
        public class DoTask : WorkplaceCommandBase
        {
            public int Seconds = 2;

            public override void Execute()
            {
                base.Execute();
                Mover mover = worker.GetComponent<Mover>();

                Vector3Int workplaceCellPos = GameManager.ConvertToGridPosition(workplace.transform.position);

                var holdCommand = new Mover.HoldCommand();
                //holdCommand.Sender = this;
                holdCommand.Mover = mover;
                holdCommand.Seconds = Seconds;
                holdCommand.ExecutionFinishedEvent.AddListener(OnExecutionEnded);

                worker.GetComponent<Mover>().AddCommand(holdCommand);

            }
        }
    }*/
}
