using Assets.Scripts.AgentSystem;
using Assets.Scripts.AgentSystem.JobSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PlaceableObjectBehaviour
{
    public class Workplace : MonoBehaviour
    {
        [SerializeField] private int _workerCapacity = 1;
        [SerializeField] private List<Worker> _assignedWorkers;

        [SerializeField] private List<Command> _workCycle;

        [SerializeField] private List<IWorkerAgentTask> _workerTasks;

        public List<Command> WorkCycle { get => _workCycle; set => _workCycle = value; }
        public List<IWorkerAgentTask> WorkerTasks { get => _workerTasks;  }

        public void AddWorker(Worker worker)
        {
            if( _assignedWorkers.Count < _workerCapacity && !_assignedWorkers.Contains(worker) )
            {
                _assignedWorkers.Add(worker);
            }
        }

        public Vector3Int GetResourcePosition()
        {
            return new Vector3Int(0, 0, 0);
        }
    }
}
