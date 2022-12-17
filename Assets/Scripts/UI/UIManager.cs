using UI;
using Utills;
using GameStates;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using BuildingSystem;

public class UIManager : SingletoneBase<UIManager>
{
    public Button BTN_cancel;
    public Button BTN_ok;

    public Action OnCancelClicked;
    public Action OnOkClicked;
    
    VisualElement _root;

    #region Unity Methods

    private void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        BTN_cancel = _root.Q<Button>("BTN_cancel");

        BTN_cancel.clicked += CancelButtonHandler;

        BTN_ok = _root.Q<Button>("BTN_ok");

        BTN_ok.clicked += OkButtonHandler;

        ListView buildingListView = _root.Q<ListView>("BuildingListView");
        VisualTreeAsset buildingButtonTemplate = Resources.Load<VisualTreeAsset>("UI/UXML/ButtonTemplate");

        CreateBuildingListViewItems(buildingListView, buildingButtonTemplate);
    }

    private void OnDisable()
    {
        BTN_cancel.clicked -= CancelButtonHandler;
        BTN_ok.clicked -= OkButtonHandler;
    }

    private void OkButtonHandler()
    {
        OnOkClicked?.Invoke();
    }

    private void CancelButtonHandler()
    {
        OnCancelClicked?.Invoke();
    }

    #endregion



    private void CreateBuildingListViewItems(ListView buildingListView, VisualTreeAsset buildingButtonTemplate)
    {
        buildingListView.makeItem = () => buildingButtonTemplate.Instantiate();

        buildingListView.itemsSource = BuildingSystemManager.Instance.PlacableTiles;

        buildingListView.bindItem = (elem, index) =>
        {
            Label titleLabel = elem.Q<Label>("ItemTitle");
            Button imageButton = elem.Q<Button>("ItemButton");

            IMapElement provider = buildingListView.itemsSource[index] as IMapElement;

            titleLabel.text = (provider as IInfo).Name;

            imageButton.style.backgroundImage = new StyleBackground( (provider as IInfo).Icon );
            imageButton.clicked += () =>
            {
                BuildingSystemManager.SetNewTileToBuild(provider);
                GameManager.Instance.SwitchState(new BuildingState());
                BTN_cancel.visible = true;
            };
        };

        buildingListView.fixedItemHeight = 100;
    }

    

    

    






}
