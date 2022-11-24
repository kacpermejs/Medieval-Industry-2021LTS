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
        [SerializeField] private int _workerCapacity = 1;
        [SerializeField] private List<Worker> _assignedWorkers = new List<Worker>();

        //[SerializeField] private List<UnityAction> _workCycle;

        [SerializeField] private IWorkerAgentTask _workerTask;

        [field:SerializeField]
        public bool IsOpen { get; private set; }
        //public List<UnityAction> WorkCycle { get => _workCycle; set => _workCycle = value; }
        public IWorkerAgentTask WorkerTask { get => _workerTask; }

        #region UI Elements
            private Button _AddWorkerButton;
        #endregion
        
        public string title => "Workplace";



        public bool AddWorker(Worker worker)
        {
            if( _assignedWorkers.Count < _workerCapacity && !_assignedWorkers.Contains(worker) )
            {
                _assignedWorkers.Add(worker);
                worker.Workplace = this;
                return true;
            }
            else
            {
                return false;
            }
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
