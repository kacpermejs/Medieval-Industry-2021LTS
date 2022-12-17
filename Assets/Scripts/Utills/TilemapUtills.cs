

using UnityEngine.Tilemaps;
using UnityEngine;
using BuildingSystem;
using System;

namespace Utills
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

        public static bool CheckIfAreaEmpty(BoundsInt area, Tilemap tilemap)
        {
            var tiles = TilemapUtills.GetTilesBlock(area, tilemap);
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
            var tiles = TilemapUtills.GetTilesBlock(area2, tilemap);
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

        #endregion
    }
}
