using Assets.Scripts.AgentSystem.Movement;
using UnityEngine;
using Assets.Scripts.JobSystem;

namespace Assets.Scripts.PlaceableObjectBehaviour.Workplace
{
    public partial class Workplace
    {

        [CreateAssetMenu]
        public class DoWorkplaceProcessingCommand : WorkplaceCommandBase
        {


            public int Seconds = 2;

            public override void Execute()
            {
                base.Execute();
                Mover mover = worker.GetComponent<Mover>();

                Vector3Int workplaceCellPos = GameManager.ConvertToGridPosition(workplace.transform.position);

                Mover.HoldCommand holdCommand = ScriptableObject.CreateInstance<Mover.HoldCommand>();
                holdCommand.Sender = this;
                holdCommand.Mover = mover;
                holdCommand.Seconds = Seconds;
                holdCommand.ExecutionFinishedEvent.AddListener(OnExecutionEnded);

                worker.GetComponent<Mover>().AddCommand(holdCommand);

            }
        }
    }
}
