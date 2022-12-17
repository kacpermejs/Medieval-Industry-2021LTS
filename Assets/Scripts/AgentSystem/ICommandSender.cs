using Utills;

namespace AgentSystem
{
    public interface ICommandSender<TCommand, TCommandReciever> where TCommand : ICommand
    {
        void SendCommand(TCommandReciever reciever, TCommand command);
    }
}
