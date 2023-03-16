using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aoiti.Pathfinding; //import the pathfinding library
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class Teleport : MonoBehaviour
{
    Pathfinder<Vector3Int> pathfinder;
    List<Vector3Int> trail = new List<Vector3Int>();
    [SerializeField] Tilemap tilemap;
    public float tilescheck = 0;
    float attackrange; 
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


    public void PlayerTeleport(Vector3 mousePosition){
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
        targetNode = tilemap.WorldToCell(target);
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
            targetNode = tileM.getClosestTiletoObject(go, originNode, attackrange, (int)tilescheck);
            outClick = true;
        }
        if(!tileM.inArea(originNode,targetNode, (int)tilescheck)){
            AIreturn();
        }

        if (pathfinder.GenerateAstarPath(originNode, targetNode, out trail))
        {   
            if(tileM.inArea(originNode,targetNode, (int)tilescheck)){
                tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position),true);
                tileM.setWalkable(this.gameObject,targetNode,false);
                transform.position = tilemap.GetCellCenterWorld(targetNode);
                /*foreach(Vector3Int v in trail){
                    this.gameObject.GetComponent<CharacterEvents>().onHighLight.Invoke(v);
                }*/
            }
            else{
                trail.Clear();
            }
        }
        if(this.gameObject.GetComponent<ActionCenter>().ifmoved() || outClick){
            if(outClick){outClick = false;}
            this.gameObject.GetComponent<CharacterEvents>().onMoveStop.Invoke();
        }
    }
    public void EnemyTeleport(){
        KeyValuePair<GameObject,Vector3Int> target = tileM.getClosestReachablePlayer("Player", originNode, attackrange,(int)tilescheck);
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
        if(!tileM.inArea(originNode,targetNode,(int) tilescheck)){
            AIreturn();
        }
        this.gameObject.GetComponent<CharacterEvents>().onUnHighLight.Invoke(trail);
        if (pathfinder.GenerateAstarPath(originNode, targetNode, out trail))
        {
            if(tileM.inArea(originNode,targetNode, (int)tilescheck)){
                tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position),true);
                tileM.setWalkable(this.gameObject,targetNode,false);
                isMoving = false;
                transform.position = tilemap.GetCellCenterWorld(targetNode);
                foreach(Vector3Int v in trail){
                    this.gameObject.GetComponent<CharacterEvents>().onHighLight.Invoke(v);
                }
            }
        }
        if(this.gameObject.GetComponent<ActionCenter>().ifmoved() || outClick){
            if(outClick){outClick = false;}
            this.gameObject.GetComponent<CharacterEvents>().onMoveStop.Invoke();
            this.gameObject.GetComponent<CharacterEvents>().onHighLight.Invoke(originNode);
        }
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
    public void setRange(){
        tilescheck = this.gameObject.GetComponent<StatUpdate>().getMaxTiles() + 0.5f;
    }

    public void setOrigin(){
        if(tilemap == null){
            tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        }
        originNode = tilemap.WorldToCell(this.gameObject.transform.position);

    }
    void AIreturn(){
        isMoving = false;
        outClick = true;
        return;
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
    public void setTilemap(Tilemap tilemap){
        this.tilemap = tilemap;
    }
    public Vector3Int getTarget(){
        return targetNode;

    }
}
