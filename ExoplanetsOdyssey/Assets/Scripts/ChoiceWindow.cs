using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ChoiceWindow : MonoBehaviour {
    public GameObject fenetrechoix;
    bool active = false;
    public List<int> actual;

    public void Awake()
    {
        actual = new List<int>();
    }
    
    public void showChoiceWindow()
    {
        active = !active;
        fenetrechoix.SetActive(active);

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

