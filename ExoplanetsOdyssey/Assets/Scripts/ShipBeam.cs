using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipBeam : MonoBehaviour
{
    public GameObject player;
    public GameObject gest;
    
    public void Beam()                          //add coroutine for animation
    {
        
        gest.GetComponent<PlanetModificationsSaver>().computeChangesInFile();
        player.GetComponent<PlayerInventory>().computeChangesToFile();
        SceneManager.LoadScene(2);
    }
}
