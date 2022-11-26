using Assets.Scripts.ItemSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.PlaceableObjectBehaviour
{
    public class Resource : MonoBehaviour
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _itemAmount;
        [SerializeField] private bool _renewable;



        [field: SerializeField]
        public int Id { get; private set; }
        public Item Item { get => _item; }
        public int ItemAmount { get => _itemAmount; }
        public bool IsDepleted { get => _itemAmount <= 0; }

        public UnityEvent OnDepleted = new UnityEvent();
        public UnityEvent OnRenewed = new UnityEvent();

        public void Renew(int amount)
        {
            if (_renewable)
            {
                _itemAmount += amount;
                OnRenewed?.Invoke();
            }
        }

        public void Consume(int amount)
        {
            if (amount <= _itemAmount)
            {
                _itemAmount -= amount;
            }
            else
            {
                _itemAmount = 0;
            }

            if(_itemAmount <= 0)
            {
                OnDepleted?.Invoke();
            }
        }

        public static void FindResourcesInRangeNonAlloc(
            int targetId,
            Vector2 pivotPoint,
            float SearchingRadius,
            ref List<Resource> foundResources,
            Func<Resource, bool> condition)
        {
            var pointA = new Vector3( pivotPoint.x - SearchingRadius, pivotPoint.y - SearchingRadius);
            var pointB = new Vector3( pivotPoint.x + SearchingRadius, pivotPoint.y + SearchingRadius);

            Collider2D[] colliders = Physics2D.OverlapAreaAll(pointA, pointB);

            foreach (var unit in colliders)
            {
                if (unit.TryGetComponent<Resource>(out var selectedResource))
                {
                    if (selectedResource.Id == targetId &&
                        (condition != null ? condition.Invoke(selectedResource) : true))
                    {
                        foundResources.Add(selectedResource);
                    }
                }
            }
        }

        public static Resource FindClosestAvaliableResource(
            Vector3 startingPoint,
            int targetId,
            float radius,
            int iterations = 1,
            Func<Resource, bool> condition = null)
        {
            List<Resource> foundResources = new List<Resource>();

            //central point area
            FindResourcesInRangeNonAlloc(
                targetId,
                new Vector2(startingPoint.x, startingPoint.y),
                radius,
                ref foundResources,
                condition == null ? condition : (r) => { return true; });

            for (int i = 1; i <= iterations; i++)
            {
                int width = 2 * i + 1;
                int cellCount = 8 * i;

                //search next cell ring
                for (int j = 0; j < cellCount; j++)
                {
                    // TODO: temporary
                    HorizontalSearching(j);
                    VerticalSearching(-2*j);
                    HorizontalSearching(-2*j);
                    VerticalSearching(2*j);
                    HorizontalSearching(j-1);

                }

                //if ring contains targets then break
                if (foundResources.Count > 0)
                {
                    int distance = int.MaxValue;
                    Resource choice = null;
                    foreach (var item in foundResources)
                    {
                        int newDistance = CalculateDistance(startingPoint, item);
                        if ( newDistance < distance)
                        {
                            choice = item;
                            distance = newDistance;
                        }
                    }
                    return choice;
                }

            }

            return null;
        }

        private static int CalculateDistance(Vector3 startingPoint, Resource item)
        {
            var resourceWorld = item.gameObject.transform.position;
            var startGrid = GameManager.ConvertToGridPosition(startingPoint);
            Vector3Int resourceGrid = GameManager.ConvertToGridPosition(resourceWorld);

            //manhattan distance
            var diff = startGrid - resourceGrid;

            return Mathf.Abs(diff.x) + Mathf.Abs(diff.y);
        }

        private static void VerticalSearching(int distance)
        {

        }

        private static void HorizontalSearching(int distance)
        {
        }
    }
}

