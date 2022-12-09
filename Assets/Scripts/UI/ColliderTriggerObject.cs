﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Collider2D))]
    public class ColliderTriggerObject : MonoBehaviour
    {
        public UnityEvent MouseDown;

        private void OnMouseDown()
        {
            MouseDown?.Invoke();
        }
    }
}