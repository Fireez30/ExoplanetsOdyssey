using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlanetModificationsSaver : MonoBehaviour {

    public List<TileChange> actualPlanetModifs;
    int actualSeed;
	// Use this for initialization

	void Awake () {
        actualPlanetModifs = new List<TileChange>();
	}

     void Start()
    {
        actualSeed = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>().getSeedToGen();
    }


    public void computeChangesInFile()
    {
        if (actualPlanetModifs.Count == 0)//if player changed nothing, just pass this method
            return;

        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/" + actualSeed + ".plnt"))//if no file exists
        {
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/" + actualSeed + ".plnt").Close();//create it

        }

        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/" + actualSeed+".plnt");//read file for modifications

        for (int i =0; i < lines.Length; i++)//for each modifications made by the player
        {
            if (!(lines[i].Contains("#")))
            {
                string[] tmp = lines[i].Split(';');
                foreach (TileChange t in actualPlanetModifs)//if we find a more recent modif
                {
                    if (t.x == int.Parse(tmp[0]) && t.y == int.Parse(tmp[1]))
                    {
                        lines[i] = t.x + ";" + t.y + ";" + t.planetType + ";" + t.tileIndex;//change the line
                        actualPlanetModifs.Remove(t);//dont loop
                    }
                }
            }
        }

        System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/" + actualSeed + ".plnt", lines);//update file with updated modifications on already modified tiles

        string[] newLines = new string[actualPlanetModifs.Count];//now we will add new tiles modifications
        int index = 0;
        foreach (TileChange t in actualPlanetModifs)//for each modification not already in file
        {
            newLines[index] = t.x + ";" + t.y + ";" + t.planetType + ";" + t.tileIndex;//create a new line
            index++;//go to the line in the file
        }
        string[] final = new string[lines.Length+newLines.Length];//sum old changed and new changes in a new tab to write
        for (int i = 0; i < lines.Length; i++)
        {
            final[i] = lines[i];//already existing modifs
        }
        for (int i = 0; i < newLines.Length; i++)
        {
            final[i + lines.Length] = newLines[i];//adding to the end the new ones
        }

        System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/" + actualSeed + ".plnt", final);//Write file
        actualPlanetModifs.Clear();//just a clear
    }

    public void AddModification(int seed, string planetType,int index, int xcord, int ycord)//used by others scripts to add modification
    {
        TileChange t;//create a new modif
        t.planetType = planetType;//used to choose the good palette
        t.tileIndex = index;//Wich tile in the palette
        t.x = xcord;//tilemap position x coordinate
        t.y = ycord;//tilemap position y coordinate
        foreach (TileChange k in actualPlanetModifs)
        {
                if (k.x == xcord && k.y == ycord)//if an older modif exist
                {
                    actualPlanetModifs.Remove(k);//remove it
                    break;
                }
        }
        actualPlanetModifs.Add(t);//push the more recent modif to the list
    }
}

[System.Serializable]
public struct TileChange
{
    public string planetType;//the palette to use
    public int tileIndex;//wich rule tile in the palette
    public int x;//x coordinate of the change
    public int y;//y coordinate of the change
}
