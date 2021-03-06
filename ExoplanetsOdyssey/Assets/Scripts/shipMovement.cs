﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class shipMovement : MonoBehaviour {

    public float speed;
    public Vector3 heading;
    public int moveCost;
    //public Rigidbody2D rb;
    //public float timerConsoCarbu;
    
    [FMODUnity.EventRef]
    public string movement = "event:/ShipMovement";
    FMOD.Studio.EventInstance moveEv;
    
    private bool moving = false;
    Transform theTarget;

    void Start()
    {
        moveEv = FMODUnity.RuntimeManager.CreateInstance(movement);
    }
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
        if (moving)
        {
            heading = theTarget.position - gameObject.transform.position;
            transform.Translate(heading * Time.fixedDeltaTime * speed);
        }

    }

    public void MoveTo(Transform target)
    {
        heading = target.position - gameObject.transform.position;
        theTarget = target;
        StartCoroutine(Move());
    }

    public bool getMoving()
    {
        return moving;
    }

    IEnumerator Move()
    {
        
        
        moving = true;
        moveEv.start();
        yield return new WaitUntil(() => ((transform.position - theTarget.position).magnitude <= 0.2));
        moveEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        moving = false;
    }

    public int calculateCost(int indexSystem)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject != null)
            {
                float distance = (this.gameObject.transform.position - hit.transform.position).magnitude;//world distance
                moveCost = ((int)distance);//change move cost
                if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>().GetFuelTankState() == 0)//if fuel tank not working
                {                                          //Vers le vaisseau
                    moveCost *= 2;//if tank damaged, +10% cost
                }
            }

        }
        return moveCost;
    }
}
