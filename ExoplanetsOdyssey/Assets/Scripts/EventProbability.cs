using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventProbability : MonoBehaviour {

    private ShipInventory SI;

    // Les états du : scanner, réservoir de carburant et d'oxygène sont dans ShipInventory sous forme de 0/1
    public float probaOxy = 5f;
    public float probaFuel = 5f;
    public float probaScan = 5f;


    void checkProbaBreak()
    {
        float O, F, S;
        O = Random.Range(0f, 100f);     // Oxygen
        F = Random.Range(0f, 100f);     // Fuel
        S = Random.Range(0f, 100f);     // Scanner

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
