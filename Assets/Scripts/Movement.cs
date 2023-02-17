using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aoiti.Pathfinding; //import the pathfinding library
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    Pathfinder<Vector3Int> pathfinder;
    HighlightReachableTiles hightlightReachableTile;
    List<Vector3Int> path = new List<Vector3Int>();
    Tilemap tilemap;
    [SerializeField] float movementSpeed = 100f;  // Add this to control the movement speed
    int maxTiles; // Add this to limit the number of tiles the object can travel
    public int tilescheck = 0;
    int attackrange; 
    public int tilesTraveled = 0; // Add this to keep track of the number of tiles the object has traveled
    public int tilesTraveled_damage = 0;
    public bool isMoving = false;
    public bool turn = false;
    public bool moved = false;
    public bool origin = false;
    public bool attacking= false;
    public bool dead = false;
    GridGraph gridGraph;
    //Vector3Int targetNode;
    Vector3Int originNode;
    GameObject targetEnemy;

    private void Start()
    {
    // Get the Tilemap component from the scene
    maxTiles = this.gameObject.GetComponent<StatUpdate>().getMaxTiles();
    attackrange = this.gameObject.GetComponent<StatUpdate>().getAttackRange();
    tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
    gridGraph = GameObject.Find("Tilemanager").GetComponent<GridGraph>();
    hightlightReachableTile = this.gameObject.GetComponent<HighlightReachableTiles>();
    pathfinder = new Pathfinder<Vector3Int>(GetDistance, GetNeighbourNodes);
    hightlightReachableTile.HighlightReachable(); // Highlight the reachable tiles at the start of the game
    transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));
    tilescheck = maxTiles;
    }

    private void Update()
    {
        gridGraph = GameObject.Find("Tilemanager").GetComponent<GridGraph>();

        targetEnemy = null;
        if(turn && !moved)
         {
            moving();
        }
        else{
           notmoving();
        }
    }

    public void notmoving(){
         if(this.gameObject.GetComponent<StatUpdate>().getbuff(7)){
                    this.gameObject.GetComponent<StatUpdate>().Damage -= tilesTraveled_damage;
                    tilesTraveled_damage = 0;      
            }
            origin = false;
            gridGraph.setWalkable(tilemap.WorldToCell(transform.position), false);
            gridGraph.GetNodeFromWorld(tilemap.WorldToCell(transform.position)).occupant = this.gameObject;
            hightlightReachableTile.UnhighlightReachable();
            hightlightReachableTile.UnhighlightEnemy();
    }

    public void moving(){
            hightlightReachableTile.UnhighlightReachable();
            setRange();
            //Debug.Log(targetEnemy);
            setOrigin();
            highlight();
            if (Input.GetMouseButtonDown(0)) //check for a new target
            {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int targetNode = tilemap.WorldToCell(target);
            Vector3Int startNode = tilemap.WorldToCell(transform.position);
            int distance = Mathf.Abs(startNode.x - targetNode.x) + Mathf.Abs(startNode.y - targetNode.y); // Manhattan distance
            if(gridGraph.GetNodeFromWorld(targetNode)!=null){
            if(!gridGraph.GetNodeFromWorld(targetNode).walkable && gridGraph.GetNodeFromWorld(targetNode).occupant == null){
                Debug.Log("Target occupied.");
                //Debug.Log(gridGraph.GetNodeFromWorld(targetNode).occupant.name);
                return;
            }
            
            if(gridGraph.GetNodeFromWorld(targetNode).occupant != null){
                if(gridGraph.GetNodeFromWorld(targetNode).occupant.tag == "Enemy"){
                    //Debug.Log(gridGraph.GetNodeFromWorld(targetNode).occupant.name);
                    targetEnemy = gridGraph.GetNodeFromWorld(targetNode).occupant;
                    targetEnemy.GetComponent<StatUpdate>().flag = true;
                    targetNode = tilemap.WorldToCell(getClosestTiletoPlayer());
                    if(this.gameObject.GetComponent<StatUpdate>().getbuff(10)){
                        flagAdjacent();
                    }
                   
                }
            }
            }
            if(targetEnemy!=null){
                Debug.Log(GetDistance(tilemap.WorldToCell(targetEnemy.transform.position), tilemap.WorldToCell(transform.position)));
            }
            if(attacking && targetEnemy != null && GetDistance(tilemap.WorldToCell(targetEnemy.transform.position), tilemap.WorldToCell(transform.position)) <= attackrange){
                    AttackCheck();
                    return;
            }
            if(!ValidTile(targetNode)){
                //Debug.Log("pog");
                return;
            }
            gridGraph.setWalkable(startNode,true);
            gridGraph.resetOccupant(startNode);
            path = new List<Vector3Int>();
            //Debug.Log("Start Node: " + startNode);
            //Debug.Log("Closest Reachable Tile: " + targetNode);
            //Debug.Log("GetDistance: " + GetDistance(startNode, targetNode));
            //Debug.Log("GetNeighbourNodes: " + GetNeighbourNodes(startNode));
            
            if (pathfinder.GenerateAstarPath(startNode, targetNode, out path))
            {
                if(path.Count > tilescheck && !ValidTile(targetNode)){
                    Debug.Log("Target is too far away.");
                    isMoving = false;
                    path = null;
                }
                else{
                    tilesTraveled = 0;
                    isMoving = true;
                }
            }
            

        }
        if(isMoving && tilesTraveled_damage>0){
                this.gameObject.GetComponent<StatUpdate>().Damage -= tilesTraveled_damage;
                tilesTraveled_damage = 0;
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
                if(this.gameObject.GetComponent<StatUpdate>().getbuff(7)){
                    tilesTraveled_damage = tilesTraveled;
                    this.gameObject.GetComponent<StatUpdate>().Damage += tilesTraveled;
                }
                tilesTraveled = 0; 
                isMoving = false;
            }

        if ((path == null || path.Count == 0) && tilesTraveled >= tilescheck)
        {   
            if(this.gameObject.GetComponent<StatUpdate>().getbuff(7)){
                tilesTraveled_damage = tilesTraveled;    
                this.gameObject.GetComponent<StatUpdate>().Damage += tilesTraveled;
            }
            tilesTraveled = 0; 
            isMoving = false;
        }
        
    }
    void setRange(){
         if(this.gameObject.GetComponent<StatUpdate>().getbuff(14)){
                
                tilescheck = maxTiles/2;
            }
            else{
                tilescheck = maxTiles;
            }
    }

    void setOrigin(){
        if(!origin){
            hightlightReachableTile.highlightorigin = tilemap.WorldToCell(transform.position);
            originNode = tilemap.WorldToCell(transform.position);
            origin = true;
        }
    }
    void highlight(){
         if (isMoving)
            {
                hightlightReachableTile.UnhighlightReachable();
            }
            else
            {        
                hightlightReachableTile.HighlightReachable();
            }
    }
   public bool IsAdjacent(Vector3Int node1, Vector3Int node2)
    {
        int xDiff = Mathf.Abs(node1.x - node2.x);
        int yDiff = Mathf.Abs(node1.y - node2.y);
        if (xDiff + yDiff == 1)
        {
            return true;
        }
        return false;
    }
    public float GetDistance(Vector3Int A, Vector3Int B)
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
                    if (tilemap.HasTile(neighbourPos) && gridGraph.GetNodeFromWorld(neighbourPos)!=null && gridGraph.GetNodeFromWorld(neighbourPos).walkable && ValidTile(neighbourPos))
                    {
                        neighbours.Add(neighbourPos, 1);
                    }
                    /*else if(!gridGraph.GetNodeFromWorld(neighbourPos).walkable && gridGraph.GetNodeFromWorld(neighbourPos).occupant != null){
                        neighbours.Add(neighbourPos, 1);
                    }*/
                }
            }
        }
    }
    return neighbours;
}

    List<Node> GetTilesInArea(Vector3Int cellpos){
        List<Node> lst = new List<Node>();
        foreach(Node n in gridGraph.GetReachableTiles()){
            //Vector3Int cellpos = tilemap.WorldToCell(transform.position);
            if(Mathf.Abs((int)(n.gridX - (cellpos.x + gridGraph.GridSize.x/2))) + Mathf.Abs((int)(n.gridY - (cellpos.y+ gridGraph.GridSize.y/2))) <= tilescheck){
                lst.Add(n);
            }
        }
        return lst;
    }

    List<Node> GetTilesInArea_en(Vector3Int cellpos){
        List<Node> lst = new List<Node>();
        foreach(Node n in gridGraph.GetReachableTiles_en()){
            //Vector3Int cellpos = tilemap.WorldToCell(transform.position);
            if(Mathf.Abs((int)(n.gridX - (cellpos.x + gridGraph.GridSize.x/2))) + Mathf.Abs((int)(n.gridY - (cellpos.y+ gridGraph.GridSize.y/2))) <= tilescheck){
                lst.Add(n);
            }
        }
        return lst;
    }

    public bool inArea(Vector3Int target){
        foreach(Node n in GetTilesInArea_en(originNode)){
            if(n.gridX == target.x + gridGraph.GridSize.x/2 && n.gridY == target.y + gridGraph.GridSize.y/2){
                return true;
            }
        }
        return false;
    }

    bool ValidTile(Vector3Int target){
        foreach(Node n in GetTilesInArea_en(originNode)){
            if(n.gridX == target.x + gridGraph.GridSize.x/2 && n.gridY == target.y + gridGraph.GridSize.y/2){
                return true;
            }
        }
        return false;
    }

    Vector3Int getClosestTiletoPlayer(){
        GameObject player = targetEnemy;
        //Debug.Log(player.name);
        Node ans = null;
        int mindis = int.MaxValue;   
        foreach(Node n in GetTilesInArea(originNode)){
            Vector3Int cellpos = tilemap.WorldToCell(player.transform.position);
            Vector3Int target = tilemap.WorldToCell(new Vector3Int((int)n.worldPosition.x,(int)n.worldPosition.y,0));
            int distance = Mathf.Abs((int)(n.gridX - (cellpos.x + gridGraph.GridSize.x/2))) + Mathf.Abs((int)(n.gridY - (cellpos.y+ gridGraph.GridSize.y/2)));
            //List<Vector3Int> testpath = new List<Vector3Int>();
            //pathfinder.GenerateAstarPath(originNode, target, out testpath);
            //Debug.Log(target+": "+testpath.Count);
            if(distance <= mindis /*&& testpath.Count <= maxTiles*/){
                if( n.walkable){
                mindis = distance;
                ans = n;}
            }
        }
        return new Vector3Int((int)ans.worldPosition.x, (int) ans.worldPosition.y, 0);
    }

    public void Attack(){
        if(!attacking && hightlightReachableTile.EnemyInRange()){
        hightlightReachableTile.HighlightEnemy();
        attacking = true;
        }
        else{
            attacking = false;
            hightlightReachableTile.UnhighlightEnemy();
        }
    }
    public void AttackCheck(){
        foreach(GameObject en in GameObject.FindGameObjectsWithTag("Enemy")){
            if(en.GetComponent<StatUpdate>().flag){
                this.gameObject.GetComponent<StatUpdate>().attackEn(en);
            }
        }
        attacking = false;
            
        turn = false;
        moved = true;
        
    }

    public void AttackFlaged(){
        foreach(GameObject en in GameObject.FindGameObjectsWithTag("Enemy")){
            if(en.GetComponent<StatUpdate>().flag){
                this.gameObject.GetComponent<StatUpdate>().attackEn(en);
            }
        }
    }

    void flagAdjacent(){
        foreach(GameObject en in GameObject.FindGameObjectsWithTag("Enemy")){
            Vector3Int cellpos = tilemap.WorldToCell(targetEnemy.transform.position);
            Vector3Int enpos = tilemap.WorldToCell(en.transform.position);
            if(IsAdjacent(cellpos,enpos)){
                en.GetComponent<StatUpdate>().flag = true;;
            }
        }
    }

    public bool getTurn(){
        return turn;
    }

    
}