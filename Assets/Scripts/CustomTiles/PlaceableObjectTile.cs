using Assets.Scripts.BuildingSystem;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.CustomTiles
{
    [CreateAssetMenu]
    public class PlaceableObjectTile : Tile, IMapElement
    {
        public PlaceableObject PlaceableObject => gameObject.GetComponent<PlaceableObject>();

        #region IMapElement Properties Implementation

        public string Name => PlaceableObject.name;
        public bool Walkable => PlaceableObject.Walkable;
        public bool CanWalkThrough => PlaceableObject.CanWalkThrough;
        public bool CanBuildUpon => PlaceableObject.CanBuildUpon;
        public float WalkingSpeedFactor => PlaceableObject.WalkingSpeedFactor;
        public bool IsGroundFeature => PlaceableObject.IsGroundFeature;
        public bool UseStandardRules => PlaceableObject.UseStandardRules;
        public Sprite Icon => PlaceableObject.Icon;
        public IMapElement.DestinationMapLayer Layer => PlaceableObject.Layer;


        public bool CanBePlaced(Tilemap tilemap, BoundsInt area)
        {
            return true;
        }



        public void Place(Tilemap tilemap, Vector3Int position)
        {
            BoundsInt area = new BoundsInt();

            area.size = PlaceableObject.Bounds.size;
            area.position = position - new Vector3Int(area.size.x / 2, area.size.y / 2, 0);


            var obj = PlaceableObject;

            int size = area.size.x * area.size.y * area.size.z;

            if (obj.ComponentTilesIndecies.Count != size)
                throw new Exception("Arrays don't match in size");

            TileBase[] arr = new TileBase[size];
            int index;

            for (int i = 0; i < size; i++)
            {

                index = obj.ComponentTilesIndecies[i];
                if (index == -2)
                {
                    arr[i] = this;
                }
                else if (index == -1)
                {
                    arr[i] = null;
                }
                else
                {
                    arr[i] = obj.Tiles[index];
                }
            }
            tilemap.SetTilesBlock(area, arr);
        }

        #endregion


    }
}
