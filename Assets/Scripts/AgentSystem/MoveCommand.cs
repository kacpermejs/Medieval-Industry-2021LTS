using UnityEngine;
using Utills;

namespace AgentSystem
{
    [System.Serializable]
    public class MoveCommand : AgentCommandBase
    {
        protected Vector3Int _position;

        public MoveCommand()
        {

        }
        public MoveCommand(Vector3Int position)
        {
            _position = position;
        }
        public override bool  CanExecute()
        {
            return _agent.gameObject.HasComponent<IActorMove>();
        }

        public override void Execute()
        {
            IActorMove mover = _agent.GetComponent<IActorMove>();
            mover.Move(_position, ExecutionEnded);
        }
        public override object Clone()
        {
            return new MoveCommand(_position);
        }
    }



    
}
