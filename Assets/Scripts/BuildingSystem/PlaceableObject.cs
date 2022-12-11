using Asstes.Scripts.Managers;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Assets.Scripts.BuildingSystem
{
    public class PlaceableObject : MonoBehaviour, IMapElement
    {
        [SerializeField] private TileBase[] _tiles;
        [Header("Make sure number of cells bound is equal to number of indecies")]
        [SerializeField] private List<int> _componentTilesIndecies;

        [SerializeField] private BoundsInt _bounds;

        #region Properties

        public BoundsInt Bounds { get => _bounds; }
        public List<int> ComponentTilesIndecies { get => _componentTilesIndecies; set => _componentTilesIndecies = value; }
        public TileBase[] Tiles { get => _tiles; set => _tiles = value; }
        
        private Vector3Int _gridPosition;

        #region IMapElement Properties Implementation
        [Header("Fields for IMapElement interrface:")]

        [SerializeField] private bool _walkable = false;
        [SerializeField] private bool _canWalkThrough = false;
        [SerializeField] private bool _canBuildUpon = false;
        [SerializeField] private float _walkingSpeedFactor = 0.5f;
        [SerializeField] private IMapElement.DestinationMapLayer _layer;
        [SerializeField] private bool _useStandardRules;

        public string Name => name;
        public bool Walkable => _walkable;
        public bool CanWalkThrough => _canWalkThrough;
        public bool CanBuildUpon => _canBuildUpon;
        public float WalkingSpeedFactor => _walkingSpeedFactor;
        public bool UseStandardRules => _useStandardRules;
        public Sprite Icon
        {
            get
            {
                //this prefab has at least 1 "Sprite" child GameObject
                return gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
            }
        }
        public IMapElement.DestinationMapLayer Layer => _layer;

        #endregion 

        #endregion



        #region Unity Methods
        
        private void Start()
        {
            
            _gridPosition = GameManager.ConvertToGridPosition(transform.localPosition);

            BoundsInt area = new BoundsInt();

            area.size = Bounds.size;
            area.position = _gridPosition - new Vector3Int(area.size.x / 2, area.size.y / 2, 0);


            int size = area.size.x * area.size.y * area.size.z;

            if (ComponentTilesIndecies.Count != size)
                throw new Exception("Arrays don't match in size");

            TileBase[] arr = new TileBase[size];
            int index;

            for (int i = 0; i < size; i++)
            {

                index = ComponentTilesIndecies[i];
                if (index == -1)
                {
                    arr[i] = null;
                }
                else
                {
                    if (Tiles[index] is IMapElement)
                        arr[i] = Tiles[index];
                    else
                        throw new Exception("Not a IMapElement");
                }
            }
            GameManager.Instance.TilemapColliders.SetTilesBlock(area, arr);    
        }

        private void OnDestroy()
        {
            if (!GameManager.Instance.IsDestroyed())
            {
                _gridPosition = GameManager.ConvertToGridPosition(transform.localPosition);

                BoundsInt area = new BoundsInt();

                area.size = Bounds.size;
                area.position = _gridPosition - new Vector3Int(area.size.x / 2, area.size.y / 2, 0);


                int size = area.size.x * area.size.y * area.size.z;

                if (ComponentTilesIndecies.Count != size)
                    throw new Exception("Arrays don't match in size");

                TileBase[] arr = new TileBase[size];

                for (int i = 0; i < size; i++)
                {
                    arr[i] = null;
                }
                GameManager.Instance.TilemapColliders.SetTilesBlock(area, arr);
            }
        }

        #endregion
        #region IMapElement Methods

        public void Place(Tilemap tilemap, Vector3Int position)
        {
            var worldGameobjectPosition = tilemap.layoutGrid.CellToLocal(position);

            Instantiate(this.gameObject, worldGameobjectPosition, Quaternion.identity);
        }

        public bool CanBePlaced(Tilemap tilemap, BoundsInt area)
        {
            return true;
        }

        #endregion

    }
}