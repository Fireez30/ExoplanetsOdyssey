using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenLeak : MonoBehaviour {

	public int LeakSpeed = 5;	// x% d'oxygène partent par seconde si il y a une fuite
	public PlayerInventory PI;
	
	public bool leak = false;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("Loss", 1, 1); // premier 1 = fonction se déclanche 1 s après l'appel, second 1 = appel toute les 1 secondes
	}

	void Loss() {
		if(leak == true)
		{
			PI.oxygenAmount -= LeakSpeed;
			if(PI.oxygenAmount < 0) {PI.oxygenAmount = 0;}
		}
	}
}
