using Assets.Scripts.BuildingSystem;
using Assets.Scripts.Managers;
using Assets.Scripts.PlaceableObjectBehaviour;
using Assets.Scripts.Utills;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Asstes.Scripts.Managers
{
    public enum GameState
    {
        /// <summary>
        /// Clicking on building to open their properties popup windows, selecting units to command them
        /// </summary>
        Default,
        /// <summary>
        /// When units are selected you can make them do things
        /// </summary>
        UnitCommanding,
        /// <summary>
        /// Builder is enabled Clicking on the map will place selected building there
        /// </summary>
        BuildMode

    }

    /// <summary>
    /// This class is basically a manager of managers
    /// <br/>
    /// It enables and disables functionalities depending on the state
    /// <br/>
    /// State can be changed from the outside
    /// <br/>
    /// </summary>
    public partial class GameManager : SingletoneBase<GameManager>, IMasterManager
    {
        [SerializeField] private Grid _gridLayout;
        [SerializeField] private Tilemap _tilemapGround;
        [SerializeField] private Tilemap _tilemapColliders;
        [SerializeField] private Tilemap _tilemapMarkers;

        public static event Action<BoundsInt> OnMapChanged;

        public Grid GridLayout { get => _gridLayout; }
        public Tilemap TilemapGround { get => _tilemapGround; }
        public Tilemap TilemapMarkers { get => _tilemapMarkers; }
        public Tilemap TilemapColliders { get => _tilemapColliders; }

        public List<Storage> StorageBuildings;

        public GameState GameState { get; private set; }

        #region UnityMethods

        void Start()
        {
            StorageBuildings = FindObjectsOfType<Storage>().ToList();
            SwitchState(GameState.Default);
        }

        #endregion

        public void Activate(ISlaveManager slaveManager)
        {
            slaveManager.Enable();
        }

        public void SwitchState(GameState newState)
        {
            GameState = newState;

            //Transitions
            switch (GameState)
            {
                case GameState.Default:
                    GetComponentInChildren<AgentSelectionManager>().Enable();
                    GetComponentInChildren<BuildingSystemManager>().Disable();
                    break;
                case GameState.UnitCommanding:
                    GetComponentInChildren<AgentSelectionManager>().Enable();
                    GetComponentInChildren<BuildingSystemManager>().Disable();
                    break;
                case GameState.BuildMode:
                    GetComponentInChildren<BuildingSystemManager>().Enable();
                    GetComponentInChildren<AgentSelectionManager>().Disable();
                    break;
                default:
                    break;
            }
        }

        #region Map management
        public static Vector3Int ConvertToGridPosition(Vector3 pos)
        {
            return Instance.GridLayout.LocalToCell(pos);
        }

        public static Vector3 ConvertToLocalWorldPosition(Vector3Int pos)
        {
            return Instance.GridLayout.CellToLocal(pos);
        }

        public static void NotifyMapChanged(BoundsInt area)
        {
            OnMapChanged?.Invoke(area);
        }

        #endregion


    }
}