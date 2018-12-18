using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAffichageFer : MonoBehaviour
{
	public Text iron;
	private ShipInventory s;
	
	// Use this for initialization
	void Awake ()
	{
		s = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>();
		Refresh();
	}
	
	public void Refresh ()
	{
		iron.text = "Fer : "+s.ironAmount;
	}
}
