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
    List<Vector3Int> trail = new List<Vector3Int>();
    [SerializeField] Tilemap tilemap;
    [SerializeField] float movementSpeed = 100f;  // Add this to control the movement speed
    public float tilescheck = 0;
    int attackrange; 
    public int tilesTraveled = 0; // Add this to keep track of the number of tiles the object has traveled
    public bool isMoving = false;
    public bool outClick = false;
    public bool setPath = false;
    TileManager tileM;
    Vector3Int originNode;
    Vector3Int targetNode;
    private void Start()
    {
        // Get the Tilemap component from the scene
        attackrange = this.gameObject.GetComponent<StatUpdate>().getAttackRange();
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();        
        tilemap = tileM.getTileMap();
        pathfinder = new Pathfinder<Vector3Int>(tileM.GetDistance, GetNeighbourNodes);
        transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));
        tilescheck = this.gameObject.GetComponent<StatUpdate>().getMaxTiles() + 0.5f;
        originNode = tilemap.WorldToCell(transform.position);
    }

    public void setPathAI(){
        if(!setPath){ 
            //GameObject targetPlayer = tileM.getClosestReachablePlayer("Player", transform.position, attackrange,tilescheck);
            KeyValuePair<GameObject,Vector3Int> target = tileM.getClosestReachablePlayer("Player", originNode, attackrange,tilescheck);
            Vector3Int startNode = tilemap.WorldToCell(transform.position);  
              
            targetNode = target.Value;   
            if(tileM.EnemyInRange("Player", attackrange, this.gameObject)){
                AIreturn();
            }
            if(!tileM.GetNodeFromWorld(targetNode).walkable && tileM.GetNodeFromWorld(targetNode).occupant == null){
                //Debug.Log("Target occupied.");
                AIreturn();
            }
            if(tileM.GetNodeFromWorld(targetNode).occupant != null && tileM.GetNodeFromWorld(targetNode).occupant.tag == "Enemy"){
                //Debug.Log("Target occupied.");
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

    public void setPathPlayer(Vector3 mousePosition){
            Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
            targetNode = tilemap.WorldToCell(target);
            Vector3Int startNode = tilemap.WorldToCell(transform.position);           
    
            if(!tileM.GetNodeFromWorld(targetNode).walkable && tileM.GetNodeFromWorld(targetNode).occupant == null){
                //Debug.Log("Target occupied.");
                AIreturn();
            }
            if(tileM.GetNodeFromWorld(targetNode).occupant != null && tileM.GetNodeFromWorld(targetNode).occupant.tag == "Player"){
                //Debug.Log("Target occupied.");
                AIreturn();
            }
            if(tileM.GetNodeFromWorld(targetNode).occupant != null){
                GameObject go = tileM.GetNodeFromWorld(targetNode).occupant;
                targetNode = tileM.getClosestTiletoObject(go, originNode, attackrange, tilescheck);
                outClick = true;
            }
            if(!tileM.inArea(originNode,targetNode, tilescheck)){
                AIreturn();
            }
            
            path = new List<Vector3Int>();
            if (pathfinder.GenerateAstarPath(startNode, targetNode, out path))
            {
                if(path.Count > tilescheck && !tileM.inArea(originNode,targetNode,tilescheck)){
                    Debug.Log("Target is too far away.");
                    isMoving = false;
                    path = null;
                }
                else{
                    tileM.setWalkable(this.gameObject,startNode,true);
                    tilesTraveled = 0;
                    isMoving = true;
                }
            }
    }

    public void moveTo(){

            if (path != null && path.Count > 0)
            {
                Vector3 targetPosition = tilemap.GetCellCenterWorld(path[0]);
                Vector3 dir = (targetPosition - transform.position).normalized;

                if (path.Count > 1)
                {
                    if (!tileM.IsAdjacent(path[0], path[1]))
                    {
                        path.RemoveAt(0);
                        return;
                    }
                }

                if ((transform.position - targetPosition).sqrMagnitude < movementSpeed * movementSpeed * Time.deltaTime * Time.deltaTime)
                {
                    Vector3Int node =  tilemap.WorldToCell(targetPosition);
                    this.gameObject.GetComponent<CharacterEvents>().onUnHighLight.Invoke(trail);
                    if (pathfinder.GenerateAstarPath(originNode, node, out trail))
                    {
                        trail.Add(originNode);
                        foreach(Vector3Int v in trail){
                            this.gameObject.GetComponent<CharacterEvents>().onHighLight.Invoke(v);
                        }
                    }
                    path.RemoveAt(0);
                    tilesTraveled++; // Increment the tiles traveled
                    
                }
                transform.position += dir * movementSpeed * Time.deltaTime;  // Move the character based on the movement speed
            }
    }

    public void moving(){
           
            if ((path == null || path.Count == 0))
            {
                if(isMoving || tilesTraveled >= tilescheck){
                    isMoving = false;
                    tilesTraveled = 0;
                }
                if(this.gameObject.GetComponent<ActionCenter>().ifmoved() || outClick){
                    if(outClick){outClick = false;}
                    this.gameObject.GetComponent<CharacterEvents>().onMoveStop.Invoke();
                    this.gameObject.GetComponent<CharacterEvents>().onHighLight.Invoke(originNode);
                }
                
            }
    }
    public void setRange(){
        tilescheck = this.gameObject.GetComponent<StatUpdate>().getMaxTiles() + 0.5f;
    }

    public void setOrigin(){
        if(tilemap == null){
            tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        }
        originNode = tilemap.WorldToCell(this.gameObject.transform.position);

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
    public float getTilesCheck(){
        return tilescheck;
    }
    void AIreturn(){
        setPath = true;
        path = null;
        isMoving = true;
        outClick = true;
        return;
    }
    public void setTilemap(Tilemap tilemap){
        this.tilemap = tilemap;
    }

    public void SetsetPath(bool setPath){
        this.setPath = setPath;
    }
    public int gettrailCount(){
        Debug.Log(trail.Count);
        return trail.Count;
    }
    public List<Vector3Int> getTrail(){
        return trail;
    }
    public void ClearTrail(){
        trail.Clear();
    }
}