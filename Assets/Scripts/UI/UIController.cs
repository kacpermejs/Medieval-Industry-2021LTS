using Assets.Scripts.BuildingSystem;
using Assets.Scripts.UI;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public Button BTN_cancel;
    public Button BTN_house;
    
    VisualElement _root;
    public static UIController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        BTN_cancel = _root.Q<Button>("BTN_cancel");

        BTN_cancel.clicked += OnCancelClicked;

        ListView buildingListView = _root.Q<ListView>("BuildingListView");
        VisualTreeAsset buildingButtonTemplate = Resources.Load<VisualTreeAsset>("UI/UXML/ButtonTemplate");

        CreateBuildingListViewItems(buildingListView, buildingButtonTemplate);

        ShowPopup();
    }

    public void ShowPopup()
    {
        var popups = _root.Q("PopupContainer");

        var popUp = new PopUpCustomControl();
        popups.Add(popUp);

    }

    private void CreateBuildingListViewItems(ListView buildingListView, VisualTreeAsset buildingButtonTemplate)
    {
        buildingListView.makeItem = () => buildingButtonTemplate.Instantiate();
        buildingListView.bindItem = (elem, index) =>
        {
            Label titleLabel = elem.Q<Label>("ItemTitle");
            Button imageButton = elem.Q<Button>("ItemButton");

            titleLabel.text = BuildManager.Instance.PlacableTiles[index].Name;

            imageButton.style.backgroundImage = new StyleBackground(BuildManager.Instance.PlacableTiles[index].Icon);
            imageButton.clicked += () =>
            {
                BuildManager.Instance.SetNewTileToBuild(index);
                GameManager.Instance.UpdateGameState(GameState.BuildMode);
                BTN_cancel.visible = true;

                // TODO: I think there will be a memory leak if you don't unsubscribe - You go check it
            };
        };


        buildingListView.itemsSource = BuildManager.Instance.PlacableTiles; //TODO: awful, I know to much
        buildingListView.fixedItemHeight = 100;
    }

    private void OnDisable()
    {
        BTN_cancel.clicked -= OnCancelClicked;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnCancelClicked()
    {
        switch (GameManager.Instance.GameState)
        {
            case GameState.Default:
                break;
            case GameState.BuildMode:
                EscapeBuildMode();
                break;
            default:
                break;
        }

    }

    private void EscapeBuildMode()
    {
        GameManager.Instance.UpdateGameState(GameState.Default);
        BTN_cancel.visible = false;
    }




}
