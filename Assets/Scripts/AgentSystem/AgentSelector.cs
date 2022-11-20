using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.AgentSystem;
using Assets.Scripts.Utills;
using Assets.Scripts.AgentSystem.Movement;

namespace Assets.Scripts.AgentSystem
{
    public partial class AgentSelector : SingletoneBase<AgentSelector>
    {
        private List<SelectableAgent> _agentList;
        private State.Base _currentSelectorState;
        private State.WorkerSelection _activeSelectionState = new();
        private State.MovementSelection _noSelectionState = new();

        public IReadOnlyCollection<SelectableAgent> AgentList => _agentList.AsReadOnly();

        public event Action OnSelectionChanged;
        public event Action OnSelectionConfirmed;
        public event Action OnSelectionCanceled;

        public State.Base CurrentSelectorState { get => _currentSelectorState; private set => _currentSelectorState = value; }

        #region Unity Methods

        private void Awake()
        {
            GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
            _agentList = new List<SelectableAgent>();
        }

        private void Start()
        {
            SwitchState(_noSelectionState);
        }

        private void Update()
        {
            CurrentSelectorState.UpdateState(this);
        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
        }

        #endregion
        private void GameManagerOnGameStateChanged(GameState obj)
        {
            switch (obj)
            {
                case GameState.Default:
                    //Hands on commands
                    foreach (var agent in _agentList)
                    {
                        //use mouse movement
                    }
                    break;
                case GameState.BuildMode:
                    break;
                case GameState.WorkerAssignment:
                    //selection needs confirmation

                    break;
                default:
                    break;
            }
        }

        public static void SwitchState(State.Base state)
        {
            Instance.CurrentSelectorState = state;
            Instance.CurrentSelectorState.EnterState(Instance);
            //Debug.Log("State changed!");
        }

        public static void AddAnotherAgent(SelectableAgent agent)
        {
            Instance._agentList.Add(agent);
        }

        public static void ReplaceAll(SelectableAgent agent)
        {
            foreach (var member in Instance._agentList)
            {
                member.Deselect();
            }

            Instance._agentList.Clear();

            Instance._agentList.Add(agent);

        }


    }
}

