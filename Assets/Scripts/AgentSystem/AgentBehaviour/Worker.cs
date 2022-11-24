using Assets.Scripts.AgentSystem;
using Assets.Scripts.PlaceableObjectBehaviour;
using System;
using UnityEngine;
using Assets.Scripts.Utills;
using Assets.Scripts.PlaceableObjectBehaviour.Workplace;
using static Assets.Scripts.PlaceableObjectBehaviour.Workplace.Workplace;
using static Assets.Scripts.AgentSystem.AgentBehaviour.Worker;

namespace Assets.Scripts.AgentSystem.AgentBehaviour
{
    public enum WorkerState
    {
        Idle = 0,

        Working = 10,

        Waiting = 20,
    }

    public partial class Worker : AIBehaviour, ISelectableAgent, IFiniteStateMachine<WorkerStateBase>
    {
        [SerializeField] private Workplace _workplace;
        [field: SerializeField] public WorkerState WorkerState { get; private set; }
        //[SerializeField, ReadOnlyInspector] private int _currentInstructionIndex = 0;
        //[SerializeField, ReadOnlyInspector] private bool _commandExecuted = true;

        public Workplace Workplace { get; set; }


        public bool IsSelected { get; private set; }

        private SelectionMarker _marker;

        public WorkerStateBase CurrentState { get; private set; }



        #region UnityMethods

        public override void OnEnable()
        {
            base.OnEnable();
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

        public override void OnDisable()
        {
            base.OnDisable();
        }

        #endregion

        public void SwitchState(WorkerStateBase state)
        {
            CurrentState = state;
            CurrentState.EnterState(this);
        }

        private void DoWorkplaceTask()
        {
            
        }

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


    }
}
