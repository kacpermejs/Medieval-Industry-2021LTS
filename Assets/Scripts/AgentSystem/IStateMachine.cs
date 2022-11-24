namespace Assets.Scripts.AgentSystem
{
    public interface IFiniteStateMachine<T>
    {
        T CurrentState { get; }
        void SwitchState(T state);
    }
}
