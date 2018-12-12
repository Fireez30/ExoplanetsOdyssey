using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseShipHelp : MonoBehaviour, IPointerClickHandler
{

    public GameObject helpWindows;
	
	public void OnPointerClick(PointerEventData eventData) { 
        helpWindows.SetActive(!helpWindows.activeSelf);
	}
}
