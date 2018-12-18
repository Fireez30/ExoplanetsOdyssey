using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRepFenetre : MonoBehaviour
{

	public GameObject wind;

	public void ChangerEtat()
	{
		wind.SetActive(!wind.activeSelf);
	}
}
