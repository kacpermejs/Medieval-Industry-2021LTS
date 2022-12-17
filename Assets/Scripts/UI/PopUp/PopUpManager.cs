using Utills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class PopUpManager : SingletoneBase<PopUpManager>
    {
        private List<PopUpCustomControl> _popUps = new List<PopUpCustomControl>();
        private VisualElement _root;
        private VisualElement _popupContainer;

        #region Unity Methods

        private void OnEnable()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _popupContainer = _root.Q("PopupContainer");

        }

        #endregion

        public static PopUpCustomControl OpenNewPopUp(string header, VisualElement content)
        {
            var popUp = new PopUpCustomControl();

            popUp.SetContent(header, content);

            Instance._popUps.Add(popUp);
            Instance._popupContainer.Add(popUp);

            return popUp;
        }


    }
}
