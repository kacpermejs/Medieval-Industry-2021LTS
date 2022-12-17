using Utills;

namespace AgentSystem
{
    public interface ICommandReciever<TCommand> where TCommand : ICommand
    {
        void AddCommand(TCommand command);
    }
}
