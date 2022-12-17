namespace Utills
{
    public interface IFiniteStateMachine<T>
    {
        T CurrentState { get; }
        void SwitchState(T state);
    }
}
