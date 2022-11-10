using Assets.Scripts.AgentSystem;
using Assets.Scripts.PlaceableObjectBehaviour;
using Assets.Scripts.Utills;
using System;
using UnityEngine;

namespace Assets.Scripts.AgentSystem.JobSystem
{
    public enum WorkerState
    {
        Idle = 0,

        Working = 10,

        Waiting = 20,
    }

    public class Worker : AIBehaviourInvoker
    {
        [SerializeField] private WorkerState _workerState;
        private WorkerState _previousWorkerState;
        [SerializeField] private Workplace _workplace;


        private Command _command;
        [SerializeField, ReadOnlyInspector] private int _currentInstructionIndex = 0;
        [SerializeField, ReadOnlyInspector] private bool _commandExecuted = true;

        private WorkerStateBase _currentWorkerState;

        public IdleState IdleState = new IdleState();
        public WorkingState WorkingState = new WorkingState();
        public WaitingState WaitingState = new WaitingState();

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


        private void Start()
        {
            _currentWorkerState = IdleState;
            _currentWorkerState.EnterState(this);
        }


        private void Update()
        {
            if(_workerState != _previousWorkerState)
            {
                
                switch (_workerState)
                {
                    case WorkerState.Idle:
                        SwitchState(IdleState);
                        break;
                    case WorkerState.Working:
                        SwitchState(WorkingState);
                        break;
                    case WorkerState.Waiting:
                        SwitchState(WaitingState);
                        break;
                    default:
                        break;
                }
            }
            _previousWorkerState = _workerState;

            _currentWorkerState.UpdateState(this);
        }


        #endregion

        public void SwitchState(WorkerStateBase state)
        {
            _currentWorkerState = state;
            _currentWorkerState.EnterState(this);
            Debug.Log("State changed!");
        }

        public void AddCommand(Command command)
        {
            _command = command;
        }

        public void FollowWorkplaceInstructions()
        {
            //If previous command was finished
            if (_commandExecuted)
            {
                //Query another command
                AddCommand(Workplace.WorkCycle[_currentInstructionIndex]);

                if (_command is GoToWorkplaceCommand)
                {
                    ((GoToWorkplaceCommand)_command).worker = this;
                    ((GoToWorkplaceCommand)_command).workplace = this.Workplace;
                }
                else if (_command is GoToResourceCommand)
                {
                    ((GoToResourceCommand)_command).worker = this;
                    ((GoToResourceCommand)_command).workplace = this.Workplace;
                }
                else if (_command is ProcessingCommand)
                {
                    ((ProcessingCommand)_command).worker = this;
                    ((ProcessingCommand)_command).workplace = this.Workplace;
                }
                else
                {

                    return;
                }

                _command.ExecutionFinishedEvent.AddListener(OnCommandFinished);
                _commandExecuted = false;
                _command.Execute();

            }


        }

        private void ProcessingFinishedHandler(object sender, EventArgs e)
        {
            Debug.Log("Worker finished processing");
        }

        private void ProcessingStartedHandler(object sender, EventArgs e)
        {
            Debug.Log("Worker started processing");

        }
        

        private void OnCommandFinished()
        {

            int count = Workplace.WorkCycle.Count;
            if ( count > 1 && _currentInstructionIndex < count - 1)
            {
                _currentInstructionIndex++;
            }
            else
            {
                _currentInstructionIndex = 0;
            }

            _commandExecuted = true;
            _command.ExecutionFinishedEvent.RemoveListener(OnCommandFinished);
        }
    }
}
