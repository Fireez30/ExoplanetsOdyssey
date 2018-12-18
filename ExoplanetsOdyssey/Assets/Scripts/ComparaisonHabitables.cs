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
        lines = new List<string>();
        ComputeLines();
    }

    void Start()
    {
        t = GameObject.Find("Text").GetComponent<Text>();
        foreach (string s in lines)
            t.text += s + '\n';
    }

    void Update()
    {
        //scroll
        t.transform.position = new Vector2(t.transform.position.x, t.transform.position.y+0.005f);
    }

    void ComputeLines()
    {
        lines.Add("Félicitation vous avez choisi les " + planetNumber + " planètes que vous considérez habitables.\n");
        lines.Add("Voyons maintenant ce qui va ou ne va pas dans vos choix :\n");
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
                lines.Add("La planète " + (choices.IndexOf(c) + 1) + " du système " + c.systemName + " est en effet une planète habitable");
                lines.Add("C'est une planète rocheuse");
                lines.Add("Sa température est comprise entre 0 et 80 degrés Celsius");
                lines.Add("Elle a une masse de l'ordre de 10^24");
                lines.Add("On note une présence d'atmosphère respirable\n");
                //lines.Add("Son voisinage cosmique est calme");
                nbCorrects++;
            }
            else
            {
                string s = "La planète " + (choices.IndexOf(c) + 1) + " du système " + c.systemName + " n'est pas une planète habitable, pour les raisons suivantes : \n";

                int seed = param.getSeed(c.systemIndex, c.planetIndex);
                string[] infos = param.getInfo(seed).Split(',');

                if (infos[0] == "Gazeuse")
                    s += "- C'est une planète gazeuse \n";
                if (0 > Int32.Parse(infos[1]))
                    s += "- La planète est trop froide : " + infos[1] + "°\n";
                else if (80 < Int32.Parse(infos[1]))
                    s += "- La planète est trop chaude : " + infos[1] + "°\n";

                if (Int32.Parse(infos[2].Split('^')[1]) == 20)
                    s += "- La planète est trop légère\n";
                else if (Int32.Parse(infos[2].Split('^')[1]) <= 24 && Double.Parse(infos[2].Split('^')[0].Split('*')[0]) < 2.5)
                    s += "- La planète est trop légère\n";
                else if (Int32.Parse(infos[2].Split('^')[1]) == 28)
                    s += "- La planète est trop lourde\n";
                if (infos[3].Contains("pas"))
                    s += "- La planète n'a pas d'atmosphère\n";
                //if (!infos[4].Contains("calme"))
                //    s += infos[4];


                lines.Add(s);
            }
        }
        lines.Add("Vous avez fais " + nbCorrects + " choix corrects sur " + planetNumber + "\n");

        float pourcentage = (float)nbCorrects / (float)planetNumber;

        if (pourcentage <= 0.3f)
            lines.Add("\n Il va falloir s'entrainer un peu plus, pourquoi ne pas faire une nouvelle partie ?");
        else if (pourcentage <= 0.7f && pourcentage > 0.3f)
            lines.Add("Bravo, tu as résolu certains des mystères de l'espace, même si certains restent encore à découvrir !");
        else
            lines.Add("Félicitation, les planètes n'ont plus de secrets pour toi !");

    }
}
