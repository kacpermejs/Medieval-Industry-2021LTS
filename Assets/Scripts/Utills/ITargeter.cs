using System;
using UnityEngine;

namespace Utills
{
    public interface ITargeter
    {
        Transform CurrentTarget { get; set; }
    }
}
