using Assets.Scripts.BuildingSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

namespace Assets.Scripts.TerrainGeneration
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
                .ToDictionary(elem => elem.Name);


            for (int x = -128; x < 128; x++)
            {
                for (int y = -128; y < 128; y++)
                {
                    var position = new Vector3Int(x, y, 0);
                    //leave editor placed tiles in place
                    if (!GameManager.Instance.TilemapGround.HasTile(position))
                    {
                        //GameManager.Instance.TilemapGround.SetTile(position, _tiles["Grass"] as TileBase);
                    }


                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

