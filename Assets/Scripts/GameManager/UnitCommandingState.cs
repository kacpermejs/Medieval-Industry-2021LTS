using AgentSystem;
using BuildingSystem;

namespace GameStates
{
    public class UnitCommandingState : GameSateBase
    {
        public override void EnterState(GameManager obj)
        {
            obj.scriptEnablers[typeof(BuildingSystemManager)].Disable();
            obj.scriptEnablers[typeof(AgentSelectionManager)].Enable();
            obj.scriptEnablers[typeof(UnitCommander)].Enable();
            //TODO: Enable Unit commanding script
        }

        public override void UpdateState(GameManager obj) { }
    }

}