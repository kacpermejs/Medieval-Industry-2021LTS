using UnityEngine.Analytics;
using UnityEngine.Events;

namespace Assets.Scripts.AgentSystem.Movement
{
    public partial class Mover
    {

        public class HoldCommand : MoverComandBase
        {
            public int Seconds;
            public override void Execute()
            {
                Mover.StartCoroutine(Mover.HoldForSeconds(Seconds));
            }
        }
    }

}