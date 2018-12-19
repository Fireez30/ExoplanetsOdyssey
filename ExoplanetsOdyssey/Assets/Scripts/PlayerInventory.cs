using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInventory : MonoBehaviour {

    public ShipInventory SI;
    public int fuelAmount;
    public int ironAmount;
    public int oxygenAmount;
    public List<int> tileAmounts;                                                                       //index = tile type, value = amount in inventory

	// Use this for initialization
	void Start () {
        SI = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>();

        for (int i = 0; i < 4; i++)
        tileAmounts.Add(0);
        fuelAmount = 0;
        ironAmount = 0;
        // oxygenAmount = 0;
        SI.shipOxygenAmount -= oxygenAmount;
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/saves/player.save"))             //Si le fichier n'existe pas on le crée
        {
            System.IO.File.Create(Application.streamingAssetsPath + "/saves/player.save").Close();
            string[] baseQuantity = new string[3];
            baseQuantity[0] = "0;0;0";                                                                  //fuel, iron , playeroxygen
            baseQuantity[1] = "0;0;0;0";                                                                //tiles (not useful for this script)
            baseQuantity[2] = "0;100;100;100";                                                          //shipoxygen, engine fuel tank and oxygen tank State
            System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/player.save", baseQuantity);
        }
        else
        {                                                                                               //Sinon on le lis
            string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/player.save");

            string[] tmp = lines[0].Split(';');
            fuelAmount = 0;
            ironAmount = 0;
            oxygenAmount = int.Parse(tmp[2]);

            string[] tmp2 = lines[1].Split(';');
            for (int i = 0; i < tmp2.Length; i++)
            {
                tileAmounts[i]=int.Parse(tmp2[i]);
            }
        }
        GameObject ui = GameObject.Find("Canvas");                                                      //On affiche le nombre de tiles que le joueur possède à l'initialisation de la scène de la planète
        if (ui)
        {
            UIScript canvas = ui.GetComponent<UIScript>();
            canvas.UpdateNbTiles(0, tileAmounts[0]);
            canvas.UpdateNbTiles(1, fuelAmount);
            canvas.UpdateNbTiles(2, ironAmount);
        }
        else
        {
            Debug.LogError("Aucun canvas trouvé dans la scène de la planète - PlayerInventory line 44");
        }
        OxygenIntake();
    }
	
    //Sauvegarde l'inventaire actuel du joueur (à appeler au retour dans le vaisseau)
	public void computeChangesToFile()
    {
        string[] lines = System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/saves/player.save");
        lines[0] = fuelAmount+";"+ironAmount+";"+oxygenAmount;
        lines[1] = "";
        for (int i = 0; i < tileAmounts.Count-1; i++)
            lines[1] += tileAmounts[i]+";";
        lines[1] += tileAmounts[tileAmounts.Count - 1];
        System.IO.File.WriteAllLines(Application.streamingAssetsPath + "/saves/player.save", lines);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>().ReadFile();
    }

    public void OxygenIntake()
    {
        oxygenAmount = 100;
        if (SI.shipOxygenAmount >= 100)
        {
            if (gameObject.name == "Character")
            {
                SI.shipOxygenAmount -= 100;
            }
            oxygenAmount = 100;
        }
        else
        {
            oxygenAmount = SI.shipOxygenAmount;
            SI.shipOxygenAmount = 0;
        }
    }

    public void ressourcesBackToShip()
    {
        if (gameObject.name == "Character")
        {
            SI.fuelAmount += fuelAmount;
            SI.ironAmount += ironAmount;
            SI.shipOxygenAmount += oxygenAmount;
            oxygenAmount = 0;
            fuelAmount = 0;
            ironAmount = 0;
        }
    }
}
