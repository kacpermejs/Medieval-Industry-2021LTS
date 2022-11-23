using Assets.Scripts.AgentSystem;
using Assets.Scripts.AgentSystem.JobSystem;
using Assets.Scripts.AgentSystem.Movement;
using UnityEngine;

namespace Assets.Scripts.PlaceableObjectBehaviour
{
    [CreateAssetMenu]
    public class GoToTaskAreaCommand : Command
    {
        [HideInInspector]
        public Workplace workplace;
        [HideInInspector]
        public Worker worker;
        public override void Execute()
        {
            base.Execute();
            Mover mover = worker.GetComponent<Mover>();

            Vector3Int resourceCellPos = workplace.GetTaskPosition();

            MoveCommand moveCommand = ScriptableObject.CreateInstance<MoveCommand>();
            moveCommand.CreateCommand(this, mover, resourceCellPos, comeNextTo: false);
            moveCommand.ExecutionFinishedEvent.AddListener(OnExecutionEnded);

            worker.GetComponent<Mover>().AddCommand(moveCommand);

        }
    }
}
