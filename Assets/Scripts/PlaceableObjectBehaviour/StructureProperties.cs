using Assets.Scripts.BuildingSystem;
using Assets.Scripts.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.PlaceableObjectBehaviour
{
    [RequireComponent(typeof(PlaceableObject))]
    public class StructureProperties : MonoBehaviour
    {
        
        private void OnMouseDown()//Request popup window
        {
            //Build popup window content
            var content = new TabbedMenuCustomControl();

            var tabs = GetComponents<IUICreator>();

            foreach (var tab in tabs)
            {
                content.AddTab(tab.title, tab.CreateUIContent(), false);
                tab.RegisterCallbacks();
            }

            //Attach content to pop-up window
            //Debug.Log("open");
            PopUpManager.Instance.OpenNewPopup(new Label("fhsdfyug"));

        }
        private void OnDestroy()
        {
            var tabs = GetComponents<IUICreator>();

            foreach (var tab in tabs)
            {
                tab.UnregisterCallbacks();
            }  
        }
    }

    public interface IUICreator
    {
        string title { get; }
        VisualElement CreateUIContent();

        void RegisterCallbacks();

        void UnregisterCallbacks();
    }
}


