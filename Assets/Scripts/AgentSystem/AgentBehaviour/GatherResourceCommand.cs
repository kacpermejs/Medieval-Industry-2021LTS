﻿using Assets.Scripts.Utills;
using Utills;

namespace AgentSystem
{
    public class GatherResourceCommand : AgentCommandBase
    {
        public override bool CanExecute()
        {
            return _agent.gameObject.HasComponent<Gatherer>();
        }

        public override void Execute()
        {
            Gatherer gatherer = _agent.GetComponent<Gatherer>();
            gatherer.Gather(ExecutionEnded);
        }

        public override object Clone()
        {
            return new GatherResourceCommand();
        }
    }
}
