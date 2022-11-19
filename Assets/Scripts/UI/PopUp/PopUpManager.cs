using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class PopUpManager : MonoBehaviour
    {
        private List<PopUpCustomControl> _popUps = new List<PopUpCustomControl>();
        private VisualElement _root;
        private VisualElement _popupContainer;

        /*TabbedMenuController _menuController;*/

        public static PopUpManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }


        private void OnEnable()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _popupContainer = _root.Q("PopupContainer");

        }

        public void OpenNewPopup(VisualElement content)
        {
            var popUp = new PopUpCustomControl();

            popUp.contentContainer.Add(content);

            _popUps.Add(popUp);
            _popupContainer.Add(popUp);
        }


    }
}
