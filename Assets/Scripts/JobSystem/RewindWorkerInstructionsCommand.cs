using Assets.Scripts.AgentSystem.AgentBehaviour;

namespace Assets.Scripts.JobSystem
{
    public class RewindWorkerInstructionsCommand : Worker.WorkerCommandBase
    {
        public override Worker.WorkerCommandBase Clone()
        {
            return new RewindWorkerInstructionsCommand();
        }

        public override void Execute()
        {
            base.Execute();
            TargetWorker.ResetCounter();
            ExecutionEnded();
        }
    }
}
