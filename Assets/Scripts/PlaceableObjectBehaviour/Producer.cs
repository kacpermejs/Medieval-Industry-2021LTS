using Assets.Scripts.ItemSystem;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PlaceableObjectBehaviour
{
    public class Producer : MonoBehaviour
    {
        [SerializeField] private Recipe[] _recipes;

        private int _activeRecipe = 0;
        private bool _working = true;
        private int _progress = 0;

        private Resource _currentResource;

        // Start is called before the first frame update
        void Start()
        {
            //Find closest resource avaliable (A*)



            //TODO: this is a simulation of a npc arriving at his workplace and starting up the production process
            ProductionStartup();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ProductionStartup()
        {
            StartCoroutine(ProductionProgress1s());
        }

        public IEnumerator ProductionProgress1s()
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

        public void OnProductionCycleFinished()
        {
            Debug.Log("Produced: " + _recipes[_activeRecipe].OutputAmount + " " + _recipes[_activeRecipe].Output);
        }
    }

}
