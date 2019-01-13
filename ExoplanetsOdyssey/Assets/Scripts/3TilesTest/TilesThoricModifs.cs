using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class TilesThoricModifs : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string breakSound = "event:/Minage";
    public string obtentionSound = "event:/Obtention";
    public string blocSound = "event:/PoseBloc";

    FMOD.Studio.EventInstance miningEv, obtentionEv,blocEv;
    
    public Tilemap tilemap;                                                                             //Tilemap centrale
    Tilemap[] maps;                                                                                     //Les 3 tilemaps
    int mapsIndex;                                                                                      //Index de la tilemap sur laquelle le joueur influe
    public List<TileBase> tileList;                                                                     //Les différents types de tiles présentes sur la planète
    public int currentIndex;                                                                            //Index de la tile actuellement sélectionné dans l'inventaire
    public GameObject player;                                                                           //Perso de la tilemap du centre
    public UIScript canvas;                                                                             //Script qui gère l'affichage de l'inventaire du joueur


    GameObject GM;                                                                                      //GameObject avec les scripts qui subsistent entre les scènes
    private Vector3Int memTile;                                                                         //Pour conserver la position de la tile que le joueur essaie de casser
    private float timerBreakTile;                                                                       //Pour gérer le temps que met la tile à casser (-1 si incassable)
    private Parameters p;
    private bool isPlaying;
    /*Initialisation des variables de classes*/
    private void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager");
        p = GM.GetComponent<Parameters>();
        memTile = new Vector3Int();
        maps = new Tilemap[3];
        mapsIndex = 0;
        timerBreakTile = 0;
        isPlaying = false;
    }

    /*Range les tilemaps aux bons index de maps*/
    void Start()
    {
        miningEv = FMODUnity.RuntimeManager.CreateInstance(breakSound);
        blocEv = FMODUnity.RuntimeManager.CreateInstance(blocSound);
        obtentionEv = FMODUnity.RuntimeManager.CreateInstance(obtentionSound);
        maps[0] = tilemap;
        Tilemap[] t = GameObject.FindObjectsOfType<Tilemap>();
        for (int i = 0; i < t.Length; i++)                                                              //used to sort tilemaps in the tab, 0 = center one
        {
            if (t[i].name.Equals("left"))
            {
                maps[1] = t[i];                                                                         // 1 = most left one
            }
            else if (!(t[i] == tilemap))
            {
                maps[2] = t[i];                                                                         // 2 = most right one
            }
        }

    }

    /*Gestion boutons. Casser/Placer une tile + inventaire*/
    void FixedUpdate()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);//get mouse pos on screen
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);                                    //change screen position to world position

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))                                         //choose wich tile map will be use if we click
        {
            if (worldPos.x < 0 && mapsIndex != 1)                                                       // if the click was on the left tilemap
            {
                mapsIndex = 1;                                                                          //the index must change to the left one
            }
            else if (worldPos.x > gameObject.GetComponent<TilesLevelGeneration>().worldWidth && mapsIndex != 2)//if the click was on the right tilemap
            {
                mapsIndex = 2;                                                                          //index must change
            }
            else if (mapsIndex != 0)                                                                    //else we are in the center map
            {
                mapsIndex = 0;                                                                          //index must change
            }
        }
        if (Input.GetMouseButton(0))                                                                    //Clic gauche
        {
            
            Vector3Int tilePos = maps[mapsIndex].WorldToCell(worldPos);                                 //Récupère la position dans la tilemap de la tile où est la souris
            float timeBreak = -1;                                                                       //Temps nécessaire pour casser une tile
            TileBase tile = maps[mapsIndex].GetTile(tilePos);
            if (!isPlaying)
            {
                miningEv.start();
                isPlaying = true;
            }
            if (tile)                                                                                   //S'il y a bien une tile à la position de la souris, on modifie le temps nécessaire pour la casser en fonction de son type
            {
                string nameTile = tile.name;
                switch (nameTile)
                {
                    case "ground": 
                        timeBreak = 0.2f;
                        miningEv.setParameterValue("miningIron",0);
                        miningEv.setParameterValue("miningFuel",0);
                        break;
                    
                    case "metal": 
                        timeBreak = 0.5f; 
                        miningEv.setParameterValue("miningIron",1);
                        miningEv.setParameterValue("miningFuel",0);
                        break;
                    
                    case "carburant": 
                        timeBreak = 0.8f; 
                        miningEv.setParameterValue("miningIron",0);
                        miningEv.setParameterValue("miningFuel",1);
                        break;
                    
                    default: 
                        timeBreak = -1; 
                        miningEv.setParameterValue("miningIron",0);
                        miningEv.setParameterValue("miningFuel",0);
                        break;                                                    //Si tileBreak == -1, alors on ne peux pas casser la tile
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
                obtentionEv.start();
                TileBase actual = maps[mapsIndex].GetTile(tilePos);                                         //Récupère la tile cassé
                for (int i = 0; i < tileList.Count; i++)                                                    //Parcours la liste de tile présente sur la planète pour savoir quelle action faire
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
                /* SI CASSAGE DE BLOCS POSE PROBLEME, C'EST PROBABLEMENT A CAUSE DE ÇA*/
                tilemap.SetTile(tilePos, null);                                                             //apply modif to all maps 
                maps[1].SetTile(tilePos, null);
                maps[2].SetTile(tilePos, null);
                gameObject.GetComponent<PlanetModificationsSaver>().AddModification(gameObject.GetComponent<TilesLevelGeneration>().getPlaneteSeed(), GM.GetComponent<Parameters>().planetType, -1, tilePos.x, tilePos.y);//send the modification to the modif saver
            }
        }
        else                                                                                            //Si on a relâché le clic, on reset le cassage de tile
        {
            isPlaying = false;
            miningEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            if (maps[mapsIndex].GetTile(memTile))
                maps[mapsIndex].SetColor(memTile, new Color(1, 1, 1, 1));
            timerBreakTile = 0;
            memTile = new Vector3Int();
        }
        if (Input.GetMouseButton(1))                                                                    //Clic droit
        {
            Vector3Int tilePos = maps[mapsIndex].WorldToCell(worldPos);                                 //Coordonnée écran vers coordonnée tilemap

            if (!maps[mapsIndex].GetTile(tilePos) && player.GetComponent<PlayerInventory>().tileAmounts[currentIndex] > 0 &&
                tilePos != maps[mapsIndex].WorldToCell(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z)))     //S'il n'y a pas déjà une tile à cette position, que le joueur possède au moins une tile de ce genre et qu'on essaie pas de placer un bloc sur le joueur
            {
                blocEv.start();
                tilemap.SetTile(tilePos, tileList[currentIndex]);                                       //apply modif to all maps
                maps[1].SetTile(tilePos, tileList[currentIndex]);
                maps[2].SetTile(tilePos, tileList[currentIndex]);
                int nb = --player.GetComponent<PlayerInventory>().tileAmounts[currentIndex];            //Décrémente le nombre de tile dans l'inventaire
                canvas.UpdateNbTiles(0, nb);                                                            //Update le nombre dans l'UI
                gameObject.GetComponent<PlanetModificationsSaver>().AddModification(gameObject.GetComponent<TilesLevelGeneration>().getPlaneteSeed(), GM.GetComponent<Parameters>().planetType, currentIndex, tilePos.x, tilePos.y);//send the modification to the modif saver
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))                                                        //Change la tile sélectionné dans l'inventaire du joueur (vers la gauche)
        {
            if (currentIndex != 0)
            {
                //currentIndex--;
            }

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))                                                       //Change la tile sélectionné dans l'inventaire du joueur (vers la droite)
        {
            //currentIndex = (currentIndex +1)%tileList.Count;
        }

        /*DEBUG ONLY*/
        if (Input.GetKeyDown(KeyCode.R))                                                                //Reload la planète + sauvegarde changement fait à la planète
        {
          //  GM.GetComponent<PlanetModificationsSaver>().computeChangesInFile();
          //  player.GetComponent<PlayerInventory>().computeChangesToFile();
          //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (p.deadByOxygen)
        {
            miningEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}
