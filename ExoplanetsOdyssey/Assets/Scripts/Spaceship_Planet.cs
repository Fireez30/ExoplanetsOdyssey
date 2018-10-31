using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spaceship_Planet : MonoBehaviour {

    private Text info;

    [SerializeField]
    private string infoPlaceholder = "";


    void Start () {
        info = GameObject.Find("Canvas").GetComponentInChildren<Text>();
        if (info == null)
            Debug.LogError("Texte non trouvé");
    }
	
	void OnMouseOver() {
        if (Input.GetMouseButtonDown(0))
            info.text = infoPlaceholder;
    }
}
