using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IronDisplay : MonoBehaviour
{
    public Text t;

    // Use this for initialization
    void Awake()
    {
        int iron = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipInventory>().ironAmount;
        t.text = "Fer : " + iron;
        t.color = Color.white;
    }
}
