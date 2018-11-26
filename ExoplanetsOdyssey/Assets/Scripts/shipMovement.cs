using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class shipMovement : MonoBehaviour {

    public float speed;
    public Rigidbody2D rb;
    public float timerConsoCarbu;

    public int moveCost;
    private float timer = 0;

	// Update is called once per frame
	void FixedUpdate () {
  /*      float moveH = Input.GetAxis("Horizontal"), moveV = Input.GetAxis("Vertical");                                                                //-1 si joueur appuie sur Q/<- ou 1 si le joueur appuie sur D/->
        if (moveH != 0 || moveV != 0)
        {
            Vector2 force = new Vector2(speed * moveH * Time.fixedDeltaTime, speed * moveV * Time.fixedDeltaTime);
            force.Normalize();                                                                                                                          //Vecteur normalisé pour pas aller plus vite en allant en diagonale
            rb.AddForce(force);
        }
        if(rb.velocity.magnitude > 0.1)                                                                                                              //Si le vaisseau est pas immobile, on enlève du carburant
        {
            timer += Time.fixedDeltaTime;
            if(timer > timerConsoCarbu)
            {
                timer = 0;
                Debug.Log("PErte de 1 de carburant");
            }
        }
        */


    }

    public int calculateCost(int indexSystem)
    {
        float distance = (this.gameObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).magnitude;
        moveCost = 0;
        //calculate the moveCost here
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>().GetFuelTankState() == 0)//if fuel tank not working
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>().setCurrentSystem(indexSystem);                                    //Pour que le GameManager sache quel système a été sélectionné (pour récupérer la bonne seed de planète)
            SceneManager.LoadScene(1);                                              //Vers le vaisseau
            moveCost += moveCost / 10;//
        }

        return moveCost;
    }
}
