using Assets.Scripts.BuildingSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.CustomTiles
{
    [CreateAssetMenu]
    public class TerrainTile : Tile, IMapElement
    {
        #region IMapElement Properties Implementation

        [SerializeField] private bool _walkable = true;
        [SerializeField] private bool _canWalkThrough = false;
        [SerializeField] private bool _canBuildUpon = true;
        [SerializeField] private float _walkingSpeedFactor = 1;
        [SerializeField] private bool _isGroundFeature = true;
        [SerializeField] private bool _useStandardRules;
        [SerializeField] private IMapElement.DestinationMapLayer _layer = IMapElement.DestinationMapLayer.Ground;

        public string Name => name;
        public bool Walkable => _walkable;
        public bool CanWalkThrough => _canWalkThrough;
        public bool CanBuildUpon => _canBuildUpon;
        public float WalkingSpeedFactor => _walkingSpeedFactor;
        public bool IsGroundFeature => _isGroundFeature;
        public bool UseStandardRules => _useStandardRules;
        public Sprite Icon => sprite;
        public IMapElement.DestinationMapLayer Layer => _layer;


        public bool CanBePlaced(Tilemap tilemap, BoundsInt area)
        {
            return true;
        }

        public void Place(Tilemap tilemap, Vector3Int position)
        {
            tilemap.SetTile(position, this);
        }

        #endregion

    }
}
