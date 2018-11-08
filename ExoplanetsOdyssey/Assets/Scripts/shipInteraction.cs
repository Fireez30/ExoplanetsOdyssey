using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* GENERE ET POSITIONNE LES PLANETES DANS LA SCENE DU VAISSEAU */
public class shipInteraction : MonoBehaviour {

    public GameObject planetPrefab;
    public int xMin, xMax;

    private List<int> seedPlanets;
    private Parameters param;

	// Use this for initialization
	void Awake () {
        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        seedPlanets = param.getAllSeedsSystem();                                                        //Recupere les seeds des planetes du systèmes
        for(int i =0;i<seedPlanets.Count;i++)                                                           //Positionne chaque planètes sur le tableau de bord
        {
            Vector3 pos = new Vector3(xMin + (i / (float)seedPlanets.Count) * (xMax - xMin), 2, 0);
            GameObject temp = Instantiate(planetPrefab, pos, Quaternion.identity);
            temp.GetComponent<Spaceship_Planet>().setIndexPlanet(i);                                    //Pour que chaque planète connaise son index au sein du système
        }
    }
}
