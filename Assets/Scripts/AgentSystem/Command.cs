using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.AgentSystem
{
    public abstract class Command : ScriptableObject
    {
        [HideInInspector]
        public object Sender;
        public int Priority = 20;

        public UnityEvent ExecutionStartedEvent = new UnityEvent();
        public UnityEvent ExecutionFinishedEvent = new UnityEvent();

        public virtual void Execute()
        {
            Debug.Log("Command " + this.GetType().Name + " started execution!");
            ExecutionStartedEvent?.Invoke();
        }

        public virtual void OnExecutionEnded()
        {
            Debug.Log("Command " + this.GetType().Name + " ended execution!");
            ExecutionFinishedEvent?.Invoke();
        }

        private void OnDestroy()
        {
            ExecutionStartedEvent?.RemoveAllListeners();
            ExecutionFinishedEvent?.RemoveAllListeners();
        }
    }


    
}
