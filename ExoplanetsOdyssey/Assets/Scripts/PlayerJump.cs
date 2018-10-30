﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {

	public Rigidbody2D rb;

	[Range(1,10)]
	public float jumpVelocity;
	public float fallMult = 2.5f;
	public float lowMult = 2f;
    
	// Update is called once per frame
	void FixedUpdate () {

		if( Input.GetButtonDown("Jump") && rb.velocity.y == 0 ) 
		{
			rb.velocity = Vector2.up * jumpVelocity;
		}

		if( rb.velocity.y < 0 )
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * Time.fixedDeltaTime; 
		}
		else if( rb.velocity.y > 0 && !Input.GetButton("Jump") )
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowMult - 1) * Time.fixedDeltaTime; 
		}
	}
}
