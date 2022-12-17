using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class TabbedMenuCustomControl : VisualElement
    {
        private const string TAB_LABEL_CLASS_NAME = "tab";
        private const string CONTENT_CONTAINER_NAME = "tabContentContainer";
        private const string TAB_CONTAINER_NAME = "tabs";
        private const string TAB_SUFFIX = "Tab";
        private const string TAB_CONTENT_SUFFIX = "Content";
        private Dictionary<string, VisualElement> _tabContentContainers = new Dictionary<string, VisualElement>();
        private Dictionary<string, Label> _tabLabelButtons = new Dictionary<string, Label>();

        private TabbedMenuController _controller;

        private VisualElement _tabContentContainer;
        private VisualElement _tabContainer;
        public int Selected = 0;

        public new class UxmlFactory : UxmlFactory<TabbedMenuCustomControl> { }

        public TabbedMenuCustomControl()
        {
            this.styleSheets.Add(Resources.Load<StyleSheet>("UI/Styles/TabbedMenu"));

            //tab button container
            _tabContainer = new VisualElement();
            _tabContainer.name = TAB_CONTAINER_NAME;

            //tab content container
            _tabContentContainer = new VisualElement();
            _tabContentContainer.name = CONTENT_CONTAINER_NAME;

            //TODO: temporary
            /*var tempVE = new VisualElement();
            tempVE.style.backgroundColor = Color.red;
            tempVE.style.width = 100;
            tempVE.style.height = 150;
            AddTab("temp", tempVE, true);

            var tempVE2 = new VisualElement();
            tempVE2.style.backgroundColor = Color.blue;
            tempVE2.style.width = 100;
            tempVE2.style.height = 150;
            AddTab("temp2", tempVE2, false);*/

            //Init();
        }

        public void Init()
        {
            hierarchy.Clear();

            _tabContainer.Clear();
            _tabContentContainer.Clear();

            foreach (var elem in _tabLabelButtons)
            {
                _tabContainer.Add(elem.Value);

                _tabContentContainer.Add(_tabContentContainers[elem.Key]);

            }

            hierarchy.Add(_tabContainer);
            hierarchy.Add(_tabContentContainer);

            _controller = new TabbedMenuController(this);
            _controller.RegisterTabCallbacks();
        }

        public void AddTab(string title, VisualElement tabContent, bool select)
        {
            _tabLabelButtons.Add(title, CreateTabLabelButton(title));
            _tabContentContainers.Add(title, CreateContentContainer(title, tabContent));


            if (select)
            {
                _tabLabelButtons[title].AddToClassList("tab-selected");
            }
            else
            {
                _tabContentContainers[title].AddToClassList("unselectedTabContent");
            }
            
        }

        private VisualElement CreateContentContainer(string title, VisualElement content)
        {
            var tabContentContainer = new VisualElement();
            tabContentContainer.name = title + TAB_CONTENT_SUFFIX;
            tabContentContainer.styleSheets.Add(this.styleSheets[0]);
            tabContentContainer.Add(content);

            return tabContentContainer;

        }

        private static Label CreateTabLabelButton(string title)
        {
            Label newLabelButton = new Label();
            newLabelButton.name = title + TAB_SUFFIX;
            newLabelButton.text = title;
            newLabelButton.AddToClassList(TAB_LABEL_CLASS_NAME);

            return newLabelButton;
        }

    }
}
