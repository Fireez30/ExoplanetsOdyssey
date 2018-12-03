using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComparaisonHabitables : MonoBehaviour {

    List<Choice> choices;
    List<Choice> reals;
    List<string> lines;
    int planetNumber;
    public int nbCorrects;
	// Use this for initialization
	void Awake () {
        choices = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UserChoices>().choices;
        reals = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>().habitables;
        planetNumber = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>().nbHabitable;
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
                s += "test";
                //ajouter ici les test et construction du texte pour EXPLIQUER le choix a l'utilisateur
                lines.Add(s);
            }
        }

        lines.Add("Au final vous avez fais " + nbCorrects + " choix corrects sur " + planetNumber);
        //ajouter les félicitations tout ca tout ca
    }
}
