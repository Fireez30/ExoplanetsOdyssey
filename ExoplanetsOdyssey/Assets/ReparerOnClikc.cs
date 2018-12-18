using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReparerOnClikc : MonoBehaviour
{

	public int outil;
	public GameObject panel;
	private ShipInventory s;

	public GameObject ironAmout;
	// Use this for initialization
	void Awake ()
	{
		s = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>();
	}
	
	// Update is called once per frame
	public void ToRepareOnClick()
	{
			if (outil == 0)
			{
				s.repareScanner();
			}
			else if (outil == 1) //reparer oxygen
			{
				s.repareOxygenTank();
			}
			else if (outil == 2) //reparer carburant
			{
				s.repareFuelTank();
			}
			ironAmout.GetComponent<UpdateAffichageFer>().Refresh();
			panel.GetComponent<RefreshShipLeds>().Refresh();
	}
}
