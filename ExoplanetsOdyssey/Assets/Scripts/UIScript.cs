using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Modifie les textes dans l'inventaire du joueur
public class UIScript : MonoBehaviour {

    public List<Text> tilesText;                            //Les textes à modifier
    public Text oxygen;                                     //Affiche niveau d'oxygène restant (idéalement via un slider)

    public void Awake()
    {
        foreach (Text t in tilesText)
            t.text = "0";
    }

    public void UpdateNbTiles(int index, int nb)
    {
        tilesText[index].text = "" + nb;
    }
}
