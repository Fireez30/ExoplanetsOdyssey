using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters : MonoBehaviour {

    public string actualPlanet;//seed de la map
    public string planetType;
    public static Parameters Instance;

    

	// Use this for initialization
	void Awake () {
        if (!Instance)
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
