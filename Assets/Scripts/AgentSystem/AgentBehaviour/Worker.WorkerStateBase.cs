namespace Assets.Scripts.AgentSystem.AgentBehaviour
{
    public partial class Worker
    {
        public abstract class WorkerStateBase : IState<Worker>
        {
            public abstract void EnterState(Worker worker);
            public abstract void UpdateState(Worker worker);
            
        }
    }
    
}
