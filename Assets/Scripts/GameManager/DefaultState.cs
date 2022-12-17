using AgentSystem;
using BuildingSystem;

namespace GameStates
{
    public class DefaultState : GameSateBase
    {
        public override void EnterState(GameManager obj)
        {
            obj.scriptEnablers[typeof(BuildingSystemManager)].Disable();
            obj.scriptEnablers[typeof(AgentSelectionManager)].Enable();
            obj.scriptEnablers[typeof(UnitCommander)].Disable();

        }

        public override void UpdateState(GameManager obj) { }
    }

}