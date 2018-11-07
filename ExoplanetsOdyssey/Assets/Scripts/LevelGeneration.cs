using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGeneration : MonoBehaviour
{

    public int worldWidth;
    public int worldHeight;
    public List<Palette> tilesList;
    public Tilemap tiles;
    public string worldseed;
    public GameObject player;

    private int maxSurface;
    private System.Random rand;
    private int[,] mapBase;
    private GameObject GM;
    private string planetType;

    // Use this for initialization
    void Awake()
    {
        rand = new System.Random(worldseed.GetHashCode());
        GM = GameObject.FindGameObjectWithTag("GameManager");
        planetType = GM.GetComponent<Parameters>().planetType;
        worldseed = GM.GetComponent<Parameters>().actualPlanet;
        TileMapGen();
        if (player)
            player.transform.Translate(mapBase.GetUpperBound(0) / 2, maxSurface + 5, 0);
        Camera.main.transform.Translate(player.transform.position.x, player.transform.position.y, 0);
    }

    void Update()
    {
        Vector3 playPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(playPos);
        Vector3Int tilePos = tiles.WorldToCell(screenPos);


        if (tilePos.x > tiles.cellBounds.xMax)
        {
            tiles = Instantiate(tiles, new Vector3(tiles.cellBounds.xMax, 0, 0), new Quaternion(0, 0, 0, 0), tiles.transform);

        }
    }

    public TileBase getTileFromPalette(string ptype, int index)
    {
        foreach (Palette p in tilesList)
        {
            if (p.type.Equals(ptype))
            {
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
                    tilemap.SetTile(new Vector3Int(x, y, 0), getTileFromPalette(planetType, 0));
                }
                else if (map[x, y] == 2)
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

    public void writeMap(int[,] map)
    {
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/" + worldseed + ".trn"))
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/" + worldseed + ".trn").Close();

        string[] lines = new string[map.GetUpperBound(0)];
        int currentid = 0;
        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            string line = "";
            //Loop through the height of the map
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                line += map[x, y] + ";";
            }
            line += map[x, map.GetUpperBound(1)];
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
                Debug.Log("display : " + lines[i]);
                string[] tmp = lines[i].Split(';');
                Debug.Log("apres split " + tmp[0]);
                for (int j = 0; j < tmp.Length; j++)
                {
                    Debug.Log("contenu: " + int.Parse(tmp[j]));
                    map[i, j] = int.Parse(tmp[j]);
                }
            }
        }

        return map;
    }

    public static int[,] GenerateArray(int width, int height, int tile)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= map.GetUpperBound(1); y++)
            {
                map[x, y] = tile;
            }
        }
        return map;
    }
    public void SetRessourcesInMap(int[,] map, int chance)
    {

        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= map.GetUpperBound(1); y++)
            {
                int memChance = chance;
                if (map[x, y] == -1)
                {
                    chance *= 30;
                    map[x, y] = 1;
                }
                if (rand.Next(500) < chance && map[x, y] == 1)
                {
                    int type = rand.Next(2);
                    map[x, y] = type + 2;
                }
                chance = memChance;
            }
        }
    }

    public int[,] SpreadRessourcesInMap(int[,] map)
    {

        int[,] map2 = new int[map.GetUpperBound(0) + 1, map.GetUpperBound(1) + 1];
        for (int i = 0; i <= map.GetUpperBound(0); i++)                             //Deep copy de la map
        {
            for (int i2 = 0; i2 <= map.GetUpperBound(1); i2++)
                map2[i, i2] = map[i, i2];
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
                    int tailleChunck = rand.Next(1, 4);
                    for (int i = 0; i < tailleChunck; i++)
                    {
                        int dir = rand.Next(4);
                        switch (dir)
                        {
                            case 0: newX--; break;
                            case 1: newX++; break;
                            case 2: newY--; break;
                            case 3: newY++; break;
                        }
                        if (newX > 0 && newY > 0 && newX <= map.GetUpperBound(0) && newY <= map.GetUpperBound(1))
                            map2[newX, newY] = type;
                    }
                }
            }
        }
        return map2;
    }
    public void RandomWalkTopSmoothed(int[,] map, int minSectionWidth, int maxSectionWidth)
    {
        //Determine the start position
        int lastHeight = rand.Next(map.GetUpperBound(1) / 2) + map.GetUpperBound(1) / 2;
        maxSurface = lastHeight;
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
                if (lastHeight > maxSurface)
                    maxSurface = lastHeight;
            }
            for (int i = 0; i < sectionWidth; i++)
            {
                for (int y = lastHeight; y >= 0; y--)
                {
                    map[x + i, y] = 1;
                }
            }
            x += sectionWidth;
        }
    }

    public void RenderOldChanges()
    {
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/" + worldseed + ".plnt"))
        {
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/" + worldseed + ".plnt").Close();
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
                    tiles.SetTile(new Vector3Int(int.Parse(tmp[0]), int.Parse(tmp[1]), 0), getTileFromPalette(tmp[2], int.Parse(tmp[3])));
                }

            }
        }
    }

    public void GenerateUnbreakableTiles()
    {
        for (int i = 0; i <= mapBase.GetUpperBound(0); i++)
        {
            tiles.SetTile(new Vector3Int(i, 0, 0), getTileFromPalette(planetType, 3));
        }
    }

    float factorielle(int n)
    {
        int resultat = 1;
        for (int i = 2; i <= n; i++)
            resultat = resultat * i;
        return resultat;
    }

    float calculCoeff(int i, int n, float t)
    {
        return (factorielle(n) / (factorielle(i) * factorielle(n - i))) * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i);
    }


    public void GenerateCave(int[,] map, Vector2 ptStart, int chance)
    {
        List<Vector2> TabcontrolPoint = new List<Vector2>();
        int nbControlPoint = 3;                                                          //3 points générés + point de départ = 4 points de contrôle
        TabcontrolPoint.Add(ptStart);
        float minX = 9999999, maxX = -1, minY = 9999999, maxY = -1;                             //Pour calculer la distance 4 entre les deux points les plus éloignés (pour pas générer trop de points)
        for (int i = 0; i < nbControlPoint; i++)
        {
            Vector2 v = new Vector2(rand.Next(worldWidth), rand.Next(maxSurface));
            TabcontrolPoint.Add(v);
            if (v.x > maxX) maxX = v.x;
            if (v.x < minX) minX = v.x;
            if (v.y > maxY) maxY = v.y;
            if (v.y < minY) minY = v.y;
        }
        int nbPoints = (int)(maxY - minY + maxX - minX);
        Vector2[] tab = new Vector2[nbPoints];
        for (int i = 0; i < nbPoints; i++)                                              //Calcul des coordonnées de chacuns des points de la courbe de Bézier
        {
            float u = (float)(i) / (float)(nbPoints);
            float x = 0, y = 0;
            for (int i2 = 0; i2 < TabcontrolPoint.Count; i2++)
            {
                x += calculCoeff(i2, TabcontrolPoint.Count - 1, u) * TabcontrolPoint[i2].x;
                y += calculCoeff(i2, TabcontrolPoint.Count - 1, u) * TabcontrolPoint[i2].y;
            }
            tab[i] = new Vector2(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            if (rand.Next(nbPoints * 20) < chance)                                        //On génère une nouvelle courbe par récursivité pour créer des branchements au tunel
            {
                GenerateCave(map, tab[i], chance - 5);
            }
        }
        foreach (Vector2 v in tab)                                                      //Pour chaque points de la courbe, on enlève les voisins dans une certaine range
        {
            int range = rand.Next(1, 4);
            for (int x = -1 * range - 1; x <= range; x++)
            {
                for (int y = -1 * range - 1; y <= range; y++)
                {
                    if (v.x + x >= 0 && v.x + x <= map.GetUpperBound(0) && v.y + y >= 0 && v.y + y <= map.GetUpperBound(1))
                    {
                        if (x == -1 * range - 1 || x == range || y == -1 * range - 1 || y == range)  //Si l'une de ces tiles est un mur, on lui attribu une valeur différente pour que ce mur ai plus de chance de générer des ressources.
                        {
                            if (map[(int)v.x + x, (int)v.y + y] == 1)
                                map[(int)v.x + x, (int)v.y + y] = -1;
                        }
                        else
                            map[(int)v.x + x, (int)v.y + y] = 0;
                    }

                }
            }
        }

    }

    public void TileMapGen()
    {
        mapBase = GenerateArray(worldWidth, worldHeight, 0);
        RandomWalkTopSmoothed(mapBase, 4, 10);
        GenerateCave(mapBase, new Vector2(rand.Next(worldWidth), maxSurface), 20);
        SetRessourcesInMap(mapBase, 1);
        mapBase = SpreadRessourcesInMap(mapBase);
        RenderMap(mapBase, tiles);
        UpdateMap(mapBase, tiles);
        RenderOldChanges();
        GenerateUnbreakableTiles();
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