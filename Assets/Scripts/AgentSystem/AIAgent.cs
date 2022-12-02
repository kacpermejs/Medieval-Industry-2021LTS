using System;
using UnityEngine;
using Assets.Scripts.AgentSystem.AgentBehaviour;

namespace Assets.Scripts.AgentSystem
{
    /// <summary>
    /// This Component handles enabling different behaviour components of an AIAgent depending on encountered conditions
    /// </summary>
    public partial class AIAgent : MonoBehaviour
    {
        private AIBehaviour[] _stateComponents;

        #region Unity Methods

        private void Awake()
        {
            _stateComponents = GetComponents<AIBehaviour>();
        }

        private void OnEnable()
        {
            foreach (var component in _stateComponents)
            {
                component.enabled = false;
            }
            //pick starting behaviour
            _stateComponents[0].enabled = true;
        }

        #endregion
    }
}
