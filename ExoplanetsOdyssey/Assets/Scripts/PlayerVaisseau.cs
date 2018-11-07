using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVaisseau : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed;

	void Start () {
		
	}
	
	void Update () {
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        transform.Rotate(0, rotation, 0);
    }
}
