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
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/player.save").Close();
            string[] baseQuantity = new string[3];
            baseQuantity[0] = "0;0;0";//fuel, iron , playeroxygen
            baseQuantity[1] = "0;0;0;0";//tiles (not useful for this script)
            baseQuantity[2] = "0;100;100;100";//shipoxygen, engine tank and oxygen State
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
        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/player.save");
        lines[0] = fuelAmount+";"+ironAmount+";"+oxygenAmount;
        lines[1] = "";
        for (int i = 0; i < tileAmounts.Count-1; i++)
            lines[1] += tileAmounts[i]+";";
        lines[1] += tileAmounts[tileAmounts.Count - 1];
        System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/player.save", lines);
    }
}
