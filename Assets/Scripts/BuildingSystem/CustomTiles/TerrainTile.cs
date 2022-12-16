using Assets.Scripts.Utills;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.BuildingSystem.CustomTiles
{
    [CreateAssetMenu]
    public class TerrainTile : Tile, IMapElement, IInfo
    {
        [SerializeField] private bool _walkable = true;
        [SerializeField] private bool _canWalkThrough = false;
        [SerializeField] private bool _canBuildUpon = true;
        [SerializeField] private float _walkingSpeedFactor = 0.5f;
        [SerializeField] private bool _useStandardRules;
        [SerializeField] private DestinationMapLayer _layer = DestinationMapLayer.Ground;

        public string Name => name;
        public bool Walkable => _walkable;
        public bool CanWalkThrough => _canWalkThrough;
        public bool CanBuildUpon => _canBuildUpon;
        public float WalkingSpeedFactor => _walkingSpeedFactor;
        public bool UseStandardRules => _useStandardRules;
        public Sprite Icon => sprite;
        public DestinationMapLayer Layer => _layer;


        public bool CanBePlaced(Tilemap tilemap, BoundsInt area)
        {
            return true;
        }

        public void Place(Tilemap tilemap, Vector3Int position)
        {
            tilemap.SetTile(position, this);
        }

    }
}
