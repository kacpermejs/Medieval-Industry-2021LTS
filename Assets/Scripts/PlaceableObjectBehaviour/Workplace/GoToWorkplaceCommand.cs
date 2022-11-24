using Assets.Scripts.AgentSystem;
using Assets.Scripts.JobSystem;
using Assets.Scripts.AgentSystem.Movement;
using UnityEngine;
using Assets.Scripts.AgentSystem.AgentBehaviour;

namespace Assets.Scripts.PlaceableObjectBehaviour.Workplace
{
    public partial class Workplace
    {
        [CreateAssetMenu]
        public class GoToWorkplaceCommand : WorkplaceCommandBase
        {
            public override void Execute()
            {
                base.Execute();
                Mover mover = worker.GetComponent<Mover>();

                Vector3Int workplaceCellPos = GameManager.ConvertToGridPosition(workplace.transform.position);

                var moveCommand = ScriptableObject.CreateInstance<Mover.MoveCommand>();
                moveCommand.CreateCommand(this, mover, workplaceCellPos, comeNextTo: true);
                moveCommand.ExecutionFinishedEvent.AddListener(OnExecutionEnded);

                worker.GetComponent<Mover>().AddCommand(moveCommand);

            }

            public override void OnExecutionEnded()
            {
                base.OnExecutionEnded();

            }
        }
    }
}
