using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RandomEventWindow : MonoBehaviour {

    public GameObject FuelSlot;
    public GameObject OxygenSlot;
    public GameObject ScannerSlot;

    public Sprite red;
    public Sprite green;

    public Text display;
    ShipInventory p;
    public bool ok = false;


    string OxyBreak_name  = "event:/OxyBreak";
    string FuelBreak_name = "event:/FuelBreak";
    string ScanBreak_name = "event:/ScanBreak";
    FMOD.Studio.EventInstance OxyBreak;
    FMOD.Studio.EventInstance FuelBreak;
    FMOD.Studio.EventInstance ScanBreak;


    // Use this for initialization
    void Awake () {
        p = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>();
	}

    private void Start()
    {
        OxyBreak = FMODUnity.RuntimeManager.CreateInstance(OxyBreak_name);
        FuelBreak = FMODUnity.RuntimeManager.CreateInstance(FuelBreak_name);
        ScanBreak = FMODUnity.RuntimeManager.CreateInstance(ScanBreak_name);
    }

    public bool GetOk()
    {
        return ok;
    }

    public void OK()
    {
        if (ok)
            ok = false;
        else
            ok = true;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>().windowsOpened = false;
        this.gameObject.SetActive(false);
    }

    public void UpdateLights()
    {
        //OxyBreak.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        //FuelBreak.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        //ScanBreak.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        print("hollé holla!");

        if  (p.GetFuelTankState() == 0 && FuelSlot.GetComponent<SpriteRenderer>().sprite != red)
        {
            print("hollé holla! FUEL");

            FuelSlot.GetComponent<SpriteRenderer>().sprite = red;

            FuelBreak.start();
        }
        else if (p.GetFuelTankState() == 1 && FuelSlot.GetComponent<SpriteRenderer>().sprite != green)
        {
            FuelSlot.GetComponent<SpriteRenderer>().sprite = green;
        }

        if (p.GetOxygenTankState() == 0 && OxygenSlot.GetComponent<SpriteRenderer>().sprite != red)
        {
            print("hollé holla! Oxygen");

            OxygenSlot.GetComponent<SpriteRenderer>().sprite = red;

            OxyBreak.start();
        }
        else if (p.GetOxygenTankState() == 1 && OxygenSlot.GetComponent<SpriteRenderer>().sprite != green)
        {
            OxygenSlot.GetComponent<SpriteRenderer>().sprite = green;
        }

        if (p.GetScannerState() == 0 && ScannerSlot.GetComponent<SpriteRenderer>().sprite != red)
        {
            print("hollé holla! SCAN");

            ScannerSlot.GetComponent<SpriteRenderer>().sprite = red;

            ScanBreak.start();
        }
        else if (p.GetScannerState() == 1 && ScannerSlot.GetComponent<SpriteRenderer>().sprite != green)
        {
            ScannerSlot.GetComponent<SpriteRenderer>().sprite = green;
        }

        if (p.GetScannerState() == 0 || p.GetOxygenTankState() == 0 || p.GetFuelTankState() == 0)
        {
            display.text = "Ouch ! Il semblerait qu'au moins un de vos appareils soit endommagé !";
        }
        else
        {
            display.text = "Il n'y a eu aucun soucis durant le voyage jusqu'à cette étoile.";
        }
    }
}
