

using UnityEngine.Tilemaps;
using UnityEngine;

namespace Assets.Scripts.Utills
{
    public static class TilemapUtills
    {
        #region Tilemap utility extension
        public static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
        {
            TileBase[] arr = new TileBase[area.size.x * area.size.y * area.size.z];
            int counter = 0;

            foreach (var v in area.allPositionsWithin)
            {
                Vector3Int pos = new Vector3Int(v.x, v.y, v.z);
                arr[counter] = tilemap.GetTile(pos);
                counter++;
            }
            return arr;
        }

        public static void SetTilesBlock(BoundsInt area, TileBase tile, Tilemap tilemap)
        {
            int size = area.size.x * area.size.y * area.size.z;
            TileBase[] arr = new TileBase[size];
            FillArrayWithTiles(ref arr, tile);
            tilemap.SetTilesBlock(area, arr);


        }

        public static void FillArrayWithTiles(ref TileBase[] arr, TileBase tile)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = tile;
            }
        }

        #endregion
    }
}
