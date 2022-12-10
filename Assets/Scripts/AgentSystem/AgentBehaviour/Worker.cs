using Assets.Scripts.AgentSystem;
using Assets.Scripts.PlaceableObjectBehaviour;
using UnityEngine;
using Assets.Scripts.Utills;
using Assets.Scripts.JobSystem;
using System.Collections.Generic;
using System.Security.Cryptography;
using Assets.Scripts.BuildingSystem;

namespace Assets.Scripts.AgentSystem.AgentBehaviour
{

    public partial class Worker : AIBehaviour, ISelectableAgent, IFiniteStateMachine<Worker.WorkerStateBase>
    {
        #region PrivateFields

        private SelectionMarker _marker;
        private WorkerCommandBase _currentCommand;

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
            _marker = GetComponent<SelectionMarker>();
        }

        private void Update()
        {
            CurrentState.UpdateState(this);
        }

        #endregion

        #region Public methods

        #region ISelectableAgent

        public void Select()
        {
            IsSelected = true;
            _marker.Select();

        }

        public void Deselect()
        {
            IsSelected = false;
            _marker.Deselect();
        }

        #endregion

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
            if(_currentCommand != null)
            {
                if (_currentCommand.Finished)
                {
                    QueryNextCommand();
                }
            }
            else
            {
                QueryNextCommand();
            }
            
            CommandExecution();

        }

        private void QueryNextCommand()
        {
            _currentCommand = Task.QueryCommand(_currentCommandIndex, this);
        }

        private void CommandExecution()
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
        }

    }
}
