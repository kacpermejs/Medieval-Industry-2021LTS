using ItemSystem;
using UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ItemSystem
{

    public class Storage : MonoBehaviour, IUICreator
    {
        [SerializeField] private List<Item> _itemFilters;

        private HashSet<Item> _storedItems = LoadStoredItems(0);

        public bool HasFilters => _itemFilters.Count > 0;

        //UICreator
        public string Title => "Storage";

        public bool CanStoreItem(Item item, int quantity)
        {
            //TODO
            return true;
        }

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

        #region Unity Methods
        private void Awake()
        {
            if(HasFilters)
            {
                _itemFilters.ForEach(
                    (elem) =>
                    {
                        _storedItems.Add(elem);
                    });
            }
            else
            {
                //Something to handle accepting any type of item
            }
        }
        #endregion

        private static HashSet<Item> LoadStoredItems(int id)
        {
            //here is a placeholder for serialisation
            return new HashSet<Item>();
        }
    }
}

