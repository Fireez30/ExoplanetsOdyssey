using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipBeam : MonoBehaviour
{
    public GameObject player;
    public GameObject gest;
    public MusicControl t;


    public void Beam()                          //add coroutine for animation
    {
        gest.GetComponent<PlanetModificationsSaver>().computeChangesInFile();
        t.StopMusic();
        SceneManager.LoadScene(2);
    }
}
