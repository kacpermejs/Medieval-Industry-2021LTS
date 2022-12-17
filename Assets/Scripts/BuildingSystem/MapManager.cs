using System;
using UnityEngine;
using Utills;
using UnityEngine.Tilemaps;

namespace BuildingSystem
{
    public class MapManager : SingletoneBase<MapManager>
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
    }
}
