using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour {

    [FMODUnity.EventRef]
    public string Music = "event:/Music/Planet";
	
	[FMODUnity.EventRef]
	public string Vent = "event:/ventGrotte";
    
    public TilesLevelGeneration t;
    
	private int Rand;
	public int diceCheck;
	private Parameters p;
    FMOD.Studio.EventInstance musicEv;

	FMOD.Studio.EventInstance ventEv;
	// Use this for initialization
	void Awake () {
		musicEv = FMODUnity.RuntimeManager.CreateInstance(Music);
		ventEv = FMODUnity.RuntimeManager.CreateInstance(Vent);
		p = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
		musicEv.start();
		ventEv.start();
		Rand = 0;
	}

	public void StopMusic()
	{
		musicEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		ventEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Random.Range(0, 1000) <= diceCheck)
		{
			Rand = Random.Range(0, 100);
			SendRandomChangeToFMOD();
		}
		
        musicEv.setParameterValue("Profondeur", t.getProfondeur());
		ventEv.setParameterValue("Profondeur", t.getProfondeur());
        
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
