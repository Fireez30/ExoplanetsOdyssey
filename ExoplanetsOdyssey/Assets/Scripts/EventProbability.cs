using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventProbability : MonoBehaviour {

    private ShipInventory SI;
    private Parameters p;

    // Les états du : scanner, réservoir de carburant et d'oxygène sont dans ShipInventory sous forme de 0/1
    public float probaOxy = 5f;
    public float probaFuel = 5f;
    public float probaScan = 5f;

    void Awake()
    {
        SI = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>();
        p = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
    }

    public void checkProbaBreak()
    {
        float O, F, S;
        O = p.getRandomInt(0,100);     // Oxygen
        F = p.getRandomInt(0,100);     // Fuel
        S = p.getRandomInt(0,100);     // Scanner
        Debug.Log("O= " + O + " F= "+F+" S= "+S);
        Debug.Log("probaOxy = " + probaOxy + " probaFuel = "+probaFuel + " probaScan = "+probaScan);
        if (O < probaOxy )
        {
            if ( SI.GetOxygenTankState() == 1)
            {
                SI.SetOxygenTankState(0);
            }
        }

        else if (F < probaFuel)
        {
            if (SI.GetFuelTankState() == 1)
            {
                SI.SetFuelTankState(0);
            }
        }

        else if (S < probaScan)
        {
            if (SI.GetScannerState() == 1)
            {
                SI.SetScannerState(0);
            }
        }
    }
}
