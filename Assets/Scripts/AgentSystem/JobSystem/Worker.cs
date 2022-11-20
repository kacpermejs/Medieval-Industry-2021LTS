using Assets.Scripts.AgentSystem;
using Assets.Scripts.PlaceableObjectBehaviour;
using System;
using UnityEngine;
using Assets.Scripts.Utills;

namespace Assets.Scripts.AgentSystem.JobSystem
{
    public enum WorkerState
    {
        Idle = 0,

        Working = 10,

        Waiting = 20,
    }

    public partial class Worker : AIBehaviourInvoker
    {
        [SerializeField] private Workplace _workplace;
        [field: SerializeField] public WorkerState WorkerState { get; private set; }
        [SerializeField, ReadOnlyInspector] private int _currentInstructionIndex = 0;
        [SerializeField, ReadOnlyInspector] private bool _commandExecuted = true;
        private WorkerState _previousWorkerState;

        private StateBase _currentWorkerState;
        public Workplace Workplace
        {
            get => _workplace;
            set
            {
                _workplace = value;
                WorkerAssignedHandler();
            }
        }

        public StateBase CurrentWorkerState { get => _currentWorkerState; private set => _currentWorkerState = value; }

        private Command _command;
        private IdleState _idleState = new IdleState();
        private WorkingState _workingState = new WorkingState();
        private WaitingState _waitingState = new WaitingState();


        #region UnityMethods


        private void Start()
        {
            CurrentWorkerState = _idleState;
            CurrentWorkerState.EnterState(this);
        }


        private void Update()
        {
            if(WorkerState != _previousWorkerState)
            {
                
                switch (WorkerState)
                {
                    case WorkerState.Idle:
                        SwitchState(_idleState);
                        break;
                    case WorkerState.Working:
                        SwitchState(_workingState);
                        break;
                    case WorkerState.Waiting:
                        SwitchState(_waitingState);
                        break;
                    default:
                        break;
                }
            }
            _previousWorkerState = WorkerState;

            CurrentWorkerState.UpdateState(this);
        }


        #endregion

        #region Handlers
        private void WorkerAssignedHandler()
        {
            if (true)
            {
                CurrentWorkerState = _waitingState;
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
            _currentInstructionIndex++;
            _currentInstructionIndex = _currentInstructionIndex % Workplace.WorkCycle.Count;
            _commandExecuted = true;
            _command.ExecutionFinishedEvent.RemoveListener(OnCommandFinished);
        }

        #endregion

        public void SwitchState(StateBase state)
        {
            CurrentWorkerState = state;
            CurrentWorkerState.EnterState(this);
            //Debug.Log("State changed!");
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
                else if (_command is GoToTaskAreaCommand)
                {
                    ((GoToTaskAreaCommand)_command).worker = this;
                    ((GoToTaskAreaCommand)_command).workplace = this.Workplace;
                }
                else if (_command is DoWorkplaceProcessingCommand)
                {
                    ((DoWorkplaceProcessingCommand)_command).worker = this;
                    ((DoWorkplaceProcessingCommand)_command).workplace = this.Workplace;
                }
                else if (_command is DoTask)
                {
                    ((DoTask)_command).worker = this;
                    ((DoTask)_command).workplace = this.Workplace;
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

        
    }
}
