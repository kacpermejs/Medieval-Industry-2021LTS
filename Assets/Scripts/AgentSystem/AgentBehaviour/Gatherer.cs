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
            if (TargetResourceTransform != null
                && TargetResourceTransform.TryGetComponent<Resource>(out var resource))
            {
                resource.Consume(1);
            }
            else
            {
                throw new NullReferenceException("No target");
            }
            _callback();
        }

        

    }
}
