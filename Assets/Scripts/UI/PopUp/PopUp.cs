using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class PopUp : MonoBehaviour
    {
        /*private TabbedMenuController controller;

        private void OnEnable()
        {
            UIDocument menu = GetComponent<UIDocument>();
            VisualElement root = menu.rootVisualElement;

            controller = new(root);

            controller.RegisterTabCallbacks();
        }*/

        private List<PopUpCustomControl> _popUps;
        VisualElement _root;

        /*TabbedMenuController _menuController;*/

        private void Awake()
        {
            /*_popUps = new List<PopUpCustomControl>();*/
        }


        private void OnEnable()
        {
            /*UIDocument menu = GetComponent<UIDocument>();
            _root = menu.rootVisualElement;

            var windowRoot = _root.Q("Screen").Q("PopUpTemplate");
            _menuController = new(windowRoot);

            _menuController.RegisterTabCallbacks();

            PopUpCustomControl newPopUp = new PopUpCustomControl();


            _popUps.Add(newPopUp);

            _root.Add(newPopUp);
*/


        }


    }
}
