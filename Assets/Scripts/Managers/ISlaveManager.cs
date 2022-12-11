namespace Asstes.Scripts.Managers
{
    public interface ISlaveManager
    {
        bool AlwaysActive { get; }

        void Enable();
        void Disable();
    }
}