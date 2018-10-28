using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters : MonoBehaviour {

    public float actualPlanet;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        actualPlanet = 0;
	}

}
