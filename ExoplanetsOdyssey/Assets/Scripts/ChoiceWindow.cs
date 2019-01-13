using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceWindow : MonoBehaviour {
    
    [FMODUnity.EventRef]
    public string Music = "event:/journal";
    
    FMOD.Studio.EventInstance journalEv;
    
    public GameObject fenetrechoix;
    bool active = false;
    public List<int> actual;

    public void Awake()
    {
        actual = new List<int>();
        journalEv = FMODUnity.RuntimeManager.CreateInstance(Music);
    }
    
    public void showChoiceWindow()
    {
        if (!active)
        {
            journalEv.start();
        }
        active = !active;
        fenetrechoix.SetActive(active);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>().windowsOpened = active;
    }

    public void HandleChoice(bool o,int id)
    {
        if (o)
        {
            switch (id)
            {
                case -1:
                    actual.Clear();
                    break;
                default:
                    if (!actual.Contains(id))
                    {
                        actual.Add(id);
                    }
                    break;               
            }
        }
        else
        {
            actual.Remove(id);
        }
    }
    
    public void SetChoice(int i)
    {
        if (i == -1)
        {
            actual.Clear();
            actual.Add(-1);
        }
        else if (!actual.Contains(i))
        {
            actual.Add(i);
        }
    }

    public void SendChoice()
    {
        UserChoices gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UserChoices>();
        if (actual.Count == 1 && actual[0] == -1)
        {
            for (int i = 0; i < 3; i++)
            {
                gm.removeChoice(i);
            }
        }
        else
        {
            foreach (int ac in actual)
            {
                gm.addChoice(ac);
            }
        }
        actual.Clear();
        showChoiceWindow();

        GameObject.Find("ObjDisplay").GetComponent<ObjDisplay>().Refresh();
    }
}

