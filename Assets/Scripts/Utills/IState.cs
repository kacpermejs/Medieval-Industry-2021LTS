namespace Assets.Scripts.AgentSystem
{
    public interface IState<T>
    {
        void EnterState(T obj);
        void UpdateState(T obj);
    }
}