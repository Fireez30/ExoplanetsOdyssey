using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class TileModif : MonoBehaviour {

    public Tilemap tilemap;
    public List<TileBase> tileList;
    public int currentIndex;

    public GameObject player;

    GameObject GM;
    private Vector3Int memTile;
    private float timerBreakTile;

    private void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager");
        memTile = new Vector3Int();
        timerBreakTile = 0;
    }


    void FixedUpdate () {

        if (Input.GetMouseButton(0))                                                                    //Clic gauche
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3Int tilePos = tilemap.WorldToCell(screenPos);                                        //Récupère la position dans la tilemap de la tile où est la souris
            float timeBreak = -1;                                                                       //Temps nécessaire pour casser une tile
            TileBase tile = tilemap.GetTile(tilePos);

            if (!tile.Equals(tileList[3]))
            {
                if (tile)                                                                                   //S'il y a bien une tile à la position de la souris, on modifie le temps nécessaire pour la casser en fonction de son type
                {
                    string nameTile = tile.name;
                    switch (nameTile)
                    {
                        case "ground": timeBreak = 0.4f; break;
                        case "Redstuff": timeBreak = 1f; break;
                        case "BlueStuff": timeBreak = 1.5f; break;
                        default: timeBreak = -1; break;                                                    //Si tileBreak == -1, alors on ne peux pas casser la tile
                    }
                }
                if (tilePos != memTile && timeBreak != -1)                                                    //Si la souris est plus sur la même tiles, on reset le cassage de tile
                {
                    if (tilemap.GetTile(memTile))
                        tilemap.SetColor(memTile, new Color(1, 1, 1, 1));
                    timerBreakTile = 0;
                    memTile = tilePos;
                }
                else if (tilePos == memTile && timeBreak != -1 && timerBreakTile < timeBreak)                  //Si la souris est sur la même tile mais pas depuis assez longtemps pour la casser, on change l'opacité de la tile
                {
                    timerBreakTile += Time.fixedDeltaTime;
                    tilemap.SetColor(tilePos, new Color(1, 1, 1, 1 - (timerBreakTile) / timeBreak));
                }
                else if (timerBreakTile >= timeBreak && tilePos == memTile && timeBreak != -1)                 //Si on est toujours sur la même tile et qu'on l'a cassé
                {
                    TileBase actual = tilemap.GetTile(tilePos);
                    for (int i = 0; i < tileList.Count; i++)
                        if (tileList[i].Equals(actual))
                        {
                            if (i == 1)
                                player.GetComponent<PlayerInventory>().fuelAmount++;
                            else if (i == 2)
                                player.GetComponent<PlayerInventory>().ironAmount++;
                            else
                                player.GetComponent<PlayerInventory>().tileAmounts[i]++;
                        }

                    tilemap.SetTile(tilePos, null);
                    GM.GetComponent<PlanetModificationsSaver>().AddModification(GM.GetComponent<LevelGeneration>().worldseed, GM.GetComponent<Parameters>().planetType, -1, tilePos.x, tilePos.y);
                }
            }
        }
        else                                                                                            //Si on a relâché le clic, on reset le cassage de tile
        {
            if (tilemap.GetTile(memTile))
                tilemap.SetColor(memTile, new Color(1, 1, 1, 1));
            timerBreakTile = 0;
            memTile = new Vector3Int();
        }
        if (Input.GetMouseButton(1))                                                                    //Clic droit
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3Int tilePos = tilemap.WorldToCell(screenPos);

            if (!tilemap.GetTile(tilePos) && player.GetComponent<PlayerInventory>().tileAmounts[currentIndex] > 0 && tilePos != tilemap.WorldToCell(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z)))
            {
                tilemap.SetTile(tilePos, tileList[currentIndex]);
                player.GetComponent<PlayerInventory>().tileAmounts[currentIndex]--;
                GM.GetComponent<PlanetModificationsSaver>().AddModification(GM.GetComponent<LevelGeneration>().worldseed, GM.GetComponent<Parameters>().planetType, currentIndex, tilePos.x, tilePos.y);
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
