using System;
using UnityEngine;
using AgentSystem;
using System.Linq;
using Utills;
using AgentSystem.Movement;
using System.Collections.Generic;
using UnityEngine.Events;
using BuildingSystem;

namespace AgentSystem
{

    public partial class Agent : MonoBehaviour, ISelectableAgent, ICommandReciever<AgentCommand>
    {
        AgentBehaviour[] agentBehaviours;

        private AgentCommand currentCommand;
        private Queue<AgentCommand> commandQueue = new();

        public bool CanBeSelected => agentBehaviours.Any();

        #region Unity Methods

        private void Awake()
        {
            agentBehaviours = GetComponents<AgentBehaviour>();
        }

        private void Update()
        {
            if (currentCommand == null && commandQueue.Any())
            {
                HandleCommand();
            }
        }

        #endregion

        public void AddCommand(AgentCommand command)
        {
            commandQueue.Enqueue(command);
        }
        public void HandleCommand()
        {
            var command = commandQueue.Dequeue();

            command.SetAgent(this);
            currentCommand = command;
            currentCommand.OnExecutionEnded += EndedHandler;
            currentCommand.Execute();
        }

        private void EndedHandler()
        {
            currentCommand.OnExecutionEnded -= EndedHandler;
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

    public class PriorityComparer : IComparer<IPriority>
    {
        public int Compare(IPriority x, IPriority y) => x.Priority > y.Priority ? 1 : x.Priority < y.Priority ? -1 : 0;
        
    }

    public interface IPriority
    {
        int Priority { get; }
    }


    [System.Serializable]
    public abstract class AgentCommand : ICommand, ICloneable//, IPriority
    {
        public event UnityAction OnExecutionEnded;


        protected Agent _agent;

        public abstract void Execute();
        public virtual void ExecutionEnded()
        {
            OnExecutionEnded?.Invoke();
        }

        public abstract object Clone();

        internal void SetAgent(Agent agent)
        {
            _agent = agent;
        }
    }

    [System.Serializable]
    public class MoveCommand : AgentCommand
    {
        protected Vector3Int _position;

        public MoveCommand()
        {

        }
        public MoveCommand(Vector3Int position)
        {
            _position = position;
        }

        public override object Clone()
        {
            return new MoveCommand(_position);
        }

        public override void Execute()
        {
            IActorMove mover = _agent.GetComponent<IActorMove>();
            mover.Move(_position, ExecutionEnded);
        }
    }

    [System.Serializable]
    public class MoveToTargetCommand : MoveCommand
    {
        public MoveToTargetCommand()
        {
        }

        public override object Clone()
        {
            return new MoveToTargetCommand();
        }
        public override void Execute()
        {
            _position = MapManager.ConvertToGridPosition(_agent.GetComponent<ITargeter>().CurrentTarget.position);
            base.Execute();
        }

    }

    [System.Serializable]
    public class HoldCommand : AgentCommand
    {
        protected int _seconds;

        public HoldCommand(int seconds)
        {
            _seconds = seconds;
        }

        public override object Clone()
        {
            return new HoldCommand(_seconds);
        }

        public override void Execute()
        {
            IActorHold holder = _agent.GetComponent<IActorHold>();
            holder.HoldForSeconds(_seconds, ExecutionEnded);
        }

            
    }



    
}
