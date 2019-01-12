using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*UPGRADE : A METTRE DANS PlayerMove POUR PAS AVOIR 500M SCRIPTS DE 10 LIGNES
            AJOUTER UNE VITESSE DE CHUTE MAXIMALE POUR PAS PASSER A TRAVERS LE SOL LORS DE CHUTE TROP LONGUE*/

public class PlayerJump : MonoBehaviour {

	public Rigidbody2D rb;

	[Range(1,10)]
	public float jumpVelocity;
	public float fallMult = 2.5f;
	public float lowMult = 2f;

    public GameObject comp;
    TilesLevelGeneration tlg;
    string nom = "event:/saut";
    FMOD.Studio.EventInstance Saut;
    float profondeur;

    private void Start()
    {
        Saut = FMODUnity.RuntimeManager.CreateInstance(nom);
        tlg = comp.GetComponent<TilesLevelGeneration>();
    }

    // Update is called once per frame
    void FixedUpdate () {

		if( Input.GetButtonDown("Jump") && rb.velocity.y == 0 ) 
		{
			rb.velocity = Vector2.up * jumpVelocity;
            print("prof == " + profondeur);
            profondeur = tlg.getProfondeur();
            Saut.setParameterValue("Saut", profondeur);
            Saut.start();
		}

		if( rb.velocity.y < 0 )
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * Time.fixedDeltaTime;
            Saut.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
		else if( rb.velocity.y > 0 && !Input.GetButton("Jump") )
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowMult - 1) * Time.fixedDeltaTime; 
		}
	}
}
