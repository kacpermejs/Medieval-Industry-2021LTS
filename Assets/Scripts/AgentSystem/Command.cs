﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.AgentSystem
{
    public abstract class Command : ScriptableObject, ICommand
    {
        [HideInInspector]
        public object Sender;
        public int Priority = 20;

        private UnityEvent _executionStartedEvent = new UnityEvent();
        private UnityEvent _executionFinishedEvent = new UnityEvent();

        public UnityEvent ExecutionStartedEvent { get => _executionStartedEvent; private set => _executionStartedEvent = value; }
        public UnityEvent ExecutionFinishedEvent { get => _executionFinishedEvent; private set => _executionFinishedEvent = value; }

        public virtual void Execute()
        {
            //Debug.Log("Command " + this.GetType().Name + " started execution!");
            ExecutionStartedEvent?.Invoke();
        }

        public virtual void OnExecutionEnded()
        {
            //Debug.Log("Command " + this.GetType().Name + " ended execution!");
            ExecutionFinishedEvent?.Invoke();
        }

        private void OnDestroy()
        {
            ExecutionStartedEvent?.RemoveAllListeners();
            ExecutionFinishedEvent?.RemoveAllListeners();
        }
    }



}
