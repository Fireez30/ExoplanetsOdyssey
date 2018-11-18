using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniverseBeam : MonoBehaviour {

	public void BeamtoUniverse()
    {
        SceneManager.LoadScene(0);
    }
}
