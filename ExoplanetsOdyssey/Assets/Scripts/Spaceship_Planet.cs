using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Spaceship_Planet : MonoBehaviour {

    private Text info;
    private Parameters param;
    private int indexPlanet;

    [SerializeField]
    private string infoPlaceholder = "";


    void Awake () {
        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        info = GameObject.Find("Canvas").GetComponentInChildren<Text>();
        if (info == null)
            Debug.LogError("Texte non trouvé");
    }
	
	void OnMouseOver() {
        if (Input.GetMouseButtonDown(0))        //<--- A placer dans le mouseDown
            info.text = infoPlaceholder;
    }
    private void OnMouseDown()
    {
        param.setCurrentPlanet(indexPlanet);
        SceneManager.LoadScene(3);
    }
    public void setIndexPlanet(int i) {
        indexPlanet = i;
    }
}
