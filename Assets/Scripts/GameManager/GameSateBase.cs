using AgentSystem;
using System.Linq;

namespace GameStates
{

    public abstract class GameSateBase : IState<GameManager>
    {
        public abstract void EnterState(GameManager obj);

        public abstract void UpdateState(GameManager obj);
    }

}