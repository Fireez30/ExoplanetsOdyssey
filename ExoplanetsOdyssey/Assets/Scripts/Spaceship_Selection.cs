using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spaceship_Selection : MonoBehaviour {

    public string name;                 //Préférer changer de scène avec un int, plus robuste que le nom d ela scène qui risque de changer

    void OnMouseDown()
    {
        SceneManager.LoadScene(name);
    }
}
