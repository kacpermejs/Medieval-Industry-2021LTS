using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{

    public class PopUpCustomControl : VisualElement, IPopUp
    {
        public new class UxmlFactory : UxmlFactory<PopUpCustomControl> { }

        private const string STYLESHEET_PATH = "UI/Styles/PopUpStylesheet";

        private VisualElement _contentContainer;

        public PopUpCustomControl()
        {
            this.styleSheets.Add(Resources.Load<StyleSheet>(STYLESHEET_PATH));
            this.AddToClassList("windowContainer");
            this.pickingMode = PickingMode.Ignore;


            var window = new VisualElement();

            window.AddToClassList("windowBackground");
            window.name = "window";
            


            var header = new VisualElement();
            header.name = "header";

            var headerLabel = new Label("HeaderLabel");
            header.Add(headerLabel);

            _contentContainer = new VisualElement();
            _contentContainer.name = "ContentContainer";

            //Adding Dragging behaviour
            window.AddManipulator(new Dragger
            {
                TargetElements =
                {
                    headerLabel,
                }
            });

            //Hierarhy population
            hierarchy.Add(window);

            window.Add(header);
            window.Add(_contentContainer);

            var btn = new Label("text");
            
            SetContent(CreateMenuTest());

        }

        public void Close()
        {
        }

        public void Open()
        {
            
        }

        public void SetContent(VisualElement content)
        {
            _contentContainer.Clear();
            _contentContainer.Add(content);
        }

        private static VisualElement CreateMenuTest()
        {
            var menu = new TabbedMenuCustomControl();
            var label = new Label();
            label.name = "abcd";
            label.text = "abcd";
            menu.AddTab("ABCD", label, false);
            menu.Init();

            return menu;
        }

    }
}


/*
 
.unity-label {
    margin: 0;
    padding: 0;
}

.WindowHeader {
    flex-direction: row;
    justify-content: space-between;
}

.Close {
    width: 10px;
    height: 10px;
    background-color: rgb(197, 40, 40);
    border-left-width: 2px;
    border-right-width: 2px;
    border-top-width: 2px;
    border-bottom-width: 2px;
    border-left-color: rgba(0, 0, 0, 0.97);
    border-right-color: rgba(0, 0, 0, 0.97);
    border-top-color: rgba(0, 0, 0, 0.97);
    border-bottom-color: rgba(0, 0, 0, 0.97);
    border-top-left-radius: 4px;
    border-bottom-left-radius: 4px;
    border-top-right-radius: 4px;
    border-bottom-right-radius: 4px;
    margin-left: 2px;
    margin-right: 2px;
    margin-top: 2px;
    margin-bottom: 2px;
    padding-left: 4px;
    padding-right: 4px;
    padding-top: 4px;
    padding-bottom: 4px;

    font-size: 6px;
    -unity-font-style: bold;
}

.TabbedMenuArea {
    margin-left: 3px;
    margin-right: 3px;
    margin-top: 3px;
    margin-bottom: 3px;
}

.windowBackground {
    background-color: rgb(159, 62, 35);
    border-left-width: 2px;
    border-right-width: 2px;
    border-top-width: 2px;
    border-bottom-width: 2px;
    border-left-color: rgb(108, 61, 48);
    border-right-color: rgb(108, 61, 48);
    border-top-color: rgb(108, 61, 48);
    border-bottom-color: rgb(108, 61, 48);
    border-top-left-radius: 4px;
    border-bottom-left-radius: 4px;
    border-top-right-radius: 4px;
    border-bottom-right-radius: 4px;
    max-width: 200px;
    min-width: 100px;
    align-items: stretch;
}

.windowContainer {
    position: absolute;
    width = 100%;
    height = 100%;
    align-items: center;
    justify-content: center;
    align-content: center;
    
}

 
 */