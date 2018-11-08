using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInventory : MonoBehaviour {

    private int fuelAmount;
    private int ironAmount;
    private int shipOxygenAmount;                                                        // Not the same than in player inventory !!! Player inventory amount = part of this one

    private int engineState;                                                             // 0 - 100%
    private int tankState;                                                               //0 - 100%
    private int oxygenState;                                                             //0 - 100%

    void Start () {

        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/player.save"))
        {
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/player.save").Close();
            string[] baseQuantity = new string[3];
            baseQuantity[0] = "0;0;0";//fuel, iron , playeroxygen
            baseQuantity[1] = "0;0;0;0";//tiles (not useful for this script)
            baseQuantity[2] = "0;100;100;100";//shipoxygen, engine fuel tank and oxygen tank State
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
