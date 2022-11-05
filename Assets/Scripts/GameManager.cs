using Assets.Scripts.BuildingSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum GameState
{
    /// <summary>
    /// Free camera, clicking on building to open their properties popup windows
    /// </summary>
    Default,
    /// <summary>
    /// Builder is enabled Clicking on the map will place selected building there
    /// </summary>
    BuildMode,

}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Grid _gridLayout;
    [SerializeField] private Tilemap _tilemapGround;
    //[SerializeField] private Tilemap _tilemapResources;
    [SerializeField] private Tilemap _tilemapSurface;
    [SerializeField] private Tilemap _tilemapMarkers;



    public Grid GridLayout { get => _gridLayout; }
    public Tilemap TilemapGround { get => _tilemapGround; }
    public Tilemap TilemapSurface { get => _tilemapSurface; }
    //public Tilemap TilemapResources { get => _tilemapResources; }
    public Tilemap TilemapMarkers { get => _tilemapMarkers; }

    private List<PlaceableObject> _objects;

    public GameState GameState { get; private set; }

    public static Action<GameState> OnGameStateChanged;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateGameState(GameState.Default);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateGameState(GameState newState)
    {
        GameState = newState;

        switch (newState)
        {
            case GameState.Default:
                HandleDefaultStateEnter();
                break;
            case GameState.BuildMode:
                HandleBuildModeEnter();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleBuildModeEnter()
    {

    }

    private void HandleDefaultStateEnter()
    {

    }
}
