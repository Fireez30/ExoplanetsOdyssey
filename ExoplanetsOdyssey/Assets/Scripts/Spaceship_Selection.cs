using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spaceship_Selection : MonoBehaviour {

    public string name;

    void OnMouseDown()
    {
        SceneManager.LoadScene(name);
    }
}
