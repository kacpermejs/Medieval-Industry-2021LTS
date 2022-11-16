using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{

    public class PopUpCustomControl : VisualElement, IPopUp
    {
        public new class UxmlFactory : UxmlFactory<PopUpCustomControl> { }

        private const string USS_CONTAINER = "popup_container";

        public PopUpCustomControl()
        {

            var window = new VisualElement();
            hierarchy.Add(window);

            //Resources.Load<VisualTreeAsset>("UI/UXML/PopUpTemplate");
/*
            var elem = _contentTreeAsset.Instantiate();

            _controller = new TabbedMenuController();

            _controller.RegisterTabCallbacks();*/


        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public void CreateNewPropertiesPopup(GameObject target)
        {
            VisualTreeAsset propertiesPopUpTemplate = Resources.Load<VisualTreeAsset>("UI/UXML/PopUpTemplate");

            var elem = propertiesPopUpTemplate.Instantiate();

            //elem.Q("Header").Q<Label>("Title").text = target.name;

            //_root.Add(elem);
        }
    }
}
