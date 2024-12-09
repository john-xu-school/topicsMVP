using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public Tilemap tm;

    public TileBase[] allTileType;

    public TileBase healthTile;
    public TileBase emptyTile;
    public Dictionary<Vector3, Vector3> healthEffectPos;

    public GameObject healEffect;


    private int[,] mapOfType;

    public float spawnRate = 0.01f;

    

    // Start is called before the first frame update
    void Start()
    {
        mapOfType = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                mapOfType[i, j] = Random.Range(0, allTileType.Length);
                tm.SetTile(new Vector3Int(i,-j), allTileType[mapOfType[i,j]]);
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float prob = Random.Range(0f, 1f);
                if (prob > 1-spawnRate)
                {
                    //healthPos.Add(new Vector2Int(i, -j));
                    tm.SetTile(new Vector3Int(i, -j), healthTile);
                    Instantiate(healEffect, new Vector3(i + 0.5f, -j + 0.5f, 0), Quaternion.identity);
                    //healthEffectPos.Add(new Vector3Int(i, -j), new Vector3(i + 0.5f, -j + 0.5f, 0));
                    //tm.SetTile(new Vector3Int(i, -j), emptyTile);
                }
                
            }
        }
    }
}
