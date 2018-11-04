using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemInteraction : MonoBehaviour {

    GameObject text;
	// Use this for initialization
	void Start () {
        text = GameObject.FindGameObjectWithTag("SystemInfos");
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
}
