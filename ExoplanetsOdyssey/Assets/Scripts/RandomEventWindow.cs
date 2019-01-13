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
    // Use this for initialization
    void Awake () {
        p = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>();
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
        if  (p.GetFuelTankState() == 0 && FuelSlot.GetComponent<SpriteRenderer>().sprite != red)
        {
            FuelSlot.GetComponent<SpriteRenderer>().sprite = red;
        }
        else if (p.GetFuelTankState() == 1 && FuelSlot.GetComponent<SpriteRenderer>().sprite != green)
        {
            FuelSlot.GetComponent<SpriteRenderer>().sprite = green;
        }

        if (p.GetOxygenTankState() == 0 && OxygenSlot.GetComponent<SpriteRenderer>().sprite != red)
        {
            OxygenSlot.GetComponent<SpriteRenderer>().sprite = red;
        }
        else if (p.GetOxygenTankState() == 1 && OxygenSlot.GetComponent<SpriteRenderer>().sprite != green)
        {
            OxygenSlot.GetComponent<SpriteRenderer>().sprite = green;
        }

        if (p.GetScannerState() == 0 && ScannerSlot.GetComponent<SpriteRenderer>().sprite != red)
        {
            ScannerSlot.GetComponent<SpriteRenderer>().sprite = red;
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
