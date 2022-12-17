namespace AgentSystem
{
    [System.Serializable]
    public class HoldCommand : AgentCommandBase
    {
        protected int _seconds;

        public HoldCommand(int seconds)
        {
            _seconds = seconds;
        }

        public override object Clone()
        {
            return new HoldCommand(_seconds);
        }

        public override void Execute()
        {
            IActorHold holder = _agent.GetComponent<IActorHold>();
            holder.HoldForSeconds(_seconds, ExecutionEnded);
        }

            
    }



    
}
