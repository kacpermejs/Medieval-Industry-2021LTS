using Assets.Scripts.JobSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PlaceableObjectBehaviour
{
    public class Workplace : MonoBehaviour
    {
        [SerializeField] private int _workerCapacity = 1;
        [SerializeField] private List<Worker> _assignedWorkers;

        //[SerializeField] private bool assigned = false;

        private void Awake()
        {
            GetComponent<WorkplaceTask>().OnProductionStarted += ProcessingStartedHandler;
            GetComponent<WorkplaceTask>().OnProductionFinished += ProcessingFinishedHandler;
        }
        private void OnDestroy()
        {
            GetComponent<WorkplaceTask>().OnProductionStarted -= ProcessingStartedHandler;
            GetComponent<WorkplaceTask>().OnProductionFinished -= ProcessingFinishedHandler;
        }

        private void ProcessingStartedHandler(object sender, EventArgs e)
        {
            Debug.Log("Task started");
        }


        private void ProcessingFinishedHandler(object sender, EventArgs e)
        {
            Debug.Log("Task finished");
        }

        public void DoProcessingAction()
        {
            GetComponent<WorkplaceTask>().StartDoingTask();
        }

        public void AddWorker(Worker worker)
        {
            if( _assignedWorkers.Count < _workerCapacity && !_assignedWorkers.Contains(worker) )
            {
                _assignedWorkers.Add(worker);
            }
        }
    }
}
