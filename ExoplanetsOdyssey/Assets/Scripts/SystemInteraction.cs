using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemInteraction : MonoBehaviour {

    GameObject text;
    
    private SystemsGenerator generator;
    private int indexSystem;
    private Parameters param;

	// Use this for initialization
	void Awake () {
        text = GameObject.FindGameObjectWithTag("SystemInfos");
        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
	}
	
	// Update is called once per frame
	void OnMouseEnter () {
        text.transform.position = Input.mousePosition;
        text.GetComponent<Text>().text = gameObject.name;
        text.SetActive(true);
    }

    void OnMouseExit()
    {
        text.SetActive(false);
    }

    private void OnMouseDown()
    {
        Debug.Log("Du système vers le vaisseau ");
        param.setCurrentSystem(indexSystem);
        SceneManager.LoadScene(1);
        //Change de scène pour aller dans le vaisseau
    }

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
