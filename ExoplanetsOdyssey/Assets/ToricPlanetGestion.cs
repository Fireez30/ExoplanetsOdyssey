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
    int offset = 10;//change this to change triggers position in the middle tilemap

    void Start()
    {
        Cameras[1].gameObject.SetActive(false);
        Cameras[2].gameObject.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
        if (MainCamera.transform.position.x <= offset && !player.GetComponent<PlayerMove>().facingRight)//If player cross left trigger and he is facing left
        {
            Charas.Sort(new TransformComparer());//sort player to easily get the right most one
            Cameras.Sort(new CameraComparer());
            Cameras[2].gameObject.SetActive(true);
            Cameras[0].gameObject.SetActive(false);
            Cameras[1].gameObject.SetActive(false);
            //MainCamera.GetComponent<Cinemachine.CinemachineBrain>(). = Charas[2];//change camera to go on the right most character
            Charas[0].position = new Vector3(2 * worldWidht - offset, Charas[2].position.y, Charas[2].position.z);//left character has to go to the right now
        }

        if (MainCamera.transform.position.x >= worldWidht - offset && player.GetComponent<PlayerMove>().facingRight)//If player cross right trigger and he is facing right
        {
            Charas.Sort(new TransformComparer());//sort players to easily get the left most one
            Cameras.Sort(new CameraComparer());
            Cameras[0].gameObject.SetActive(true);
            Cameras[2].gameObject.SetActive(false);
            Cameras[1].gameObject.SetActive(false);
            //MainCamera.transform.GetComponent<Cinemachine.CinemachineBrain>().Follow = Charas[0];//camera to go on the left most chaarcter
            Charas[2].position = new Vector3(-2 * worldWidht + offset, Charas[0].position.y, Charas[0].position.z);//right player tp to left of the world
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