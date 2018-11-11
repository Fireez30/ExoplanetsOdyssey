using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour {

    public TextAsset csvFile;

    private string[] infos;

	void Start () {
        infos = csvFile.text.Split('\n');
	}
	
	public string getPlanetInfo()
    {
        int r = Random.Range(0, infos.Length);
        return infos[r];
    }
}
