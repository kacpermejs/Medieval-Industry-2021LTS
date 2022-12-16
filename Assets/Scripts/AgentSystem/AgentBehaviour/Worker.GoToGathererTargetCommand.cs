namespace Assets.Scripts.AgentSystem.AgentBehaviour
{

    public partial class Worker
    {
        public class GoToGathererTargetCommand : WorkerCommandBase
        {
            public GoToGathererTargetCommand(Worker worker = null) : base(worker) { }

            public override WorkerCommandBase Clone()
            {
                return new GoToGathererTargetCommand();
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
