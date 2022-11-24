using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AgentSystem
{
    public class AIAgent : MonoBehaviour
    {
        public abstract class State : IState<AIAgent>
        {
            public void EnterState(AIAgent obj)
            {
                throw new NotImplementedException();
            }

            public void UpdateState(AIAgent obj)
            {
                throw new NotImplementedException();
            }
        }
    }

    public interface AIBehaviour
    {
        void Start();
        void Update();
    }

    /*public abstract class AIBehaviourInvoker : MonoBehaviour
    {
        public Command AbandonCommand;

        public Command InterruptCurrentActionCommand;

        public Command ContinueInterruptedActionsCommand;

    }*/
}
