using UnityEngine;
using Assets.Scripts.AgentSystem;
using Assets.Scripts.AgentSystem.AgentBehaviour;

namespace Assets.Scripts.PlaceableObjectBehaviour.Workplace
{
    public partial class Workplace
    {
        public abstract class WorkplaceCommandBase : Command
        {
            [HideInInspector]
            public Workplace workplace;
            [HideInInspector]
            public Worker worker;
        }
    }
}
