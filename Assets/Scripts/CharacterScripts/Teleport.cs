using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aoiti.Pathfinding; //import the pathfinding library
using UnityEngine.EventSystems;

public class Teleport : MonoBehaviour
{
    Pathfinder<Vector3Int> pathfinder;
    List<Vector3Int> trail = new List<Vector3Int>();
    public float tilescheck = 0;
    float attackrange; 
    public bool isMoving = false;
    public bool outClick = false;
    TileManager tileM;
    Vector3Int originNode;
    Vector3Int targetNode;
    private void Start()
    {
        // Get the tileM component from the scene
        attackrange = this.gameObject.GetComponent<StatUpdate>().getAttackRange();
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();        
        pathfinder = new Pathfinder<Vector3Int>(tileM.GetDistance, GetNeighbourNodes);
        transform.position = tileM.GetCellCenterWorld(tileM.WorldToCell(transform.position));
        tilescheck = this.gameObject.GetComponent<StatUpdate>().getMaxTiles() + 0.5f;
        originNode = tileM.WorldToCell(transform.position);
    }


    public void PlayerTeleport(Vector3 mousePosition){
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
        targetNode = tileM.WorldToCell(target);
        Node n = tileM.GetNodeFromWorld(targetNode);
        if(n.occupant != null && n.occupant.tag == "Enemy"){
            if(tileM.entityInRange(gameObject, n.occupant, gameObject.GetComponent<StatUpdate>().getAttackRange())){
                return;
            }
        }
        //Debug.Log(targetNode);
        //Debug.Log(tileM.WorldToCell(transform.position));
        if(n.occupant != null){
            GameObject go = n.occupant;
            targetNode = tileM.getClosestTiletoObject(go, originNode, (int)attackrange, (int)tilescheck);
            outClick = true;
        }
        
        if (pathfinder.GenerateAstarPath(originNode, targetNode, out trail) && tileM.inArea(originNode,targetNode, tilescheck))
        {   
            tileM.setWalkable(this.gameObject,tileM.WorldToCell(transform.position),true);
            tileM.setWalkable(this.gameObject,targetNode,false);
            transform.position = tileM.GetCellCenterWorld(targetNode);
        }
        else
        {
            trail.Clear();
        }
        if(this.gameObject.GetComponent<ActionCenter>().ifmoved() || outClick){
            if(outClick){outClick = false;}
            this.gameObject.GetComponent<CharacterEvents>().onMoveStop.Invoke();
        }
    }
    public void EnemyTeleport(){
        KeyValuePair<GameObject,Vector3Int> target = tileM.getClosestReachablePlayer("Player", originNode, (int)attackrange,(int)tilescheck);
        Vector3Int startNode = tileM.WorldToCell(transform.position);  
              
        targetNode = target.Value;   
        if(tileM.EnemyInRange("Player", (int)attackrange, this.gameObject)){
            AIreturn();
        }

        this.gameObject.GetComponent<CharacterEvents>().onUnHighLight.Invoke(trail);
        if (pathfinder.GenerateAstarPath(originNode, targetNode, out trail))
        {
            if(tileM.inArea(originNode,targetNode, (int)tilescheck)){
                tileM.setWalkable(this.gameObject,tileM.WorldToCell(transform.position),true);
                tileM.setWalkable(this.gameObject,targetNode,false);
                transform.position = tileM.GetCellCenterWorld(targetNode);
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
                        if (tileM.inArea(originNode,neighbourPos,(int)tilescheck) && tileM.GetNodeFromWorld(neighbourPos)!=null && tileM.GetNodeFromWorld(neighbourPos).walkable)
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
    public void setisMoving(bool move){
        this.isMoving = move;
    }
    public Vector3Int getOrigin(){
        return originNode;
    }
    public float getTilesCheck(){
        return tilescheck;
    }
    public void setRange(){
        tilescheck = this.gameObject.GetComponent<StatUpdate>().getMaxTiles() ;//+ 0.5f;
    }

    public void setOrigin(){
        if(tileM == null){
            tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        }
        originNode = tileM.WorldToCell(this.gameObject.transform.position);
    }
    void AIreturn(){
        isMoving = false;
        outClick = true;
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
    public void settileM(TileManager tileM){
        this.tileM = tileM;
    }
    public Vector3Int getTarget(){
        return targetNode;

    }
}
