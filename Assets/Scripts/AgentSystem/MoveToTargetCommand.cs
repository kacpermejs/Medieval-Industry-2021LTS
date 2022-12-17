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

        public override object Clone()
        {
            return new MoveToTargetCommand();
        }
        public override void Execute()
        {
            _position = MapManager.ConvertToGridPosition(_agent.GetComponent<ITargeter>().CurrentTarget.position);
            base.Execute();
        }

    }



    
}
