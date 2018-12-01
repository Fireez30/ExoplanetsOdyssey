using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelDisplay : MonoBehaviour {
    public Text t;

	// Use this for initialization
	void Awake () {
        int fuel = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>().fuelAmount;
        t.text = "Carburant : " + fuel;
        t.color = Color.white;
	}
}
