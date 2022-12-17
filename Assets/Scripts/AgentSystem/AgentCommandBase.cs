using System;
using Utills;
using UnityEngine.Events;

namespace AgentSystem
{
    [System.Serializable]
    public abstract class AgentCommandBase : ICommand, ICloneable//, IPriority
    {
        public event UnityAction OnExecutionEnded;

        protected Agent _agent;

        public abstract void Execute();
        public virtual void ExecutionEnded()
        {
            OnExecutionEnded?.Invoke();
        }

        public abstract object Clone();

        internal void SetAgent(Agent agent)
        {
            _agent = agent;
        }
    }



    
}
