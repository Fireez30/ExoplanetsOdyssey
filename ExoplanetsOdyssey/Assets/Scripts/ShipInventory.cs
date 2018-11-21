using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInventory : MonoBehaviour {

    private int fuelAmount;
    private int ironAmount;
    private int shipOxygenAmount;                                                        // Not the same than in player inventory !!! Player inventory amount = part of this one

    private int scannerState;                                                             // 0 - 1
    private int fuelTankState;                                                               //0 - 1
    private int oxygenTankState;                                                             //0 - 1

    void Start () {

        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/player.save"))
        {
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/player.save").Close();
            string[] baseQuantity = new string[3];
            baseQuantity[0] = "0;0;0";//fuel, iron , playeroxygen
            baseQuantity[1] = "0;0;0;0";//tiles (not useful for this script)
            baseQuantity[2] = "0;1;1;1";//shipoxygen, scanner, fuel tank and oxygen tank State
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
            scannerState = int.Parse(tmp2[1]);
            fuelTankState = int.Parse(tmp2[2]);
            oxygenTankState = int.Parse(tmp2[3]);
        }
    }

    public void computeChangesToFile()
    {
        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/player.save");
        string[] tmp = lines[0].Split(';');
        lines[0] = fuelAmount + ";" + ironAmount + ";" + tmp[2];
        lines[2] = shipOxygenAmount + ";" + scannerState + ";" + fuelTankState + ";" + oxygenTankState;
        System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/player.save", lines);
    }

}
