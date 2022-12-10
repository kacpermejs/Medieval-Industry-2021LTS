using Assets.Scripts.AgentSystem.AgentBehaviour;
using Assets.Scripts.Utills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.JobSystem
{
    public abstract class WorkerTaskBase : MonoBehaviour
    {

        [field: SerializeField] protected List<Worker.WorkerCommandBase> _instructions = new List<Worker.WorkerCommandBase>();
        //[field: SerializeField] protected List<UnityAction> _actions = new ();

        //protected int _instructionID = 0;
        //private bool _busy = false;
        [field: SerializeField, ReadOnlyInspector] public int WorkerCount { get; private set; }

        public abstract bool CanPerformTask { get;  }

        public int NumberOfInstructions => _instructions.Count;

        public virtual void AssignWorker(Worker worker)
        {
            WorkerCount++;
        }

        public Worker.WorkerCommandBase QueryCommand(int index, Worker targetWorker)
        {
            var clonedCommand = _instructions[index].Clone();

            clonedCommand.TargetWorker = targetWorker;

            return clonedCommand;
        }

        /*public void PerformAction()
        {
            if(!_busy)
            {
                _busy = true;
                _instructions[_instructionID]?.Invoke();
            }
        }

        public void Repeat()
        {
            _instructionID = -1;
            ActionPerformed();
        }

        protected void ActionPerformed()
        {
            OnActionPerformed?.Invoke();
            _instructionID++;
            _busy = false;
        }*/

    }

}

