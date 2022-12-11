using Assets.Scripts.AgentSystem;
using Assets.Scripts.BuildingSystem;
using Assets.Scripts.UI;
using Assets.Scripts.Utills;
using Asstes.Scripts.Managers;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : SingletoneBase<UIManager>
{
    public Button BTN_cancel;
    public Button BTN_ok;
    
    VisualElement _root;

    #region Unity Methods

    private void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        BTN_cancel = _root.Q<Button>("BTN_cancel");

        BTN_cancel.clicked += OnCancelClicked;

        BTN_ok = _root.Q<Button>("BTN_ok");

        BTN_ok.clicked += OnOkClicked;

        ListView buildingListView = _root.Q<ListView>("BuildingListView");
        VisualTreeAsset buildingButtonTemplate = Resources.Load<VisualTreeAsset>("UI/UXML/ButtonTemplate");

        CreateBuildingListViewItems(buildingListView, buildingButtonTemplate);
    }

    #endregion

    private void CreateBuildingListViewItems(ListView buildingListView, VisualTreeAsset buildingButtonTemplate)
    {
        buildingListView.makeItem = () => buildingButtonTemplate.Instantiate();
        buildingListView.bindItem = (elem, index) =>
        {
            Label titleLabel = elem.Q<Label>("ItemTitle");
            Button imageButton = elem.Q<Button>("ItemButton");

            titleLabel.text = BuildingSystemManager.Instance.PlacableTiles[index].Name;

            imageButton.style.backgroundImage = new StyleBackground(BuildingSystemManager.Instance.PlacableTiles[index].Icon);
            imageButton.clicked += () =>
            {
                BuildingSystemManager.Instance.SetNewTileToBuild(index);
                GameManager.Instance.SwitchState(GameState.BuildMode);
                BTN_cancel.visible = true;

                // TODO: I think there will be a memory leak if you don't unsubscribe - You go check it
            };
        };


        buildingListView.itemsSource = BuildingSystemManager.Instance.PlacableTiles; //TODO: awful, I know to much
        buildingListView.fixedItemHeight = 100;
    }

    private void OnDisable()
    {
        BTN_cancel.clicked -= OnCancelClicked;
        BTN_ok.clicked -= OnOkClicked;
    }

    void OnCancelClicked()
    {
        switch (GameManager.Instance.GameState)
        {
            case GameState.BuildMode:
                EscapeBuildMode();
                break;
            default:
                break;
        }

    }

    void OnOkClicked()
    {
        switch (GameManager.Instance.GameState)
        {
            case GameState.BuildMode:
                break;
            default:
                break;
        }
    }

    private void EscapeBuildMode()
    {
        GameManager.Instance.SwitchState(GameState.Default);
        BTN_cancel.visible = false;
        BTN_ok.visible = false;
    }






}
