using Assets.Scripts.AgentSystem.AgentBehaviour;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.JobSystem
{
    public abstract class WorkerAgentTask : MonoBehaviour
    {
        protected Worker _assignedWorker;

        protected List<UnityAction> _instructions = new List<UnityAction>();

        protected int _instructionID = 0;

        private bool _busy = false;

        public UnityAction NextAction { get => _instructions[_instructionID]; }

        public event Action OnActionPerformed;

        public virtual void AssignWorker(Worker worker)
        {
            _assignedWorker = worker;
        }

        public void PerformAction()
        {
            if(!_busy)
            {
                NextAction?.Invoke();
                _busy = true;
            }
        }

        protected void ActionPerformed()
        {
            OnActionPerformed?.Invoke();
            _instructionID++;
            _busy = false;
        }

        public void Repeat()
        {
            _instructionID = 0;
            ActionPerformed();
        }

    }

}

