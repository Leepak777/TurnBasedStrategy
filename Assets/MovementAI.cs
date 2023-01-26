using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aoiti.Pathfinding; //import the pathfinding library
using UnityEngine.Tilemaps;

public class MovementAI : MonoBehaviour
{
     Pathfinder<Vector3Int> pathfinder;
    public HighlightReachableTiles hightlightReachableTile;
    List<Vector3Int> path = new List<Vector3Int>();
    Tilemap tilemap;
    [SerializeField] float movementSpeed = 0.1f;  // Add this to control the movement speed
    [SerializeField] int maxTiles = 10; // Add this to limit the number of tiles the object can travel
    int tilesTraveled = 0; // Add this to keep track of the number of tiles the object has traveled
    bool isMoving = false;
    public bool turn = false;
    public bool moved = false;
    public bool setPath = false;
    public GridGraph gridGraph;
    Vector3Int targetNode;
    private void Start()
    {
    // Get the Tilemap component from the scene
    tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    pathfinder = new Pathfinder<Vector3Int>(GetDistance, GetNeighbourNodes);
    hightlightReachableTile.HighlightReachable(); // Highlight the reachable tiles at the start of the game
    }

    private void Update()
    {
        if(turn && !moved)
         {
            //Debug.Log(getClosestTiletoPlayer());
            if (isMoving)
                {
                    hightlightReachableTile.UnhighlightReachable();
                }
                else
                {        
                    hightlightReachableTile.HighlightReachable();
                }
        if (!setPath) //check for a new target
        {
           
            targetNode = tilemap.WorldToCell(getClosestTiletoPlayer());
            Vector3Int startNode = tilemap.WorldToCell(transform.position);
            int distance = Mathf.Abs(startNode.x - targetNode.x) + Mathf.Abs(startNode.y - targetNode.y); // Manhattan distance

            if(!gridGraph.GetNodeFromWorld(targetNode).walkable){
                Debug.Log("Target occupied.");
                Debug.Log(gridGraph.GetNodeFromWorld(targetNode).occupant.name);
                return;
            }
            gridGraph.setWalkable(startNode,true);
            path = new List<Vector3Int>();
            if (pathfinder.GenerateAstarPath(startNode, targetNode, out path))
            {
                //Debug.Log(path.Count);
                if(path.Count > maxTiles){
                    Debug.Log("Target is too far away.");
                    isMoving = false;
                    path = null;
                }
                else{
                    tilesTraveled = 0;
                    isMoving = true;
                }
            }
            setPath = true;
        }

        if (path != null && path.Count > 0)
        {
            Vector3 targetPosition = tilemap.GetCellCenterWorld(path[0]);
            Vector3 dir = (targetPosition - transform.position).normalized;

            if (path.Count > 1)
            {
                if (!IsAdjacent(path[0], path[1]))
                {
                    path.RemoveAt(0);
                    return;
                }
            }

            if ((transform.position - targetPosition).sqrMagnitude < movementSpeed * movementSpeed * Time.deltaTime * Time.deltaTime)
            {
                path.RemoveAt(0);
                tilesTraveled++; // Increment the tiles traveled
            }
            transform.position += dir * movementSpeed * Time.deltaTime;  // Move the character based on the movement speed
        }
           if ((path == null || path.Count == 0) && isMoving)
            {
                tilesTraveled = 0; 
                turn = false;
                moved = true;   
                isMoving = false;
            }

        if ((path == null || path.Count == 0) && tilesTraveled >= maxTiles)
        {    
            tilesTraveled = 0; 
            turn = false;
            moved = true;   
            isMoving = false;
        }


    }
    else{
        gridGraph.setWalkable(tilemap.WorldToCell(transform.position), false);
        gridGraph.GetNodeFromWorld(tilemap.WorldToCell(transform.position)).occupant = this.gameObject;
        hightlightReachableTile.UnhighlightReachable();
    }
    }



   bool IsAdjacent(Vector3Int node1, Vector3Int node2)
{
    int xDiff = Mathf.Abs(node1.x - node2.x);
    int yDiff = Mathf.Abs(node1.y - node2.y);
    if (xDiff + yDiff == 1)
    {
        return true;
    }
    return false;
}
float GetDistance(Vector3Int A, Vector3Int B)
{
    // Use Manhattan distance for tilemap
    return Mathf.Abs(A.x - B.x) + Mathf.Abs(A.y - B.y);
}

float GetDistance2(Vector2 A, Vector3 B)
{
    // Use Manhattan distance for tilemap
    return Mathf.Abs((int)A.x - B.x) + Mathf.Abs((int)A.y - B.y);
}

Dictionary<Vector3Int, float> GetNeighbourNodes(Vector3Int pos)
{
    Dictionary<Vector3Int, float> neighbours = new Dictionary<Vector3Int, float>();
    for (int i = -1; i < 2; i++)
    {
        for (int j = -1; j < 2; j++)
        {
            if (i == 0 && j == 0) continue;{
                if (Mathf.Abs(i) == Mathf.Abs(j)) continue;{
                    Vector3Int neighbourPos = new Vector3Int(pos.x + i, pos.y + j, pos.z);
                    if (tilemap.HasTile(neighbourPos) && gridGraph.GetNodeFromWorld(neighbourPos).walkable)
                    {
                        neighbours.Add(neighbourPos, 1);
                    }
                }
            }
        }
    }
    return neighbours;
}

GameObject getClosestPlayer(){
    int mindis = int.MaxValue;   
    GameObject close = null; 
    foreach(Node n in gridGraph.Grid){
        if(n.occupant != null && (n.occupant.name == "black1" || n.occupant.name == "black2")){
            //Debug.Log(n.worldPosition);
            if(Mathf.Abs((int)(n.worldPosition.x - transform.position.x)) + Mathf.Abs((int)(n.worldPosition.y - transform.position.y)) < mindis){
                mindis = Mathf.Abs((int)(n.worldPosition.x - transform.position.x)) + Mathf.Abs((int)(n.worldPosition.y - transform.position.y));
                close = n.occupant;
            }
        }
    }
    return close;
}

    List<Node> GetTilesInArea(){
        List<Node> lst = new List<Node>();
        foreach(Node n in gridGraph.GetReachableTiles()){
            Vector3Int cellpos = tilemap.WorldToCell(transform.position);
            if(Mathf.Abs((int)(n.gridX - (cellpos.x + gridGraph.GridSize.x/2))) + Mathf.Abs((int)(n.gridY - (cellpos.y+ gridGraph.GridSize.y/2))) <= maxTiles){
                lst.Add(n);
            }
        }
        return lst;
    }

    Vector3Int getClosestTiletoPlayer(){
        GameObject player = getClosestPlayer();
        Node ans = null;
        int mindis = int.MaxValue;   
        foreach(Node n in GetTilesInArea()){
            Vector3Int cellpos = tilemap.WorldToCell(player.transform.position);
            int distance = Mathf.Abs((int)(n.gridX - (cellpos.x + gridGraph.GridSize.x/2))) + Mathf.Abs((int)(n.gridY - (cellpos.y+ gridGraph.GridSize.y/2)));
            if(distance < mindis){
                mindis = distance;
                ans = n;
            }
        }
        return new Vector3Int((int)ans.worldPosition.x, (int) ans.worldPosition.y, 0);
    }

}