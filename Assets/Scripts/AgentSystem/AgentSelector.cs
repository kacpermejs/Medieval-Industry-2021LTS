using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.AgentSystem;
using Assets.Scripts.Utills;

namespace Assets.Scripts.AgentSystem
{
    public partial class AgentSelector : SingletoneBase<AgentSelector>
    {
        public List<AIAgent> AgentList;
        private StateBase _currentSelectorState;
        private StateActive _activeState;
        private StateInactive _inactiveState;

        public void SwitchState(StateBase state)
        {
            _currentSelectorState = state;
            _currentSelectorState.EnterState(this);
            Debug.Log("State changed!");
        }
    }
}

