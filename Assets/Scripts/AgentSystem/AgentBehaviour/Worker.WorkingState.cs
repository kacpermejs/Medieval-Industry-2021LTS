namespace Assets.Scripts.AgentSystem.AgentBehaviour
{
    public partial class Worker
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
                    if(worker.Workplace.WorkerTask != null)
                    {
                        worker.Workplace.WorkerTask.PerformAction();
                    }
                    else
                    {
                        worker.SwitchState(new WaitingState());
                    }
                }
                else
                {
                    worker.SwitchState(new IdleState());
                }
            }
        }
    }
    
}
