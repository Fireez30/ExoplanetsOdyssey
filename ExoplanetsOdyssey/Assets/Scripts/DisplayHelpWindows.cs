using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHelpWindows : MonoBehaviour
{
    public GameObject window;
    // Use this for initialization
    void Awake()
    {
        Parameters p = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        if (p.shipSceneFirstVisit == true)
        {
            window.SetActive(true);
            p.shipSceneFirstVisit = false;
        }
    }

}
