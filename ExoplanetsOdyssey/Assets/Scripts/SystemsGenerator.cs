using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemsGenerator : MonoBehaviour {
    public GameObject support;
    public GameObject ship;
    public GameObject starTemplate;
    public Canvas canvas;
    public int nbOfSystems;
    List<GameObject> stars;
    public List<Sprite> starTypes;
    public List<string> starNames;
    public int xMin, xMax, yMin, yMax;

    private Parameters GM;

    void Start () {
        Debug.Log("Start gen systeme");
        List<int> indexs = new List<int>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        stars = new List<GameObject>();
        nbOfSystems = GM.nbSystem;
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/universe.map"))                        //Is it the first time we generate this system?
        {
            Debug.Log("Pas de fichier de sauvegarde");
            for (int i=0; i<nbOfSystems;i++)
            {
                Debug.Log("Création systeme "+i+" sur "+nbOfSystems);
                float posx = GM.getRandomFloat(xMin, xMax);                                     //Sélectionne une position aléatoire pour chacun de nos systèmes
                float posy = GM.getRandomFloat(yMin,yMax);
                int i2 = 0;
                while(i2<stars.Count && posx == stars[i2].transform.position.x && posy == stars[i2].transform.position.y)
                {
                    if(posx == stars[i2].transform.position.x && posy == stars[i2].transform.position.y)
                    {
                        posx = GM.getRandomFloat(xMin,xMax);                                     //Sélectionne une position aléatoire pour chacun de nos systèmes
                        posy = GM.getRandomFloat(yMin,yMax);
                        i2 = 0;
                    }
                    else
                        i2++;
                }
                Debug.Log("Position choisie");
                Vector3 position = new Vector3(posx, posy, 0);
                GameObject tmp = Instantiate(starTemplate, position, Quaternion.identity);                      //instantiate the GameObject
                tmp.GetComponent<SystemInteraction>().setIndex(i);
                //Que le système généré connaisse son index pour retrouver les bonnes seeds de planètes via le GameManager

                Debug.Log("Après instantiation");
                float scale = Random.Range(0.8f, 1.2f);                                                         //scale of the system, to create different sizes and not just always the same
                tmp.transform.localScale = new Vector3(scale * tmp.transform.localScale.x, scale * tmp.transform.localScale.y, 0);//Random scale the gameObject
                int indexSprite = Random.Range(0, starTypes.Count);//Pour avoir une couleure aléatoire
                indexs.Add(indexSprite);
                tmp.GetComponent<SpriteRenderer>().sprite = starTypes[indexSprite];                               //Change its color
                tmp.name = starNames[Random.Range(0, starNames.Count)];
                starNames.Remove(tmp.name);
                stars.Add(tmp);
            }

            string[] lines = new string[stars.Count];                                                               //Write all stars in file to save
            for (int i = 0; i < stars.Count; i++)
            {
                lines[i] = stars[i].transform.position.x + ";" + stars[i].transform.position.y + ";" + stars[i].transform.localScale.x + ";" + stars[i].transform.localScale.y + ";" +
                            indexs[i] + ";" + stars[i].name + stars[i].GetComponent<SystemInteraction>().indexSystem; //Write positions (dont care of z) , scale (dont care of z) , and index of the sprite
            }
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/universe.map").Close();
            Debug.Log("File has been created");
            System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/universe.map", lines);
        }
        else                                                                                                        //if file already exist, recreate the scene
        {
            string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/universe.map");
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tmp = lines[i].Split(';');
                GameObject GO = Instantiate(starTemplate, new Vector3(float.Parse(tmp[0]), float.Parse(tmp[1]), 0), Quaternion.identity);
                GO.transform.localScale = new Vector3(float.Parse(tmp[2]), float.Parse(tmp[3]), 0);
                GO.GetComponent<SpriteRenderer>().sprite = starTypes[int.Parse(tmp[4])];
                GO.name = tmp[5];
                GO.GetComponent<SystemInteraction>().setIndex(i);
                stars.Add(GO);
            }
        }

        int c = GM.currentSystem;
        if (c != -1)
        {
            ship.transform.position = stars[c].transform.position;
        }

    }
}
