using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Assets.Scripts.CustomTiles;
using Assets.Scripts.Utills;

namespace Assets.Scripts.BuildingSystem
{
    public class BuildManager : SingletoneBase<BuildManager>
    {
        public enum MarkerType
        {
            GreenBox,
            RedBox,
            GreenTile,
            RedTile,
            GreenDot
        }

        [SerializeField] private RoadTile _roadTile;

        // TODO: temporary assignment - better load from resources
        [SerializeField] private TileBase _greenBox;
        [SerializeField] private TileBase _redBox;
        [SerializeField] private TileBase _redTile;
        [SerializeField] private TileBase _greenTile;
        [SerializeField] private TileBase _greenDot;

        private Dictionary<MarkerType, TileBase> _markerTiles = new Dictionary<MarkerType, TileBase>();

        private bool _buildMode = false;

        private IMapElement _tileToPlace;

        private List<IMapElement> _placableTiles = new List<IMapElement>();

        public List<IMapElement> PlacableTiles { get => _placableTiles; }

        private List<PlaceableObject> _buildings;
        private List<PlaceableObject> _resources;

        private Camera _camera;

        #region Unity methods

        //public static BuildManager Instance { get; private set; }

        private void Awake()
        {
            //Instance = this;

            _camera = Camera.main;

            // TODO: Optimize loading - loading all GameObjects wastes a lot of memory
            /*LoadPlacableObjectTiles("Prefabs/BuildingPrefabs");
            LoadPlacableObjectTiles("Prefabs/ResourcePrefabs");*/
            LoadPlaceableObjectPrefabs("Prefabs/BuildingPrefabs");
            LoadPlaceableObjectPrefabs("Prefabs/ResourcePrefabs");
            LoadRoadTiles();

            GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
        }

        //void OnEnable()
        //{

        //}

        // Start is called before the first frame update
        void Start()
        {
            _markerTiles.Add(MarkerType.GreenBox, _greenBox);
            _markerTiles.Add(MarkerType.RedBox, _redBox);
            _markerTiles.Add(MarkerType.GreenTile, _greenTile);
            _markerTiles.Add(MarkerType.RedTile, _redTile);
            _markerTiles.Add(MarkerType.GreenDot, _greenDot);
        }

        // Update is called once per frame
        void Update()
        {
            if (_buildMode && _tileToPlace != null)
            {
                //Need to cleanup markers after placement


                if (Input.GetMouseButtonDown(0))
                {
                    //Do not place any object if mouse is over a UI object
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        Vector2 screenPoint = _camera.ScreenToWorldPoint(Input.mousePosition);

                        Vector3Int gridPoint = GameManager.Instance.GridLayout.LocalToCell(screenPoint);

                        Tilemap destTilemap;
                        switch (_tileToPlace.Layer)
                        {
                            case IMapElement.DestinationMapLayer.Markers:
                                destTilemap = GameManager.Instance.TilemapMarkers;
                                break;
                            default:
                                destTilemap = GameManager.Instance.TilemapGround;
                                break;
                        }

                        Debug.Log(gridPoint);
                        Place(destTilemap, gridPoint, _tileToPlace, true);
                    }


                }
            }
        }

        #endregion

        #region LoadingAssets

        private void LoadPlaceableObjectPrefabs(string path)
        {
            Resources.LoadAll<PlaceableObject>(path)
                .Where(obj => !obj.name.StartsWith("[base]"))
                .ToList()
                .ForEach(obj => PlacableTiles.Add(obj) );
        }

        private void LoadRoadTiles()
        {
            PlacableTiles.Add(_roadTile);
        }

        #endregion

        #region EventHandlers

        private void GameManagerOnGameStateChanged(GameState state)
        {
            _buildMode = state == GameState.BuildMode;

            if (state == GameState.Default)
            {
                ClearAllMarkers();
            }
            else if (state == GameState.BuildMode)
            {
                //DisplayBuildingMarkers();
            }
        }

        #endregion

        #region Markers

        private void DisplayBuildingMarkers()
        {
            /*var area = new BoundsInt(new Vector3Int(-50, -50, 1), new Vector3Int(100, 100, 1));
            GameManager.SetTilesBlock(area, _markerTiles[MarkerType.GreenTile], GameManager.Instance.TilemapMarkers);*/
        }

