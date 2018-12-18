using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	public void LoadScene () {
        //Setup GameManager
        SceneManager.LoadScene(1);//load universe
	}
	
}
