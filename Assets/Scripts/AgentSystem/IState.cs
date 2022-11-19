namespace Assets.Scripts.AgentSystem
{
    public interface IState<T>
    {
        void EnterState(T worker);
        void UpdateState(T worker);
    }
}