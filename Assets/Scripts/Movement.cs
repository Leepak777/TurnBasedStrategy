using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aoiti.Pathfinding; //import the pathfinding library
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

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
    public int tilesfat = 0;
    public int turnsElapsed = 0;
    public bool isMoving = false;
    public bool turn = false;
    public bool moved = false;
    public bool origin = false;
    public bool attacking= false;
    public bool dead = false;
    public bool setPath = false;
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
    Node locn = gridGraph.GetNodeFromWorld(tilemap.WorldToCell(transform.position));
    Vector3Int loc = new Vector3Int((int)locn.worldPosition.x,(int)locn.worldPosition.y,0);
    this.gameObject.GetComponentInChildren<Ghost>().setLocation(loc);

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
            /*if(this.gameObject.GetComponent<StatUpdate>().getbuff(7)){
                    this.gameObject.GetComponent<StatUpdate>().Damage -= tilesTraveled_damage;
                    tilesTraveled_damage = 0;      
            }*/
            origin = false;
            gridGraph.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position), false);
            hightlightReachableTile.UnhighlightReachable();
            hightlightReachableTile.UnhighlightEnemy();
    }

    public void setPathAI(){
        if(!setPath){
            GameObject targetPlayer = getClosestPlayer();
            Vector3Int targetNode = tilemap.WorldToCell(targetPlayer.transform.position);
            Vector3Int startNode = tilemap.WorldToCell(transform.position);           
            
            if(!gridGraph.GetNodeFromWorld(targetNode).walkable && gridGraph.GetNodeFromWorld(targetNode).occupant == null){
                Debug.Log("Target occupied.");
                //Debug.Log(gridGraph.GetNodeFromWorld(targetNode).occupant.name);
                return;
            }
            
            if(gridGraph.GetNodeFromWorld(targetNode).occupant != null){
                GameObject go = gridGraph.GetNodeFromWorld(targetNode).occupant;
                if(go.tag == "Player"){
                    if(inArea(tilemap.WorldToCell(transform.position),tilemap.WorldToCell(go.transform.position),attackrange)){
                        targetEnemy = go;
                        targetEnemy.GetComponent<StatUpdate>().flag = true;
                        AttackCheck("Player");
                        return;
                    }
                    //Debug.Log(gridGraph.GetNodeFromWorld(targetNode).occupant.name);
                    targetNode = tilemap.WorldToCell(getClosestTiletoObject(go));
                    /*if(this.gameObject.GetComponent<StatUpdate>().getbuff(10)){
                        flagAdjacent();
                    }*/
                }
                
            }
             if(!inArea(originNode,targetNode, tilescheck)){
                return;
            }
            gridGraph.setWalkable(this.gameObject,startNode,true);
            path = new List<Vector3Int>();
            /*Debug.Log("Start Node: " + startNode);
            Debug.Log("Closest Reachable Tile: " + targetNode);
            Debug.Log("GetDistance: " + GetDistance(startNode, targetNode));
            Debug.Log("GetNeighbourNodes: " + GetNeighbourNodes(startNode));*/
            
            if (pathfinder.GenerateAstarPath(startNode, targetNode, out path))
            {
                if(path.Count > tilescheck && !inArea(originNode,targetNode,tilescheck)){
                    Debug.Log("Target is too far away.");
                    isMoving = false;
                    path = null;
                }
                else{
                    tilesTraveled = 0;
                    isMoving = true;
                }
                 setPath = true;
            }
            }
    }

    public bool GetMouseButtonDown(int button)
    {
        if(GameObject.Find("AttackPrompt").GetComponent<AttackPrompt>().checkOnButton()){
            return false;
        }
        return Input.GetMouseButtonDown(button);
    }

    public void setPathPlayer(){
        //this.gameObject.GetComponentInChildren<Ghost>().enabled = true;
        //this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
        this.gameObject.GetComponentInChildren<Ghost>().setOnOff(true);
        Vector3 shadowtarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int shadowtargetNode = tilemap.WorldToCell(shadowtarget);
        if(inArea(originNode,shadowtargetNode, tilescheck)){
            Node locn = gridGraph.GetNodeFromWorld(shadowtargetNode);
            Vector3Int loc = new Vector3Int((int)locn.worldPosition.x,(int)locn.worldPosition.y,0);
            this.gameObject.GetComponentInChildren<Ghost>().setLocation(loc);
        }
        else{
            this.gameObject.GetComponentInChildren<Ghost>().setOnOff(false);
        }
        if (GetMouseButtonDown(0)) //check for a new target
            {
            this.gameObject.GetComponentInChildren<Ghost>().setOnOff(false);
            //this.gameObject.GetComponentInChildren<Ghost>().enabled = false;
            //this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int targetNode = tilemap.WorldToCell(target);
            Vector3Int startNode = tilemap.WorldToCell(transform.position);           

            if(!gridGraph.GetNodeFromWorld(targetNode).walkable && gridGraph.GetNodeFromWorld(targetNode).occupant == null){
                Debug.Log("Target occupied.");
                //Debug.Log(gridGraph.GetNodeFromWorld(targetNode).occupant.name);
                return;
            }
            
            if(gridGraph.GetNodeFromWorld(targetNode).occupant != null){
                GameObject go = gridGraph.GetNodeFromWorld(targetNode).occupant;
                if(go.tag == "Enemy"){
                    if(attacking && inArea(tilemap.WorldToCell(transform.position),tilemap.WorldToCell(go.transform.position),attackrange)){
                        targetEnemy = go;
                        targetEnemy.GetComponent<StatUpdate>().flag = true;
                        AttackCheck("Enemy");
                        return;
                    }
                    //Debug.Log(gridGraph.GetNodeFromWorld(targetNode).occupant.name);
                    targetNode = tilemap.WorldToCell(getClosestTiletoObject(go));
                    /*if(this.gameObject.GetComponent<StatUpdate>().getbuff(10)){
                        flagAdjacent();
                    }*/
                }
                
            }
             if(!inArea(originNode,targetNode, tilescheck)){
                return;
            }
            gridGraph.setWalkable(this.gameObject,startNode,true);
            path = new List<Vector3Int>();
            //Debug.Log("Start Node: " + startNode);
            //Debug.Log("Closest Reachable Tile: " + targetNode);
            //Debug.Log("GetDistance: " + GetDistance(startNode, targetNode));
            //Debug.Log("GetNeighbourNodes: " + GetNeighbourNodes(startNode));
            
            if (pathfinder.GenerateAstarPath(startNode, targetNode, out path))
            {
                if(path.Count > tilescheck && !inArea(originNode,targetNode,tilescheck)){
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
    }

    public void moveTo(){
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
    }

    public void moving(){
            hightlightReachableTile.UnhighlightReachable();
            setRange();
            //Debug.Log(targetEnemy);
            setOrigin();
            highlight();
            if(gameObject.tag == "Player"){
                setPathPlayer();
            }
            else{
                setPathAI();
            }
            moveTo();
           
            if ((path == null || path.Count == 0))
            {
                /*if(this.gameObject.GetComponent<StatUpdate>().getbuff(7)){
                    tilesTraveled_damage = tilesTraveled;
                    this.gameObject.GetComponent<StatUpdate>().Damage += tilesTraveled;
                }*/
                if(isMoving || tilesTraveled >= tilescheck){
                    tilesfat = tilesTraveled;
                    isMoving = false;
                    if(this.gameObject.tag == "Enemy"){
                        turn = false;
                        moved = true;   
                    }
                    tilesTraveled = 0;
                }
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
                        if (inArea(originNode,neighbourPos,tilescheck) && gridGraph.GetNodeFromWorld(neighbourPos)!=null && gridGraph.GetNodeFromWorld(neighbourPos).walkable)
                        {
                            neighbours.Add(neighbourPos, 1);
                        }
                    }
                }
            }
        }
        return neighbours;
    }

    public bool inArea(Vector3Int start,Vector3Int target, float range){
        foreach(Node n in gridGraph.GetTilesInArea(start,range)){
            if(n.gridX == target.x  && n.gridY == target.y){
                return true;
            }
        }
        return false;
    }

    public Vector3Int inAreaTile(Vector3Int start,Vector3Int target, float range){
        foreach(Node n in gridGraph.GetTilesInArea(start,range)){
            if(n.gridX == target.x  && n.gridY == target.y){
                return new Vector3Int((int)n.worldPosition.x, (int) n.worldPosition.y, 0);
            }
        }
        return originNode;
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

    Vector3Int getClosestTiletoObject(GameObject go){
        GameObject player = go;
        Node ans = null;
        int mindis = int.MaxValue;    
        Vector3Int cellpos = tilemap.WorldToCell(player.transform.position);
        foreach(Node n in gridGraph.GetTilesInArea(originNode,tilescheck)){
            Vector3Int target = tilemap.WorldToCell(new Vector3Int((int)n.worldPosition.x,(int)n.worldPosition.y,0));
            int distance = Mathf.Abs((int)(n.gridX - (cellpos.x))) + Mathf.Abs((int)(n.gridY - (cellpos.y)));
                if(n.walkable && distance < mindis && distance >= (int)attackrange){
                    mindis = distance;
                    ans = n;
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
    public void AttackCheck(string tag){
        foreach(GameObject en in GameObject.FindGameObjectsWithTag(tag)){
            if(en.GetComponent<StatUpdate>().flag){
                this.gameObject.GetComponent<StatUpdate>().attackEn(en);
            }
        }
        attacking = false;
            
        turn = false;
        moved = true;
        
    }

    /*public void AttackFlaged(){
        foreach(GameObject en in GameObject.FindGameObjectsWithTag("Enemy")){
            if(en.GetComponent<StatUpdate>().flag){
                this.gameObject.GetComponent<StatUpdate>().attackEn(en);
            }
        }
    }*/

    /*void flagAdjacent(){
        foreach(GameObject en in GameObject.FindGameObjectsWithTag("Enemy")){
            Vector3Int cellpos = tilemap.WorldToCell(targetEnemy.transform.position);
            Vector3Int enpos = tilemap.WorldToCell(en.transform.position);
            if(IsAdjacent(cellpos,enpos)){
                en.GetComponent<StatUpdate>().flag = true;;
            }
        }
    }*/

    public bool getTurn(){
        return turn;
    }

    public void startTurn(){
        turn = true;
        moved = false;
    }


    public void endTurn(){
        tilesfat = 0;
        turn = false;
        moved = true;
    }

    public int getTilesFat(){
        return tilesfat;
    }

    public Vector3Int getOrigin(){
        return originNode;
    }

    
}