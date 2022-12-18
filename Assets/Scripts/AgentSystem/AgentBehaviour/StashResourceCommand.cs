using Utills;

namespace AgentSystem
{
    public class StashResourceCommand : AgentCommandBase
    {
        public override bool CanExecute()
        {
            return _agent.gameObject.HasComponent<Gatherer>();
        }

        public override void Execute()
        {
            Gatherer gatherer = _agent.GetComponent<Gatherer>();
            gatherer.Stash(ExecutionEnded);
        }

        public override object Clone()
        {
            return new StashResourceCommand();
        }
    }
}
