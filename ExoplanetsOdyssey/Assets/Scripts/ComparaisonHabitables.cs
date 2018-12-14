using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComparaisonHabitables : MonoBehaviour {

    List<Choice> choices;
    List<Choice> reals;
    List<string> lines;
    int planetNumber;
    public int nbCorrects;

    private Parameters param;
    private Text t;

	// Use this for initialization
	void Awake () {
        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        choices = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UserChoices>().choices;
        reals = param.habitables;
        planetNumber = param.nbHabitable;
        ComputeLines();
    }

    void Start()
    {
        t = transform.GetComponent<Text>();
        foreach (string s in lines)
            t.text += s + '\n';
    }

    void Update()
    {
        //scroll
        t.transform.localPosition = new Vector2(t.transform.localPosition.x, t.transform.position.y * Time.deltaTime);
    }

    void ComputeLines()
    {
        lines.Add("Félicitations vous avez choisi les " + planetNumber + " planètes que vous considérez habitables.");
        lines.Add("Voyons maintenant ce qui va ou ne va pas dans vos choix :");
        foreach (Choice c in choices)
        {
            bool flag = false;

            foreach (Choice t in reals)
            {
                if (c.planetIndex == t.planetIndex  && c.systemIndex == t.systemIndex)
                {
                    flag = true;
                }
            }

            if (flag)
            {
                lines.Add("La planète " + c.planetName + " du système " + c.systemName + " est en effet une planète habitable");
                nbCorrects++;
            }
            else
            {
                string s = "La planète " + c.planetName + " du système " + c.systemName + " n'est pas une planète habitable, pour les raisons suivantes : ";

                int seed = param.getSeed(c.systemIndex, c.planetIndex);
                string[] infos = param.getInfo(seed).Split(',');

                if (infos[0] == "Gazeuse")
                    s += "elle est gazeuse \n";
                if (20 > Int32.Parse(infos[1]))
                    s += "elle est trop froide : " + infos[1] + "°\n";
                else if (40 < Int32.Parse(infos[1]))
                    s += "elle est trop chaude : " + infos[1] + "°\n";
                if (Int32.Parse(infos[2].Split('^')[1]) == 20)
                    s += "elle est trop légère\n";
                else if (Int32.Parse(infos[2].Split('^')[1]) == 28)
                    s += "elle est trop lourde\n";
                if (infos[3].Contains("pas"))
                    s += "il n'y a pas d'atmosphère\n";
                if (!infos[4].Contains("calme"))
                    s += infos[4];


                lines.Add(s);
            }
        }
        lines.Add("Au final vous avez fais " + nbCorrects + " choix corrects sur " + planetNumber);
        //ajouter les félicitations tout ca tout ca
    }
}
