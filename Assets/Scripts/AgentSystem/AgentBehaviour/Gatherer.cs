using ItemSystem;
using System;
using UnityEngine;
using Utills;

namespace AgentSystem
{
    public class Gatherer : AgentBehaviour, ITargeter
    {
        public Transform CurrentTarget { get; set; }

        private Item heldItem;
        private int heldItemAmount;

        public void Gather(Action callback)
        {
            var heldItemPair = CurrentTarget.GetComponent<Resource>().Consume(1);
            heldItem = heldItemPair.Key;
            heldItemAmount = heldItemPair.Value;
            callback();
        }

        public void Stash(Action callback)
        {
            if (heldItemAmount > 0)
            {
                CurrentTarget.GetComponent<Storage>().AddItem(heldItem, heldItemAmount);
                heldItemAmount = 0;
            }
            callback();
        }
    }
}
