namespace Assets.Scripts.AgentSystem
{
    public partial class AIAgent
    {
        public abstract class AIStateBase : IState<AIAgent>
        {
            public abstract void EnterState(AIAgent obj);

            public abstract void UpdateState(AIAgent obj);
        }

    }
}
