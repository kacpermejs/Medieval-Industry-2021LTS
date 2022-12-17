using AgentSystem;
using BuildingSystem;

namespace GameStates
{
    public class BuildingState : GameSateBase
    {
        public override void EnterState(GameManager obj)
        {
            obj.scriptEnablers[typeof(BuildingSystemManager)].Enable();
            obj.scriptEnablers[typeof(AgentSelectionManager)].Disable();
            obj.scriptEnablers[typeof(UnitCommander)].Disable();


            UIManager.Instance.OnCancelClicked =
                () =>
                {
                    GameManager.Instance.SwitchState(new DefaultState());
                    UIManager.Instance.BTN_cancel.visible = false;
                    UIManager.Instance.BTN_ok.visible = false;
                };
        }

        public override void UpdateState(GameManager obj) { }

    }

}