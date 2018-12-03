﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*Script du GameManager : génère les seeds des planètes et les atribu aux différents systèmes*/
public class Parameters : MonoBehaviour {

    public string seedBase;                         //seed du jeu (temps système)
    public static Parameters Instance;              //Pour avoir un singleton
    public int nbSystem, nbPlanete;                 //Nb systèmes / Nb planètes à générer par systèmes
    public string planetType;

    private List<List<int>> seedsPlanetes;          //Stocke toutes les seeds de nos planètes, rangé par système List[indexSystem][indexPlanete] 
    private Dictionary<int, string> typePlanete;    //associe le type de planete a chaque seed
    public int currentSystem = -1;
    public string currentSystemName; //pour l'affichage final de l'utilisateur
    public int currentPlanet;         //Système sélectionné par le joueur / Planète choisi par le joueur -> Permet de retrouver la seed de la planète dans seedsPlanetes
    public string currentPlanetName; //pour l'affichage final a l'utilisateur
    private System.Random rand;                     //Le random de notre jeu (pour évènements aléatoire et génération de seeds)
    public int nbHabitable;

    public List<Choice> habitables;

    public int maxFuel;
    public int maxOxygen;
    public int maxIron;
		
	public bool firstMove;

	// Use this for initialization
	void Awake () {
        if (!Instance)                              //Structure Singleton
        {
            Instance = this;
            DontDestroyOnLoad(this);                //Conserver antre les scènes
            seedsPlanetes = new List<List<int>>();
            typePlanete = new Dictionary<int, string>();
            habitables = new List<Choice>();
            rand = new System.Random(seedBase.GetHashCode());   //Random seedé
            for(int i = 0; i < nbSystem; i++)       //Génère les seeds des planètes
            {
                seedsPlanetes.Add(new List<int>());
                for (int i2 = 0; i2 < nbPlanete; i2++)
                {
                    int seed = rand.Next(-999999999, 999999999);
                    seedsPlanetes[i].Add(seed);
                    string info = generatePlanet(seed, i, i2);
                    typePlanete.Add(seed, info);
                }
            }

            while(nbHabitable < 10)
            {
                for (int i = 0; i < nbSystem; i++)       //Génère les seeds des planètes
                {
                    for (int i2 = 0; i2 < nbPlanete; i2++)
                    {
                        int seed = seedsPlanetes[i][i2];
                        if (seed < 0)
                        {
                            int newSeed = rand.Next(-999999999, 999999999);
                            seedsPlanetes[i][i2] = newSeed;
                            string info = generatePlanet(newSeed, i, i2);
                            typePlanete.Remove(seed);
                            typePlanete.Add(newSeed, info);
                        }
                       
                    }
                }
                if (nbHabitable >= 10)
                    break;
            }
						firstMove = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    //Récupère la seed de la planète choisie par le joueur
    public int getSeedToGen()
    {
        return seedsPlanetes[currentSystem][currentPlanet];
    }

    public void setCurrentSystem(int i,string name)
    {
        currentSystem = i;
        currentSystemName = name;
    }

    public void setCurrentPlanet(int i,string name)
    {
        currentPlanet = i;
        currentPlanetName = name;
    }

    //Récupère toutes les seeds des planètes d'un système
    public List<int> getAllSeedsSystem()
    {
        return seedsPlanetes[currentSystem];
    }

    public int getRandomInt(int max)
    {
        return rand.Next(max);
    }
    public int getRandomInt(int min, int max)
    {
        return rand.Next(min, max);
    }

    public float getRandomFloat(double minimum, double maximum)
    {
        return (float)(rand.NextDouble() * (maximum - minimum) + minimum);
    }



    /**
     * Au cas ou
     **/
    public void generatePlanets()
    {
        string filePath = "";
        string planetInfo = "";

        int nbPlanets = nbSystem * nbPlanete;

        StreamWriter sw = new StreamWriter(filePath);


        sw.WriteLine(planetInfo);

        sw.Flush();
        sw.Close();
    }

    private string[] type = { "Rocheuse", "Gazeuse" };

    private string generatePlanet(int seed, int numSysteme, int numPlanet)
    {
        string info = "";

        if(seed < 0) //inhabitable
        {
            int mod = seed % 2;
            if (mod == 0)
                info += type[0];
            else
                info += type[1];
            
            info += ",";
            int temperature = seed % 300;
            if(mod == 0) // t < 0
                info += Mathf.Abs(temperature);
            else
                info += temperature;

        }
        else // habitable
        {
            ++nbHabitable;
            info += type[0];

            int temp = seed % 300;
            //normalisée = (originale - MIN) * (max - min) / (MAX - MIN) + min 
            //[MIN, MAX] : interval d'origine 
            //[min, max] : interval cible
            int temperature = (temp * 20) / 300 + 20;
            info += "," + temperature;
            habitables.Add(new Choice(numSysteme, numPlanet));
        }



        return info;
    }

    private bool isHabitable()
    {
        return false;
    }

    public Dictionary<int, string> getTypes()
    {
        return typePlanete;
    }
}
