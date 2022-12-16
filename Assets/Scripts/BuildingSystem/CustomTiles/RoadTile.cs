using Assets.Scripts.BuildingSystem;
using Assets.Scripts.UI;
using Assets.Scripts.Utills;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.BuildingSystem.CustomTiles
{
    [CreateAssetMenu]
    public class RoadTile : IsometricRuleTile<RoadTile.Neighbor>, IMapElement, IInfo
    {
        #region private fields
        //IMapElement
        [SerializeField] private bool _walkable = true;
        [SerializeField] private bool _canWalkThrough = false;
        [SerializeField] private bool _canBuildUpon = true;
        [SerializeField] private float _walkingSpeedFactor = 2f;
        [SerializeField] private DestinationMapLayer _layer;
        [SerializeField] private bool _useStandardRules = false;
        //Road specific
        [SerializeField] private TileBase[] _canReplace;
        #endregion

        #region properties
        public string Name => name;
        public bool Walkable => _walkable;
        public bool CanWalkThrough => _canWalkThrough;
        public bool CanBuildUpon => _canBuildUpon;
        public float WalkingSpeedFactor => _walkingSpeedFactor;
        public bool UseStandardRules => _useStandardRules;
        public Sprite Icon => m_DefaultSprite;
        public DestinationMapLayer Layer => _layer;

        #endregion

        #region public methods
        //IMapElement
        public void Place(Tilemap tilemap, Vector3Int position)
        {
            tilemap.SetTile(position, this);
        }

        public bool CanBePlaced(Tilemap tilemap, BoundsInt area)
        {
            var tiles = TilemapUtills.GetTilesBlock(area, tilemap);
            foreach (var tile in tiles)
            {
                foreach (var groundTile in _canReplace)
                {
                    if (tile == groundTile)
                    {
                        break;
                    }
                    return false;
                }
            }

            return true;
        }

        //RuleTile
        public class Neighbor : IsometricRuleTile.TilingRule.Neighbor
        {
            //public const int Null = 3;
            //public const int NotNull = 4;
        }

        public override bool RuleMatch(int neighbor, TileBase tile)
        {
            /*switch (neighbor) {
                case Neighbor.Null: return tile == null;
                case Neighbor.NotNull: return tile != null;
            }*/
            return base.RuleMatch(neighbor, tile);
        }
        #endregion

    }
}