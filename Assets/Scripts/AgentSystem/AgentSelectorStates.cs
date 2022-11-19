namespace Assets.Scripts.AgentSystem
{
    public partial class AgentSelector
    {
        public abstract class StateBase : IState<AgentSelector>
        {
            public abstract void EnterState(AgentSelector worker);

            public abstract void UpdateState(AgentSelector worker);
        }

        public class StateActive : StateBase
        {
            public override void EnterState(AgentSelector worker)
            {
                throw new System.NotImplementedException();
            }

            public override void UpdateState(AgentSelector worker)
            {
                throw new System.NotImplementedException();
            }
        }

        public class StateInactive : StateBase
        {
            public override void EnterState(AgentSelector worker)
            {
                throw new System.NotImplementedException();
            }

            public override void UpdateState(AgentSelector worker)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}