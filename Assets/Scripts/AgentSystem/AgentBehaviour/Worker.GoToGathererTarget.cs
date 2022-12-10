namespace Assets.Scripts.AgentSystem.AgentBehaviour
{

    public partial class Worker
    {
        public class GoToGathererTarget : WorkerCommandBase
        {
            public GoToGathererTarget(Worker worker = null) : base(worker) { }

            public override WorkerCommandBase Clone()
            {
                return new GoToGathererTarget();
            }

            public override void Execute()
            {
                base.Execute();

                Gatherer gatherer = TargetWorker.GetComponent<Gatherer>();

                var command = new GoToLocationDynamicCommand(gatherer.TargetResourceTransform, TargetWorker);
                command.OnExecutionEnded += ExecutionEnded;
                command.Execute();

            }
        }



    }
}
