using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesLevelGeneration : MonoBehaviour {

    public int worldWidth;//world size in width
    public int worldHeight;//world size in height
    public List<Palette> tilesList;//list of all planet types and the different tiles for each type
    //public TileBase transparent;
    public GameObject parent;//needed to setup the 3 tilemaps in  the hierarchy
    public Tilemap tiles;//the main tilemap
    Tilemap leftCopy;//left copy (generated in TileMapGen())
    Tilemap rightCopy;//right copy (generated in TileMapGen())
    public GameObject player;//main player
    public GameObject leftPlayer;//setup the leftcopy of the player in Awake
    public GameObject rightPlayer;//setup the rightcopy of the player in Awake

    private int maxSurface;
    private System.Random rand;
    private int[,] mapBase;
    private GameObject GM;
    private string planetType;
    private int planeteSeed;

    public int getPlaneteSeed()
    {
        return planeteSeed;
    }

    // Use this for initialization
    void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager");
        planetType = GM.GetComponent<Parameters>().planetType;
        planeteSeed = GM.GetComponent<Parameters>().getSeedToGen();
        rand = new System.Random(planeteSeed);//setup a seeded random
        TileMapGen();//creation of the different tilemaps
        if (player)//setup players in the world
        {
            player.transform.Translate(mapBase.GetUpperBound(0) / 2, maxSurface+5, 0);//middle position player
            leftPlayer.transform.Translate(player.transform.position.x-worldWidth, player.transform.position.y, 0);//the left player is on the left map
            rightPlayer.transform.Translate(player.transform.position.x + worldWidth, player.transform.position.y, 0);//the right player is on the right map
        }
        Debug.Log(maxSurface);
        Debug.Log(player.transform.position.x + " / " + player.transform.position.y);
        Camera.main.transform.Translate(/*player.transform.position.x*/10, player.transform.position.y,0);
    }

    /*void Update()
    {
        Vector3 playPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(playPos);
        Vector3Int tilePos = tiles.WorldToCell(screenPos);


        if (tilePos.x > tiles.cellBounds.xMax)
        {
            tiles = Instantiate(tiles, tiles.CellToWorld(new Vector3Int(tiles.cellBounds.xMax, 0, 0)), Quaternion.identity);

        }
    }*/

    public TileBase getTileFromPalette(string ptype, int index)
    {
        foreach (Palette p in tilesList)//for each palette in the game
        {
            if (p.type.Equals(ptype))//find the one for this planet
            {
                return p.Alltiles[index];//get the tile using the index
            }
        }

        return null;//if not exist, don't put a tile
    }

    public void RenderMap(int[,] map, Tilemap tilemap)
    {
        //Clear the map (ensures we dont overlap)
        tilemap.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            //Loop through the height of the map
            for (int y = 0; y <= map.GetUpperBound(1); y++)
            {
                // 1 = tile, 2 = fuel 3 = iron
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), getTileFromPalette(planetType, 0));//set the classic tile (index 0 in the palette)
                }
                else if (map[x, y] == 2)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), getTileFromPalette(planetType, 1));//set the fuel tile (index 1 in the palette)
                }
                else if (map[x, y] == 3)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), getTileFromPalette(planetType, 2));//set the iron tile (index 2 in the palette)
                }
            }
        }
    }

    public void writeMap(int[,] map)
    {
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/" + planeteSeed + ".trn"))//if map dont exist in the files
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/" + planeteSeed + ".trn").Close();//create it

        string[] lines = new string[map.GetUpperBound(0)];//line = x pos, characters = y pos
        int currentid = 0;
        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            string line = "";
            //Loop through the height of the map
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                line += map[x, y] + ";";//add each tile as a character
            }
            line += map[x, map.GetUpperBound(1)];//add the last character without separator
            lines[currentid] = line;//store the line
            currentid++;//add another line
        }

        System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/" + planeteSeed + ".trn", lines);//write lines created

    }

    public int[,] loadMap()
    {
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/" + planeteSeed + ".trn"))
            return null;//if no map of this kind exist, just return nothing

        int[,] map = new int[worldWidth + 2, worldHeight];//+2 because we generate 2 tiles to create smooth transitions

        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/" + planeteSeed + ".trn");//if map exist load it

        for (int i = 0; i < lines.Length; i++)//for each line = x coordinate
        {
            if (!(lines[i].Contains("#")))
            {
                Debug.Log("display : " + lines[i]);
                string[] tmp = lines[i].Split(';');//split line 
                Debug.Log("apres split " + tmp[0]);
                for (int j = 0; j < tmp.Length; j++)
                {
                    Debug.Log("contenu: " + int.Parse(tmp[j]));
                    map[i, j] = int.Parse(tmp[j]);//the map at x , j is the character read in the file
                }
            }
        }

        return map;
    }

    public static int[,] GenerateArray(int width, int height, int tile)//just generate a world sized array to store the map template
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
    public void SetRessourcesInMap(int[,] map, int chance)//randomly generate ressources
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
                int type = map[x, y];//for each tile, 
                if (type != 0 && type != 1)//if its a ressource
                {
                    int newX = x;
                    int newY = y;
                    int tailleChunck = rand.Next(1, 4);//generate a size of the chunck of those ressources
                    for (int i = 0; i < tailleChunck; i++)
                    {
                        int dir = rand.Next(4);//switch direction of the chunck
                        switch (dir)
                        {
                            case 0: newX--; break;
                            case 1: newX++; break;
                            case 2: newY--; break;
                            case 3: newY++; break;
                        }
                        if (newX > 0 && newY > 0 && newX <= map.GetUpperBound(0) && newY <= map.GetUpperBound(1))//if the tile chosen is in the map
                            map2[newX, newY] = type;//it becomes a ressource
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
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/" + planeteSeed + ".plnt"))//if no file of this planet exist
        {
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/" + planeteSeed + ".plnt").Close();//create it
        }

        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/" + planeteSeed + ".plnt");//get file

        for (int i = 0; i < lines.Length; i++)
        {
            if (!(lines[i].Contains("#")))
            {
                string[] tmp = lines[i].Split(';');
                if (int.Parse(tmp[3]) == -1)//if the change stored is a break of the tile
                {
                    tiles.SetTile(new Vector3Int(int.Parse(tmp[0]), int.Parse(tmp[1]), 0), null);//Delete the tile on the map
                }
                else
                {
                    tiles.SetTile(new Vector3Int(int.Parse(tmp[0]), int.Parse(tmp[1]), 0), getTileFromPalette(tmp[2], int.Parse(tmp[3])));//else put a tile of the good type, in the good palette
                }

            }
        }
    }

    public void GenerateUnbreakableTiles()
    {
        for (int i = 0; i <= mapBase.GetUpperBound(0); i++)
        {
            tiles.SetTile(new Vector3Int(i, 0, 0), getTileFromPalette(planetType, 3));//the bottom line of the tilemap becomes unbreakable
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
        mapBase = GenerateArray(worldWidth+2, worldHeight, 0);//create the array
        RandomWalkTopSmoothed(mapBase, 4, 10);//generate the map using a smooth random walk 
        GenerateCave(mapBase, new Vector2(rand.Next(worldWidth), maxSurface), 20);//generate caves using bezier
        SetRessourcesInMap(mapBase, 1);//create ressources randomly
        mapBase = SpreadRessourcesInMap(mapBase);//spread them
        RenderMap(mapBase, tiles);//render the tilemap using the template 
        UpdateMap(mapBase, tiles);
        RenderOldChanges();//get old changes and apply them
        GenerateUnbreakableTiles();//create a line of unbreakable tiles at the bottom
        rightCopy = Instantiate(tiles,new Vector3(worldWidth+2,0 , 0), Quaternion.identity,parent.transform);//instantiate left copy 
        leftCopy = Instantiate(tiles, new Vector3(-worldWidth-2, 0, 0), Quaternion.identity, parent.transform);//instantiate right copy
        leftCopy.name = "left";//useful to sort tilemaps in another script , dont remove !!!!

        int maxX = rightCopy.cellBounds.xMax - 1;
        for (int i = 0; i < rightCopy.cellBounds.yMax; i++)//create transparent tiles to smooth the contacts betweeen tilemaps
        {
            rightCopy.SetTile(new Vector3Int(-1, i, 0), rightCopy.GetTile(new Vector3Int(maxX, i, 0)));
            rightCopy.SetColor(new Vector3Int(-1, i, 0), new Color(1, 1, 1, 0.2f));
            rightCopy.SetColliderType(new Vector3Int(-1, i, 0), Tile.ColliderType.None);
        }
        maxX = leftCopy.cellBounds.xMax;
        for (int i = 0; i < leftCopy.cellBounds.yMax; i++)//create transparent tiles to smooth the contacts betweeen tilemaps
        {
            leftCopy.SetTile(new Vector3Int(maxX, i, 0), leftCopy.GetTile(new Vector3Int(0, i, 0)));
            leftCopy.SetColor(new Vector3Int(leftCopy.cellBounds.xMax - 1, i, 0), new Color(1, 1, 1, 0.5f));
            leftCopy.SetColliderType(new Vector3Int(leftCopy.cellBounds.xMax - 1, i, 0), Tile.ColliderType.None);
        }

        int avantDerniere = tiles.cellBounds.xMax - 1, derniere = tiles.cellBounds.xMax; ;
        for (int i = 0; i < tiles.cellBounds.yMax; i++)//create transparent tiles to smooth the contacts betweeen tilemaps
        {
            tiles.SetTile(new Vector3Int(-1, i, 0), tiles.GetTile(new Vector3Int(avantDerniere, i, 0)));
            tiles.SetTile(new Vector3Int(derniere, i, 0), tiles.GetTile(new Vector3Int(0, i, 0)));

            tiles.SetColor(new Vector3Int(derniere, i, 0), new Color(1, 1, 1, 0.5f));
            tiles.SetColliderType(new Vector3Int(derniere, i, 0), Tile.ColliderType.None);
            tiles.SetColor(new Vector3Int(-1, i, 0), new Color(1, 1, 1, 0.2f));
            tiles.SetColliderType(new Vector3Int(-1, i, 0), Tile.ColliderType.None);

        }
    }

    public static void UpdateMap(int[,] map, Tilemap tilemap) //Takes in our map and tilemap, setting null tiles where needed
    {
        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= map.GetUpperBound(1); y++)
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
