using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour {

    [FMODUnity.EventRef]
    public string Music = "event:/Music/Planet";

	private int Rand;
	public int diceCheck;
	private Parameters p;
    FMOD.Studio.EventInstance musicEv;
    
	// Use this for initialization
	void Awake () {
		musicEv = FMODUnity.RuntimeManager.CreateInstance(Music);
		p = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
		musicEv.start();
		Rand = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Random.Range(0, 1000) <= diceCheck)
		{
			Rand = Random.Range(0, 100);
			SendRandomChangeToFMOD();
		}

		if (p.deadByOxygen)
		{
			musicEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
	}

	public void SendRandomChangeToFMOD()
	{
		musicEv.setParameterValue("Random", Rand);
	}
}
