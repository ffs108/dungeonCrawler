using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectToEachGrid : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] walkables = GameObject.FindGameObjectsWithTag("Walkable");
        Debug.Log(walkables.Length);
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach(GameObject walkable in walkables) {
            walkable.AddComponent<GridTileObject>();
            GridTileObject ObjScript = walkable.GetComponent<GridTileObject>();
            ObjScript.isWalkable = true;
            //move
            ObjScript.pos = walkable.transform.position;
        }

        foreach (GameObject wall in walls)
        {
            wall.AddComponent<GridTileObject>();
            GridTileObject ObjScript = wall.GetComponent<GridTileObject>();
            ObjScript.isWalkable = false;
        }

    }

}
