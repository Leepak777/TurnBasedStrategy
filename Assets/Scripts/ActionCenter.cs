using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aoiti.Pathfinding; //import the pathfinding library
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class startTurnEvent : UnityEvent{

}

public class endTurnEvent : UnityEvent<int>{
    
}

public class duringTurnEvent : UnityEvent{
    
}
public class undoEvent :UnityEvent<int>{

}

public class ActionCenter : MonoBehaviour
{
    // Start is called before the first frame update
    HighlightReachableTiles hightlightReachableTile;
    Tilemap tilemap;
    public bool dead = false;
    //bool gameTurn = true;
    public int tilesfat = 0;
    GameObject targetEnemy;
    Movement movement;
    StatUpdate statupdate;
    TileManager tileM;
    TurnManager TM;
    Attack atk;
    Ghost ghost;
    startTurnEvent start;
    endTurnEvent end;
    duringTurnEvent during;
    undoEvent undo;
    private List<Vector3Int> Trail = new List<Vector3Int>();
    private Dictionary<int,Vector3Int> pastOrigin = new Dictionary<int, Vector3Int>();
    void Start()
    {
        
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        hightlightReachableTile = new HighlightReachableTiles();
        statupdate = this.gameObject.GetComponent<StatUpdate>();
        movement = this.gameObject.GetComponent<Movement>();
        atk = this.gameObject.GetComponent<Attack>();
        ghost = this.gameObject.GetComponentInChildren<Ghost>();
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        Node locn = tileM.GetNodeFromWorld(tilemap.WorldToCell(transform.position));
        Vector3Int loc = new Vector3Int((int)locn.worldPosition.x,(int)locn.worldPosition.y,0);
        this.gameObject.GetComponentInChildren<Ghost>().setLocation(loc);
        start = new startTurnEvent();
        end = new endTurnEvent();
        during = new duringTurnEvent();
        undo = new undoEvent();
        if(start != null){
            start.AddListener(beginningTurn);
        }
        if(end != null){
            end.AddListener(endingTurn);
        }
        if(during != null){
            during.AddListener(duringTurn);
        }
        if(undo != null){
            undo.AddListener(undoTurn);
        }
        transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));
    }

    // Update is called once per frame
    void Update()
    {
 
        
    }
    public void beginningTurn(){
        movement.setOrigin();
        movement.setRange();
        hightlightReachableTile.UnhighlightReachable();
        hightlightReachableTile.HighlightReachable(gameObject);
        if(this.gameObject.tag == "Player"){
                GameObject.Find("Main Camera").GetComponent<CameraController>().trackPlayer(TM.getCurrenPlay());
        }
        if(statupdate.getDictStats("fat") > 100){
            endingTurn(0);
        }
        
    }
    public void saveTurnStatData(){
        statupdate.startSaveStat();
        if(pastOrigin.ContainsKey(TM.getGameTurn())){
            pastOrigin[TM.getGameTurn()] =  movement.getOrigin();    
        }
        else{
            pastOrigin.Add(TM.getGameTurn(), movement.getOrigin());
        }
    }
    public void endingTurn(int i){
        if(i == 0){
            movement.setPath = false;
            if(this.gameObject.tag == "Enemy"){
                atk.Attacking("Player");
            }
            statupdate.checkFatigue(tilesfat);
            statupdate.setDamage(0);
            tilesfat = 0;
        }
        notmoving();
    }


    public void duringTurn(){
        if(!atk.isAttacking()){
            ghost.setGhost();
            movement.moving();
            //highlight();
        }
    }

    public void undoTurn(int i){
        if(pastOrigin.ContainsKey(i)){
            tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position), true);
            transform.position = tilemap.GetCellCenterWorld( pastOrigin[i]);
            pastOrigin.Remove(i);
            statupdate.revertStat(i);
        }

    }

    public void notmoving(){
            tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position), false);
            hightlightReachableTile.UnhighlightReachable();
            hightlightReachableTile.UnhighlightEnemy();
            clearTrail();
    }
    public void onMove(){
        hightlightReachableTile.UnhighlightReachable();
        if(Trail.Count > 0){
            clearTrail();
        }
    }
    public void onStop(){
        hightlightReachableTile.UnhighlightReachable();
        hightlightReachableTile.HighlightReachable(gameObject);
    }
    public void addTrail(Vector3Int tile){
        Trail.Add(tile);
    }
    public List<Vector3Int> getTrail(){
        return Trail;
    }
    public void endTurn(){
        TM.setGameState(1);
    }
    public HighlightReachableTiles getHighLight(){
        return hightlightReachableTile;
    }
    public void setTilesFat(int tilesfat){
        this.tilesfat = tilesfat;
    }
    public int getTilesFat(){
        return tilesfat;
    }
    public GameObject getTargetEnemy(){
        return targetEnemy;
    }
    public void setTargetEnemy(GameObject go){
        this.targetEnemy = go;
    }
    void clearTrail(){
        hightlightReachableTile.UnhighlightTrail(Trail);
        Trail.Clear();
    }
    public void addOrigin(Vector3Int origin){
        pastOrigin.Add(TM.getGameTurn(),origin);
    }
    public void inovkeEvent(int i, int x){
        switch(i){
            case 0:
                start.Invoke();
                break;
            case 1:
                end.Invoke(x);
                break;
            case 2:
                during.Invoke();
                break;
            case 3:
                undo.Invoke(x);
                break;
        }
    }
}
