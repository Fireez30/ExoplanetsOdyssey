using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToricPlanetGestion : MonoBehaviour {
    public List<Transform> Charas;//List of players transform
    public List<Cinemachine.CinemachineVirtualCamera> Cameras;
    public GameObject MainCamera;//To change camera player's following
    public GameObject player;//to get player's facing, just need 1 because the 3 characters in the scene always face the same
    public int worldWidht;
    public int worldHeight;
    public int currentPlayer = 0;
    void Start()
    {
        Cameras[1].gameObject.SetActive(false);
        Cameras[2].gameObject.SetActive(false);
        Charas.Sort(new TransformComparer());//sort player to easily get the right most one
        Cameras.Sort(new CameraComparer());
        foreach (Transform t in Charas)
        {
            Debug.Log("player name :" + t.gameObject.name);
        }
    }
	// Update is called once per frame
	void Update () {
        if (Charas[1].position.x < 0 && !player.GetComponent<PlayerMove>().facingRight)//If player cross left trigger and he is facing left
        {
            Cameras[2].gameObject.SetActive(true);
            Cameras[1].gameObject.SetActive(false);
            Cameras[0].gameObject.SetActive(false);
            //MainCamera.GetComponent<Cinemachine.CinemachineBrain>(). = Charas[2];//change camera to go on the right most character
            Charas[0].Translate(2 * worldWidht,0,0);//left character has to go to the right now
            Debug.Log("tp gauche -> droite");
            Charas.Sort(new TransformComparer());//sort player to easily get the right most one
            Cameras.Sort(new CameraComparer());
        }

        if (Charas[1].position.x >= worldWidht && player.GetComponent<PlayerMove>().facingRight)//If player cross right trigger and he is facing right
        {

            Cameras[0].gameObject.SetActive(true);
            Cameras[1].gameObject.SetActive(false);
            Cameras[2].gameObject.SetActive(false);
            //MainCamera.transform.GetComponent<Cinemachine.CinemachineBrain>().Follow = Charas[0];//camera to go on the left most chaarcter
            Charas[2].Translate(-2 * worldWidht, 0, 0);//right player tp to left of the world
            Debug.Log("tp droite -> gauche");
            Charas.Sort(new TransformComparer());//sort players to easily get the left most one
            Cameras.Sort(new CameraComparer());
        }
    }
}

class TransformComparer : IComparer<Transform>//Class to compare 2 Transform to sort players in the scene, using their x position
{
    int IComparer<Transform>.Compare(Transform a, Transform b)
    {
        if (a.position.x == b.position.x)
            return 0;
        if (a.position.x > b.position.x)
            return 1;
        else
            return -1;
    }
}

class CameraComparer : IComparer<Cinemachine.CinemachineVirtualCamera>//Class to compare 2 Transform to sort players in the scene, using their x position
{
    int IComparer<Cinemachine.CinemachineVirtualCamera>.Compare(Cinemachine.CinemachineVirtualCamera a, Cinemachine.CinemachineVirtualCamera b)
    {
        if (a.transform.position.x == b.transform.position.x)
            return 0;
        if (a.transform.position.x > b.transform.position.x)
            return 1;
        else
            return -1;
    }
}