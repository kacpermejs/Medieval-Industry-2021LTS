using BuildingSystem;
using Utills;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

namespace TerrainGeneration
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private SpriteAtlas SpriteAtlas;

        private Dictionary<string, IMapElement> _tiles;

        // Start is called before the first frame update
        void Start()
        {
            //Load terrain tiles
            _tiles = Resources
                .LoadAll<TileBase>("Tiles/TerrainTiles")
                .Cast<IMapElement>()
                .ToDictionary(elem => (elem as IInfo).Name);


            for (int x = -127; x < 127; x++)
            {
                for (int y = -127; y < 127; y++)
                {
                    var position = new Vector3Int(x, y, -1);
                    //leave editor placed tiles in place
                    if (!MapManager.Instance.TilemapGround.HasTile(position))
                    {
                        MapManager.Instance.TilemapGround.SetTile(position, _tiles["Grass"] as TileBase);
                    }


                }
            }
        }
    }
}

