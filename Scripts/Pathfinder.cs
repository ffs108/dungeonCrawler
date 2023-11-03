using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private List<GridTileObject> openList;
    private List<GridTileObject> closedList;


    // The same 2d list but in gameObject form, in case
    public List<List<GridTileObject>> nodeList;

    // Prints the gameObject List
    private void PrintGameObjectList()
    {
        string representativeRow = "";
        for (int i = 0; i < nodeList.Count(); i++)
        {
            representativeRow += "[";
            for (int j = 0; j < nodeList[i].Count(); j++)
            {
                if (nodeList[i][j].isWalkable == false)
                {
                    representativeRow += "X";
                }
                else if (nodeList[i][j].isWalkable == true)
                {
                    representativeRow += "0";
                }
                else
                {
                    representativeRow += "?";
                }
            }
            representativeRow += "]\n";

        }
        Debug.Log(representativeRow);
    }

    // Start is called before the first frame update
    void Start()
    {
        nodeList = new List<List<GridTileObject>>();
        // When creating the grid, it assumes 2 things:
        // 1. The objects are on a 4x4 scale, that is one unit of the list is roughly 4 (whatever units unity uses) long.
        // 2. The labyrinth as a whole is connected. The code would not work if you attached a child gameObject to the labyrinth
        //    that wasn't connected to it as a whole, and would simply assume it was based on what row it's in.

        // These are the bounds that the labyrinth is constrained to. Only the x coordinate really matters, but it
        // only depends on how it's oriented.
        int leftMostX = 2;
        int rightMostX = 102;
        int lowestZ = 2;
        int highestZ = 78;

        List<GridTileObject> gs = new List<GridTileObject>();
        GridTileObject[] ts = gameObject.GetComponentsInChildren<GridTileObject>();
        foreach(GridTileObject t in ts)
        {
            if(t != null && t.gameObject != null && t.gameObject.name != "Labyrinth")
            {
                gs.Add(t.gameObject.GetComponent<GridTileObject>());
            }
        }
        // Loops through entire array multiple times to separate each row of the array
        // based on x value. Not very efficient, but gets the job done.
        for(int i = highestZ; i >= lowestZ; i -= 4)
        {
            List<GridTileObject> newList = new List<GridTileObject>();
            foreach (GridTileObject g in gs)
            {
                if(g.transform.position.z == i)
                {
                    newList.Add(g);
                }
            }
            newList = newList.OrderBy(x => Vector3.Distance(x.transform.position, Vector3.zero)).ToList();
            nodeList.Add(newList);
        }
        // Uncomment if you want the mapList to be printed.
        //printMapList();
        PrintGameObjectList();
        UpdateGridVals();
    }

    private void UpdateGridVals()
    {
        for (int i = 0; i < nodeList.Count(); i++)
        {
            for (int j = 0; j < nodeList[i].Count(); j++)
            {
                nodeList[i][j].x = i;
                nodeList[i][j].y = j;

                nodeList[i][j].g = 9999999;
                nodeList[i][j].UpdateF();
                nodeList[i][j].cameFromTile = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GridTileObject> FindPath(int startX, int startY, int endX, int endY)
    {
        GridTileObject startNode = nodeList[startX][startY];
        GridTileObject endNode = nodeList[endX][endY];

        openList = new List<GridTileObject> { startNode };
        closedList = new List<GridTileObject>();

        UpdateGridVals();

        startNode.g = 0;
        startNode.h = CalculateDist(startNode, endNode);
        startNode.UpdateF();

        // A* loop 
        GridTileObject curNode = startNode;
        closedList.Add(curNode);
        while (!(curNode.x == endNode.x && curNode.y == endNode.y))
        {
            //Debug.Log("hi");
            foreach (GridTileObject n in GetNeighbourList(curNode.x, curNode.y))
            {
                if (!n.isWalkable)
                {
                    continue;
                }
                if (closedList.Contains(n))
                {
                    continue;
                }
                else if (openList.Contains(n))
                {
                    //compute new_g
                    int new_g = curNode.g + 1;
                    if (new_g < n.g)
                    {
                        n.cameFromTile = curNode;
                        n.g = new_g;
                        n.UpdateF();
                    }
                }
                else
                {
                    n.cameFromTile = curNode;
                    n.h = CalculateDist(n, endNode);
                    n.g = curNode.g + 1;
                    n.UpdateF();
                    openList.Add(n);
                }
            }
            if (openList.Count == 0)
            {
                break;
            }
            curNode = GetLowestFCostNode(openList);
            openList.Remove(curNode);
            closedList.Add(curNode);
        }

        // Out of nodes on the openList
        if ((curNode.x == endNode.x && curNode.y == endNode.y))
        {
            return CalculatePath(endNode);
        }
        return null;

    }

    private List<GridTileObject> CalculatePath(GridTileObject endNode)
    {
        List<GridTileObject> path = new List<GridTileObject>();
        path.Add(endNode);
        GridTileObject currentNode = endNode;

        while (currentNode.cameFromTile != null)
        {
            path.Add(currentNode.cameFromTile);
            currentNode = currentNode.cameFromTile;
        }
        path.Reverse();

        return path;

    }

    private GridTileObject GetLowestFCostNode(List<GridTileObject> openList)
    {
        GridTileObject lowestFCostNode = openList[0];
        for (int i = 1; i < openList.Count; i++)
        {
            if (openList[i].f < lowestFCostNode.f)
            {
                lowestFCostNode = openList[i];
            }
        }
        return lowestFCostNode;
    }

    private List<GridTileObject> GetNeighbourList(int curX, int curY)
    {
        List<GridTileObject> neighbourList = new List<GridTileObject>();

        neighbourList.Add(nodeList[curX - 1][curY]);
        neighbourList.Add(nodeList[curX + 1][curY]);
        neighbourList.Add(nodeList[curX][curY - 1]);
        neighbourList.Add(nodeList[curX][curY + 1]);

        return neighbourList;
    }

    private int CalculateDist (GridTileObject a, GridTileObject b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);

        return xDistance + yDistance;
    }

}


