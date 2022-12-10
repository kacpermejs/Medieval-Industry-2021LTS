using Assets.Scripts.AgentSystem.Movement;
using Assets.Scripts.PlaceableObjectBehaviour;
using System;
using UnityEngine;

namespace Assets.Scripts.AgentSystem.AgentBehaviour
{

    public partial class Worker
    {

        public class GathererGatherResourceCommand : WorkerCommandBase
        {
            public GathererGatherResourceCommand(Worker worker = null) : base(worker)
            {
                TargetWorker = worker;
            }

            public override WorkerCommandBase Clone()
            {
                return new GathererGatherResourceCommand(TargetWorker);
            }

            public override void Execute()
            {
                base.Execute();
                Gatherer gatherer = TargetWorker.GetComponent<Gatherer>();
                gatherer.Gather(ExecutionEnded);
            }


        }



    }
}
