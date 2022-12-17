using AgentSystem;
using UnityEngine;

namespace AgentSystem
{
    public interface ISelectableAgent
    {
        bool CanBeSelected { get; }

        void Select();

        void Deselect();
    }
}