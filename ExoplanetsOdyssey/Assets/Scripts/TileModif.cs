using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class TileModif : MonoBehaviour {

    public Tilemap tilemap;
    public List<TileBase> tileList;
    public int currentIndex;
    GameObject GM;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager");
    }
    void Update () {
		if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3Int tilePos = tilemap.WorldToCell(screenPos);

            if (tilemap.GetTile(tilePos))
            {
                tilemap.SetTile(tilePos, null);
                GM.GetComponent<PlanetModificationsSaver>().AddModification(GM.GetComponent<LevelGeneration>().worldseed, GM.GetComponent<Parameters>().planetType,-1,tilePos.x,tilePos.y);
            }
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3Int tilePos = tilemap.WorldToCell(screenPos);

            if (!tilemap.GetTile(tilePos))
            {
                tilemap.SetTile(tilePos, tileList[currentIndex]);
                GM.GetComponent<PlanetModificationsSaver>().AddModification(GM.GetComponent<LevelGeneration>().worldseed, GM.GetComponent<Parameters>().planetType, currentIndex, tilePos.x, tilePos.y);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentIndex != 0)
            {
                currentIndex--;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentIndex < tileList.Count - 1)
            {
                currentIndex++;
            }
            else
            {
                currentIndex = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GM.GetComponent<PlanetModificationsSaver>().computeChangesInFile();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
