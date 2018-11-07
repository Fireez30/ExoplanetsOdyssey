using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class TilesThoricModifs : MonoBehaviour
{

    public Tilemap tilemap;
    Tilemap[] maps;
    int mapsIndex;
    public List<TileBase> tileList;
    public int currentIndex;
    public GameObject player;
    public UIScript canvas;


    GameObject GM;
    public GameObject planetManager;
    private Vector3Int memTile;
    private float timerBreakTile;

    private void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager");
        memTile = new Vector3Int();
        maps = new Tilemap[3];
        mapsIndex = 0;
        timerBreakTile = 0;
    }

    void Start()
    {
        maps[0] = tilemap;
        Tilemap[] t = GameObject.FindObjectsOfType<Tilemap>();
        for (int i = 0; i < t.Length; i++)
        {
            if (t[i].name.Equals("left"))
            {
                maps[1] = t[i];
            }
            else if (!(t[i] == tilemap))
            {
                maps[2] = t[i];
            }
        }

    }

    void FixedUpdate()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            if (worldPos.x < 0 && mapsIndex != 1)
            {
                mapsIndex = 1;
            }
            else if (worldPos.x > planetManager.GetComponent<TilesLevelGeneration>().worldWidth && mapsIndex != 2)
            {
                mapsIndex = 2;
            }
            else if (mapsIndex != 0)
            {
                mapsIndex = 0;
            }
        }
        if (Input.GetMouseButton(0))                                                                    //Clic gauche
        {
            
            Vector3Int tilePos = maps[mapsIndex].WorldToCell(worldPos);                                        //Récupère la position dans la tilemap de la tile où est la souris
            float timeBreak = -1;                                                                       //Temps nécessaire pour casser une tile
            TileBase tile = maps[mapsIndex].GetTile(tilePos);

            if (tile)                                                                                   //S'il y a bien une tile à la position de la souris, on modifie le temps nécessaire pour la casser en fonction de son type
            {
                string nameTile = tile.name;
                switch (nameTile)
                {
                    case "ground": timeBreak = 0.2f; break;
                    case "Redstuff": timeBreak = 0.5f; break;
                    case "BlueStuff": timeBreak = 0.8f; break;
                    default: timeBreak = -1; break;                                                    //Si tileBreak == -1, alors on ne peux pas casser la tile
                }
            }
            if (tilePos != memTile && timeBreak != -1)                                                    //Si la souris est plus sur la même tiles, on reset le cassage de tile
            {
                if (maps[mapsIndex].GetTile(memTile))
                    maps[mapsIndex].SetColor(memTile, new Color(1, 1, 1, 1));
                timerBreakTile = 0;
                memTile = tilePos;
            }
            else if (tilePos == memTile && timeBreak != -1 && timerBreakTile < timeBreak)                  //Si la souris est sur la même tile mais pas depuis assez longtemps pour la casser, on change l'opacité de la tile
            {
                timerBreakTile += Time.fixedDeltaTime;
                maps[mapsIndex].SetColor(tilePos, new Color(1, 1, 1, 1 - (timerBreakTile) / timeBreak));
            }
            else if (timerBreakTile >= timeBreak && tilePos == memTile && timeBreak != -1)                 //Si on est toujours sur la même tile et qu'on l'a cassé
            {
                TileBase actual = maps[mapsIndex].GetTile(tilePos);
                for (int i = 0; i < tileList.Count; i++)
                    if (tileList[i].Equals(actual))
                    {
                        if (tileList[i].Equals(actual))
                        {
                            if (i == 1)
                            {
                                int nb = ++player.GetComponent<PlayerInventory>().fuelAmount;
                                canvas.UpdateNbTiles(1, nb);
                            }
                            else if (i == 2)
                            {
                                int nb = ++player.GetComponent<PlayerInventory>().ironAmount;
                                canvas.UpdateNbTiles(2, nb);
                            }
                            else
                            {
                                int nb = ++player.GetComponent<PlayerInventory>().tileAmounts[i];
                                canvas.UpdateNbTiles(0, nb);
                            }
                        }
                    }

                tilemap.SetTile(tilePos, null);
                maps[1].SetTile(tilePos, null);
                maps[2].SetTile(tilePos, null);
                GM.GetComponent<PlanetModificationsSaver>().AddModification(planetManager.GetComponent<TilesLevelGeneration>().getPlaneteSeed(), GM.GetComponent<Parameters>().planetType, -1, tilePos.x, tilePos.y);
            }
        }
        else                                                                                            //Si on a relâché le clic, on reset le cassage de tile
        {
            if (maps[mapsIndex].GetTile(memTile))
                maps[mapsIndex].SetColor(memTile, new Color(1, 1, 1, 1));
            timerBreakTile = 0;
            memTile = new Vector3Int();
        }
        if (Input.GetMouseButton(1))                                                                    //Clic droit
        {
            Vector3Int tilePos = maps[mapsIndex].WorldToCell(worldPos);

            if (!maps[mapsIndex].GetTile(tilePos) && player.GetComponent<PlayerInventory>().tileAmounts[currentIndex] > 0 && tilePos != maps[mapsIndex].WorldToCell(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z)))
            {
                tilemap.SetTile(tilePos, tileList[currentIndex]);
                maps[1].SetTile(tilePos, tileList[currentIndex]);
                maps[2].SetTile(tilePos, tileList[currentIndex]);
                int nb = --player.GetComponent<PlayerInventory>().tileAmounts[currentIndex];
                canvas.UpdateNbTiles(0, nb);
                GM.GetComponent<PlanetModificationsSaver>().AddModification(planetManager.GetComponent<TilesLevelGeneration>().getPlaneteSeed(), GM.GetComponent<Parameters>().planetType, currentIndex, tilePos.x, tilePos.y);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentIndex != 0)
            {
                currentIndex--;
            }
            else
                currentIndex = tileList.Count - 1;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentIndex < tileList.Count - 1)
            {
                currentIndex++;
            }
            else
            {
                currentIndex = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GM.GetComponent<PlanetModificationsSaver>().computeChangesInFile();
            player.GetComponent<PlayerInventory>().computeChangesToFile();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
