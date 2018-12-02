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

    RandomEventWindow eventW;
    // Use this for initialization
    void Awake () {
        fenetre = GameObject.FindGameObjectWithTag("SystemInfos");
        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        cost = GameObject.FindGameObjectWithTag("cout").GetComponent<Text>();
        system = GameObject.FindGameObjectWithTag("universewintext").GetComponentInChildren<Text>();
        ship = GameObject.FindGameObjectWithTag("ship");
        eventW = GameObject.FindGameObjectWithTag("eventdisplay").GetComponent<RandomEventWindow>();
    }

    // Affiche le nom du système quand on passe sa souris dessus
    void OnMouseEnter()
    {
        Debug.Log("Mouse entered");
        //Vector3 pos = gameObject.transform.position;
        if (indexSystem != param.currentSystem)//check if mouse is on a different system
        {
                //if it's
                Debug.Log("We are not the same");
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
                costvalue = ship.GetComponent<shipMovement>().calculateCost(indexSystem);
                if (costvalue == 0){ costvalue = 1; }
                fenetre.transform.position = Camera.main.ScreenToWorldPoint(pos);
                if (param.firstMove)
                {
                    costvalue = 0;
                }
                system.text = gameObject.name;
                cost.text = costvalue + "";
                fenetre.SetActive(true);
        }
    }

    //Fait disparaitre le nom du système quand la souris n'est plus dessus
    void OnMouseExit()
    {
        cost.color = Color.white;
        fenetre.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!ship.GetComponent<shipMovement>().moving)
        {
            if (costvalue <= GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>().fuelAmount)
            {
                ship.GetComponent<shipMovement>().MoveTo(this.gameObject.transform);
                StartCoroutine(Transport());
                eventW.gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                eventW.gameObject.transform.position = new Vector3(eventW.gameObject.transform.position.x, eventW.gameObject.transform.position.y, 0);
            }
            else
            {
                cost.color = Color.red;
            }
        }
    }

    private IEnumerator Transport()
    {
        eventW.UpdateLights();
        yield return new WaitUntil(() => !ship.GetComponent<shipMovement>().getMoving() && eventW.GetOk());
        if (param.firstMove)//first move is free
        {
            param.setCurrentSystem(indexSystem);
            param.firstMove = false;
            SceneManager.LoadScene(1);
        }
        else
        {
            param.setCurrentSystem(indexSystem);                                    //Pour que le GameManager sache quel système a été sélectionné (pour récupérer la bonne seed de planète)
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>().fuelAmount -= costvalue;
            SceneManager.LoadScene(1); //Vers le vaisseau
        }
    }

    //POur que le système connaisse son index
    public void setIndex(int i)
    {
        indexSystem = i;
    }

}
