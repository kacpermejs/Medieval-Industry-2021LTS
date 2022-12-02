namespace Assets.Scripts.AgentSystem.AgentBehaviour
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
                if(worker.Workplace.WorkerTask != null)
                {
                    worker.SwitchState(new WorkingState());
                }
            }
        }
    }
    
}
