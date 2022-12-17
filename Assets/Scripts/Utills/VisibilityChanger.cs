﻿using UnityEngine;

namespace Utills
{
    public  class VisibilityChanger : MonoBehaviour
    {

        public void MakeVisible()
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }

        public void MakeInvisible()
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}