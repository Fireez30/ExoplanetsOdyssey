using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {

	Rigidbody2D rb;

	[Range(1,10)]
	public float jumpVelocity;
	public float fallMult = 2.5f;
	public float lowMult = 2f;

	// Use this for initialization
	void Start () {
		rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

		if( Input.GetButtonDown("Jump") && rb.velocity.y == 0 ) 
		{
			rb.velocity = Vector2.up * jumpVelocity;
		}

		if( rb.velocity.y < 0 )
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * Time.deltaTime; 
		}
		else if( rb.velocity.y > 0 && !Input.GetButton("Jump") )
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowMult - 1) * Time.deltaTime; 
		}
	}
}
