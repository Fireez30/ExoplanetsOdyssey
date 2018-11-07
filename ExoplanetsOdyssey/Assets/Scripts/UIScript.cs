using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    public List<Text> tilesText;
    public Text oxygen;

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
