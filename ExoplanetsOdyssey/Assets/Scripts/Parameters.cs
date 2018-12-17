using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

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

    public bool shipSceneFirstVisit; //set this to true when visited ship scene for the first time ! 
    public bool universeSceneFirstVisit;
    public int maxFuel;
    public int maxOxygen;
    public int maxIron;

    public bool comeFromGazeuse;
	public bool firstMove;

	// Use this for initialization
	void Awake () {
        if (!Instance)                              //Structure Singleton
        {
            Instance = this;
            DontDestroyOnLoad(this);                //Conserver antre les scènes
            seedsPlanetes = new List<List<int>>();
            shipSceneFirstVisit = true;
            universeSceneFirstVisit = true;
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

    public void ToGazeuse()
    {
        comeFromGazeuse = true;
        SceneManager.LoadScene(1);
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

            int masse = Mathf.Abs(seed % 8000);
            int r = rand.Next(1, 3);
            string puissance;
            if (r == 1)
                puissance = "*10^20";
            else if (r == 2)
                puissance = "*10^24";
            else
                puissance = "*10^28";

            info += "," + masse / 1000.0f + puissance;
            info += seed % 2 == 0 ? ",présence d'atmosphère" : ", pas d'atmosphère";

            int prox = (seed / 10000) % 4;
            if (prox == 0)
                info += ",environnement proche calme";
            else if (prox == 1)
                info += ", amas globulaire proche";
            else if (prox == 2)
                info += ", forte présence de rayons gamma";
            else
                info += ", trou noir à proximité";
        }
        else // habitable
        {
            ++nbHabitable;
            info += type[0];

            int temp = seed % 300;
            //normalisée = (originale - MIN) * (max - min) / (MAX - MIN) + min 
            //[MIN, MAX] : interval d'origine 
            //[min, max] : interval cible
            //entre 20 et 40
            int temperature = (temp * 20) / 300 + 20;
            info += "," + temperature;

            int masse = seed % 8000;
            string puissance = "*10^24";

            info += "," + masse / 1000.0f + puissance;

            info += ",présence d'atmosphère";
            info += ", environnement proche calme";
            habitables.Add(new Choice(numSysteme, numPlanet));
        }
        return info;
    }

    public bool isCurrentHabitable()
    {
        return habitables.Contains(new Choice(currentSystem,currentPlanet));
    }

    public Dictionary<int, string> getTypes()
    {
        return typePlanete;
    }

    public int getSeed(int numSystem, int numPlanet)
    {
        return seedsPlanetes[numSystem][numPlanet];
    }

    public string getInfo(int seed)
    {
        string info;
        if (!typePlanete.TryGetValue(seed, out info))
            return null;
        return info;
    }
}
