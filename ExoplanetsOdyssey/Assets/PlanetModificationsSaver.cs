using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlanetModificationsSaver : MonoBehaviour {

    public Dictionary<float,List<TileChange>> visitedPlanets;
	// Use this for initialization

	void Start () {
        visitedPlanets = new Dictionary<float, List<TileChange>>();
	}

    void Update()
    {
        foreach (float key in visitedPlanets.Keys)
        {
            Debug.Log("planete : " + key + " nombre d'elements : " + visitedPlanets[key].Count);
        }
    }

    public void AddModification(float seed,TileBase id, bool type, int xcord, int ycord)
    {
        TileChange t;
        t.type = type;
        t.id = id;
        t.x = xcord;
        t.y = ycord;
        if (visitedPlanets.ContainsKey(seed))
        {
           
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
    public bool type;// 0 -> Tile deletion, no use of id , 1 -> tile addition, player added the tile "id"
    public TileBase id;//which tile has been added
    public int x;//x coordinate of the change
    public int y;//y coordinate of the change
}
