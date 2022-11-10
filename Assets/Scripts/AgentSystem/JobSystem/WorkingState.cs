namespace Assets.Scripts.AgentSystem.JobSystem
{
    public class WorkingState : WorkerStateBase
    {
        public override void EnterState(Worker worker)
        {
            
        }

        public override void UpdateState(Worker worker)
        {
            if (worker.Workplace != null)
            {
                worker.FollowWorkplaceInstructions();

            }
        }
    }
}
