using UnityEngine;

namespace Assets.Scripts.AgentSystem.JobSystem
{
    public abstract class WorkerStateBase
    {
        public abstract void EnterState(Worker worker);
        public abstract void UpdateState(Worker worker);
        
    }
}
