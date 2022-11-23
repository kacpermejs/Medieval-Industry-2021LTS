using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.BuildingSystem
{
    public interface IMapElement
    {
        string Name { get; }
        bool Walkable { get; }
        bool CanBuildUpon { get; }
        bool CanWalkThrough { get; }
        float WalkingSpeedFactor { get; }
        bool UseStandardRules { get; }
        Sprite Icon { get; }
        DestinationMapLayer Layer { get; }
        public enum DestinationMapLayer
        {
            Surface,
            Ground,
            Markers
        }
        void Place(Tilemap tilemap, Vector3Int position);
        bool CanBePlaced(Tilemap tilemap, BoundsInt area);
    }
}