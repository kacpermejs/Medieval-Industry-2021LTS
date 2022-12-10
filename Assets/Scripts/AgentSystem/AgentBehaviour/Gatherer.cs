using Assets.Scripts.AgentSystem.Movement;
using Assets.Scripts.PlaceableObjectBehaviour;
using System;
using UnityEngine;

namespace Assets.Scripts.AgentSystem.AgentBehaviour
{
    public class Gatherer : MonoBehaviour
    {

        public Transform TargetResourceTransform { get; set; }

        public void Gather(Action _callback)
        {
            /*Mover mover = GetComponent<Mover>();

            var command = new Mover.HoldCommand();
            command.Seconds = 1; //TODO: resource gathering
            command.OnExecutionEnded += _callback;
            mover.AddCommand(command);*/
            if (TargetResourceTransform != null)
            {
                TargetResourceTransform.GetComponent<Resource>()
                                       .Consume(1);
            }
            else
            {
                throw new NullReferenceException("No target");
            }
            _callback();
        }

        

    }
}
