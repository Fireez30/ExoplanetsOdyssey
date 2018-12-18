using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjDisplay : MonoBehaviour
{
    public Text t;

    // Use this for initialization
    void Awake()
    {
        List<Choice> obj = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UserChoices>().choices;
        t.text = "Planètes choisies : " + obj.Count + "/5";
        t.color = Color.white;
    }

    public void Refresh()
    {
        List<Choice> obj = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UserChoices>().choices;
        t.text = "Planètes choisies : " + obj.Count + "/5";
        t.color = Color.white;
    }
}
