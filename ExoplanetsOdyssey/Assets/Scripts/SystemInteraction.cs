using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemInteraction : MonoBehaviour {

    GameObject fenetre;
    EventProbability ep;
    
    public int indexSystem;
    private Parameters param;
    private Text system;
    private Text cost;
    int costvalue = 0;
    private int offset = 30;
    private static int nb = 0;
    shipMovement ship;

    RandomEventWindow eventW;
    // Use this for initialization
    void Awake () {
        fenetre = GameObject.FindGameObjectWithTag("SystemInfos");
        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        cost = GameObject.FindGameObjectWithTag("cout").GetComponent<Text>();
        system = GameObject.FindGameObjectWithTag("universewintext").GetComponentInChildren<Text>();
        ship = GameObject.FindGameObjectWithTag("ship").GetComponent<shipMovement>();
        eventW = GameObject.FindGameObjectWithTag("eventdisplay").GetComponent<RandomEventWindow>();

        ep = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventProbability>();
    }

    // Affiche le nom du système quand on passe sa souris dessus
    void OnMouseEnter()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 1;
        if (Input.mousePosition.y > Screen.height / 2)
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
            
        fenetre.transform.position = Camera.main.ScreenToWorldPoint(pos);
        if (param.firstMove)
        {
            costvalue = 0;
        }
        else
        {
            costvalue = ship.calculateCost(indexSystem);
            if (costvalue == 0 && (indexSystem != param.currentSystem)) costvalue = 1; 
        }
        system.text = gameObject.name;
        cost.text = costvalue + "";
        fenetre.SetActive(true);
    }

    //Fait disparaitre le nom du système quand la souris n'est plus dessus
    void OnMouseExit()
    {
        cost.color = Color.white;
        fenetre.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!ship.getMoving())
            {
                if (costvalue <= GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>().fuelAmount)
                {
                    if (indexSystem != param.currentSystem)//check if mouse is on a different system
                    {
                        ep.checkProbaBreak();                    
                    }
                    ship.MoveTo(this.gameObject.transform);
                    StartCoroutine(Transport());
                }
                else
                {
                    Debug.Log(costvalue + " / " + GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>().fuelAmount);
                    cost.color = Color.red;
                }
            }
    }

    private IEnumerator Transport()
    {
        eventW.UpdateLights();
        yield return new WaitUntil(() => !ship.getMoving());
        if (indexSystem != param.currentSystem)//check if mouse is on a different system
        {
            eventW.gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            eventW.gameObject.transform.position = new Vector3(eventW.gameObject.transform.position.x, eventW.gameObject.transform.position.y, 0);
            yield return new WaitUntil(() => eventW.GetOk());
        }  
        if (param.firstMove)//first move is free
        {
            param.setCurrentSystem(indexSystem,gameObject.name);
            param.firstMove = false;
            SceneManager.LoadScene(2);
        }
        else
        {
            param.setCurrentSystem(indexSystem,gameObject.name);                                    //Pour que le GameManager sache quel système a été sélectionné (pour récupérer la bonne seed de planète)
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>().fuelAmount -= costvalue;
            SceneManager.LoadScene(2); //Vers le vaisseau
        }
    }

    //POur que le système connaisse son index
    public void setIndex(int i)
    {
        indexSystem = i;
    }

}
