using Assets.Scripts.AgentSystem;
using Assets.Scripts.PlaceableObjectBehaviour;
using System;
using UnityEngine;
using static Assets.Scripts.AgentSystem.Mover;

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

    public class Worker : AIBehaviourInvoker
    {
        [SerializeField] private WorkerState _workerState;
        [SerializeField] private Workplace _workplace;

        [SerializeField] MoveCommand MoveCommand;
        [SerializeField] HoldCommand HoldCommand;

        [SerializeField] private bool _commandQueued = false;

        public WorkerState WorkerState { get => _workerState; set => _workerState = value; }
        public Workplace Workplace
        {
            get => _workplace;
            set
            {
                _workplace = value;
                //WorkerAssignedHandler();
            }
        }

        #region UnityMethods

        private void Awake()
        {
            MoveCommand.Mover = HoldCommand.Mover = GetComponent<Mover>();
            MoveCommand.Sender = HoldCommand.Sender = this;
        }


        private void Update()
        {
            if(Workplace != null)
            {
                if (!_commandQueued)
                {
                    switch (WorkerState)
                    {
                        case WorkerState.Idle:

                            break;
                        case WorkerState.TravelingToWorkplace:

                            Vector3Int workplaceCellPos = GameManager.ConvertToGridPosition(Workplace.transform.position);
                            MoveCommand.Destination = workplaceCellPos;
                            MoveCommand.Callback = OnMoverArrivedAtWorkplace;
                            GetComponent<Mover>().AddCommand(MoveCommand);
                            _commandQueued = true;
                            
                            break;
                        case WorkerState.TravelingToResource:
                            
                            break;
                        case WorkerState.TravelingToStorage:
                            
                            break;
                        case WorkerState.TravelingToMarketplace:
                            
                            break;
                        case WorkerState.ProcessingAtWorkplace:
                            // TODO change to command
                            //Workplace.DoProcessingAction();
                            HoldCommand.Callback = OnProcessingFinished;
                            GetComponent<Mover>().AddCommand(HoldCommand);
                            _commandQueued = true;

                            break;
                        case WorkerState.ResourceGatheringAction:

                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void OnDestroy()
        {
        }

        #endregion

        public void WorkerAssignedHandler()
        {

        }

        private void ProcessingFinishedHandler(object sender, EventArgs e)
        {
            Debug.Log("Worker finished processing");
        }

        private void ProcessingStartedHandler(object sender, EventArgs e)
        {
            Debug.Log("Worker started processing");

        }
        private void OnMoverArrivedAtWorkplace()
        {
            if (Workplace != null)
            {
                if (WorkerState == WorkerState.TravelingToWorkplace)
                {
                    WorkerState = WorkerState.ProcessingAtWorkplace;
                }
            }
            OnCommandFinished();
        }

        private void OnProcessingFinished()
        {
            if (Workplace != null)
            {
                if (WorkerState == WorkerState.ProcessingAtWorkplace)
                {
                    WorkerState = WorkerState.TravelingToResource;
                }
            }
            OnCommandFinished();
        }

        private void OnCommandFinished()
        {
            _commandQueued = false;
        }
    }
}
