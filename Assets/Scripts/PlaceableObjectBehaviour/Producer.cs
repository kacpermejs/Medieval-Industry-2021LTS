using Assets.Scripts.ItemSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.PlaceableObjectBehaviour
{

    public class Producer : MonoBehaviour
    {

        public UnityEvent OnProductionFinished = new UnityEvent();

        public UnityEvent OnProductionStarted = new UnityEvent();

        [SerializeField] private Recipe[] _recipes;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

    }

}
