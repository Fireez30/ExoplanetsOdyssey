using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceWindow : MonoBehaviour {
    public GameObject fenetrechoix;
    bool active = false;
    public int actual = -1;

    public void showChoiceWindow()
    {
        if (!active)
            active = true;
        else
            active = false;

        fenetrechoix.SetActive(active);

    }

    public void SetChoice(int i)
    {
        actual = i;
    }

    public void SendChoice()
    {
        if (actual != -1)
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<UserChoices>().addChoice(actual);
        showChoiceWindow();
    }
}

