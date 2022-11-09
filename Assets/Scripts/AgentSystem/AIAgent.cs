using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AgentSystem
{
    public class AIAgent : MonoBehaviour
    {
        public void AbandonAllActions()
        {
            var AIBehaviourComponents = GetComponents<AIBehaviourInvoker>();

            foreach (var component in AIBehaviourComponents)
            {
                component.AbandonCommand?.Execute();
            }
        }
    }

    public abstract class AIBehaviourInvoker : MonoBehaviour
    {
        public Command AbandonCommand;

        public Command InterruptCurrentActionCommand;

        public Command ContinueInterruptedActionsCommand;

    }
}
