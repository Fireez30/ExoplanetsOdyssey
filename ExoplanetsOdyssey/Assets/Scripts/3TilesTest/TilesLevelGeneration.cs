using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesLevelGeneration : MonoBehaviour {

    public int worldWidth;                                                                  //width of the world
    public int worldHeight;                                                                 //height of the world
    public List<Palette> tilesList;                                                         //list of all planet types and the different tiles for each type
    public GameObject parent;                                                               //needed to setup the 3 tilemaps in  the hierarchy
    public Tilemap tiles;                                                                   //the main tilemap
    Tilemap leftCopy;                                                                       //left copy (generated in TileMapGen())
    Tilemap rightCopy;                                                                      //right copy (generated in TileMapGen())
    public GameObject player;                                                               //main player
    public GameObject leftPlayer;                                                           //setup the leftcopy of the player in Awake
    public GameObject rightPlayer;                                                          //setup the rightcopy of the player in Awake

    private int maxSurface;                                                                 //Ordonnée de la tile la plus haute générée
    private System.Random rand;                                                             //Random utilisé pour la génération de la planète
    private int[,] mapBase;                                                                 //Double tableau qui sert à générer la map, avant de gen la tilemap
    private Parameters GM;                                                                  //GameObject avec les scripts qui se conservent entre les scènes
    private string planetType;                                                              //Type de la planète (rocheuse, gazeuse, normal)
    private int planeteSeed;                                                                //Seed de la planète (générée lors du lancement du jeu)

    public int getPlaneteSeed()
    {
        return planeteSeed;
    }

    public float getProfondeur()
    {
        return 1 - player.transform.position.y / (float)maxSurface;
    }

    // Use this for initialization
    void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        //planetType = GM.GetComponent<Parameters>().planetType;
        planetType = "classic";
        planeteSeed = GM.getSeedToGen();
        rand = new System.Random(planeteSeed);                                              //setup a seeded random
        TileMapGen();                                                                       //creation of the different tilemaps
        if (player)                                                                         //setup players in the world
        {
            player.transform.Translate(mapBase.GetUpperBound(0) / 2, maxSurface+5, 0);      //middle position player

            /*SI JOUEURS DESYNCH, ÇA VIENT PROBABLEMENT DE LA*/
            leftPlayer.transform.Translate(player.transform.position.x-worldWidth-2, player.transform.position.y, 0);//the left player is on the left map
            rightPlayer.transform.Translate(player.transform.position.x + worldWidth+2, player.transform.position.y, 0);//the right player is on the right map
        }
        Camera.main.transform.Translate(player.transform.position.x, player.transform.position.y,0);
    }

    /*A CHANGER : CHAQUE PLANETE NE DOIS CONNAITRE QU'UNE PALETTE*/
    public TileBase getTileFromPalette(string ptype, int index)
    {
        foreach (Palette p in tilesList)                                                    //for each palette in the game
        {
            if (p.type.Equals(ptype))                                                       //find the one for this planet
            {
                return p.Alltiles[index];                                                   //get the tile using the index
            }
        }

        return null;                                                                        //if not exist, don't put a tile
    }

    //Permet de remplir la tilemap en utilisant le double tableau map
    public void RenderMap(int[,] map, Tilemap tilemap)
    {
        tilemap.ClearAllTiles();                                                            //Clear the map (ensures we dont overlap)
        for (int x = 0; x <= map.GetUpperBound(0); x++)                                     //Parcours horizontale de la map
        {
            for (int y = 0; y <= map.GetUpperBound(1); y++)                                 //Parcours verticale de la map
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
                else if (map[x, y] == 4)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), getTileFromPalette(planetType, 4));//set the water tile (index 4 in the palette)
                }
                else if (map[x, y] == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }
    }

    /*PLUS LENT QUE DE REGEN LA PLANETE
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
    }*/

    //just generate a world sized array to store the map template
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

    //randomly generate ressources
    public void SetRessourcesInMap(int[,] map, int chance)
    {
        bool habitable = GM.isCurrentHabitable();
        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= map.GetUpperBound(1); y++)
            {
                int memChance = chance;
                if (map[x, y] == -1)                                                            //-1 = mur proche d'une grotte --> Plus grande proba de gen une ressource
                {
                    chance *= 30;
                    map[x, y] = 1;
                }
                if (rand.Next(500) < chance && map[x, y] == 1)                                  //On gen une ressource
                {
                    int type;
                    if (!habitable)
                        type = rand.Next(2);                                                    //On sélectionne quelle ressource on génère
                    else
                        type = rand.Next(3);
                    if(type <2)
                        map[x, y] = type + 2;
                    else
                        map[x, y] = 4;
                }
                chance = memChance;
            }
        }
    }

    //Création de chunks de ressources à partir d'une seule ressource générée
    public int[,] SpreadRessourcesInMap(int[,] map)
    {
        int[,] map2 = new int[map.GetUpperBound(0) + 1, map.GetUpperBound(1) + 1];
        for (int i = 0; i <= map.GetUpperBound(0); i++)                                         //Deep copy de la map
        {
            for (int i2 = 0; i2 <= map.GetUpperBound(1); i2++)
                map2[i, i2] = map[i, i2];
        }
        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= map.GetUpperBound(1); y++)
            {
                int type = map[x, y];                                                         //for each tile, 
                if (type != 0 && type != 1)                                                   //if its a ressource (0 = void and 1 = wall)
                {
                    int newX = x, newY = y;
                    int tailleChunck = rand.Next(1, 4);                                       //generate a size of the chunck of those ressources
                    for (int i = 0; i < tailleChunck; i++)
                    {
                        int dir = rand.Next(4);                                               //switch direction of the chunck (upgrade : change direction if a ressource is already here)
                        switch (dir)
                        {
                            case 0: newX--; break;
                            case 1: newX++; break;
                            case 2: newY--; break;
                            case 3: newY++; break;
                        }
                        if (newX > 0 && newY > 0 && newX <= map.GetUpperBound(0) && newY <= map.GetUpperBound(1))//if the tile chosen is in the map (upgrade : change direction if it's not the case)
                            map2[newX, newY] = type;//it becomes a ressource
                    }
                }
            }
        }
        return map2;
    }

    //Génére la surface de la planète (upgrade : faire en sorte que le côté droit et gauche de la map n'ai pas une hauteur trop différente)
    public void RandomWalkTopSmoothed(int[,] map, int minSectionWidth, int maxSectionWidth)
    {
        //Determine the start position
        int lastHeight = rand.Next(map.GetUpperBound(1) / 2) + map.GetUpperBound(1) / 2;
        maxSurface = lastHeight;                                                            //Conserve la hauteur maximale des tiles
        int x = 0;

        while (x <= map.GetUpperBound(0))                                                   //PArcours horizontale de la tilemap
        {
            int sectionWidth = Mathf.Min(map.GetUpperBound(0) - x + 1, rand.Next(minSectionWidth, maxSectionWidth));//Used to generate a random number of tiles at the same height
            int nextMove = rand.Next(2);                                                    //Used to determine which direction to go (0 ->go down / 1 -> go up)
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
            for (int i = 0; i < sectionWidth; i++)                                          //For each tile of this "chunck"
            {
                for (int y = lastHeight; y >= 0; y--)                                       //Fill all tiles bellow the surface
                {
                    map[x + i, y] = 1;
                }
            }
            x += sectionWidth;
        }
    }

    //Lis les changements faits par le joueur lors de ses derniers passages sur la planète pour les réappliquer
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
                if (int.Parse(tmp[3]) == -1)                                                //if the change stored is a break of the tile
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

    /*UPGRADE : LE METTRE DIRECTEMENT DANS RENDERMAP*/
    public void GenerateUnbreakableTiles()
    {
        for (int i = 0; i <= mapBase.GetUpperBound(0); i++)
        {
            tiles.SetTile(new Vector3Int(i, 0, 0), getTileFromPalette(planetType, 3));      //the bottom line of the tilemap becomes unbreakable
        }
    }

    //Calcul un factorielle d'un nombre (nécessaire à la génération de grottes)
    float factorielle(int n)
    {
        int resultat = 1;
        for (int i = 2; i <= n; i++)
            resultat = resultat * i;
        return resultat;
    }

    //Calcul coeff de Beher-truc (pour les courbes de Bézier, utilisé dans la génération de grottes)
    float calculCoeff(int i, int n, float t)
    {
        return (factorielle(n) / (factorielle(i) * factorielle(n - i))) * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i);
    }

    //Creuse dans la tilemap pour générer les grottes en utilisant des courbes de Bézier
    public void GenerateCave(int[,] map, Vector2 ptStart, int chance)
    {
        List<Vector2> TabcontrolPoint = new List<Vector2>();
        int nbControlPoint = 3;                                                             //3 points générés + point de départ = 4 points de contrôle
        TabcontrolPoint.Add(ptStart);
        float minX = 9999999, maxX = -1, minY = 9999999, maxY = -1;                          //Pour calculer la distance connexe-4 entre les deux points les plus éloignés (pour pas générer trop de points)
        for (int i = 0; i < nbControlPoint; i++)                                            //Génération des points de contrôle aléatoirement
        {
            Vector2 v = new Vector2(rand.Next(worldWidth), rand.Next(maxSurface));
            TabcontrolPoint.Add(v);
            if (v.x > maxX) maxX = v.x;
            if (v.x < minX) minX = v.x;
            if (v.y > maxY) maxY = v.y;
            if (v.y < minY) minY = v.y;
        }
        int nbPoints = (int)(maxY - minY + maxX - minX);                                    //Calcul distance connexe 4 entre les deux points les plus éloignés
        Vector2[] tab = new Vector2[nbPoints];
        for (int i = 0; i < nbPoints; i++)                                                  //Calcul des coordonnées de chacuns des points de la courbe de Bézier
        {
            float u = (float)(i) / (float)(nbPoints);
            float x = 0, y = 0;
            for (int i2 = 0; i2 < TabcontrolPoint.Count; i2++)
            {
                x += calculCoeff(i2, TabcontrolPoint.Count - 1, u) * TabcontrolPoint[i2].x;
                y += calculCoeff(i2, TabcontrolPoint.Count - 1, u) * TabcontrolPoint[i2].y;
            }
            tab[i] = new Vector2(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            if (rand.Next(nbPoints * 20) < chance)                                           //A chaque point généré, on a une certaine chance de créer une branche à ce tunel via récursivité.
            {
                GenerateCave(map, tab[i], chance - 5);
            }
        }
        foreach (Vector2 v in tab)                                                          //Pour chaque points de la courbe, on enlève les voisins dans une certaine range
        {
            int range = rand.Next(1, 4);
            for (int x = -1 * range - 2; x <= range+1; x++) 
            {
                for (int y = -1 * range - 2; y <= range+1; y++)
                {
                    if (v.x + x >= 0 && v.x + x <= map.GetUpperBound(0) && v.y + y >= 0 && v.y + y <= map.GetUpperBound(1))
                    {
                        if (x <= -1 * range - 1 || x >= range || y <= -1 * range - 1 || y >= range)  //Si l'une de ces tiles est un mur, on lui attribu une valeur différente pour que ce mur ai plus de chance de générer des ressources.
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

    //Génère tilemap de droite et gauche pour monde torique
    private void CreateCopies()
    {
        rightCopy = Instantiate(tiles, new Vector3(worldWidth + 2, 0, 0), Quaternion.identity, parent.transform);//instantiate left copy 
        leftCopy = Instantiate(tiles, new Vector3(-worldWidth - 2, 0, 0), Quaternion.identity, parent.transform);//instantiate right copy
        leftCopy.name = "left";                                                             //useful to sort tilemaps in another script , dont remove !!!!

        int maxX = rightCopy.cellBounds.xMax - 1;                                           //Memoire de l'absisces des tiles à dupliquer 
        for (int i = 0; i < rightCopy.cellBounds.yMax; i++)                                 //create transparent tiles to smooth the contacts betweeen tilemaps
        {
            rightCopy.SetTile(new Vector3Int(-1, i, 0), rightCopy.GetTile(new Vector3Int(maxX, i, 0))); //On copie la colonne la plus à droite à gauche pour avoir les bonnes transitions avec nos RuleTile
            rightCopy.SetColor(new Vector3Int(-1, i, 0), new Color(1, 1, 1, 0.00000001f));  //On rend la colonne dupliquée transparente
            rightCopy.SetColliderType(new Vector3Int(-1, i, 0), Tile.ColliderType.None);    //On enlève les colisions avec la colonne dupliquée
        }
        maxX = leftCopy.cellBounds.xMax;                                                    //Memoire de l'abscisse de la colonne où on dublique la colonne
        for (int i = 0; i < leftCopy.cellBounds.yMax; i++)                                  //create transparent tiles to smooth the contacts betweeen tilemaps
        {
            leftCopy.SetTile(new Vector3Int(maxX, i, 0), leftCopy.GetTile(new Vector3Int(0, i, 0)));    //On copie la colonne de droite à gauche
            leftCopy.SetColor(new Vector3Int(leftCopy.cellBounds.xMax - 1, i, 0), new Color(1, 1, 1, 0.00000001f));
            leftCopy.SetColliderType(new Vector3Int(leftCopy.cellBounds.xMax - 1, i, 0), Tile.ColliderType.None);
        }

        int avantDerniere = tiles.cellBounds.xMax - 1, derniere = tiles.cellBounds.xMax; ;  //Memoire des abscisses de colonne à dubliquer
        for (int i = 0; i < tiles.cellBounds.yMax; i++)                                     //create transparent tiles to smooth the contacts betweeen tilemaps
        {
            tiles.SetTile(new Vector3Int(-1, i, 0), tiles.GetTile(new Vector3Int(avantDerniere, i, 0)));    //On copie la colonne de droite à gauche
            tiles.SetTile(new Vector3Int(derniere, i, 0), tiles.GetTile(new Vector3Int(0, i, 0)));          //On copie la colonne de gauche à doite

            tiles.SetColor(new Vector3Int(derniere, i, 0), new Color(1, 1, 1, 0.00000001f));
            tiles.SetColliderType(new Vector3Int(derniere, i, 0), Tile.ColliderType.None);
            tiles.SetColor(new Vector3Int(-1, i, 0), new Color(1, 1, 1, 0.00000001f));
            tiles.SetColliderType(new Vector3Int(-1, i, 0), Tile.ColliderType.None);

        }
    }

    //Génération de la tilemap SI ÇA PLANTE C'EST PARCE QUE J'AI PASSE UPDATEMAPS DIRECTEMENT DANS RENDERMAP
    public void TileMapGen()
    {
        mapBase = GenerateArray(worldWidth+2, worldHeight, 0);                                  //create the array
        RandomWalkTopSmoothed(mapBase, 4, 10);                                                  //generate the map using a smooth random walk 
        GenerateCave(mapBase, new Vector2(rand.Next(worldWidth), maxSurface), 20);              //generate caves using bezier
        SetRessourcesInMap(mapBase, 1);                                                         //create ressources randomly
        mapBase = SpreadRessourcesInMap(mapBase);                                               //spread them
        RenderMap(mapBase, tiles);                                                              //render the tilemap using the template
        RenderOldChanges();                                                                     //get old changes and apply them
        GenerateUnbreakableTiles();                                                             //create a line of unbreakable tiles at the bottom (upgrade : à mettre directement dans RenderMap)
        CreateCopies();                                                                         //generate the left and right tilemap for looping world
    }
}
