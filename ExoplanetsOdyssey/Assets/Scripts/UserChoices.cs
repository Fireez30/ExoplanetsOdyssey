using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserChoices : MonoBehaviour
{

    public List<Choice> choices;
    public GameObject endWindows;
    private bool signalFin;
    
    // Use this for initialization
    void Awake()
    {
        choices = new List<Choice>();
        endWindows = GameObject.FindGameObjectWithTag("fenetrefin");
        signalFin = false;
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
        c2.systemName = this.gameObject.GetComponent<Parameters>().currentSystemName;
        c2.planetIndex = planet;
        c2.planetName = this.gameObject.GetComponent<Parameters>().currentPlanetName;
        choices.Add(c2);//add the actual planet as a choice

        if (choices.Count == GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>().nbHabitable)
        {
            endWindows.gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            endWindows.gameObject.transform.position = new Vector3(endWindows.gameObject.transform.position.x, endWindows.gameObject.transform.position.y, 0);
            StartCoroutine(AttenteFin());
        }
    }


    public IEnumerator AttenteFin()
    {
        yield return new WaitUntil(() => signalFin);
        SceneManager.LoadScene(4);//scene fin
    }

    public void setFin()
    {
        signalFin = true;
    }
    
    public void removeChoice(int planet)
    {
        foreach (Choice c in choices)//Cycle through already made choice
        {
            if (c.systemIndex == this.gameObject.GetComponent<Parameters>().currentSystem && c.planetIndex == planet
            ) //if choice already done
                choices.Remove(c);
        }
    }
}

[System.Serializable]
public struct Choice
{
    public int systemIndex;
    public int planetIndex;
    public string systemName;
    public string planetName;

    public Choice(int systemIndex, int planetIndex,string planet, string system)
    {
        this.systemIndex = systemIndex;
        this.planetIndex = planetIndex;
        this.planetName = planet;
        this.systemName = system;
    }

    public Choice(int systemIndex, int planetIndex)
    {
        this.systemIndex = systemIndex;
        this.planetIndex = planetIndex;
        this.planetName = "";
        this.systemName = "";
    }
    public override bool Equals(object obj)
    {
        if (!(obj is Choice))
            return false;

        Choice c = (Choice)obj;
        return c.systemIndex == this.systemIndex && c.planetIndex == this.planetIndex;

    }
}
