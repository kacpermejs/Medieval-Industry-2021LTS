using System;
using UnityEngine;

namespace Assets.Scripts.AgentSystem
{
    public abstract class Command : ScriptableObject
    {
        [HideInInspector]
        public object Sender;
        public int Priority = 20;
        public abstract void Execute();

        public virtual void OnExecutionEnded()
        {
            Debug.Log("Command " + this.GetType().Name + " ended execution!");
        }
    }


    
}
