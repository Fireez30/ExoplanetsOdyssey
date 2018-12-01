using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserChoices : MonoBehaviour
{

    public List<Choice> choices;

    // Use this for initialization
    void Awake()
    {
        choices = new List<Choice>();
    }

    public void addChoice(int planet)
    {
        foreach (Choice c in choices)//Cycle through already made choice
        {
            if (c.systemIndex == this.gameObject.GetComponent<Parameters>().currentSystem && c.planetIndex == planet)//if choice already done
                return;//do nothing
        }
        Choice c2 = new Choice();
        c2.systemIndex = this.gameObject.GetComponent<Parameters>().currentSystem;
        c2.planetIndex = planet;
        choices.Add(c2);//add the actual planet as a choice
    }

    public void removeChoice(int index)
    {
        if (!choices[index].Equals(null))
        {
            choices.RemoveAt(index);
        }
    }
}

[System.Serializable]
public struct Choice
{
    public int systemIndex;
    public int planetIndex;

    public Choice(int systemIndex, int planetIndex)
    {
        this.systemIndex = systemIndex;
        this.planetIndex = planetIndex;
    }
}
