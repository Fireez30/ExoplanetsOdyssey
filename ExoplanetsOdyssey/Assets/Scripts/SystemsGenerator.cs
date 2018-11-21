using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemsGenerator : MonoBehaviour {
    public GameObject support;
    public GameObject starTemplate;
    public Canvas canvas;
    public int nbOfSystems;
    List<GameObject> stars;
    public List<Sprite> starTypes;

	// Use this for initialization
	void Start () {
        List<int> indexs = new List<int>();
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/universe.map"))                        //Is it the first time we generate this system?
        {
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/universe.map").Close();

            stars = new List<GameObject>();

            string[] starnames = { "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Dzeta", "Eta", "Theta", "Iota", "Kappa", "Lambda", "Mu", "Nu", "Xi", "Omicron", "Pi", "Rho", "Sigma", "Tau", "Upsilon", "Phi", "Khi", "Psi", "Omega" };
            List<int> usedIndex = new List<int>();                                                                  //Upgrade : placer les noms de systèmes dans une liste et retirer le nom sélectionné à l'étape actuelle
            while (nbOfSystems > 0)
            {
                float posx = Random.Range(-canvas.transform.position.x, canvas.transform.position.x);                                     //Sélectionne une position aléatoire pour chacun de nos systèmes
                float posy = Random.Range(-canvas.transform.position.y, canvas.transform.position.y);
                bool flag = true;
                for (int i = 0; i < stars.Count; i++)
                {
                    if (stars[i].transform.position.x == posx && stars[i].transform.position.y == posy)             //If the position is not already used
                    {
                        flag = false;
                    }
                }

                if (flag)                                                                                           //if its not used
                {
                    Vector3 position = new Vector3(posx, posy, 0);
                    GameObject tmp = Instantiate(starTemplate, position, Quaternion.identity);                      //instantiate the GameObject
                    SystemInteraction interaction = tmp.GetComponent<SystemInteraction>();
                    interaction.setIndex(nbOfSystems-1);                                                            //Que le système généré connaisse son index pour retrouver les bonnes seeds de planètes via le GameManager

                    float scale = Random.Range(0.8f, 1.2f);                                                         //scale of the system, to create different sizes and not just always the same
                    tmp.transform.localScale = new Vector3(scale * tmp.transform.localScale.x, scale * tmp.transform.localScale.y, 0);//Random scale the gameObject
                    int indexSprite = Random.Range(0, starTypes.Count);//Pour avoir une couleure aléatoire
                    indexs.Add(indexSprite);
                    tmp.GetComponent<SpriteRenderer>().sprite = starTypes[indexSprite];                               //Change its color
                    int nameIndex = Random.Range(0, starnames.Length);
                    int cpt = 0;
                    while (usedIndex.Contains(nameIndex) && cpt < 26)
                    {
                        nameIndex = Random.Range(0, starnames.Length);
                        cpt++;
                    }
                    tmp.name = starnames[nameIndex];
                    stars.Add(tmp);
                    nbOfSystems--;
                }
            }

            string[] lines = new string[stars.Count];                                                               //Write all stars in file to save
            for (int i = 0; i < stars.Count; i++)
            {
                lines[i] = stars[i].transform.position.x + ";" + stars[i].transform.position.y + ";" + stars[i].transform.localScale.x + ";" + stars[i].transform.localScale.y + ";" +
                            indexs[i] + ";" + stars[i].name; //Write positions (dont care of z) , scale (dont care of z) , and index of the sprite
            }

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
            }
        }
    }
}
