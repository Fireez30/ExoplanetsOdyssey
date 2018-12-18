using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipBeam : MonoBehaviour
{
    public void Beam()                          //add coroutine for animation
    {
        SceneManager.LoadScene(2);
    }
}
