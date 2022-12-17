using System;
using UnityEngine;

namespace AgentSystem
{
    public interface ITargeter
    {
        Transform CurrentTarget { get; set; }
    }
}
