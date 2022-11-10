using Assets.Scripts.ItemSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.PlaceableObjectBehaviour
{
    public abstract class WorkplaceTask : MonoBehaviour
    {
        public UnityEvent OnProductionFinished = new UnityEvent();

        public UnityEvent OnProductionStarted = new UnityEvent();
        public abstract void StartDoingTask();
    }

    public class Producer : WorkplaceTask
    {
        [SerializeField] private Recipe[] _recipes;

        private int _activeRecipe = 0;
        private bool _working = false;
        private int _progress = 0;

        private Resource _currentResource;

        // Start is called before the first frame update
        void Start()
        {
            //Find closest resource avaliable (A*)



            //TODO: this is a simulation of a npc arriving at his workplace and starting up the production process
            //ProductionStartup();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ProductionStartup()
        {
            _working = true;
            StartCoroutine(ProductionProgress1s());
            OnProductionStarted?.Invoke();
        }

        private IEnumerator ProductionProgress1s()
        {
            if (_progress >= _recipes[_activeRecipe].TimeNeeded)
            {
                _progress = 0;
                OnProductionCycleFinished();
            }
            yield return new WaitForSeconds(1);

            _progress++;
            //Debug.Log("Progress: " + _progress);

            // check if production hasn't been interrupted
            if (_working)
                StartCoroutine(ProductionProgress1s());
        }

        private void OnProductionCycleFinished()
        {
            Debug.Log("Produced: " + _recipes[_activeRecipe].OutputAmount + " " + _recipes[_activeRecipe].Output);
            _working = false;
            OnProductionFinished?.Invoke();
            
        }

        public override void StartDoingTask()
        {
            ProductionStartup();
        }
    }

}
