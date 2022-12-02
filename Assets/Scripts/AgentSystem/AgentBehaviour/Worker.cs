using Assets.Scripts.AgentSystem;
using Assets.Scripts.PlaceableObjectBehaviour;
using System;
using UnityEngine;
using Assets.Scripts.Utills;
using Assets.Scripts.PlaceableObjectBehaviour.Workplace;
using Assets.Scripts.JobSystem;

namespace Assets.Scripts.AgentSystem.AgentBehaviour
{

    public partial class Worker : AIBehaviour, ISelectableAgent, IFiniteStateMachine<Worker.WorkerStateBase>
    {
        private SelectionMarker _marker;

        [field: SerializeField, ReadOnlyInspector]
        public Workplace Workplace { get; private set; }

        [field: SerializeField, ReadOnlyInspector]
        public bool IsSelected { get; private set; }

        [field: SerializeField, ReadOnlyInspector]
        public WorkerStateBase CurrentState { get; private set; }

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
        public void SwitchState(WorkerStateBase state)
        {
            CurrentState = state;
            CurrentState.EnterState(this);
        }

        public void AssignWorkplace(Workplace workplace)
        {
            Workplace = workplace;
        }

        #endregion

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
