using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public GameObject Tuto;

	// Use this for initialization
	void Start () {
		if ( GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>().tutorial == false )
        {
            Tuto.SetActive(true);
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>().tutorial = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
