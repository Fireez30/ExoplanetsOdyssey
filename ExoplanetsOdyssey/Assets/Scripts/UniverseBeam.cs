﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniverseBeam : MonoBehaviour {

	public void BeamtoUniverse()
	{
	    GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>().windowsOpened = false;
        SceneManager.LoadScene(1);
    }
}
