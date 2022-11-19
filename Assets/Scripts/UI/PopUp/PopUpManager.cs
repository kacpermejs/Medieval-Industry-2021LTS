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
        private VisualElement _windowRoot;

        /*TabbedMenuController _menuController;*/

        public static PopUpManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }


        private void OnEnable()
        {
            UIDocument menu = GetComponent<UIDocument>();
            _root = menu.rootVisualElement;

            _windowRoot = _root.Q("Screen");

        }

        public void OpenNewPopup(VisualElement content)
        {
            var popUp = new PopUpCustomControl();

            popUp.contentContainer.Add(content);

            _popUps.Add(popUp);
            _windowRoot.Add(popUp);
        }


    }
}
