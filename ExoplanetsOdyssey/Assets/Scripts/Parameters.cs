using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Script du GameManager : génère les seeds des planètes et les atribu aux différents systèmes*/
public class Parameters : MonoBehaviour {

    public string seedBase;                         //seed du jeu (temps système)
    public static Parameters Instance;              //Pour avoir un singleton
    public int nbSystem, nbPlanete;                 //Nb systèmes / Nb planètes à générer par systèmes
    public string planetType;

    private List<List<int>> seedsPlanetes;          //Stocke toutes les seeds de nos planètes, rangé par système List[indexSystem][indexPlanete] 
    private Dictionary<int, string> typePlanete;    //associe le type de planete a chaque seed
    public int currentSystem,currentPlanet;         //Système sélectionné par le joueur / Planète choisi par le joueur -> Permet de retrouver la seed de la planète dans seedsPlanetes
    private System.Random rand;                     //Le random de notre jeu (pour évènements aléatoire et génération de seeds)

	// Use this for initialization
	void Awake () {
        if (!Instance)                              //Structure Singleton
        {
            Instance = this;
            DontDestroyOnLoad(this);                //Conserver antre les scènes
            seedsPlanetes = new List<List<int>>();
            rand = new System.Random(seedBase.GetHashCode());   //Random seedé
            for(int i = 0; i < nbSystem; i++)       //Génère les seeds des planètes
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
    
    //Récupère la seed de la planète choisie par le joueur
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

    //Récupère toutes les seeds des planètes d'un système
    public List<int> getAllSeedsSystem()
    {
        return seedsPlanetes[currentSystem];
    }
}
