using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInventory : MonoBehaviour {

    public int fuelAmount;
    public int ironAmount;
    public int shipOxygenAmount;// Not the same than in player inventory !!! Player inventory amount = part of this one

    public int engineState;// 0 - 100%
    public int tankState;//0 - 100%
    public int oxygenState;//0 - 100%

    // Use this for initialization
    void Start () {
        fuelAmount = 0;
        ironAmount = 0;
        shipOxygenAmount = 0;
        engineState = 100;
        tankState = 100;
        oxygenState = 100;

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

            string[] tmp2 = lines[2].Split(';');
            shipOxygenAmount = int.Parse(tmp2[0]);
            engineState = int.Parse(tmp2[1]);
            tankState = int.Parse(tmp2[2]);
            oxygenState = int.Parse(tmp2[3]);
        }
    }

    public void computeChangesToFile()
    {
        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/player.save");
        string[] tmp = lines[0].Split(';');
        lines[0] = fuelAmount + ";" + ironAmount + ";" + tmp[2];
        lines[2] = shipOxygenAmount + ";" + engineState + ";" + tankState + ";" + oxygenState;
        System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/player.save", lines);
    }

}
