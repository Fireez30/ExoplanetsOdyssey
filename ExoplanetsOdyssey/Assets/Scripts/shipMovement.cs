using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipMovement : MonoBehaviour {

    public float speed;
    public Rigidbody2D rb;
    public float timerConsoCarbu;

    private float timer = 0;
	// Update is called once per frame
	void FixedUpdate () {
        float moveH = Input.GetAxis("Horizontal"), moveV = Input.GetAxis("Vertical");                                                                //-1 si joueur appuie sur Q/<- ou 1 si le joueur appuie sur D/->
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
    }
}
