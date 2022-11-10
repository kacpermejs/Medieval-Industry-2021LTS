using Assets.Scripts.BuildingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AgentSystem.JobSystem
{
    public class JobSystemManager : MonoBehaviour
    {
        public static JobSystemManager Instance { get; private set; }

        #region Unity methods

        private void Awake()
        {
            Instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CreateAreaJob()
        {

        }

        #endregion
    }
}
