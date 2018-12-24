using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetEnvironment : MonoBehaviour
{

	public bool isImpacted;
	public int chance;
	public float shakeDuration;
	public float shakeFactor;
	public float decreaseFactor;
	public Camera cam;
	private GameObject gm;
	private Parameters p;
	private ShipInventory s;
	
	// Use this for initialization
	void Awake ()
	{
		gm = GameObject.FindGameObjectWithTag("GameManager");
		p = gm.GetComponent<Parameters>();
		s = gm.GetComponent<ShipInventory>();
		isImpacted = (p.getInfosActual().Split(',')[4] != "environnement proche calme");
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (isImpacted)
		{
			int de = p.getRandomInt(0, 500);
			if (de < chance)
			{
				shakeDuration = 2;
			}

			if (shakeDuration > 0)
			{
				cam.transform.localPosition = Random.insideUnitSphere * shakeFactor;
				shakeDuration -= Time.fixedDeltaTime * decreaseFactor;
			}
			else
			{
				shakeDuration = 0.0f;
			}
		}
	}
}
