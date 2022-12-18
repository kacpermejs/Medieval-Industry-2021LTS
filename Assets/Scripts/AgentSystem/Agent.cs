using UnityEngine;
using AgentSystem;
using System.Linq;
using System.Collections.Generic;

namespace AgentSystem
{

    public partial class Agent : MonoBehaviour, ISelectableAgent, ICommandReciever<AgentCommandBase>
    {
        HashSet<AgentBehaviour> agentBehaviours;

        private AgentCommandBase currentCommand;
        private Queue<AgentCommandBase> commandQueue = new();

        public bool CanBeSelected => GetComponent<Friendly>() != null;

        #region Unity Methods

        private void Awake()
        {
            agentBehaviours = GetComponents<AgentBehaviour>().ToHashSet();
        }

        private void Update()
        {
            if (currentCommand == null && commandQueue.Any())
            {
                HandleCommand();
            }
        }

        #endregion

        public void AddCommand(AgentCommandBase command)
        {
            commandQueue.Enqueue(command);
        }

        public void HandleCommand()
        {
            var command = commandQueue.Dequeue();

            command.SetAgent(this);
            if (command.CanExecute())
            {

            }
            currentCommand = command;
            currentCommand.OnExecutionEnded += CommandFinishedHandler;
            currentCommand.Execute();
        }

        private void CommandFinishedHandler()
        {
            currentCommand.OnExecutionEnded -= CommandFinishedHandler;
            currentCommand = null;
        }

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
    }
    
}
