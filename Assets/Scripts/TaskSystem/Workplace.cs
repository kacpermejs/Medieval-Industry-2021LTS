using AgentSystem;
using TaskSystem;
using GameStates;
using UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace TaskSystem
{
    public partial class Workplace : MonoBehaviour, IUICreator
    {
        [field: SerializeField] public Worker AssignedWorker { get; private set; }

        [field: SerializeField] public WorkerTaskBase WorkerTask { get; private set; }

        #region UI Elements
        private Button _AddWorkerButton;

        public string Title => "Workplace";

        #endregion

        public void AddWorker(Worker worker)
        {
            AssignedWorker = worker;
            worker.AssignWorkplace(this);
            WorkerTask.AssignWorker(worker);

        }

        #region IUICreator methods
        public VisualElement CreateUIContent()
        {

            VisualElement content = Resources.Load<VisualTreeAsset>("UI/UXML/WorkplaceTabContent")
                                             .Instantiate();

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
            foreach (var agent in AgentSelectionManager.Instance.AgentList)
            {
                if (agent is Agent agent2)
                {
                    if (agent2.TryGetComponent<Worker>(out Worker worker))
                    {
                        AddWorker(worker);
                    }
                }
            }
        }

    }
}
