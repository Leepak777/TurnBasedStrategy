using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aoiti.Pathfinding; 
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
        pathfinder = new Pathfinder<Vector3Int>(tileM.GetDistance,GetNeighbourNodes);
        transform.position = tileM.GetCellCenterWorld(tileM.WorldToCell(transform.position));
        tilescheck = this.gameObject.GetComponent<StatUpdate>().getMaxTiles() + 0.5f;
        originNode = tileM.WorldToCell(transform.position);
    }


    public void PlayerTeleport(Vector3 mousePosition){
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
        targetNode = tileM.WorldToCell(target);
        targetNode = tileM.getCloestTile(targetNode,originNode,attackrange,tilescheck);
        Node n = tileM.GetNodeFromWorld(targetNode);
        if(n.occupant != null && n.occupant.tag == "Enemy"){
            if(tileM.entityInRange(gameObject, n.occupant, gameObject.GetComponent<StatUpdate>().getAttackRange())){
                return;
            }
        }
        
        //Debug.Log(targetNode);
        //Debug.Log(tileM.WorldToCell(transform.position));
        if (pathfinder.GenerateAstarPath(originNode, targetNode, out trail))
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
        if(tileM.EnemyInRange("Player", attackrange, this.gameObject)){
            return;
        }
        KeyValuePair<GameObject,Vector3Int> target = tileM.getClosestReachablePlayer("Player", originNode,attackrange,tilescheck);
        Vector3Int startNode = tileM.WorldToCell(transform.position);  
              
        targetNode = target.Value;   
        

        if (pathfinder.GenerateAstarPath(originNode, targetNode, out trail))
        {
            if(tileM.inArea(originNode,targetNode, (int)tilescheck)){
                tileM.setWalkable(this.gameObject,tileM.WorldToCell(transform.position),true);
                tileM.setWalkable(this.gameObject,targetNode,false);
                transform.position = tileM.GetCellCenterWorld(targetNode);
                
            }
        }
        if(this.gameObject.GetComponent<ActionCenter>().ifmoved() || outClick){
            if(outClick){outClick = false;}
            this.gameObject.GetComponent<CharacterEvents>().onMoveStop.Invoke();
        }
    }
    public List<Vector3Int> GetPosTrail(Vector3 mousePosition){
        List<Vector3Int> fuckme = new List<Vector3Int>();
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
        targetNode = tileM.WorldToCell(target);
        targetNode = tileM.getCloestTile(targetNode,originNode,attackrange,tilescheck);
        if(pathfinder.GenerateAstarPath(tileM.WorldToCell(transform.position), targetNode, out fuckme)){
            return fuckme;
        }
        return null;
    }
   public void resetPos(){
        tileM.setWalkable(this.gameObject,tileM.WorldToCell(transform.position),true);
        tileM.setWalkable(this.gameObject,originNode,false);
        transform.position = tileM.GetCellCenterWorld(originNode);
   }
    public Dictionary<Vector3Int, float> GetNeighbourNodes(Vector3Int pos)
    {
        List<Node> area = tileM.GetTilesInArea(pos,1);
        Dictionary<Vector3Int, float> neighbours = new Dictionary<Vector3Int, float>();
        foreach(Node n in area){
            Vector3Int Loc = new Vector3Int(n.gridX,n.gridY,pos.z);
            float distance = tileM.GetDistance(pos,Loc);
            if(n.walkable && tileM.inArea(pos,Loc,tilescheck)){ 
                neighbours.Add(Loc, distance);
            }
        }
        return neighbours;
        /*int x = pos.x;
        int y = pos.y;
        bool evenColumn = (y % 2 == 0);

        // Check adjacent tiles
        if (evenColumn)
        {
            int[,] adjacentOffsets = new int[,] {
                {1, 0},
                {0, 1},
                {-1, 1},
                {-1, 0},
                {-1, -1},
                {0, -1},
            };

            for (int i = 0; i < adjacentOffsets.GetLength(0); i++)
            {
                int dx = adjacentOffsets[i, 0];
                int dy = adjacentOffsets[i, 1];

                Vector3Int neighbourPos = new Vector3Int(x + dx, y + dy, pos.z);

                // Check if the node is walkable and in range
                if (tileM.inArea(pos, neighbourPos, tilescheck) && tileM.GetNodeFromWorld(neighbourPos).walkable)
                {
                    float distance = tileM.GetDistance(pos, neighbourPos);
                    neighbours.Add(neighbourPos, distance);
                }
            }
        }
        else
        {
            int[,] adjacentOffsets = new int[,] {
                {1, 0},
                {1, 1},
                {0, 1},
                {-1, 0},
                {0, -1},
                {1, -1},
            };

            for (int i = 0; i < adjacentOffsets.GetLength(0); i++)
            {
                int dx = adjacentOffsets[i, 0];
                int dy = adjacentOffsets[i, 1];

                Vector3Int neighbourPos = new Vector3Int(x + dx, y + dy, pos.z);

                // Check if the node is walkable and in range
                if (tileM.inArea(pos, neighbourPos, tilescheck) && tileM.GetNodeFromWorld(neighbourPos).walkable)
                {
                    float distance = tileM.GetDistance(pos, neighbourPos);
                    neighbours.Add(neighbourPos, distance);
                }
            }
        }

        return neighbours;*/
    }


    public bool getisMoving(){
        return isMoving;
    }
    public int getAttackRange(){
        return (int)attackrange;
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
