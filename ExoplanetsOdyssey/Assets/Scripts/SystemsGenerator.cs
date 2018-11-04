using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemsGenerator : MonoBehaviour {
    public GameObject support;
    public GameObject starTemplate;
    public int nbOfSystems;
    List<GameObject> stars;

    Vector3 sizeOfUniverse;

	// Use this for initialization
	void Start () {

        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/universe.map"))//Is it the first time we generate this system?
        {
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/universe.map").Close();

            stars = new List<GameObject>();
            sizeOfUniverse = support.GetComponent<Renderer>().bounds.extents;//Store bounds of the Prefab positions

            List<Color> starTypes = new List<Color>();
            starTypes.Add(Color.red);
            starTypes.Add(new Color(32 / 255f, 176 / 255f, 232 / 255f));//Light blue
            starTypes.Add(Color.yellow);
            starTypes.Add(new Color(1.0f, 0.64f, 0.0f));//orange

            string[] starnames = { "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Dzeta", "Eta", "Theta", "Iota", "Kappa", "Lambda", "Mu", "Nu", "Xi", "Omicron", "Pi", "Rho", "Sigma", "Tau", "Upsilon", "Phi", "Khi", "Psi", "Omega" };
            List<int> usedIndex = new List<int>();

            while (nbOfSystems > 0)
            {
                float posx = Random.Range(-sizeOfUniverse.x, sizeOfUniverse.x);
                float posy = Random.Range(-sizeOfUniverse.y, sizeOfUniverse.y);
                bool flag = true;
                for (int i = 0; i < stars.Count; i++)
                {
                    if (stars[i].transform.position.x == posx && stars[i].transform.position.y == posy)//If the position is not already used
                    {
                        flag = false;
                    }
                }

                if (flag)//if its not used
                {
                    Vector3 position = new Vector3(posx, posy, 0);
                    GameObject tmp = Instantiate(starTemplate, position, Quaternion.identity);//instantiate the GameObject
                    float scale = Random.Range(0.8f, 1.2f);//must use a variable because random is ..
                    tmp.transform.localScale = new Vector3(scale * tmp.transform.localScale.x, scale * tmp.transform.localScale.y, 0);//Random scale the gameObject
                    int index = Random.Range(0, starTypes.Count);//must use a variable because random is ..
                    tmp.GetComponent<SpriteRenderer>().color = starTypes[index];//Change its color
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

            string[] lines = new string[stars.Count];//Write all stars in file to save
            for (int i = 0; i < stars.Count; i++)
            {
                lines[i] = stars[i].transform.position.x + ";" + stars[i].transform.position.y + ";" + stars[i].transform.localScale.x + ";" + stars[i].transform.localScale.y + ";" + stars[i].GetComponent<SpriteRenderer>().color.r.ToString() + ";" + stars[i].GetComponent<SpriteRenderer>().color.g.ToString() + ";" + stars[i].GetComponent<SpriteRenderer>().color.b.ToString() + ";" + stars[i].name;//Write positions (dont care of z) , scale (dont care of z) , and RGB from the color
            }

            System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/universe.map", lines);
        }
        else //if file already exist, recreate the scene
        {
            string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/universe.map");
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tmp = lines[i].Split(';');
                GameObject GO = Instantiate(starTemplate, new Vector3(float.Parse(tmp[0]), float.Parse(tmp[1]), 0), Quaternion.identity);
                GO.transform.localScale = new Vector3(float.Parse(tmp[2]), float.Parse(tmp[3]), 0);
                GO.GetComponent<SpriteRenderer>().color = new Color(float.Parse(tmp[4]), float.Parse(tmp[5]), float.Parse(tmp[6]),1);
                GO.name = tmp[7];
            }
        }
    }
	

	// Update is called once per frame
	void Update () {
		
	}
}
