using Assets.Scripts.PlaceableObjectBehaviour.Workplace;
using System;
using UnityEngine;

namespace Assets.Scripts.AgentSystem.AgentBehaviour
{
    public partial class Worker
    {
        public abstract class WorkerStateBase : IState<Worker>
        {
            public abstract void EnterState(Worker worker);
            public abstract void UpdateState(Worker worker);
            
        }

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
                        worker.DoWorkplaceTask();
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

        /// <summary>
        /// To be implemented, e.g. wandering about
        /// </summary>
        public class IdleState : WorkerStateBase
        {
            public override void EnterState(Worker worker)
            {
                
            }

            public override void UpdateState(Worker worker)
            {
                if(worker.Workplace != null)
                {
                    worker.SwitchState(new WaitingState());
                }
                else
                {
                    //Wandering - now sannding still
                }
            }
        }
    }
    
}
