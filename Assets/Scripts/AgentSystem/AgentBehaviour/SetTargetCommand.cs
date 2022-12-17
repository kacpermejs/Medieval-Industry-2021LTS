using System;
using UnityEngine;
using Utills;

namespace AgentSystem
{
    [System.Serializable]
    public class SetTargetCommand : AgentCommandBase
    {
        private Func<Transform> _targetProvider;

        public SetTargetCommand(Func<Transform> function)
        {
            _targetProvider = function;
        }

        public override object Clone()
        {
            return new SetTargetCommand(_targetProvider);
        }

        public override void Execute()
        {
            var targeter = _agent.GetComponent<ITargeter>();

            targeter.CurrentTarget = _targetProvider();

            ExecutionEnded();
        }
    }
}
