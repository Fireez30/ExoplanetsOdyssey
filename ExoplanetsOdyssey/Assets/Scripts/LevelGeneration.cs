using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGeneration : MonoBehaviour {

    public int worldWidth;
    public int worldHeight;

    public TileBase basic;
    public TileBase fuel;
    public TileBase iron;

    public Tilemap tiles;
    public float worldseed;

    int[,] mapBase;
    GameObject GM;
    GameObject player;
	// Use this for initialization
	void Start () {
        GM = GameObject.FindGameObjectWithTag("GameManager");
        TileMapGen();
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.Translate(mapBase.GetUpperBound(0) / 2, mapBase.GetUpperBound(1) + 1, 0);
        Camera.main.transform.Translate(mapBase.GetUpperBound(0) / 2, mapBase.GetUpperBound(1) + 1, 0);
    }

public static void RenderMap(int[,] map, Tilemap tilemap, TileBase basic,TileBase fuel, TileBase iron)
    {
        //Clear the map (ensures we dont overlap)
        tilemap.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                // 1 = tile, 0 = no tile
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), basic);
                }
                else if (map[x,y] == 2)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), fuel);
                }
                else if (map[x, y] == 3)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), iron);
                }
            }
        }
    }

    public static int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (empty)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 1;
                }
            }
        }
        return map;
    }
    public void SetRessourcesInMap(int[,] map,int chance)
    {
        //Seed our random
        System.Random rand2 = new System.Random(worldseed.GetHashCode());

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                int rand = rand2.Next(200);
                if (rand < chance && map[x,y] == 1)
                {
                    int type = rand2.Next(2);
                    map[x, y] = type+2;
                }
            }
        }
    }

    public int[,] SpreadRessourcesInMap(int[,] map)
    {
        System.Random rand2 = new System.Random(worldseed.GetHashCode());

        int [,] map2 = new int[map.GetUpperBound(0)+1,map.GetUpperBound(1)+1];
        for (int i = 0; i <= map.GetUpperBound(0); i++)
        {
            for (int i2 = 0; i2 <= map.GetUpperBound(1); i2++)
            {
                map2[i, i2] = map[i, i2];
            }
        }
        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= map.GetUpperBound(1); y++)
            {
                int type = map[x, y];
                if (type != 0 && type != 1)
                {
                    int newX = x;
                    int newY = y;
                    int tailleChunck = rand2.Next(1,4);
                    for (int i = 0; i < tailleChunck; i++)
                    {
                        int dir = rand2.Next(4);
                        switch (dir)
                        {
                            case 0: newX--;break;
                            case 1: newX++;break;
                            case 2: newY--;break;
                            case 3: newY++;break;
                        }
                        if (newX > 0 && newY > 0 && newX <= map.GetUpperBound(0) && newY <= map.GetUpperBound(1))
                            map2[newX, newY] = type;
                    }
                }
            }
        }
        return map2;
    }
    public static int[,] RandomWalkTopSmoothed(int[,] map, float seed, int minSectionWidth, int maxSectionWidth)
    {
        //Seed our random
        System.Random rand = new System.Random(seed.GetHashCode());

        //Determine the start position
        int lastHeight = rand.Next(map.GetUpperBound(1) / 2) + map.GetUpperBound(1) / 2;

        int x = 0;


        //Work through the array width
        
        while (x <= map.GetUpperBound(0))
        {
            //Used to keep track of the current sections width
            int sectionWidth = Mathf.Min(map.GetUpperBound(0) - x + 1, rand.Next(minSectionWidth, maxSectionWidth));
            //Used to determine which direction to go
            int nextMove = rand.Next(2);
            if (nextMove == 0 && lastHeight > 0)
            {
                lastHeight--;
            }
            else if (nextMove == 1 && lastHeight < map.GetUpperBound(1))
            {
                lastHeight++;
            }
            for(int i=0; i< sectionWidth; i++)
            {
                for (int y = lastHeight; y >= 0; y--)
                {
                    map[x+i, y] = 1;
                }
            }
            x += sectionWidth;
        }


        //Return the modified map
        return map;
    }

    public void RenderOldChanges()
    {
        if (GM.GetComponent<PlanetModificationsSaver>().visitedPlanets.Count > 0)
            foreach (float key in GM.GetComponent<PlanetModificationsSaver>().visitedPlanets.Keys)
            {
                if (key.Equals(worldseed))
                {
                    Debug.Log("Changes found !");
                    foreach (TileChange t in GM.GetComponent<PlanetModificationsSaver>().visitedPlanets[key])
                    {
                        Debug.Log("Mise en place d'un changement a la posion x : " + t.x + " et y : " + t.y);
                        tiles.SetTile(new Vector3Int(t.x, t.y, 0), t.id);
                    }
                    return;
                }
            }
    }

    public void TileMapGen()
    {
        mapBase = GenerateArray(worldWidth, worldHeight, true);
        mapBase = RandomWalkTopSmoothed(mapBase, worldseed, 4, 10);
        SetRessourcesInMap(mapBase, 1);
        mapBase = SpreadRessourcesInMap(mapBase);
        RenderMap(mapBase, tiles, basic,fuel,iron);
        UpdateMap(mapBase, tiles);
        RenderOldChanges();

    }

    private void Update()
    {

    }

    public static void UpdateMap(int[,] map, Tilemap tilemap) //Takes in our map and tilemap, setting null tiles where needed
        {
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    //We are only going to update the map, rather than rendering again
                    //This is because it uses less resources to update tiles to null
                    //As opposed to re-drawing every single tile (and collision data)
                    if (map[x, y] == 0)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), null);
                    }
                }
            }
        }
}
