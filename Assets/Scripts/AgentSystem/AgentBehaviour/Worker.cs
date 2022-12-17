using AgentSystem;
using UnityEngine;
using Utills;
using TaskSystem;
using System.Collections.Generic;
using System.Security.Cryptography;
using BuildingSystem;
using System;

namespace AgentSystem
{

    public partial class Worker : MonoBehaviour, IFiniteStateMachine<Worker.WorkerStateBase>, ICommandSender<AgentCommand, Agent>
    {
        #region PrivateFields

        private SelectionMarkerController _marker;
        private AgentCommand _currentCommand;

        private int _currentCommandIndex = 0;

        public WorkerTaskBase Task { get; private set; }
        
        #endregion

        #region Properties

        [field: SerializeField, ReadOnlyInspector]
        public Workplace Workplace { get; private set; }

        [field: SerializeField, ReadOnlyInspector]
        public bool IsSelected { get; private set; }

        [field: SerializeField, ReadOnlyInspector]
        public WorkerStateBase CurrentState { get; private set; }

        #endregion

        #region UnityMethods

        public void OnEnable()
        {
            CurrentState = new IdleState();
            CurrentState.EnterState(this);
        }

        private void Awake()
        {
            _marker = GetComponent<SelectionMarkerController>();
        }

        private void Update()
        {
            CurrentState.UpdateState(this);
        }

        #endregion

        #region Public methods

        public void SwitchState(WorkerStateBase state)
        {
            CurrentState = state;
            CurrentState.EnterState(this);
        }

        public void AssignWorkplace(Workplace workplace) => Workplace = workplace;
        public void AssignTask(WorkerTaskBase task)
        {
            Task = task;
            ResetCounter();
        }

        public void ResetCounter() => _currentCommandIndex = 0;

        #endregion

        private void CommandUpdate()
        {

            if (_currentCommand == null)
            {
                QueryNextCommand();
            }

        }

        private void QueryNextCommand()
        {
            //_currentCommand = Task.QueryCommand(_currentCommandIndex, this);
            _currentCommand =  Task.GetCommand(_currentCommandIndex);
            _currentCommand.OnExecutionEnded += EndHandler;

            SendCommand(GetComponent<Agent>(), _currentCommand);

        }

        private void EndHandler()
        {
            _currentCommand.OnExecutionEnded -= EndHandler;
            _currentCommand = null;
            _currentCommandIndex++;
        }

        public void SendCommand(Agent reciever, AgentCommand command)
        {
            reciever.AddCommand(command);
        }

        /*private void CommandExecution()
        {
            if (_currentCommand != null && !_currentCommand.Started)
            {
                _currentCommand.OnExecutionEnded += CommandEndedHandler;
                _currentCommand.Execute();
            }
        }

        private void CommandEndedHandler()
        {
            _currentCommand.OnExecutionEnded -= CommandEndedHandler;
            _currentCommandIndex++;
        }*/

    }
}
