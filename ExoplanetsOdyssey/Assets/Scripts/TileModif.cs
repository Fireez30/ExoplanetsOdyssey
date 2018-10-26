using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileModif : MonoBehaviour {

    public Tilemap tilemap;
    public Camera cam;
    public List<TileBase> tileList;
    public int currentIndex;

	void Update () {
		if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            Vector3 screenPos = cam.ScreenToWorldPoint(mousePos);
            Vector3Int tilePos = tilemap.WorldToCell(screenPos);

            if (tilemap.GetTile(tilePos))
            {
                tilemap.SetTile(tilePos, null);
            }
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            Vector3 screenPos = cam.ScreenToWorldPoint(mousePos);
            Vector3Int tilePos = tilemap.WorldToCell(screenPos);

            if (!tilemap.GetTile(tilePos))
            {
                tilemap.SetTile(tilePos, tileList[currentIndex]);
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
	}
}
