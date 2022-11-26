using Assets.Scripts.AgentSystem;
using Assets.Scripts.AgentSystem.AgentBehaviour;
using Assets.Scripts.JobSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Assets.Scripts.PlaceableObjectBehaviour.Workplace
{
    public partial class Workplace : MonoBehaviour, IUICreator
    {
        [SerializeField] private Worker _assignedWorker;

        [SerializeField] private WorkerAgentTask _workerTask;

        [field:SerializeField]
        public bool IsOpen { get; private set; }
        public WorkerAgentTask WorkerTask { get => _workerTask; }

        #region UI Elements
            private Button _AddWorkerButton;
        
        public string title => "Workplace";

        #endregion

        public void AddWorker(Worker worker)
        {
            _assignedWorker = worker;
            worker.AssignWorkplace(this);
            _workerTask.AssignWorker(worker);
            
        }

        #region IUICreator methods
        public VisualElement CreateUIContent()
        {

            VisualElement content = Resources.Load<VisualTreeAsset>("UI/UXML/WorkplaceTabContent").Instantiate();

            _AddWorkerButton = content.Q<Button>("NewWorkerButton");

            return content;
        }

        public void RegisterCallbacks()
        {
            _AddWorkerButton.RegisterCallback<ClickEvent>(AddWorkersButtonHandler);
        }
        public void UnregisterCallbacks()
        {
            _AddWorkerButton.UnregisterCallback<ClickEvent>(AddWorkersButtonHandler);
        }

        #endregion

        private void AddWorkersButtonHandler(ClickEvent evt)
        {
            foreach (var agent in AgentSelector.Instance.AgentList)
            {
                if (agent is Worker worker)
                {
                    AddWorker(worker);
                }
            }
        }

    }
}
