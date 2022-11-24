using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Scripts.AgentSystem.AIAgent;

namespace Assets.Scripts.AgentSystem
{
    /// <summary>
    /// This Component handles enabling different behaviour components of an AIAgent depending on encountered conditions
    /// </summary>
    public partial class AIAgent : MonoBehaviour, IFiniteStateMachine<AIStateBase>
    {
        private AIBehaviour[] _behaviours;
        private AIBehaviour _activeBehaviour;

        private AIStateBase _currentState;

        public AIStateBase CurrentState => _currentState;

        public void SwitchState(AIStateBase state)
        {
            _currentState = state;
            _currentState.EnterState(this);
        }

        #region Unity Methods

        private void Awake()
        {
            _behaviours = GetComponents<AIBehaviour>();
        }

        private void OnEnable()
        {
            foreach (var component in _behaviours)
            {
                component.enabled = false;
            }

        }

        private void Update()
        {
            
        }

        #endregion
    }
}
