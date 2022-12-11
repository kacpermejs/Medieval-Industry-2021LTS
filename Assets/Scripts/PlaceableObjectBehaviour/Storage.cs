using Assets.Scripts.ItemSystem;
using Assets.Scripts.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.PlaceableObjectBehaviour
{

    public class Storage : MonoBehaviour, IUICreator
    {
        public string title => "Storage";

        private List<Item> _items;

        public bool CanStoreItem(Item item, int quantity)
        {
            //TODO
            return true;
        }

        public VisualElement CreateUIContent()
        {
            var label = new Label(title);

            return label;
        }

        public void RegisterCallbacks()
        {
            
        }

        public void UnregisterCallbacks()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

