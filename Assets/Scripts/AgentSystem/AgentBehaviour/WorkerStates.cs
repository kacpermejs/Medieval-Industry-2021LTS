using UnityEngine;

namespace Assets.Scripts.AgentSystem.AgentBehaviour
{
    public partial class Worker
    {
        public abstract class StateBase : IState<Worker>
        {
            public abstract void EnterState(Worker worker);
            public abstract void UpdateState(Worker worker);
            
        }

        public class WorkingState : StateBase
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

        public class WaitingState : StateBase
        {
            public override void EnterState(Worker worker)
            {
                
            }

            public override void UpdateState(Worker worker)
            {
                
            }
        }

        public class IdleState : StateBase
        {
            public override void EnterState(Worker worker)
            {
                
            }

            public override void UpdateState(Worker worker)
            {
                
            }
        }
    }
    
}
