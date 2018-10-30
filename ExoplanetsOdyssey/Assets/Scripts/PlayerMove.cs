using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	Rigidbody2D rb;
	public float speed = 10.0f;
	bool facingRight = true;


	// Use this for initialization
	void Start () {
		rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		float move = Input.GetAxis("Horizontal");
		rb.velocity = new Vector2 (speed * move, rb.velocity.y);

		if( rb.velocity.x > 0 && !facingRight )
		{
			Flip();
		}
		else if( rb.velocity.x < 0 && facingRight )
		{
			Flip();
		}
	}

	void Flip()
	{
		facingRight = !facingRight;

		Vector3 scale = transform.localScale;

		scale.x *= -1;

		transform.localScale = scale;
	}
	
}
