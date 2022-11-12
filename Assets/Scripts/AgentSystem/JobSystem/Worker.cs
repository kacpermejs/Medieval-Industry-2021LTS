using Assets.Scripts.AgentSystem;
using Assets.Scripts.PlaceableObjectBehaviour;
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
        [field: SerializeField] public WorkerState WorkerState { get; private set; }
        private WorkerState _previousWorkerState;
        
        [SerializeField] private Workplace _workplace;
        public Workplace Workplace
        {
            get => _workplace;
            set
            {
                _workplace = value;
                WorkerAssignedHandler();
            }
        }


        private Command _command;

        [SerializeField, ReadOnlyInspector] private int _currentInstructionIndex = 0;
        [SerializeField, ReadOnlyInspector] private bool _commandExecuted = true;

        private WorkerStateBase _currentWorkerState;

        public IdleState IdleState = new IdleState();
        public WorkingState WorkingState = new WorkingState();
        public WaitingState WaitingState = new WaitingState();


        private void WorkerAssignedHandler()
        {
            if (true)
            {
                _currentWorkerState = WaitingState;
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
            if(WorkerState != _previousWorkerState)
            {
                
                switch (WorkerState)
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
            _previousWorkerState = WorkerState;

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
    }
}