        public void DisplayMarkers(BoundsInt area, MarkerType markerType)
        {
            GameManager.SetTilesBlock(area, _markerTiles[markerType], GameManager.Instance.TilemapMarkers);
        }

        public void ClearAllMarkers()
        {
            GameManager.Instance.TilemapMarkers.ClearAllTiles();
        }

        public void PlaceMarker(Vector3Int pos, MarkerType markerType)
        {
            GameManager.Instance.TilemapMarkers.SetTile(pos, _markerTiles[markerType]);
        }

        #endregion

        private void Place(Tilemap tilemap, Vector3Int gridPoint, IMapElement tile, bool useCanBePlaced)
        {
            //add offset if tile is a ground block like the road
            if (tile.Layer == IMapElement.DestinationMapLayer.Ground)
            {
                gridPoint.z -= 1;
            }
            //Only tiles with game objects attached can take area bigger than 1x1x1
            var area = new BoundsInt(gridPoint, new Vector3Int(1,1,1));

            if (tile is PlaceableObject obj)
            {
                //set bound area size
                area.size = obj.Bounds.size;
                //set offset to place building from the center not from the corner
                area.position -= new Vector3Int(area.size.x / 2, area.size.y / 2, 0);
            }


            if (useCanBePlaced)
            {
                if (!CanBePlaced(tile, tilemap, area))
                {
                    Debug.Log("<color=red>Building cannot be placed!");
                    return;//TODO: do something if cannot be placed like play audio or sth
                }
            }
            tile.Place(tilemap, gridPoint);

            //GameManager.NotifyMapChanged();
            GameManager.NotifyMapChanged(area);
        }

        private bool CanBePlaced(IMapElement tile, Tilemap tilemap, BoundsInt area)
        {
            // Standard rules
            if (tile.UseStandardRules)
            {
                BoundsInt area2 = new BoundsInt(area.position + new Vector3Int(0, 0, -1), new Vector3Int(area.size.x, area.size.y, 1));
                if
                (
                    !(CheckIfAreaEmpty(area, GameManager.Instance.TilemapGround) &&
                    CheckIfAreaEmpty(area, GameManager.Instance.TilemapColliders) &&
                    CheckIfCanBeBuiltUpon(GameManager.Instance.TilemapGround, area2) 
                    //TODO: && IsPlacementArea(tile)
                    )
                )
                {
                    return false;
                }
            }
            // Implementation specific rules
            if (!tile.CanBePlaced(tilemap, area))
            {
                return false;
            }
            return true;
        }

        public static bool CheckIfAreaEmpty(BoundsInt area, Tilemap tilemap)
        {
            var tiles = GameManager.GetTilesBlock(area, tilemap);
            foreach (var tile in tiles)
            {
                if (tile != null)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CheckIfAreaEmpty(Vector3Int blockPosition, Tilemap tilemap)
        {
            var tile = tilemap.GetTile(blockPosition);
            if (tile != null)
            {
                return false;
            }
            return true;
        }

        public static bool CheckIfCanBeBuiltUpon(Tilemap tilemap, BoundsInt area2)
        {
            var tiles = GameManager.GetTilesBlock(area2, tilemap);
            foreach (var tile in tiles)
            {
                if (tile is IMapElement element)
                {
                    if (!element.CanBuildUpon)
                    {
                        return false;
                    }
                }
                else if (tile == null)
                {
                    return false;
                }
            }

            return true;
        }


        public static bool CheckIfCanBeBuiltUpon(Tilemap tilemap, Vector3Int blockPosition)
        {
            var tile = tilemap.GetTile(blockPosition);
            if (tile is IMapElement element)
            {
                if (!element.CanBuildUpon)
                {
                    return false;
                }
            }
            else
            {
                if (tile == null)
                {
                    return false;
                }
                else
                    throw new Exception("Wrong tile class present in the tilemap");
            }


            return true;
        }

        public void SetNewTileToBuild(int index)
        {
            _tileToPlace = _placableTiles[index];
        }

        


    }
}