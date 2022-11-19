﻿using Assets.Scripts.AgentSystem;
using Assets.Scripts.AgentSystem.JobSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.PlaceableObjectBehaviour
{
    public class Workplace : MonoBehaviour, IUICreator
    {
        [SerializeField] private int _workerCapacity = 1;
        [SerializeField] private List<Worker> _assignedWorkers;

        [SerializeField] private List<Command> _workCycle;

        [SerializeField] private List<IWorkerAgentTask> _workerTasks;

        [field:SerializeField]
        public bool IsOpen { get; private set; }

        public List<Command> WorkCycle { get => _workCycle; set => _workCycle = value; }
        public List<IWorkerAgentTask> WorkerTasks { get => _workerTasks;  }

        public string title => "Workplace";

        public void AddWorker(Worker worker)
        {
            if( _assignedWorkers.Count < _workerCapacity && !_assignedWorkers.Contains(worker) )
            {
                _assignedWorkers.Add(worker);
            }
        }

        public VisualElement CreateUIContent()
        {
            var label = new Label(title);

            return label;
        }

        public Vector3Int GetResourcePosition()
        {
            return new Vector3Int(0, 0, 0);
        }
    }
}
