﻿using System.Collections;
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
    private ShipInventory inv;
    private Text temp;
    private Text mass;
    [SerializeField]                           //A enlever parce que ce sera généré aléatoirement et pas défini par nous (juste le serialize)
    private string infoPlaceholder = "";


    void Awake () {
        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        info = GameObject.FindGameObjectWithTag("planettype").GetComponent<Text>();
        temp = GameObject.FindGameObjectWithTag("planettemperature").GetComponent<Text>();
        mass = GameObject.FindGameObjectWithTag("planetemasse").GetComponent<Text>();
        csvReader = GameObject.Find("CSVReader").GetComponent<CSVReader>();
        bar = GameObject.FindGameObjectWithTag("loadingimage").GetComponent<Image>();
        inv = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>();
        loadtxt = GameObject.FindGameObjectWithTag("loadingtext").GetComponent<Text>();

        if (info == null)
            Debug.LogError("Texte non trouvé");
    }

    void Start()
    {
        infoPlaceholder = param.getInfo(seed);   
    }

    private void OnMouseOver()
    {
        if (inv.GetScannerState() == 0)
        {
            info.text = "ERROR";
            temp.text = "ERROR";
            mass.text = "ERROR";
        }
        else
        {
            string[] infos = infoPlaceholder.Split(',');
            info.text = infos[0];
            temp.text = infos[1] + "°";
            mass.text = infos[2] + "kg";
        }

       // Debug.Log("Info : " + infoPlaceholder);
    }
    private void OnMouseDown() {
        if (!param.windowsOpened)
        {
            GameObject[] planets = GameObject.FindGameObjectsWithTag("planet");
            param.setCurrentPlanet(indexPlanet, planets[indexPlanet].name);
            string[] infos = infoPlaceholder.Split(',');
            if (infos[0] == "Gazeuse") //type = gazeuse
            {
                //start couroutine dans gameManager
                param.ToGazeuse();
            }
            else
            {
                StartCoroutine(LoadScene());
            }
        }
    }

    IEnumerator LoadScene()
    {
        AsyncOperation result = SceneManager.LoadSceneAsync(3);

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
