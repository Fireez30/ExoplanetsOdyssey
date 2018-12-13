using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpdateShipUIState : MonoBehaviour {

    public Image fuel;
    public Image oxygen;
    public Image scanner;

    public Sprite red;
    public Sprite green;

    ShipInventory p;
	// Use this for initialization

	void Awake () {
        p = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>();

        if (p.GetFuelTankState() == 0 && fuel.sprite != red)
        {
            fuel.sprite = red;
        }
        else if (p.GetFuelTankState() == 1 && fuel.sprite != green)
        {
            fuel.sprite = green;
        }

        if (p.GetOxygenTankState() == 0 && oxygen.sprite != red)
        {
            oxygen.sprite = red;
        }
        else if (p.GetOxygenTankState() == 1 && oxygen.sprite != green)
        {
            oxygen.sprite = green;
        }

        if (p.GetScannerState() == 0 && scanner.sprite != red)
        {
            scanner.sprite = red;
        }
        else if (p.GetScannerState() == 1 && scanner.sprite != green)
        {
            scanner.sprite = green;
        }

    }
}
