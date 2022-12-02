using UnityEngine.Events;

namespace Assets.Scripts.AgentSystem
{
    public interface ICommand
    {
        void Execute();
    }
}