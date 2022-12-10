using System;
using UnityEngine;

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

            public WorkerCommandBase(Worker targetWorker = null)
            {
                TargetWorker = targetWorker;
            }

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

        internal class GathererSetAsTarget : WorkerCommandBase
        {
            private Func<Transform> _walkingTargetProvider;

            public GathererSetAsTarget(Func<Transform> function)
            {
                _walkingTargetProvider = function;
            }

            public override WorkerCommandBase Clone()
            {
                return new GathererSetAsTarget(_walkingTargetProvider);
            }

            public override void Execute()
            {
                base.Execute();
                Gatherer gatherer = TargetWorker.GetComponent<Gatherer>();

                gatherer.TargetResourceTransform = _walkingTargetProvider();

                ExecutionEnded();
            }
        }
    }
}
