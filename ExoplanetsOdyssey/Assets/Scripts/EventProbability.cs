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

    string OxyBreak_name  = "event:/saut";
    string FuelBreak_name = "event:/saut";
    string ScanBreak_name = "event:/saut";
    FMOD.Studio.EventInstance OxyBreak;
    FMOD.Studio.EventInstance FuelBreak;
    FMOD.Studio.EventInstance ScanBreak;

    private void Start()
    {
        OxyBreak = FMODUnity.RuntimeManager.CreateInstance(OxyBreak_name);
        FuelBreak = FMODUnity.RuntimeManager.CreateInstance(FuelBreak_name);
        ScanBreak = FMODUnity.RuntimeManager.CreateInstance(ScanBreak_name);
    }

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

        if (O < probaOxy )
        {
            if (SI.GetOxygenTankState() == 1)
            {
                SI.SetOxygenTankState(0);

                OxyBreak.start();
                OxyBreak.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }

        else if (F < probaFuel)
        {
            if (SI.GetFuelTankState() == 1)
            {
                SI.SetFuelTankState(0);

                FuelBreak.start();
                FuelBreak.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }

        else if (S < probaScan)
        {
            if (SI.GetScannerState() == 1)
            {
                SI.SetScannerState(0);

                ScanBreak.start();
                ScanBreak.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }
    }
}
