using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToricPlanetGestion : MonoBehaviour
{
    public List<Transform> Charas; //List of players transform
    public List<Cinemachine.CinemachineVirtualCamera> Cameras; //Liste des caméras

    public GameObject
        player; //to get player's facing, just need 1 because the 3 characters in the scene always face the same way

    public int worldWidht; //Size of the world in x coordinate
    public int worldHeight; //Size of the world in y coordinate

    void Start()
    {
        Cameras[1].gameObject.SetActive(false);
        Cameras[2].gameObject.SetActive(false);
        Charas.Sort(new TransformComparer()); //sort player to easily get the right most one
        Cameras.Sort(new CameraComparer());
    }
/*
    // Update is called once per frame
    void FixedUpdate()
    {
         (Charas[1].position.x < 0 && !player.GetComponent<PlayerMove>().facingRight)             //If player cross left trigger and he is facing left
        {
            Cameras[2].gameObject.SetActive(true);//camera droite
            Cameras[1].gameObject.SetActive(false);
            Cameras[0].gameObject.SetActive(false);                                                 //A priori inutile
            Charas[0].Translate(2 * worldWidht,0,0);                                                //left character has to go to the right now
            Charas.Sort(new TransformComparer());                                                   //sort player to easily get the right most one
            Cameras.Sort(new CameraComparer());
        }

        if (Charas[1].position.x >= worldWidht && player.GetComponent<PlayerMove>().facingRight)    //If player cross right trigger and he is facing right
        {//cam 0 camera droite 
            Cameras[2].gameObject.SetActive(true);
            Cameras[1].gameObject.SetActive(false);
            Cameras[0].gameObject.SetActive(false);                                                 //A priori inutile
            Charas[2].Translate(-3 * worldWidht, 0, 0);                                             //right player tp to left of the world
            Charas.Sort(new TransformComparer());                                                   //sort players to easily get the left most one
            Cameras.Sort(new CameraComparer());
        }
    }
*/

//Class to compare 2 Transform to sort players in the scene, using their x position
    class TransformComparer : IComparer<Transform>
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

//Class to compare 2 Transform to sort players in the scene, using their x position
    class CameraComparer : IComparer<Cinemachine.CinemachineVirtualCamera>
    {
        int IComparer<Cinemachine.CinemachineVirtualCamera>.Compare(Cinemachine.CinemachineVirtualCamera a,
            Cinemachine.CinemachineVirtualCamera b)
        {
            if (a.transform.position.x == b.transform.position.x)
                return 0;
            if (a.transform.position.x > b.transform.position.x)
                return 1;
            else
                return -1;
        }
    }
}