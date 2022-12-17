using System;
using Utills;

namespace AgentSystem.Movement
{
    public partial class Walker
    {
        public abstract class WalkerComandBase : ICommand
        {
            public Walker Walker;
            
            public event Action OnExecutionEnded;
            public abstract void Execute();

            internal void ExecutionEnded()
            {
                OnExecutionEnded?.Invoke();
            }
        }
    }

}