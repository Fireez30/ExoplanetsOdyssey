using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInventory : MonoBehaviour {

    public int fuelAmount;
    public int ironAmount;
    public int oxygenAmount;
    public List<int> tileAmounts;//index = tile type, value = amount in inventory

	// Use this for initialization
	void Start () {
        for (int i = 0; i < 4; i++)
            tileAmounts.Add(0);
        fuelAmount = 0;
        ironAmount = 0;
        oxygenAmount = 0;
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/player.save"))
        {
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/player.save");
            string[] baseQuantity = new string[2];
            baseQuantity[0] = "0;0;0";
            baseQuantity[1] = "0;0;0;0";
            System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/player.save", baseQuantity);
        }
        else
        {
            string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/player.save");

            string[] tmp = lines[0].Split(';');
            fuelAmount = int.Parse(tmp[0]);
            ironAmount = int.Parse(tmp[1]);
            oxygenAmount = int.Parse(tmp[2]);

            string[] tmp2 = lines[1].Split(';');
            for (int i = 0; i < tmp2.Length; i++)
            {
                tileAmounts[i]=int.Parse(tmp2[i]);
            }
        }
    }
	
	public void computeChangesToFile()
    {
        string[] tmp = new string[2];
        tmp[0] = fuelAmount+";"+ironAmount+";"+oxygenAmount;
        for (int i = 0; i < tileAmounts.Count-1; i++)
            tmp[1] += tileAmounts[i]+";";
        tmp[1] += tileAmounts[tileAmounts.Count - 1];
        System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/player.save", tmp);
    }
}
