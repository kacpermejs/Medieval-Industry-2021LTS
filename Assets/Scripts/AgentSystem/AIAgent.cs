using System;
using UnityEngine;
using Assets.Scripts.AgentSystem.AgentBehaviour;
using System.Linq;

namespace Assets.Scripts.AgentSystem
{
    public partial class AIAgent : MonoBehaviour, ISelectableAgent
    {
        private AIBehaviour[] _stateComponents;

        public bool CanBeSelected => _stateComponents.Any();

        public void Deselect()
        {
            var marker = GetComponent<SelectionMarkerController>();

            marker.Deselect();
        }

        public void Select()
        {
            var marker = GetComponent<SelectionMarkerController>();

            marker.Select();
        }

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
