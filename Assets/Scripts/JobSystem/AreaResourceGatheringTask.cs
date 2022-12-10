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
using Assets.Scripts.ItemSystem;
using System.Linq;
using Assets.Scripts.Utills;

namespace Assets.Scripts.JobSystem
{

    public partial class AreaResourceGatheringTask : WorkerTaskBase
    {
        [SerializeField] private Resource _targetPrefab;

        [SerializeField] private Collider2D _areaCollider;

        [SerializeField, ReadOnlyInspector] private Queue<Resource> _resourcesToGather = new Queue<Resource>();

        [SerializeField, ReadOnlyInspector] private Storage _storage;
        public Storage Storage => GetSuitableStorage();
        public override bool CanPerformTask => _resourcesToGather.Count > 0;

        public event Action OnLocationChanged;

        #region UnityMethods

        private void Awake()
        {
            _instructions.Add(new Worker.GathererSetAsTarget( () => QuerryResource().transform ));
            _instructions.Add(new Worker.GoToGathererTarget() );
            _instructions.Add(new Worker.GathererGatherResourceCommand());
            _instructions.Add(new Worker.GoToLocationDynamicCommand( Storage.transform ));

        }

        private void OnEnable()
        {
            ResetSelection();
            _storage = GetSuitableStorage();
        }

        #endregion

        private void ResetSelection()
        {
            /*foreach (var elem in _resourcesToGather)
            {
                elem.transform.localScale -= Vector3.one;
            }*/
            _resourcesToGather.Clear();

            FindResourcesInArea();
        }

        public void LocationChanged()
        {
            OnLocationChanged?.Invoke();

            ResetSelection();
        }

        public Resource QuerryResource()
        {
            if (_resourcesToGather.Count <= 0)
            {
                FindResourcesInArea();
            }

            if (_resourcesToGather.Count > 0)
            {
                return _resourcesToGather.Dequeue();
            }
            else
            {
                return null;
            }
        }

        private void FindResourcesInArea()
        {
            List<Resource> resources = new List<Resource>();

            Resource.FindResourcesInsideCollider2DNonAlloc(
            _targetPrefab.Id,
            _areaCollider,
            ref resources,
            condition: (r) => !r.IsDepleted
            );

            resources.ForEach((elem) =>
            {
                _resourcesToGather.Enqueue(elem);
                
            });
        }

        private Storage FindNearestStorage(Func<Storage, bool> condition)
        {
            var compatible = GameManager.Instance.StorageBuildings.Where(condition);

            Storage choice = compatible.First();

            foreach (var storage in compatible)
            {
                if (choice != null)
                {
                    var choicePosition = choice.gameObject.transform.position;
                    Vector3 nextPosition = storage.gameObject.transform.position;
                    Vector3 taskPosition = transform.position;
                    if (Vector3.Distance(taskPosition, choicePosition) > Vector3.Distance(taskPosition, nextPosition))
                    {
                        choice = storage;
                    }
                }
            }

            return choice;
        }

        private Storage GetSuitableStorage()
        {
            if (_storage != null && _storage.CanStoreItem(_targetPrefab.Item, 1))
                return _storage;
            else
                return _storage = FindNearestStorage((e) => e.CanStoreItem(_targetPrefab.Item, 1));
        }


    }
}

