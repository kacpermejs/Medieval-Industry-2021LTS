using Assets.Scripts.AgentSystem;
using Assets.Scripts.AgentSystem.JobSystem;
using Assets.Scripts.AgentSystem.Movement;
using UnityEngine;

namespace Assets.Scripts.PlaceableObjectBehaviour
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

            HoldCommand holdCommand = ScriptableObject.CreateInstance<HoldCommand>();
            holdCommand.Sender = this;
            holdCommand.Mover = mover;
            holdCommand.Seconds = Seconds;
            holdCommand.ExecutionFinishedEvent.AddListener(OnExecutionEnded);

            worker.GetComponent<Mover>().AddCommand(holdCommand);

        }
    }
}
