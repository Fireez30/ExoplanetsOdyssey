using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

/*Script du GameManager : génère les seeds des planètes et les atribu aux différents systèmes*/
public class Parameters : MonoBehaviour {

    public string seedBase;                         //seed du jeu (temps système)
    public static Parameters Instance;              //Pour avoir un singleton
    public int nbSystem, nbPlanete;                 //Nb systèmes / Nb planètes à générer par systèmes
    public string planetType;
    public int nbHabitable;
    public int nbExtraHabitable;
    private List<List<int>> seedsPlanetes;          //Stocke toutes les seeds de nos planètes, rangé par système List[indexSystem][indexPlanete] 
    private Dictionary<int, string> typePlanete;    //associe le type de planete a chaque seed
    public int currentSystem = -1;
    public string currentSystemName; //pour l'affichage final de l'utilisateur
    public int currentPlanet;         //Système sélectionné par le joueur / Planète choisi par le joueur -> Permet de retrouver la seed de la planète dans seedsPlanetes
    public string currentPlanetName; //pour l'affichage final a l'utilisateur
    private System.Random rand;                     //Le random de notre jeu (pour évènements aléatoire et génération de seeds)
    public bool windowsOpened;          //Si une fenetre est ouverte  ne peut pas cliquer sur une planete
    public List<Choice> habitables;
    public List<List<int>> planetSpritesIndexes;
    public bool shipSceneFirstVisit; //set this to true when visited ship scene for the first time ! 
    public bool universeSceneFirstVisit;
    public int maxFuel;
    public int maxOxygen;
    public int maxIron;

    public bool comeFromGazeuse;
	public bool firstMove;
    public bool tutorial;
    public bool deadByOxygen;

	// Use this for initialization
	public void LaunchGame () {
        if (!Instance)                              //Structure Singleton
        {
            Instance = this;
            DontDestroyOnLoad(this);                //Conserver antre les scènes
            int seedWorld;
            if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/seed.save"))
            {
                seedWorld = System.DateTime.Now.GetHashCode();
                System.IO.File.Create(Application.streamingAssetsPath + "/saves/seed.save").Close();
                System.IO.File.WriteAllText(Application.streamingAssetsPath + "/saves/seed.save", seedWorld + "");
            }
            else
            {
                seedWorld = Int32.Parse(System.IO.File.ReadAllText(Application.streamingAssetsPath + "/saves/seed.save"));
            }
            seedsPlanetes = new List<List<int>>();
            planetSpritesIndexes = new List<List<int>>();
            shipSceneFirstVisit = true;
            universeSceneFirstVisit = true;
            deadByOxygen = false;
            windowsOpened = false;
            nbExtraHabitable = nbHabitable;
            typePlanete = new Dictionary<int, string>();
            habitables = new List<Choice>();
            rand = new System.Random(seedWorld.GetHashCode());   //Random seedé
            for(int i = 0; i < nbSystem; i++)       //Génère les seeds des planètes
            {
                planetSpritesIndexes.Add(new List<int>());
                seedsPlanetes.Add(new List<int>());
                for (int i2 = 0; i2 < nbPlanete; i2++)
                {
                    planetSpritesIndexes[i].Add(-1);
                    int seed = rand.Next(-999999999, 999999999);
                    seedsPlanetes[i].Add(seed);
                    string info = generatePlanet(seed, i, i2);
                    typePlanete.Add(seed, info);
                }
            }

            while(habitables.Count < nbHabitable + nbExtraHabitable)
            {
                int sys = rand.Next(0, nbSystem); //prochain system test
                int pla = rand.Next(0, nbPlanete);//prochaine planet test
                if (testHabitabilite(typePlanete[seedsPlanetes[sys][pla]]) == false)
                {
                    int s = seedsPlanetes[sys][pla];
                    string i = genPlaneteHabitable(s);
                    typePlanete[s] = i;
                    habitables.Add(new Choice(sys,pla));
                }
            }
			firstMove = true;
            tutorial = false;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public string getInfosActual()
    {
        return typePlanete[seedsPlanetes[currentSystem][currentPlanet]];
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
        windowsOpened = true;
        SceneManager.LoadScene(2);
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
        System.Random planetRand = new System.Random(seed.GetHashCode());
        string info = "";
        var testType = planetRand.Next(0, 2);//Gen type planètes
        if (testType == 0) //inhabitable 0
            info += type[0]+",";
        else
            info += type[1]+",";

        var temp = planetRand.Next(-270,301); //temperature 1
        info += temp+",";

         var masse = planetRand.Next(1,10);
         int r = planetRand.Next(1, 4);
         string puissance;
         if (r == 1)
             puissance = "*10^20";
         else if (r == 2)
             puissance = "*10^24";//habitable si > 2.5x10^24 et < 1x10^28
         else
             puissance = "*10^28";

         info +=  masse + puissance; //push la masse 2

         var a = planetRand.Next(0, 2);
         info += (a == 0) ? ",présence d'atmosphère" : ", pas d'atmosphère";

         var prox = planetRand.Next(0,4);
         if (prox == 0)
             info += ",environnement proche calme";
         else if (prox == 1)
             info += ", amas globulaire proche";
         else if (prox == 2)
             info += ", forte présence de rayons gamma";
         else
             info += ", trou noir à proximité";

         if (habitables.Count < (nbHabitable + nbExtraHabitable)  && testHabitabilite(info)) // et test habitabilité
         {
            habitables.Add(new Choice(numSysteme, numPlanet));
         }
       
        return info;
    }

    public string genPlaneteHabitable(int seed)
    {
        System.Random pRand = new System.Random(seed.GetHashCode());
        string infos = "";

        infos += "Rocheuse,";
        infos += pRand.Next(0, 80)+",";
        var masse = pRand.Next(3, 10);
        infos += masse + "*10^24,";
        infos += "présence d'atmosphère,";
        infos += "environnement proche calme";

        return infos;
    }
    
    public bool testHabitabilite(string infos)
    {
        string[] criteres = infos.Split(',');
        bool flag = true;
        if (criteres[0] == "Gazeuse")
            flag = false;
        
        var temp = int.Parse(criteres[1]);
        if (temp > 80 || temp < 0)
            flag = false;

        var masse = criteres[2];
        if (masse[0] < 2.5)
            flag = false;
        else if (masse.Remove(0, 1) != "*10^24")
            flag = false;

        var atm = criteres[3];
        if (atm == " pas d'atmosphère")
            flag = false;

        if (!criteres[4].Contains("calme"))
            flag = false;
        
        //ajouter test présence proche si utilisé
        return flag;
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
