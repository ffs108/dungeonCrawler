using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class PlayController : MonoBehaviour
{
    public GameObject finder;
    Boolean isWalking;
    public float speedRate = 2f;
    public List<List<GridTileObject>> grid;
    GridTileObject start;
    Vector3 startPos;
    Vector3 targetPos;
    List<GridTileObject> path;
    private CharacterController Hero;

    private void Start()
    {
        isWalking = false;
        Hero = GetComponent<CharacterController>();
        finder = GameObject.Find("Labyrinth");
        grid = finder.GetComponent<Pathfinder>().nodeList;
        start = grid[grid.Count - 2][1]; //player is either at [1][len(grid)-2] or [len(grid)-2][1]
        startPos = transform.position;//start.pos;
    }

    private void Update()
    {
        //if leftmouse clicked
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("clicked");
            //grab position vector of clicked tile
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                GridTileObject end = hit.transform.gameObject.GetComponent<GridTileObject>();
                targetPos = hit.transform.gameObject.transform.position;//Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //find tile with the matching position and find the a* path between player and it
                Debug.Log(start.x + " " + start.y + " " + end.x + " " + end.y + targetPos);
                //find path
                path = finder.GetComponent<Pathfinder>().FindPath(start.x, start.y, end.x, end.y);
                isWalking = true;
                //set the movement from start vector to next vector in optimal path list if not null
                if (path != null && isWalking)
                {
                    //foreach (GridTileObject tile in path)
                    //{
                    //    Debug.Log("Moved to tile:" + tile.x + "," + tile.y);
                    //    Debug.Log("Heuristic and G respective are: " + tile.h + "," + tile.g);
                    //    if (Vector3.Distance(startPos, tile.pos) < 0.5f)
                    //    {
                    //        Debug.Log("Tile reached");
                    //        isWalking=false;
                    //    }
                    //    transform.position = Vector3.Lerp(transform.position, tile.pos, Time.deltaTime * speedRate);

                    //}
                    StartCoroutine(Move());
                    start = end;
                }
                
            }
            isWalking = false;
            GetComponent<Animator>().SetBool("isMove", false);
        }
    }

    private IEnumerator Move()
    {
        if (path.Count == 0) yield break;
        Vector3 begin = path[0].pos;
        for(int i = 1; i < path.Count; i++)
        {
            float t = 0f;

            Vector3 goal = path[i].pos;

            while(t < 1f)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(begin, goal, Mathf.SmoothStep(0, 1, t));
                GetComponent<Animator>().SetBool("isMove", true);
                yield return null;
            }
            begin = goal;
        }
        
    }
}