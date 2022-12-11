using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Assets.Scripts.CustomTiles;
using Assets.Scripts.Utills;
using Asstes.Scripts.Managers;

namespace Assets.Scripts.BuildingSystem
{
    public class BuildingSystemManager : SingletoneBase<BuildingSystemManager>, ISlaveManager
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

        private IMapElement _tileToPlace;

        private Camera _camera;

        private List<IMapElement> _placableTiles = new List<IMapElement>();

        public List<IMapElement> PlacableTiles { get => _placableTiles; }

        public bool AlwaysActive => false;


        #region Unity methods

        private void Awake()
        {
            //Caching camera cause it takes long to find it
            _camera = Camera.main;

            LoadPlaceableObjectPrefabs("Prefabs/BuildingPrefabs");
            LoadPlaceableObjectPrefabs("Prefabs/ResourcePrefabs");
            LoadRoadTiles();
            LoadMarkers();
        }

        private void OnEnable()
        {
            
        }

        void Update()
        {
            if (_tileToPlace != null)
            {
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
                     .ForEach(obj => PlacableTiles.Add(obj));
        }

        private void LoadRoadTiles()
        {
            PlacableTiles.Add(_roadTile);
        }

        private void LoadMarkers()
        {
            _markerTiles.Add(MarkerType.GreenBox, _greenBox);
            _markerTiles.Add(MarkerType.RedBox, _redBox);
            _markerTiles.Add(MarkerType.GreenTile, _greenTile);
            _markerTiles.Add(MarkerType.RedTile, _redTile);
            _markerTiles.Add(MarkerType.GreenDot, _greenDot);
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
            TilemapUtills.SetTilesBlock(area, _markerTiles[markerType], GameManager.Instance.TilemapMarkers);
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
                if ( !(TilemapUtills.CheckIfAreaEmpty(area, GameManager.Instance.TilemapGround)
                     && TilemapUtills.CheckIfAreaEmpty(area, GameManager.Instance.TilemapColliders)
                     && TilemapUtills.CheckIfCanBeBuiltUpon(GameManager.Instance.TilemapGround, area2)) )
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

        public void SetNewTileToBuild(int index)
        {
            _tileToPlace = _placableTiles[index];
        }

        public void Enable()
        {
            this.enabled = true;
        }

        public void Disable()
        {
            this.enabled = false;
        }
    }
}