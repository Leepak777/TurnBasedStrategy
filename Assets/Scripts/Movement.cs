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
    List<Vector3Int> path = new List<Vector3Int>();
    Tilemap tilemap;
    [SerializeField] float movementSpeed = 100f;  // Add this to control the movement speed
    int maxTiles; // Add this to limit the number of tiles the object can travel
    public int tilescheck = 0;
    int attackrange; 
    public int tilesTraveled = 0; // Add this to keep track of the number of tiles the object has traveled
    public bool isMoving = false;
    public bool setPath = false;
    TileManager tileM;
    Vector3Int originNode;
    Vector3Int targetNode;
    ActionCenter ac;
    private void Start()
    {
    // Get the Tilemap component from the scene
    maxTiles = this.gameObject.GetComponent<StatUpdate>().getMaxTiles();
    attackrange = this.gameObject.GetComponent<StatUpdate>().getAttackRange();
    tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
    tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
    ac = this.gameObject.GetComponent<ActionCenter>();
    pathfinder = new Pathfinder<Vector3Int>(GetDistance, GetNeighbourNodes);
    transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));
    tilescheck = maxTiles;
    originNode = tilemap.WorldToCell(transform.position);
    }

    private void Update()
    {
    }
    //TO-DO foreach player, if targetnode closer, user that character
    public void setPathAI(){
        if(!setPath){
            
            //GameObject targetPlayer = tileM.getClosestReachablePlayer("Player", transform.position, attackrange,tilescheck);
            targetNode = tileM.getClosestReachablePlayer("Player", originNode, attackrange,tilescheck).Value;
            Vector3Int startNode = tilemap.WorldToCell(transform.position);  
              
            //targetNode = tilemap.WorldToCell(tileM.getClosestTiletoObject(targetPlayer, originNode, attackrange, tilescheck));   
            if(tileM.EnemyInRange("Player", attackrange, this.gameObject)){
                AIreturn();
            }
            if(!tileM.GetNodeFromWorld(targetNode).walkable && tileM.GetNodeFromWorld(targetNode).occupant == null){
                Debug.Log("Target occupied.");
                AIreturn();
            }
            if(tileM.GetNodeFromWorld(targetNode).occupant != null && tileM.GetNodeFromWorld(targetNode).occupant.tag == "Enemy"){
                Debug.Log("Target occupied.");
                AIreturn();
            }
            
            if(!tileM.inArea(originNode,targetNode, tilescheck)){
                AIreturn();
            }
            tileM.setWalkable(this.gameObject,startNode,true);
            path = new List<Vector3Int>();
            
            if (pathfinder.GenerateAstarPath(startNode, targetNode, out path))
            {
                //Debug.Log("pog");
                if(path.Count > tilescheck && !tileM.inArea(originNode,targetNode,tilescheck)){
                    Debug.Log("Target is too far away.");
                    isMoving = false;
                    path = null;
                }
                else{
                    tilesTraveled = 0;
                    
                }
            }else{
                path = null;
            }
            setPath = true;
            isMoving = true;
            /*Debug.Log("start "+startNode+"target "+targetNode);
            Debug.Log("path "+path);*/
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
        
        if (GetMouseButtonDown(0)) //check for a new target
            {
            this.gameObject.GetComponentInChildren<Ghost>().setOnOff(false);
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetNode = tilemap.WorldToCell(target);
            Vector3Int startNode = tilemap.WorldToCell(transform.position);           

            if(!tileM.GetNodeFromWorld(targetNode).walkable && tileM.GetNodeFromWorld(targetNode).occupant == null){
                Debug.Log("Target occupied.");
                AIreturn();
            }
            if(tileM.GetNodeFromWorld(targetNode).occupant != null && tileM.GetNodeFromWorld(targetNode).occupant.tag == "Player"){
                Debug.Log("Target occupied.");
                AIreturn();
            }
            if(tileM.GetNodeFromWorld(targetNode).occupant != null){
                GameObject go = tileM.GetNodeFromWorld(targetNode).occupant;
                targetNode = tileM.getClosestTiletoObject(go, originNode, attackrange, tilescheck);
                
            }
            if(!tileM.inArea(originNode,targetNode, tilescheck)){
                AIreturn();
            }
            tileM.setWalkable(this.gameObject,startNode,true);
            path = new List<Vector3Int>();
            
            if (pathfinder.GenerateAstarPath(startNode, targetNode, out path))
            {
                if(path.Count > tilescheck && !tileM.inArea(originNode,targetNode,tilescheck)){
                    Debug.Log("Target is too far away.");
                    isMoving = false;
                    path = null;
                }
                else{
                    tilesTraveled = 0;
                    isMoving = true;
                }
            }
            ac.onMove();
        }
    }

    public void moveTo(){

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
                    Node node =tileM.GetNodeFromWorld(tilemap.WorldToCell(targetPosition));
                    Color c = Color.red;
                    c.a = 0.5f;
                    Vector3Int tilePos = new Vector3Int((int)node.gridX , (int)node.gridY , 0);
                    tilemap.SetColor(tilePos, c);
                    ac.addTrail(tilePos);
                    path.RemoveAt(0);
                    tilesTraveled++; // Increment the tiles traveled
                    
                }
                transform.position += dir * movementSpeed * Time.deltaTime;  // Move the character based on the movement speed
            }
    }

    public void moving(){
            if(gameObject.tag == "Player"){
                setPathPlayer();
            }
            else{
                setPathAI();
            }
            moveTo();
           
            if ((path == null || path.Count == 0))
            {
                if(isMoving || tilesTraveled >= tilescheck){
                    ac.setTilesFat(tilesTraveled);
                    isMoving = false;
                    if(this.gameObject.tag == "Enemy"){
                        ac.endTurn();   
                    }
                    tilesTraveled = 0;
                }
                ac.onStop();
            }
    }
    public void setRange(){
        tilescheck = this.gameObject.GetComponent<StatUpdate>().getMaxTiles();
    }

    public void setOrigin(){
        originNode = tilemap.WorldToCell(this.gameObject.transform.position);

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
                        if (tileM.inArea(originNode,neighbourPos,tilescheck) && tileM.GetNodeFromWorld(neighbourPos)!=null && tileM.GetNodeFromWorld(neighbourPos).walkable)
                        {
                            neighbours.Add(neighbourPos, 1);
                        }
                    }
                }
            }
        }
        return neighbours;
    }

    public bool getisMoving(){
        return isMoving;
    }
    public Vector3Int getOrigin(){
        return originNode;
    }



    public int getTilesCheck(){
        return tilescheck;
    }

    void AIreturn(){
        setPath = true;
        path = null;
        isMoving = true;
        return;
    }
    
}