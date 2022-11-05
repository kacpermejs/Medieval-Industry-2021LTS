using UnityEngine;

namespace Assets.Scripts.JobSystem
{
    public enum WorkerState
    {
        Idle = 0,

        TravelingToWorkplace = 1,
        TravelingToResource = 2,
        TravelingToStorage = 3,
        TravelingToMarketplace = 4,

        ProcessingAtWorkplace = 10,
        ResourceGatheringAction = 11,
    }

    public class Worker : MonoBehaviour
    {
        WorkerState _workerState;

        public WorkerState WorkerState { get => _workerState; private set => _workerState = value; }


    }
}
