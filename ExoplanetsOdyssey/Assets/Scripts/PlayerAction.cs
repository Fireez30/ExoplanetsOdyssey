using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour {

	public GameObject Character;
	public OxygenLeak OxyLeak;
	public PlayerInventory PI;
	public int BreathSpeed = 1;
	public int AsphyxiaSpeed = 5;
	public bool HelmetOn = true;
    public Image Img;
    public int seed;

    Parameters param;
    SpriteRenderer casque;

    // Use this for initialization
    void Start () {
		// Je sépare les deux pour plus de flexibilité si jamais faut regler
		InvokeRepeating("Asphyxia", 1, 4); // premier 1 = fonction se déclanche 1 s après l'appel, second 1 = appel toute les 1 secondes
		InvokeRepeating("Breath", 1, 4); // premier 1 = fonction se déclanche 1 s après l'appel, second 1 = appel toute les 1 secondes

        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        seed = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>().getSeedToGen();
        casque = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
		if(HelmetOn == false && CheckAtmosphere(seed) == false)
		{
			OxyLeak.leak = true;
		}
		else{ OxyLeak.leak = false; }

		TakeOff();

        Img.fillAmount = (float)PI.oxygenAmount/100;
        if( PI.oxygenAmount >= 50 )
        {
            Img.color = new Color32(255,255,255,255);
        }
        else if( PI.oxygenAmount < 50 && PI.oxygenAmount >= 25 )
        {
            Img.color = new Color32(255,255,0,255);
        }
        else if ( PI.oxygenAmount < 25 )
        {
            Img.color = new Color32(255, 0, 0, 255);
        }


    }

	void Asphyxia() {
		if(PI.oxygenAmount <= 0)
		{
			// trigger Dead
		}
	}

	void Breath()
	{
		if(HelmetOn == true)
		{
			PI.oxygenAmount -= BreathSpeed;
			if(PI.oxygenAmount < 0) {PI.oxygenAmount = 0;}
		}
	}

	void TakeOff()
	{
		if( Input.GetKeyDown(KeyCode.T) ) 
		{
			HelmetOn = !HelmetOn;
            casque.enabled = HelmetOn;
			//animation de remettage de casque si false -> true ou inverse sinon
		}
	}

    bool CheckAtmosphere(int seed)
    {
        string infoPlaceholder = param.getInfo(seed);
        string[] infos = infoPlaceholder.Split(',');
        string atmos = infos[3];
        if (atmos.Equals(" pas d'atmosphère")) { return false; }
        else { return true; }
        // présence d'atmosphère  ->  true
        // pas d'atmosphère  ->  false
    }
}
