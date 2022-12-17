﻿using ItemSystem;
using System;
using UnityEngine;
using Utills;

namespace AgentSystem
{
    public class Gatherer : AgentBehaviour, ITargeter
    {
        public Transform CurrentTarget { get; set; }

        public void Gather(Action _callback)
        {
            CurrentTarget.GetComponent<Resource>().Consume(1);
            _callback();
        }
    }
}
