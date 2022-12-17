using AgentSystem.Movement;
using ItemSystem;
using System;
using UnityEngine;

namespace AgentSystem
{



    public class GatherResourceCommand : AgentCommand
    {
        public override object Clone()
        {
            return new GatherResourceCommand();
        }

        public override void Execute()
        {
            Gatherer gatherer = _agent.GetComponent<Gatherer>();
            gatherer.Gather(ExecutionEnded);
        }


    }

    [System.Serializable]
    public class SetAsTarget : AgentCommand
    {
        private Func<Transform> _targetProvider;

        public SetAsTarget(Func<Transform> function)
        {
            _targetProvider = function;
        }

        public override object Clone()
        {
            return new SetAsTarget(_targetProvider);
        }

        public override void Execute()
        {
            var targeter = _agent.GetComponent<ITargeter>();

            targeter.CurrentTarget = _targetProvider();

            ExecutionEnded();
        }
    }
}
