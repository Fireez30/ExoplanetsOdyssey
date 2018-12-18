using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReparerOnClikc : MonoBehaviour
{

	public int outil;
	public GameObject panel;
	private ShipInventory s;

    private RefreshShipLeds r;

	public GameObject ironAmout;
	// Use this for initialization
	void Awake ()
	{
		s = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>();
        r = panel.GetComponent<RefreshShipLeds>();
	}
	
	// Update is called once per frame
	public void ToRepareOnClick()
	{
        if (outil == 0)
		{
			s.repareScanner();
            r.Refresh();
		}
		else if (outil == 1) //reparer oxygen
		{
			s.repareOxygenTank();
            r.Refresh();
        }
		else if (outil == 2) //reparer carburant
		{
			s.repareFuelTank();
            r.Refresh();    
        }

		ironAmout.GetComponent<UpdateAffichageFer>().Refresh();
		panel.GetComponent<RefreshShipLeds>().Refresh();
	}
}
