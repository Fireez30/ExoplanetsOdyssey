using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* GENERE ET POSITIONNE LES PLANETES DANS LA SCENE DU VAISSEAU */
public class shipInteraction : MonoBehaviour {

    public GameObject planetPrefab;
    public int xMin, xMax, y;
    public List<Sprite> allSprites;
    public GameObject gazeuseWindows;
    public RefreshShipLeds refreshLeds;

    private List<int> seedPlanets;
    private Parameters param;

	// Use this for initialization
	void Awake () {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (gameManager != null)
        {
            param = gameManager.GetComponent<Parameters>();
            if (param != null)
            {
                seedPlanets = param.getAllSeedsSystem();                                                        //Recupere les seeds des planetes du systèmes
                for (int i = 0; i < seedPlanets.Count; i++)                                                           //Positionne chaque planètes sur le tableau de bord
                {
                    Vector3 pos = new Vector3(xMin + (i / (float)seedPlanets.Count) * (xMax - xMin), y, 0);
                    GameObject temp = Instantiate(planetPrefab, pos, Quaternion.identity);
                    temp.GetComponent<Spaceship_Planet>().setIndexPlanet(i);                                    //Pour que chaque planète connaise son index au sein du système
                    temp.GetComponent<Spaceship_Planet>().setSeed(seedPlanets[i]);
                    temp.GetComponent<SpriteRenderer>().sprite = allSprites[Random.Range(0, allSprites.Count)];
                }
            }
            if (param.comeFromGazeuse)
            {
                param.comeFromGazeuse = false;
                gazeuseWindows.SetActive(true);
                ShipInventory s = param.gameObject.GetComponent<ShipInventory>();
                var de = param.getRandomInt(0, 3);
                if (de == 0 && s.GetScannerState() == 1)
                {
                    s.SetScannerState(0);
                }
                else if (de == 1 && s.GetFuelTankState() == 1)
                {
                    s.SetFuelTankState(0);
                }
                else if (de == 2 && s.GetOxygenTankState() == 1)
                {
                    s.SetOxygenTankState(0);
                }
            }
        }
        refreshLeds.Refresh();
    }













}
