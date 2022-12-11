using Assets.Scripts.AgentSystem.AgentBehaviour;
using UnityEngine;

namespace Assets.Scripts.AgentSystem
{
    public interface ISelectableAgent
    {
        bool CanBeSelected { get; }

        void Select();

        void Deselect();
    }
}