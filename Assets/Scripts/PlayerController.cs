using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    public Tilemap playerMap;
    public Tilemap tm;
    public TileBase playerTile;
    public Vector2Int playerPos;

    public tileBehaviorManager tileManager;
    public ProceduralGenerator pg;

    public int orientation;

    public HealthController hc;

    public LevelHandler lh;

    public GameObject playerEffects;

    private GameObject playerEffectsInstance;

    float playerFallTimer = 0;
    [SerializeField] float playerFallDelay = 0.2f;

    [SerializeField] private int frameRate = 30; // don't touch

    // Start is called before the first frame update
    void Start()
    {
        playerPos = new Vector2Int(0, 1);
        playerEffectsInstance = Instantiate(playerEffects, new Vector3(playerPos.x, playerPos.y), Quaternion.identity);
        playerMap.SetTile(new Vector3Int(playerPos.x, playerPos.y, 0), playerTile);
    }

    // Update is called once per frame
    void Update()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;

        int horInput = 0;
        if (Input.GetKeyDown(KeyCode.A))
        {
            horInput = -1;
            orientation = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            horInput = 1;
            orientation = 1;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            orientation = 0;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            
            if (tm.GetTile(new Vector3Int(playerPos.x, playerPos.y-1))){
                playerMap.SetTile(new Vector3Int(playerPos.x, playerPos.y, 0), null);
                playerMap.SetTile(new Vector3Int(playerPos.x, playerPos.y + 1, 0), playerTile);
                playerPos.y += 1;
                playerFallTimer = Time.time + playerFallDelay;
            }
           

            
        }

        if (Input.GetMouseButtonDown(0))
        {

            if (orientation == 0)
            {
                //playerPos.y -= 1;

                if (tm.GetTile(new Vector3Int(playerPos.x, playerPos.y)) == pg.healthTile)
                {
                    return;
                }

                if (playerPos.y > -pg.height - 2)
                {
                    hc.decHealth();
                    tileManager.cascade(new Vector3Int(playerPos.x, playerPos.y - 1), tm.GetTile(new Vector3Int(playerPos.x, playerPos.y - 1)));
                    tm.SetTile(new Vector3Int(playerPos.x, playerPos.y - 1, 0), null);

                }

            }
            else
            {
                if (tm.GetTile(new Vector3Int(playerPos.x + orientation, playerPos.y)) != null && tm.GetTile(new Vector3Int(playerPos.x + orientation, playerPos.y)) != pg.healthTile)
                {
                    hc.decHealth();
                    tileManager.cascade(new Vector3Int(playerPos.x + orientation, playerPos.y), tm.GetTile(new Vector3Int(playerPos.x + orientation, playerPos.y)));
                    tm.SetTile(new Vector3Int(playerPos.x + orientation, playerPos.y, 0), null);


                }

            }

        }

        if (Mathf.Abs(horInput) > 0)
        {
            if (!tm.GetTile(new Vector3Int(playerPos.x + horInput, playerPos.y, 0)) || tm.GetTile(new Vector3Int(playerPos.x + horInput, playerPos.y, 0)) == pg.healthTile || tm.GetTile(new Vector3Int(playerPos.x + horInput, playerPos.y, 0)) == pg.emptyTile)
            {
                playerPos.x += (int)horInput;
                if (playerPos.x >= 0 && playerPos.x < pg.width && playerPos.y <= 1 && playerPos.y >= -pg.height)
                {
                    playerMap.SetTile(new Vector3Int(playerPos.x - (int)horInput, playerPos.y, 0), null);
                    playerMap.SetTile(new Vector3Int(playerPos.x, playerPos.y, 0), playerTile);

                    //checkFall();
                }
                else
                {
                    playerPos.x -= (int)horInput;
                }


            }

        }


        heal();
        if (playerPos.y <= -pg.height + 2)
        {
            lh.winUI();
        }

        if (tm.GetTile(new Vector3Int(playerPos.x, playerPos.y, 0)) && tm.GetTile(new Vector3Int(playerPos.x, playerPos.y, 0)) != pg.healthTile && tm.GetTile(new Vector3Int(playerPos.x, playerPos.y, 0)) != pg.emptyTile)
        {
            hc.endGame();
        }

        checkFall();

        playerEffectsInstance.transform.position = new Vector3(playerPos.x + 0.5f, playerPos.y + 0.5f, playerEffects.transform.position.z);


    }

    public void checkFall()
    {
        int counter = 0;
        while (tm.GetTile(new Vector3Int(playerPos.x, playerPos.y - 1)) == null || tm.GetTile(new Vector3Int(playerPos.x, playerPos.y - 1)) == pg.healthTile)
        {
            
            if (Time.time >= playerFallTimer)
            {
                playerPos.y -= 1;
                playerMap.SetTile(new Vector3Int(playerPos.x, playerPos.y + 1, 0), null);
                playerMap.SetTile(new Vector3Int(playerPos.x, playerPos.y, 0), playerTile);

                playerFallTimer = Time.time + playerFallDelay;
            }


            heal();
            counter++;
            if (counter >= 60)
            {
                return;
            }
        }

    }

    void heal()
    {
        if (tm.GetTile(new Vector3Int(playerPos.x, playerPos.y, 0)) == pg.healthTile)
        {
            hc.incHealth();
            tm.SetTile(new Vector3Int(playerPos.x, playerPos.y), pg.emptyTile);
        }
    }
}
