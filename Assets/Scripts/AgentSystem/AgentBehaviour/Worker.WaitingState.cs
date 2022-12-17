namespace AgentSystem
{
    public partial class Worker
    {
        public class WaitingState : WorkerStateBase
        {
            public override void EnterState(Worker worker)
            {
                
            }

            public override void UpdateState(Worker worker)
            {
                if(worker.Workplace.WorkerTask != null && worker.Workplace.WorkerTask.CanPerformTask)
                {
                    worker.SwitchState(new WorkingState());
                }
            }
        }
    }
    
}
