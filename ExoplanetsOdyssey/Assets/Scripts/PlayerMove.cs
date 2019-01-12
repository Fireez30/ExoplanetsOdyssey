using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [FMODUnity.EventRef]
    public string footSount = "event:/Foot";

    FMOD.Studio.EventInstance footEv;


    public Rigidbody2D rb;
	public float speed = 10.0f;
	public bool facingRight = true;

	public float maxVerticalSpeed = -20.0f;
    private bool isPlaying = false;

    private void Start()
    {
        footEv = FMODUnity.RuntimeManager.CreateInstance(footSount);
    }
    // Update is called once per frame
    void FixedUpdate () {
		float move = Input.GetAxis("Horizontal");                   //-1 si joueur appuie sur Q/<- ou 1 si le joueur appuie sur D/->
		rb.velocity = new Vector2 (speed * move, rb.velocity.y);

        if (rb.velocity.y < maxVerticalSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxVerticalSpeed);
        }
        if ( rb.velocity.x > 0 && !facingRight )
		{
			Flip();
		}
		else if( rb.velocity.x < 0 && facingRight )
		{
			Flip();
		}
        if (!isPlaying && rb.velocity.x != 0)
        {
            isPlaying = true;
            footEv.start();
        }
        else if (isPlaying && rb.velocity.x == 0)
        {
            footEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            isPlaying = false;
        }
    }


	void Flip()
	{
		facingRight = !facingRight;
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
	}
	
}
