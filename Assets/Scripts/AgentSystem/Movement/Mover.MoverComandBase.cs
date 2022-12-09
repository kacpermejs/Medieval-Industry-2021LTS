using System;

namespace Assets.Scripts.AgentSystem.Movement
{
    public partial class Mover
    {
        public abstract class MoverComandBase : ICommand
        {
            public Mover Mover;
            
            public event Action OnExecutionEnded;
            public abstract void Execute();

            internal void ExecutionEnded()
            {
                OnExecutionEnded?.Invoke();
            }
        }
    }

}