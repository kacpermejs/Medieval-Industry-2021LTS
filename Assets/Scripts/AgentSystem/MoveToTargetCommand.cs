using BuildingSystem;
using Utills;

namespace AgentSystem
{
    [System.Serializable]
    public class MoveToTargetCommand : MoveCommand
    {
        public MoveToTargetCommand()
        {
        }

        public override bool CanExecute()
        {
            return _agent.gameObject.HasComponent<IActorHold>() && base.CanExecute();
        }
        public override void Execute()
        {
            _position = MapManager.ConvertToGridPosition(_agent.GetComponent<ITargeter>().CurrentTarget.position);
            base.Execute();
        }

        public override object Clone()
        {
            return new MoveToTargetCommand();
        }
    }
}
