using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour {

	public GameObject Character;

	public OxygenLeak OxyLeak;

	public PlayerInventory PI;

	public int HealthPoint;
	public int BreathSpeed = 1;
	public int AsphyxiaSpeed = 5;

	public bool HelmetOn = true;

	// Use this for initialization
	void Start () {
		HealthPoint = 100;
		// Je sépare les deux pour plus de flexibilité si jamais faut regler
		InvokeRepeating("Asphyxia", 1, 4); // premier 1 = fonction se déclanche 1 s après l'appel, second 1 = appel toute les 1 secondes
		InvokeRepeating("Breath", 1, 4); // premier 1 = fonction se déclanche 1 s après l'appel, second 1 = appel toute les 1 secondes

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(HelmetOn == false)
		{
			OxyLeak.leak = true;
		}
		else{ OxyLeak.leak = false; }

		TakeOff();		
	}

	void Asphyxia() {
		if(PI.oxygenAmount == 0)
		{
			HealthPoint -= AsphyxiaSpeed;
			if(HealthPoint < 0)
			{
				HealthPoint = 0;
				//Dead();
			}
		}
	}

	void Breath()
	{
		if(HelmetOn == true)
		{
			PI.oxygenAmount -= BreathSpeed;
			if(PI.oxygenAmount < 0) {PI.oxygenAmount = 0;}
		}
	}

	void TakeOff()
	{
		if( Input.GetKeyDown(KeyCode.T) ) 
		{
			HelmetOn = !HelmetOn;
			//animation de remettage de casque si false -> true ou inverse sinon
		}
	}
}
