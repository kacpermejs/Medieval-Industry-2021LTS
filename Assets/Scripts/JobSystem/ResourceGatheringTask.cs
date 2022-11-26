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
using UnityEngine.Events;
using Assets.Scripts.AgentSystem.Movement;
using Assets.Scripts.PlaceableObjectBehaviour.Workplace;

namespace Assets.Scripts.JobSystem
{

    public class ResourceGatheringTask : WorkerAgentTask
    {
        [SerializeField] private Resource _targetPrefab;

        private Resource _resourceToGather;

        public const int BaseSearchingRadius = 5;
        public const int Iterations = 5;

        private void Awake()
        {
            _instructions.Add(FindResource);
            _instructions.Add(GoToResource);
            _instructions.Add(GatherResource);
            _instructions.Add(GoToStorage);
            _instructions.Add(StoreGatheredResources);
            _instructions.Add(Repeat);
        }

        public void FindResource()
        {
            var resource = Resource.FindClosestAvaliableResource(
                _assignedWorker.transform.position,
                1,
                10,
                condition: (r) => !r.IsDepleted
                );
            if(resource != null)
            {
                _resourceToGather = resource;
                _resourceToGather.transform.localScale += new Vector3(1,1);
                ActionPerformed();
            }
            else
            {
                Repeat();
            }

        }
        
        public void GoToResource()
        {
            Mover mover = _assignedWorker.GetComponent<Mover>();

            Vector3Int resourceCellPos = GameManager.ConvertToGridPosition(_resourceToGather.gameObject.transform.position);

            var moveCommand = new Mover.MoveCommand(mover, resourceCellPos, comeNextTo: true);
            moveCommand.OnExecutionEnded += ActionPerformed;

            _assignedWorker.GetComponent<Mover>().AddCommand(moveCommand);
        }

        public void GatherResource()
        {
            ActionPerformed();
        }

        public void GoToStorage()
        {
            Mover mover = _assignedWorker.GetComponent<Mover>();

            Vector3Int workplaceCellPos = GameManager.ConvertToGridPosition(_assignedWorker.Workplace.transform.position);

            var moveCommand = new Mover.MoveCommand(mover, workplaceCellPos, comeNextTo: true);
            moveCommand.OnExecutionEnded += ActionPerformed;

            _assignedWorker.GetComponent<Mover>().AddCommand(moveCommand);
        }

        public void StoreGatheredResources()
        {
            ActionPerformed();
        }

        
    }

}

