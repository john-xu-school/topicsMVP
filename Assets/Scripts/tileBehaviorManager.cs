using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tileBehaviorManager : MonoBehaviour
{
    public Tilemap tm;
    public ProceduralGenerator pg;
    public bool[,] visited;

    private int rows;
    private int cols;

    public Vector3Int startingPos;

    public TileBase test;

    public PlayerController pc;

    public float tileFallDelay = 0.4f;

    public float blockShakeDelay = 3f;

    public List<Vector3Int> shouldFallBlocks;

    public Tilemap shakingMap;

    public GameObject tileDestroyEffect;



    // Start is called before the first frame update
    void Start()
    {
        rows = pg.width;
        cols = pg.height;
        visited = new bool[rows, cols];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cascade(Vector3Int pos, TileBase tileBase)
    {
        visited = new bool[rows, cols];
        startingPos = pos;

        Instantiate(tileDestroyEffect, startingPos, Quaternion.identity);
        
        List<(int r, int c)> toDelete = findConnected(pos, tileBase, visited);

        Debug.Log(toDelete.Count);

        for (int i = 0; i < toDelete.Count; i++)
        {
            if (new Vector3Int(toDelete[i].r, toDelete[i].c) == startingPos) continue;
            tm.SetTile(new Vector3Int(toDelete[i].r, toDelete[i].c), null);
            Instantiate(tileDestroyEffect, new Vector3Int(toDelete[i].r, toDelete[i].c), Quaternion.identity);
            pc.checkFall();
        }
        checkBlockFall(toDelete);
        StartCoroutine(fallBehavior());

    }

    public List<(int r, int c)> findConnected(Vector3Int pos, TileBase tileBase, bool[,] visited)
    {

        int r = pos.x;
        int c = pos.y;

        if (r < 0 || r >= rows || c >= 1 || c < -cols || visited[r, -c] || tm.GetTile(startingPos) != tileBase || tm.GetTile(startingPos) == pg.healthTile)
        {
            return new List<(int r, int c)>();
        }

        // Mark the tile as visited
        visited[r, -c] = true;
        var connectedTiles = new List<(int r, int c)> { (r, c) };

        // Directions for adjacent tiles (up, down, left, right)
        var directions = new (int dr, int dc)[]
        {
            (-1, 0), // up
            (1, 0),  // down
            (0, -1), // left
            (0, 1)   // right
        };

        // Check adjacent tiles
        foreach (var (dr, dc) in directions)
        {
            var newTiles = findConnected(new Vector3Int(r + dr, c + dc), tm.GetTile(new Vector3Int(r + dr, c + dc)), visited);
            connectedTiles.AddRange(newTiles);
        }
 
        return connectedTiles;
    }

    void checkBlockFall(List<(int r, int c)> toDelete)
    {
        
        for (int i = 0; i < toDelete.Count; i++)
        {
            shouldFallBlocks.Add(new Vector3Int(toDelete[i].r, toDelete[i].c + 1));
            for (int j = toDelete[i].c + 1; j < 0; j++)
            {
                shouldFallBlocks.Add(new Vector3Int(toDelete[i].r, toDelete[i].c + j));
            }
        }

    }

    IEnumerator fallBehavior()
    {
        for (int i = 0; i < shouldFallBlocks.Count; i++)
        {
            int r = shouldFallBlocks[i].x;
            int c = shouldFallBlocks[i].y;

            if (shouldFallBlocks[i] == startingPos) continue;

            TileBase block = tm.GetTile(new Vector3Int(r, c));

            if (tm.GetTile(new Vector3Int(r, c - 1)) == null && c >= -47 && tm.GetTile(new Vector3Int(r + 1, c)) != block && tm.GetTile(new Vector3Int(r - 1, c)) != block) {
                shakingMap.SetTile(new Vector3Int(r, c), tm.GetTile(new Vector3Int(r, c)));
            }
        }
        yield return new WaitForSeconds(blockShakeDelay);
        shakingMap.ClearAllTiles();
        
        for (int i = 0; i < shouldFallBlocks.Count; i++)
        {
            int r = shouldFallBlocks[i].x;
            int c = shouldFallBlocks[i].y;

            TileBase block = tm.GetTile(new Vector3Int(r, c));

            if(tm.GetTile(new Vector3Int(r, c - 1)) == null && c >= -47 && tm.GetTile(new Vector3Int(r + 1, c)) != block && tm.GetTile(new Vector3Int(r - 1, c)) != block)
            {
                tm.SetTile(new Vector3Int(r, c, 0), null);
                StartCoroutine(fallAnim(new Vector3Int(r, c), block));
                shouldFallBlocks.Add(new Vector3Int(r, c+1));
            }
        }

        shouldFallBlocks.Clear();
        
        
    }

    


    IEnumerator fallAnim(Vector3Int pos, TileBase tb)
    {
        int r = pos.x;
        int c = pos.y;
        while (tm.GetTile(new Vector3Int(r, c - 1)) == null && c >= -47 && tm.GetTile(new Vector3Int(r + 1, c)) != tb && tm.GetTile(new Vector3Int(r - 1, c)) != tb)
        {
            tm.SetTile(new Vector3Int(r, c, 0), null);
            tm.SetTile(new Vector3Int(r, c - 1, 0), tb);
            yield return new WaitForSeconds(tileFallDelay);
            c -= 1;
        }
        

        

    }

}
