using Assets.Scripts.BuildingSystem;
using Assets.Scripts.Pathfinding;
using Assets.Scripts.PlaceableObjectBehaviour;
using Assets.Scripts.CustomTiles;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using System;
using Assets.Scripts.AgentSystem.AgentBehaviour;

namespace Assets.Scripts.JobSystem
{
    public interface IWorkerAgentTask
    {
        void AssignWorker(Worker worker);

        void RequestTask(Worker worker);

        

    }

    public class AreaResourceGatheringTask : MonoBehaviour, IWorkerAgentTask
    {
        /// <summary>
        /// Concrete resource objects to be targeted by AI workers
        /// Queue is expandes if there is need - to fit number of assigned workers
        /// </summary>
        private Queue<Resource> _resourcesToGather = new Queue<Resource>();

        private List<Worker> _workersAssigned = new List<Worker>();

        public BoundsInt _bounds;

        private void Start()
        {
            
        }

        public void AssignWorker(Worker worker)
        {
            _workersAssigned.Add(worker);
        }

        public void OnResourcePieceGathered(Resource resource)
        {
            //add new resource object to the queue

            //find new object iin the area

        }

        private Resource FindNewResource()
        {
            //var tiles = BuildManager.GetTilesBlock(_bounds, GameManager.Instance.TilemapSurface);

            //foreach (var tile in tiles)
            //{
            //    if (tile is PlaceableObjectTile objTile)
            //    {
            //        if(objTile.gameObject.GetComponent<Resource>(). )
            //    }
            //}
            throw new System.NotImplementedException();


        }

        private void QueueNewResource()
        {

        }

        

        public void RequestTask(Worker worker)
        {
            throw new System.NotImplementedException();
        }
    }

}

