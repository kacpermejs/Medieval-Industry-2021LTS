using System;
using UnityEngine;

namespace AgentSystem
{
    public partial class Worker
    {

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
