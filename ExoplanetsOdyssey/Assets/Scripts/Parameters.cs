using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters : MonoBehaviour {

    public string seedBase;//seed de la map
    public static Parameters Instance;//ne pas toucher
    public int nbSystem, nbPlanete;
    public string planetType;

    private List<List<int>> seedsPlanetes;             //seed de la planète puis numero du système
    private Dictionary<int, string> typePlanete;    //associe le type de planete a chaque seed
    private int seedToGen;                          //déterminé lors de la selection d'une planète dans le vaisseau
    public int currentSystem,currentPlanet;         // déterminés au choix du systeme / planete
    private System.Random rand;                     // a utilise r

	// Use this for initialization
	void Awake () {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            seedsPlanetes = new List<List<int>>();
            int nbSeeds = nbPlanete * nbSystem;
            rand = new System.Random(seedBase.GetHashCode());
            for(int i = 0; i < nbSystem; i++)
            {
                seedsPlanetes.Add(new List<int>());
                for (int i2 = 0; i2 < nbPlanete; i2++)
                {
                    seedsPlanetes[i].Add(rand.Next(-999999999, 999999999));
                }

            }
        }
        else
        {
            Destroy(this);
        }
    }
    
    public int getSeedToGen()
    {
        return seedsPlanetes[currentSystem][currentPlanet];
    }

    public void setCurrentSystem(int i)
    {
        currentSystem = i;
    }

    public void setCurrentPlanet(int i)
    {
        currentPlanet = i;
    }

    public List<int> getAllSeedsSystem()
    {
        return seedsPlanetes[currentSystem];
    }
}
