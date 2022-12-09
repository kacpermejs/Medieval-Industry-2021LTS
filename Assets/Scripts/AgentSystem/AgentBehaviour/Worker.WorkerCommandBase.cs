using System;

namespace Assets.Scripts.AgentSystem.AgentBehaviour
{

    public partial class Worker
    {
        public abstract class WorkerCommandBase : ICommand
        {
            public Worker TargetWorker = null;
            private bool finished = false;
            private bool started = false;

            public bool Finished { get => finished; private set => finished = value; }
            public bool Started { get => started; private set => started = value; }

            public event Action OnExecutionEnded;
            public virtual void Execute()
            {
                Started = true;
                Finished = false;
                if (TargetWorker == null)
                {
                    throw new InvalidOperationException("TargetWorker hasn't been set! You have to set it upon cloning!");
                }
            }

            public virtual void ExecutionEnded()
            {
                Finished = true;
                OnExecutionEnded?.Invoke();
            }

            public abstract WorkerCommandBase Clone();
        }
    }
}
