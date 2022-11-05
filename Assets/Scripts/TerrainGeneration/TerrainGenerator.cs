using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

namespace Assets.Scripts.TerrainGeneration
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private SpriteAtlas SpriteAtlas;

        private TileBase[] _tiles;

        // Start is called before the first frame update
        void Start()
        {
            //Load terrain tiles
            _tiles = Resources.LoadAll<TileBase>("Tiles/TerrainTiles");


            for (int x = -20; x < 20; x++)
            {
                for (int y = -20; y < 20; y++)
                {
                    var position = new Vector3Int(x, y, 0);
                    //leave editor placed tiles in place
                    if (!GameManager.Instance.TilemapGround.HasTile(position))
                    {
                        GameManager.Instance.TilemapGround.SetTile(position, _tiles[0]);
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

