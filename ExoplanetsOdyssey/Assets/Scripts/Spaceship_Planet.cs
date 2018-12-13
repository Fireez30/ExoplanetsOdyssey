using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*Une planète dans la scène de sélection dans le vaisseau*/
public class Spaceship_Planet : MonoBehaviour {

    private Text info;
    private Parameters param;                   //Script dans le GameManager
    private int indexPlanet;
    private CSVReader csvReader;
    private Image bar;
    private Text loadtxt;
    private int seed;
    private Text temp;
    [SerializeField]                           //A enlever parce que ce sera généré aléatoirement et pas défini par nous (juste le serialize)
    private string infoPlaceholder = "";


    void Awake () {
        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        info = GameObject.FindGameObjectWithTag("planettype").GetComponent<Text>();
        temp = GameObject.FindGameObjectWithTag("planettemperature").GetComponent<Text>();
        csvReader = GameObject.Find("CSVReader").GetComponent<CSVReader>();
        bar = GameObject.FindGameObjectWithTag("loadingimage").GetComponent<Image>();
        loadtxt = GameObject.FindGameObjectWithTag("loadingtext").GetComponent<Text>();

        if (info == null)
            Debug.LogError("Texte non trouvé");
    }

    void Start()
    {
        infoPlaceholder = param.getTypes()[seed];   
    }

    private void OnMouseOver()
    {
        string[] infos = infoPlaceholder.Split(',');
        info.text = infos[0];
        temp.text = infos[1]+"°";
        Debug.Log("Info : " + infoPlaceholder);
    }
    private void OnMouseDown() {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("planet");
        param.setCurrentPlanet(indexPlanet,planets[indexPlanet].name);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation result = SceneManager.LoadSceneAsync(2);

        while (!result.isDone)
        {
            float progress = Mathf.Clamp01(result.progress / 0.9f);
            bar.fillAmount = progress;
            loadtxt.text = "Chargement en cours : " + (progress * 100) + " %";
            yield return null;
        }
    }

    public void setIndexPlanet(int i) {
        indexPlanet = i;
    }

    public void setSeed(int s) {
        seed = s;
    }
}
