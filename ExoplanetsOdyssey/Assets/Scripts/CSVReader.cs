using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour {

    public TextAsset csvFile;

    private string[] infos;

	void Start () {
        if(csvFile != null)
            infos = csvFile.text.Split('\n');
	}
	
	public string getPlanetInfo()
    {
        if(infos != null) {
            if (infos.Length > 0)
            {
                int r = Random.Range(0, infos.Length);
                return infos[r];
            }
        }
        return "";
    }
}
