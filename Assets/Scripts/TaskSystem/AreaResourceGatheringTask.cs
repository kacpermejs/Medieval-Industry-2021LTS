using BuildingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AgentSystem;
using System.Linq;
using Utills;
using GameStates;
using ItemSystem;

namespace TaskSystem
{

    public partial class AreaResourceGatheringTask : WorkerTaskBase, IInfo
    {
        [SerializeField] private SpriteRenderer _iconHolder;

        [SerializeField] private Resource _targetPrefab;

        [SerializeField] private Collider2D _areaCollider;

        [SerializeField, ReadOnlyInspector] private Queue<Resource> _resourcesToGather = new Queue<Resource>();

        [SerializeField, ReadOnlyInspector] private Storage _storage;

        public Storage Storage => GetSuitableStorage();
        public override bool CanPerformTask => _resourcesToGather.Count > 0;

        public string Name => _targetPrefab.Item.Name + " Gathering Task";

        public Sprite Icon => _targetPrefab.Item.Sprite;

        public event Action OnLocationChanged;

        #region UnityMethods

        private void Awake()
        {
            //TODO: make it into inspector functionality
            _instructions.Add(new SetTargetCommand(() => QuerryResource().transform));
            _instructions.Add(new MoveToTargetCommand());
            _instructions.Add(new GatherResourceCommand());
            _instructions.Add(new SetTargetCommand(() => Storage.transform));
            _instructions.Add(new MoveToTargetCommand());
            

        }

        private void OnEnable()
        {
            ResetSelection();
            _iconHolder.sprite = Icon;
            
        }

        private void Start()
        {
            _storage = GetSuitableStorage();
            MapManager.OnMapChanged += (e) => _storage = GetSuitableStorage();
        }

        #endregion
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
        public void LocationChanged()
        {
            OnLocationChanged?.Invoke();

            ResetSelection();
        }

        private void ResetSelection()
        {
            _resourcesToGather.Clear();

            FindResourcesInArea();
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

