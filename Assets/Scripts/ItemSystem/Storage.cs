using ItemSystem;
using UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utills;

namespace ItemSystem
{

    public class Storage : MonoBehaviour, IUICreator
    {
        [SerializeField] private List<Item> _itemFilters;
        [SerializeField] private int _baseCapacity = 10;

        [SerializeField, ReadOnlyInspector] private int _totalItems;

        private HashSet<Item> _filterSet;

        private Dictionary<Item, int> _storedItems;

        public bool HasFilters => _itemFilters.Count > 0;

        private void Awake()
        {
            _filterSet = new HashSet<Item>();
            foreach (var elem in _itemFilters)
            {
                _filterSet.Add(elem);
            }
            _storedItems = new Dictionary<Item, int>();
        }

        public bool CanStoreItem(Item item, int quantity)
        {
            if( _filterSet.Contains(item) )
            {
                return _totalItems + quantity <= _baseCapacity;
            }
            else //universal storage
            {
                return _filterSet.Count == 0 && _totalItems + quantity <= _baseCapacity;
            }
            
                
        }

        public void AddItem(Item item, int quantity)
        {
            if (CanStoreItem(item, quantity))
            {
                if (!_storedItems.ContainsKey(item))
                {
                    _storedItems.Add(item, quantity);
                }
                else
                {
                    _storedItems[item] += quantity;
                }
                _totalItems += quantity;
            }
            else
                Debug.Log("Cannot store item");
        }

        //UICreator
        public string Title => "Storage";


        public VisualElement CreateUIContent()
        {
            var label = new Label(Title);

            return label;
        }

        public void RegisterCallbacks()
        {

        }

        public void UnregisterCallbacks()
        {

        }


    }
}

