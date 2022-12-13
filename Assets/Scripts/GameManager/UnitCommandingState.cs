using Assets.Scripts.AgentSystem;
using Assets.Scripts.BuildingSystem;

namespace Assets.Scripts.GameStates
{
    public class UnitCommandingState : GameSateBase
    {
        public override void EnterState(GameManager obj)
        {
            obj.scriptEnablers[typeof(BuildingSystemManager)].Disable();
            obj.scriptEnablers[typeof(AgentSelectionManager)].Enable();
            //TODO: Enable Unit commanding script
        }

        public override void UpdateState(GameManager obj) { }
    }

}