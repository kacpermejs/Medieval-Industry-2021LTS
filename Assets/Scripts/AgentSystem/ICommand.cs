using UnityEngine.Events;

namespace Assets.Scripts.AgentSystem
{
    public interface ICommand
    {
        //UnityEvent ExecutionStartedEvent { get; }
        //UnityEvent ExecutionFinishedEvent { get; }

        void Execute();
        //void OnExecutionEnded();
    }
}