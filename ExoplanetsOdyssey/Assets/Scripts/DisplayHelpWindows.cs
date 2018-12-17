using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayHelpWindows : MonoBehaviour
{
    public GameObject window;
    public GameObject gazeuseWindows;
    
    // Use this for initialization
    void Awake()
    {
        Parameters p = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        if (p.shipSceneFirstVisit == true)
        {
            window.SetActive(true);
            p.shipSceneFirstVisit = false;
        }

        if (p.comeFromGazeuse)
        {
            p.comeFromGazeuse = false;
            gazeuseWindows.SetActive(true);
            ShipInventory s = p.gameObject.GetComponent<ShipInventory>();
            var de = p.getRandomInt(0, 2);
            if (de == 0 && s.GetScannerState() == 0)
            {
                s.SetScannerState(1);
            }
            else if (de == 1 && s.GetScannerState() == 0)
            {
                s.SetScannerState(1);
            }
            else if (de == 2 && s.GetScannerState() == 0)
            {
                s.SetScannerState(1);
            }
        }
    }

}
