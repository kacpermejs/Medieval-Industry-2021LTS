using System;
using UnityEngine;
using GameStates;
using Utills;

namespace AgentSystem
{
    public interface ICommandSender<TCommand, TCommandReciever> where TCommand : ICommand
    {
        void SendCommand(TCommandReciever reciever, TCommand command);
    }

    public interface ICommandReciever<TCommand> where TCommand : ICommand
    {
        void AddCommand(TCommand command);
        void HandleCommand();
    }

    public class UnitCommander : SingletoneBase<UnitCommander>, IScriptEnabler
    {
        

        public void Disable()
        {
            enabled = false;
        }

        public void Enable()
        {
            enabled = true;
        }
    }
}
