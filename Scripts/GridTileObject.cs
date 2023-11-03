using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileObject : MonoBehaviour
{

    public int x;
    public int y;

    public int g;
    public int f;
    public int h;

    public bool isWalkable;
    public GridTileObject cameFromTile;
    //move
    public Vector3 pos;

    public void UpdateF()
    {
        f = g + h;
    }
}
