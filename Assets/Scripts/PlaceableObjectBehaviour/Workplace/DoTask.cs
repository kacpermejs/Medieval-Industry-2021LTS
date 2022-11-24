using Assets.Scripts.AgentSystem;
using Assets.Scripts.JobSystem;
using Assets.Scripts.AgentSystem.Movement;
using UnityEngine;
using Assets.Scripts.AgentSystem.AgentBehaviour;

namespace Assets.Scripts.PlaceableObjectBehaviour.Workplace
{

    [CreateAssetMenu]
    public class DoTask : Command
    {
        [HideInInspector]
        public Workplace workplace;
        [HideInInspector]
        public Worker worker;

        public int Seconds = 2;

        public override void Execute()
        {
            base.Execute();
            Mover mover = worker.GetComponent<Mover>();

            Vector3Int workplaceCellPos = GameManager.ConvertToGridPosition(workplace.transform.position);

            var holdCommand = ScriptableObject.CreateInstance<Mover.HoldCommand>();
            holdCommand.Sender = this;
            holdCommand.Mover = mover;
            holdCommand.Seconds = Seconds;
            holdCommand.ExecutionFinishedEvent.AddListener(OnExecutionEnded);

            worker.GetComponent<Mover>().AddCommand(holdCommand);

        }
    }
}
