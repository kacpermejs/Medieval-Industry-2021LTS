using Assets.Scripts.BuildingSystem;
using Assets.Scripts.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ObjectPropertiesController : MonoBehaviour
    {
        private PopUpCustomControl _popUp;
        private bool popUpStatus = false;

        public void OpenPopup()
        {
            if (_popUp == null || popUpStatus == false)
            {
                //Build popup window content
                var content = new TabbedMenuCustomControl();

                var tabs = GetComponents<IUICreator>();

                bool selected = true;
                foreach (var tab in tabs)
                {
                    content.AddTab(tab.title, tab.CreateUIContent(), selected);
                    tab.RegisterCallbacks();
                    selected = false;
                }
                content.Init();

                //Attach content to pop-up window
                var popUp = PopUpManager.OpenNewPopUp(gameObject.name, content);

                if (popUp != null)
                {
                    popUp.OnClose += () => { popUpStatus = false; };
                    popUpStatus = true;
                    _popUp = popUp;
                }
            }
        }

        private void OnDestroy()
        {
            if (_popUp != null)
            {
                var tabs = GetComponents<IUICreator>();

                foreach (var tab in tabs)
                {
                    tab.UnregisterCallbacks();
                }  
            }
        }
    }
}


