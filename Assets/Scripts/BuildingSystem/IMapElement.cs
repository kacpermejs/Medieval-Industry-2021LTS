using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.BuildingSystem
{
    public enum DestinationMapLayer
    {
        Surface,
        Ground,
        Markers
    }

    public interface IMapElement
    {
        bool Walkable { get; }
        bool CanBuildUpon { get; }
        bool CanWalkThrough { get; }
        float WalkingSpeedFactor { get; }
        bool UseStandardRules { get; }
        DestinationMapLayer Layer { get; }
        void Place(Tilemap tilemap, Vector3Int position);
        bool CanBePlaced(Tilemap tilemap, BoundsInt area);
    }
}