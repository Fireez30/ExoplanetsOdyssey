using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileModif : MonoBehaviour {

    public Tilemap tilemap;
    public Camera cam;

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
	}
}
