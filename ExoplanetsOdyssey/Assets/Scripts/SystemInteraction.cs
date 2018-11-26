using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemInteraction : MonoBehaviour {

    GameObject fenetre;
    
    public int indexSystem;
    private Parameters param;
    private Text system;
    private Text cost;
    int costvalue = 0;
    private int offset = 30;
    private static int nb = 0;
    GameObject ship;
    // Use this for initialization
    void Awake () {
        fenetre = GameObject.FindGameObjectWithTag("SystemInfos");
        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        cost = GameObject.FindGameObjectWithTag("cout").GetComponent<Text>();
        system = GameObject.FindGameObjectWithTag("universewintext").GetComponentInChildren<Text>();
        ship = GameObject.FindGameObjectWithTag("ship");
    }
	
	// Affiche le nom du système quand on passe sa souris dessus
	void OnMouseEnter () {
        //Vector3 pos = gameObject.transform.position;
        Vector3 pos = Input.mousePosition;
        pos.z = 1;
        if (Input.mousePosition.y > Screen.height/2)
        {
            if (Input.mousePosition.x > Screen.width / 2)
            {
                pos.x -= offset;
                pos.y -= offset;
            }
            else
            {
                pos.x += offset;
                pos.y -= offset;
            }
        }
        else
        {
            if (Input.mousePosition.x > Screen.width / 2)
            {
                pos.x -= offset;
                pos.y += offset;
            }
            else
            {
                pos.x += offset;
                pos.y += offset;
            }
        }
        costvalue = ship.GetComponent<shipMovement>().calculateCost(indexSystem);
        fenetre.transform.position = Camera.main.ScreenToWorldPoint(pos);
        system.text = gameObject.name;
        cost.text = ""+costvalue;
        fenetre.SetActive(true);
    }

    //Fait disparaitre le nom du système quand la souris n'est plus dessus
    void OnMouseExit()
    {
        fenetre.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (costvalue < GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>().fuelAmount)
       {
            param.setCurrentSystem(indexSystem);                                    //Pour que le GameManager sache quel système a été sélectionné (pour récupérer la bonne seed de planète)
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>().fuelAmount -= costvalue;
            SceneManager.LoadScene(1);                                              //Vers le vaisseau
        }
        else {
            //user feedback
        }
    }

    //POur que le système connaisse son index
    public void setIndex(int i)
    {
        indexSystem = i;
    }

}
