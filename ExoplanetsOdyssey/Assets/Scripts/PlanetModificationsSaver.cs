using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlanetModificationsSaver : MonoBehaviour {

    public Dictionary<float,List<TileChange>> visitedPlanets;
    float actualSeed;
	// Use this for initialization

	void Awake () {
        visitedPlanets = new Dictionary<float, List<TileChange>>();
        actualSeed = -1;
	}

     void Start()
    {
        actualSeed = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>().actualPlanet;
    }
    void FixedUpdate()
    {
        foreach (float key in visitedPlanets.Keys)
        {
            Debug.Log("planete : " + key + " nombre d'elements : " + visitedPlanets[key].Count);
        }
    }


    public void computeChangesInFile()
    {
        if (visitedPlanets.Count == 0)
            return;

        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/" + actualSeed + ".plnt"))
        {
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/" + actualSeed + ".plnt");

        }

        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/" + actualSeed+".plnt");

        for (int i =0; i < lines.Length; i++)//on update ici les modifications de tile deja existantes
        {
            if (!(lines[i].Contains("#")))
            {
                string[] tmp = lines[i].Split(';');
                foreach (TileChange t in visitedPlanets[actualSeed])
                {
                    if (t.x == int.Parse(tmp[0]) && t.y == int.Parse(tmp[1]))
                    {
                        lines[i] = t.x + ";" + t.y + ";" + t.planetType + ";" + t.tileIndex;
                        visitedPlanets[actualSeed].Remove(t);
                    }
                }
            }
        }

        System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/" + actualSeed + ".plnt", lines);

        string[] newLines = new string[visitedPlanets[actualSeed].Count];
        int index = 0;
        foreach (TileChange t in visitedPlanets[actualSeed])//si les tiles avaient jamais été modifiées, on les rajoute a la fin du fichier
        {
            newLines[index] = t.x + ";" + t.y + ";" + t.planetType + ";" + t.tileIndex;
            index++;
        }

        System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/" + actualSeed + ".plnt", newLines);
        visitedPlanets.Clear();
    }

    public void AddModification(float seed, string planetType,int index, int xcord, int ycord)
    {
        if (actualSeed != seed)
            actualSeed = seed;

        TileChange t;
        t.planetType = planetType;
        t.tileIndex = index;
        t.x = xcord;
        t.y = ycord;
        if (visitedPlanets.ContainsKey(seed))
        {
            foreach (TileChange k in visitedPlanets[seed])
            {
                if (k.x == xcord && k.y == ycord)
                {
                    visitedPlanets[seed].Remove(k);
                    break;
                }
            }
            visitedPlanets[seed].Add(t);
        }
        else
        {
            List<TileChange> l = new List<TileChange>();
            l.Add(t);
            visitedPlanets.Add(seed,l);
        }
    }
	
}

[System.Serializable]
public struct TileChange
{
    public string planetType;//palette a utiliser 
    public int tileIndex;//Index de la tile dans la palete
    public int x;//x coordinate of the change
    public int y;//y coordinate of the change
}
