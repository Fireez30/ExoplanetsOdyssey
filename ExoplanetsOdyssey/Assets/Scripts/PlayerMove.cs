using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	public Rigidbody2D rb;
	public float speed = 10.0f;
	bool facingRight = true;
    
	// Update is called once per frame
	void FixedUpdate () {
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
