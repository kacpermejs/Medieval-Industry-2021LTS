using Assets.Scripts.AgentSystem.AgentBehaviour;
using Assets.Scripts.Utills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.TaskSystem
{
    public abstract class WorkerTaskBase : MonoBehaviour
    {
        [field: SerializeField] protected List<Worker.WorkerCommandBase> _instructions = new List<Worker.WorkerCommandBase>();
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

    }

}

