using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemInteraction : MonoBehaviour {

    GameObject text;
    
    private SystemsGenerator generator;                     //A priori inutile
    private int indexSystem;
    private Parameters param;

	// Use this for initialization
	void Awake () {
        text = GameObject.FindGameObjectWithTag("SystemInfos");
        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
	}
	
	// Affiche le nom du système quand on passe sa souris dessus
	void OnMouseEnter () {
        text.transform.position = Input.mousePosition;
        text.GetComponent<Text>().text = gameObject.name;
        text.SetActive(true);
    }

    //Fait disparaitre le nom du système quand la souris n'est plus dessus
    void OnMouseExit()
    {
        text.SetActive(false);
    }

    private void OnMouseDown()
    {
        param.setCurrentSystem(indexSystem);                                    //Pour que le GameManager sache quel système a été sélectionné (pour récupérer la bonne seed de planète)
        SceneManager.LoadScene(1);                                              //Vers le vaisseau
    }

    //POur que le système connaisse son index
    public void setIndex(int i)
    {
        indexSystem = i;
    }

    //A priori inutile
    public void setGenerator(SystemsGenerator g)
    {
        generator = g;
    }
}
