using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aoiti.Pathfinding; //import the pathfinding library
using UnityEngine.Tilemaps;

public class MovementAI : MonoBehaviour
{
     Pathfinder<Vector3Int> pathfinder;
    HighlightReachableTiles hightlightReachableTile;
    List<Vector3Int> path = new List<Vector3Int>();
    Tilemap tilemap;
    [SerializeField] float movementSpeed = 100f;  // Add this to control the movement speed
    int maxTiles; // Add this to limit the number of tiles the object can travel
    int tilesTraveled = 0; // Add this to keep track of the number of tiles the object has traveled
    public int turnsElapsed = 0;
    bool isMoving = false;
    public bool turn = false;
    public bool moved = false;
    public bool setPath = false;
    public bool dead = false;

    GridGraph gridGraph;
    Vector3Int targetNode;
    private void Start()
    {
    maxTiles = this.gameObject.GetComponent<StatUpdate>().getMaxTiles();
    // Get the Tilemap component from the scene
    tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    gridGraph = GameObject.Find("Tilemanager").GetComponent<GridGraph>();
    hightlightReachableTile = this.gameObject.GetComponent<HighlightReachableTiles>();
    pathfinder = new Pathfinder<Vector3Int>(GetDistance, GetNeighbourNodes);
    hightlightReachableTile.HighlightReachable(); // Highlight the reachable tiles at the start of the game
    transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));

    }

    private void Update()
    {
        if(turn && !moved)
         {
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
            gridGraph.setWalkable(this.gameObject,startNode,true);
            
            path = new List<Vector3Int>();
            if (startNode != targetNode && pathfinder.GenerateAstarPath(startNode, targetNode, out path))
            {
                //Debug.Log(path.Count);
                if(path.Count > maxTiles){
                    Debug.Log("Target is too far away.");
                    isMoving = false;
                    path = null;
                    isMoving = true;
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
                AttackCheck();    
                tilesTraveled = 0; 
                turn = false;
                moved = true;   
                isMoving = false;
            }

        if ((path == null || path.Count == 0) && tilesTraveled >= maxTiles)
        {    
            AttackCheck();
            tilesTraveled = 0; 
            turn = false;
            moved = true;   
            isMoving = false;
        }


    }
    else{
        gridGraph.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position), false);
        hightlightReachableTile.UnhighlightReachable();
    }
    }

    void AttackCheck(){
        GameObject player = CheckPlayerNear();
        if(player!= null){
            player.GetComponent<StatUpdate>().attackPl(player);
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
    bool isInRange(Vector3Int node1, Vector3Int node2)
    {
    int xDiff = Mathf.Abs(node1.x - node2.x);
    int yDiff = Mathf.Abs(node1.y - node2.y);
    if (xDiff + yDiff <= maxTiles)
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
                    if (tilemap.HasTile(neighbourPos) && gridGraph.GetNodeFromWorld(neighbourPos)!=null && gridGraph.GetNodeFromWorld(neighbourPos).walkable)
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
    GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Player");
    foreach(GameObject n in objectsWithTag){
        //Debug.Log(n.occupant.name);
        //Debug.Log(Mathf.Abs((int)(n.worldPosition.x - transform.position.x)) + Mathf.Abs((int)(n.worldPosition.y - transform.position.y)));
        Vector3Int cellpos = tilemap.WorldToCell(n.transform.position);
        Vector3Int curpos = tilemap.WorldToCell(transform.position);
        int distance = Mathf.Abs((int)(((curpos.x - cellpos.x) ))) + Mathf.Abs((int)((curpos.y - cellpos.y)));
        if( distance< mindis){
            mindis = distance;
            close = n;
         }  
    }
    return close;
}
GameObject CheckPlayerNear(){
     GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Player");
    foreach(GameObject n in objectsWithTag){
        Vector3Int cellpos = tilemap.WorldToCell(n.transform.position);
        Vector3Int curpos = tilemap.WorldToCell(transform.position);
        if(IsAdjacent(cellpos,curpos)){
            return n;
         }  }return null;
    
    
}

    List<Node> GetTilesInArea(){
        List<Node> lst = new List<Node>();
        foreach(Node n in gridGraph.GetReachableTiles()){
            Vector3Int cellpos = tilemap.WorldToCell(transform.position);
            if(Mathf.Abs((int)(n.gridX - (cellpos.x ))) + Mathf.Abs((int)(n.gridY - (cellpos.y))) <= maxTiles){
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
            int distance = Mathf.Abs((int)(n.gridX - (cellpos.x ))) + Mathf.Abs((int)(n.gridY - (cellpos.y)));
            if(distance < mindis ){
                if(n.walkable){
                mindis = distance;
                ans = n;
                }
            }
        }
        if(ans != null){
            return new Vector3Int((int)ans.worldPosition.x, (int) ans.worldPosition.y, 0);
        }
        Node ogpos = gridGraph.GetNodeFromWorld(tilemap.WorldToCell(transform.position));
        return new Vector3Int((int)ogpos.worldPosition.x,(int)ogpos.worldPosition.y, 0);
    }

}