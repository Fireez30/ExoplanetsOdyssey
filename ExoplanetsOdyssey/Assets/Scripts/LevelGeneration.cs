using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGeneration : MonoBehaviour {

    private System.Random rand;
    public int worldWidth;
    public int worldHeight;
    private int minSurface;

    public List<Palette> tilesList;
    public Tilemap tiles;
    public string worldseed;
    
    int[,] mapBase;
    GameObject GM;
    GameObject player;
    string planetType;
	// Use this for initialization
	void Start () {
        rand = new System.Random(worldseed.GetHashCode());
        GM = GameObject.FindGameObjectWithTag("GameManager");
        planetType = GM.GetComponent<Parameters>().planetType;
        worldseed = GM.GetComponent<Parameters>().actualPlanet;
        TileMapGen();
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.Translate(mapBase.GetUpperBound(0) / 2, mapBase.GetUpperBound(1) + 1, 0);
        Camera.main.transform.Translate(mapBase.GetUpperBound(0) / 2, mapBase.GetUpperBound(1) + 1, 0);
    }

    public TileBase getTileFromPalette(string ptype,int index)
    {
        foreach (Palette p in tilesList)
        {
            if (p.type.Equals(ptype)){
                return p.Alltiles[index];
            }
        }

        return null;
    }
public void RenderMap(int[,] map, Tilemap tilemap)
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
                    tilemap.SetTile(new Vector3Int(x, y, 0), getTileFromPalette(planetType,0));
                }
                else if (map[x,y] == 2)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), getTileFromPalette(planetType, 1));
                }
                else if (map[x, y] == 3)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), getTileFromPalette(planetType, 2));
                }
            }
        }
    }

    public void writeMap(int [,] map)
    {
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/" + worldseed + ".trn"))
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/" + worldseed + ".trn");

        string[] lines = new string[map.GetUpperBound(0)];
        int currentid = 0;
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            string line = "";
            //Loop through the height of the map
            for (int y = 0; y < map.GetUpperBound(1)-1; y++)
            {
                line += map[x, y] + ";";
            }
            line += map[x, map.GetUpperBound(1) - 2];
            lines[currentid] = line;
            currentid++;
        }

        System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/" + worldseed + ".trn", lines);

    }

    public int[,] loadMap()
    {
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/" + worldseed + ".trn"))
            return null;

        int[,] map = new int[worldWidth + 1, worldHeight + 1];

        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/" + worldseed + ".trn");

        for (int i = 0; i < lines.Length; i++)//on update ici les modifications de tile deja existantes
        {
            if (!(lines[i].Contains("#")))
            {
                Debug.Log("display : "+ lines[i]);
                string[] tmp = lines[i].Split(';');
                Debug.Log("apres split "+tmp[0]);
                for (int j=0; j < tmp.Length; j++)
                {
                    Debug.Log("contenu: "+ int.Parse(tmp[j]));
                    map[i, j] = int.Parse(tmp[j]);
                }
            }
        }

        return map;
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
    public void SetRessourcesInMap(int[,] map,int chance) {

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (rand.Next(200) < chance && map[x,y] == 1)
                {
                    int type = rand.Next(2);
                    map[x, y] = type+2;
                }
            }
        }
    }

    public int[,] SpreadRessourcesInMap(int[,] map)
    {

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
                    int tailleChunck = rand.Next(1,4);
                    for (int i = 0; i < tailleChunck; i++)
                    {
                        int dir = rand.Next(4);
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
    public int[,] RandomWalkTopSmoothed(int[,] map, int minSectionWidth, int maxSectionWidth)
    {
        //Determine the start position
        int lastHeight = rand.Next(map.GetUpperBound(1) / 2) + map.GetUpperBound(1) / 2;
        minSurface = lastHeight;
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
                if (lastHeight < minSurface)
                    minSurface = lastHeight;
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
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/" + worldseed + ".plnt"))
        {
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/" + worldseed + ".plnt");
        }

        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/" + worldseed + ".plnt");

        for (int i = 0; i < lines.Length; i++)
        {
            if (!(lines[i].Contains("#")))
            {
                string[] tmp = lines[i].Split(';');
                if (int.Parse(tmp[3]) == -1)
                {
                    tiles.SetTile(new Vector3Int(int.Parse(tmp[0]), int.Parse(tmp[1]), 0), null);
                }
                else
                {
                    tiles.SetTile(new Vector3Int(int.Parse(tmp[0]), int.Parse(tmp[1]), 0),getTileFromPalette(tmp[2], int.Parse(tmp[3])));
                }
               
            }
        }
    }

    float factorielle(int n)
    {
        if (n == 0)
        {
            return 1;
        }
        else
        {
            int resultat = 1;
            for (int i = 1; i <= n; i++)
            {
                resultat = resultat * i;
            }
            return resultat;
        }
    }

    float calculCoeff(int i, int n, float t)
    {
        return (factorielle(n) / (factorielle(i) * factorielle(n - i))) * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i);
    }


    public void GenerateCave(int[,] map, Vector2 ptStart, int chance)
    {

        List<Vector2> TabcontrolPoint = new List<Vector2>();
        int nbControlPoint = 3;
        TabcontrolPoint.Add(ptStart);
        for (int i = 0; i < nbControlPoint; i++)
            TabcontrolPoint.Add(new Vector2(rand.Next(worldWidth), rand.Next(minSurface + 5)));
        int nbPoints = 1000;
        Vector2[] tab = new Vector2[nbPoints + 1];
        for (int i = 0; i <= nbPoints; i++)
        {
            float u = (float)(i) / (float)(nbPoints);
            float x = 0, y = 0;
            for (int i2 = 0; i2 < TabcontrolPoint.Count; i2++)
            {
                x += calculCoeff(i2, TabcontrolPoint.Count - 1, u) * TabcontrolPoint[i2].x;
                y += calculCoeff(i2, TabcontrolPoint.Count - 1, u) * TabcontrolPoint[i2].y;
            }
            tab[i] = new Vector2(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            if (rand.Next(10000) < chance)
            {
                GenerateCave(map, tab[i], chance - 5);
            }
        }
        foreach (Vector2 v in tab)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (v.x + x >= 0 && v.x + x <= map.GetUpperBound(0) && v.y + y >= 0 && v.y + y <= map.GetUpperBound(1))
                        map[(int)v.x + x, (int)v.y + y] = 0;
                }
            }
        }

    }

    public void TileMapGen()
    {
            mapBase = GenerateArray(worldWidth, worldHeight, true);
            mapBase = RandomWalkTopSmoothed(mapBase, 4, 10);
            GenerateCave(mapBase, new Vector2(rand.Next(worldWidth), minSurface + 5), 20);
            SetRessourcesInMap(mapBase, 1);
            mapBase = SpreadRessourcesInMap(mapBase);
            RenderMap(mapBase, tiles);
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

[System.Serializable]
public struct Palette
{
    public string type;
    public List<TileBase> Alltiles;
}