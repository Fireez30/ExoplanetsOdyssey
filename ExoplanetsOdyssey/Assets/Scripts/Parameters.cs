using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters : MonoBehaviour {

    public float actualPlanet;
    public string planetType;
    public static Parameters Instance;

    

	// Use this for initialization
	void Awake () {
        if (Instance == false)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }


    }

}
