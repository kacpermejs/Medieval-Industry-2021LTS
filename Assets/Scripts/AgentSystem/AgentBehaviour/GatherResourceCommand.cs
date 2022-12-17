using ItemSystem;

namespace AgentSystem
{



    public class GatherResourceCommand : AgentCommandBase
    {
        public override object Clone()
        {
            return new GatherResourceCommand();
        }

        public override void Execute()
        {
            Gatherer gatherer = _agent.GetComponent<Gatherer>();
            gatherer.Gather(ExecutionEnded);
        }


    }
}
