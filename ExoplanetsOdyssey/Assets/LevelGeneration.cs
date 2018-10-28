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

    GameObject player;
	// Use this for initialization
	void Start () {
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
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                int type = Random.Range(0, 2);
                int rand = Random.Range(0, 100);
                if (rand < chance && map[x,y] == 1)
                {
                    map[x, y] = type+2;
                }
            }
        }
    }

    public void SpreadRessourcesInMap(int[,] map)
    {
        int [,] map2 = map;
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                int type = map[x, y];
                if (type != 0 && type != 1)
                {
                    int newX = x;
                    int newY = y;
                    int tailleChunck = Random.Range(1, 4);
                    for (int i = 0; i < tailleChunck; i++)
                    {
                        int dir = Random.Range(0, 4);
                        switch (dir)
                        {
                            case 0: newX--;break;
                            case 1: newX++;break;
                            case 2: newY--;break;
                            case 3: newY++;break;
                        }
                        if (newX > 0 && newY > 0 && newX < map.GetUpperBound(0) && newY < map.GetUpperBound(1))
                            map2[newX, newY] = type;
                    }
                }
            }
        }
        map = map2;
    }
    public static int[,] RandomWalkTopSmoothed(int[,] map, float seed, int minSectionWidth)
    {
        //Seed our random
        System.Random rand = new System.Random(seed.GetHashCode());

        //Determine the start position
        int lastHeight = Random.Range(map.GetUpperBound(1)/2, map.GetUpperBound(1));

        //Used to determine which direction to go
        int nextMove = 0;
        //Used to keep track of the current sections width
        int sectionWidth = 0;

        //Work through the array width
        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            //Determine the next move
            nextMove = rand.Next(2);

            //Only change the height if we have used the current height more than the minimum required section width
            if (nextMove == 0 && lastHeight > 0 && sectionWidth > minSectionWidth)
            {
                lastHeight--;
                sectionWidth = 0;
            }
            else if (nextMove == 1 && lastHeight < map.GetUpperBound(1) && sectionWidth > minSectionWidth)
            {
                lastHeight++;
                sectionWidth = 0;
            }
            //Increment the section width
            sectionWidth++;

            //Work our way from the height down to 0
            for (int y = lastHeight; y >= 0; y--)
            {
                map[x, y] = 1;
            }
        }

        //Return the modified map
        return map;
    }

    public void TileMapGen()
    {
        mapBase = GenerateArray(worldWidth, worldHeight, true);
        mapBase = RandomWalkTopSmoothed(mapBase, worldseed, 4);
        SetRessourcesInMap(mapBase, 1);
        SpreadRessourcesInMap(mapBase);
        RenderMap(mapBase, tiles, basic,fuel,iron);
        UpdateMap(mapBase, tiles);
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
