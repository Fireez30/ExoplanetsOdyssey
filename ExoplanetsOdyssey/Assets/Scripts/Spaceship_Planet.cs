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

    [SerializeField]                           //A enlever parce que ce sera généré aléatoirement et pas défini par nous (juste le serialize)
    private string infoPlaceholder = "";


    void Awake () {
        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        info = GameObject.Find("Canvas").GetComponentInChildren<Text>();
        csvReader = GameObject.Find("CSVReader").GetComponent<CSVReader>();
        if (info == null)
            Debug.LogError("Texte non trouvé");
    }

    void Start()
    {
        infoPlaceholder = csvReader.getPlanetInfo();
    }

    private void OnMouseDown() {
        info.text = infoPlaceholder;
        Debug.Log("Du vaisseau à la planète");
        param.setCurrentPlanet(indexPlanet);
        SceneManager.LoadScene(2);
    }

    public void setIndexPlanet(int i) {
        indexPlanet = i;
    }
}
