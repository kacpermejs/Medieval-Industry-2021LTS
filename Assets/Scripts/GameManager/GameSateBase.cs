using Assets.Scripts.AgentSystem;
using System.Linq;

namespace Assets.Scripts.GameStates
{

    public abstract class GameSateBase : IState<GameManager>
    {
        public abstract void EnterState(GameManager obj);

        public abstract void UpdateState(GameManager obj);
    }

}