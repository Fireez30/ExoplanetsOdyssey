using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipInteraction : MonoBehaviour {

    public GameObject planetPrefab;
    public int xMin, xMax;

    private List<int> seedPlanets;
    private Parameters param;

	// Use this for initialization
	void Awake () {
        param = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Parameters>();
        seedPlanets = param.getAllSeedsSystem();
        for(int i =0;i<seedPlanets.Count;i++)
        {
            Vector3 pos = new Vector3(xMin + (i / (float)seedPlanets.Count) * (xMax - xMin), 2, 0);
            GameObject temp = Instantiate(planetPrefab, pos, Quaternion.identity);
            temp.GetComponent<Spaceship_Planet>().setIndexPlanet(i);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
