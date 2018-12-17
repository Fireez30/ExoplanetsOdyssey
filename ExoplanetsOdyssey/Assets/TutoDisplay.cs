using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoDisplay : MonoBehaviour {

    public GameObject window;
    // Use this for initialization
    void Awake()
    {
        Parameters p = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        if (p.universeSceneFirstVisit == true)
        {
            window.SetActive(true);
            p.universeSceneFirstVisit = false;
        }
    }

}
