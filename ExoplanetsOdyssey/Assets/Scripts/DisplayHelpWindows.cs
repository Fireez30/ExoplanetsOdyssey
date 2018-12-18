using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayHelpWindows : MonoBehaviour
{
    public GameObject window;
    
    // Use this for initialization
    void Awake()
    {
        Parameters p = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        if (p.shipSceneFirstVisit == true)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>().windowsOpened = true;
            window.SetActive(true);
            p.shipSceneFirstVisit = false;
        }
    }

}
