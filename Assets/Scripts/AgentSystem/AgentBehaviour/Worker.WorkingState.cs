using System;

namespace Assets.Scripts.AgentSystem.AgentBehaviour
{
    public partial class Worker
    {
        public class WorkingState : WorkerStateBase
        {
            public override void EnterState(Worker worker)
            {
                worker.AssignTask(worker.Workplace.WorkerTask);
            }

            public override void UpdateState(Worker worker)
            {
                if (worker.Workplace != null)
                {
                    
                    if(worker.Workplace.WorkerTask != null && !worker.TaskCycleFinished())
                    {
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

        private bool TaskCycleFinished()
        {
            if (_currentCommandIndex >= Task.NumberOfInstructions)
                return true;
            else
                return false;
        }
    }
    
}
