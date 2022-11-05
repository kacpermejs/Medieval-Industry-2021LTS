using Assets.Scripts.JobSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PlaceableObjectBehaviour
{
    public class Workplace : MonoBehaviour
    {
        private List<Worker> _assignedWorkers;

        public void AddWorker(Worker worker)
        {
            _assignedWorkers.Add(worker);
        }
    }
}
