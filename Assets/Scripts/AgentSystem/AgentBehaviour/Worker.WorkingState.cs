namespace Assets.Scripts.AgentSystem.AgentBehaviour
{
    public partial class Worker
    {
        public class WorkingState : WorkerStateBase
        {
            public override void EnterState(Worker worker)
            {
                if(worker.Task == null)
                {
                    worker.ResetCounter();
                }

                worker.AssignTask(worker.Workplace.WorkerTask);
            }

            public override void UpdateState(Worker worker)
            {
                if (worker.Workplace != null)
                {
                    if(worker.Workplace.WorkerTask != null && worker.Workplace.WorkerTask.CanPerformTask)
                    {
                        //worker.Workplace.WorkerTask.PerformAction();
                        worker.CommandUpdate();
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
