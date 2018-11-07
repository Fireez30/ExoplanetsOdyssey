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
        for (int i = 0; i < t.Length; i++)//used to sort tilemaps in the tab, 0 = center one
        {
            if (t[i].name.Equals("left"))
            {
                maps[1] = t[i];// 1 = most left one
            }
            else if (!(t[i] == tilemap))
            {
                maps[2] = t[i];// 2 = most right one
            }
        }

    }

    void FixedUpdate()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);//get mouse pos on screen
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);//change screen position to world position

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))//choose wich tile map will be use if we click
        {
            if (worldPos.x < 0 && mapsIndex != 1)// if the click was on the left tilemap
            {
                mapsIndex = 1;//the index must change to the left one
            }
            else if (worldPos.x > planetManager.GetComponent<TilesLevelGeneration>().worldWidth && mapsIndex != 2)//if the click was on the right tilemap
            {
                mapsIndex = 2;//index must change
            }
            else if (mapsIndex != 0)//else we are in the center map
            {
                mapsIndex = 0;//index must change
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

                tilemap.SetTile(tilePos, null);//apply modif to all maps 
                maps[1].SetTile(tilePos, null);
                maps[2].SetTile(tilePos, null);
                GM.GetComponent<PlanetModificationsSaver>().AddModification(planetManager.GetComponent<TilesLevelGeneration>().getPlaneteSeed(), GM.GetComponent<Parameters>().planetType, -1, tilePos.x, tilePos.y);//send the modification to the modif saver
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
                tilemap.SetTile(tilePos, tileList[currentIndex]);//apply modif to all maps
                maps[1].SetTile(tilePos, tileList[currentIndex]);
                maps[2].SetTile(tilePos, tileList[currentIndex]);
                int nb = --player.GetComponent<PlayerInventory>().tileAmounts[currentIndex];
                canvas.UpdateNbTiles(0, nb);
                GM.GetComponent<PlanetModificationsSaver>().AddModification(planetManager.GetComponent<TilesLevelGeneration>().getPlaneteSeed(), GM.GetComponent<Parameters>().planetType, currentIndex, tilePos.x, tilePos.y);//send the modification to the modif saver
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))//must change, use a player inventory current tile 
        {
            if (currentIndex != 0)
            {
                currentIndex--;
            }
            else
                currentIndex = tileList.Count - 1;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))//must change, use a player inventory current tile 
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

        if (Input.GetKeyDown(KeyCode.R))//debug only, remove on release
        {
            GM.GetComponent<PlanetModificationsSaver>().computeChangesInFile();
            player.GetComponent<PlayerInventory>().computeChangesToFile();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
