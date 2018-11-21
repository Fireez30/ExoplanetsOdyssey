using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemInteraction : MonoBehaviour {

    Text text;
    
    private int indexSystem;
    private Parameters param;

    private static int nb = 0;

    // Use this for initialization
    void Awake () {
        text = GameObject.FindGameObjectWithTag("SystemInfos").GetComponent<Text>();
        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
	}
	
	// Affiche le nom du système quand on passe sa souris dessus
	void OnMouseEnter () {
        Vector3 pos = gameObject.transform.position;
        pos.z = 1;
        text.transform.position = pos;
        text.GetComponent<Text>().text = gameObject.name;
        text.enabled = true;
    }

    //Fait disparaitre le nom du système quand la souris n'est plus dessus
    void OnMouseExit()
    {
        text.enabled = false;
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

}
