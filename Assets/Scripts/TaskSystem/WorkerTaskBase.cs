using AgentSystem;
using Utills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TaskSystem
{
    public abstract class WorkerTaskBase : MonoBehaviour
    {
        //[field: SerializeField] protected List<Worker.WorkerCommandBase> _instructions = new List<Worker.WorkerCommandBase>();
        protected List<AgentCommandBase> _instructions = new();

        [field: SerializeField, ReadOnlyInspector] public int WorkerCount { get; private set; }

        public abstract bool CanPerformTask { get; }

        public int NumberOfInstructions => _instructions.Count;

        public virtual void AssignWorker(Worker worker)
        {
            WorkerCount++;
        }

        public AgentCommandBase GetCommand(int index)
        {
            var clonedCommand = _instructions[index].Clone();

            return clonedCommand as AgentCommandBase;
        }

        /*public Worker.WorkerCommandBase QueryCommand(int index, Worker targetWorker)
        {
            var clonedCommand = _instructions[index].Clone();

            clonedCommand.TargetWorker = targetWorker;

            return clonedCommand;
        }*/

    }

}

