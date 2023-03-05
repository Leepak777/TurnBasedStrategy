using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aoiti.Pathfinding; //import the pathfinding library
using UnityEngine.Tilemaps;


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
    //TurnManager TM;
    Attack atk;
    Ghost ghost;
    
    private List<Vector3Int> Trail = new List<Vector3Int>();
    private Dictionary<int,Vector3Int> pastOrigin = new Dictionary<int, Vector3Int>();
    void Awake()
    {
        
        //TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        hightlightReachableTile = new HighlightReachableTiles();
        statupdate = this.gameObject.GetComponent<StatUpdate>();
        atk = this.gameObject.GetComponent<Attack>();
        ghost = this.gameObject.GetComponentInChildren<Ghost>();
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        movement = this.gameObject.GetComponent<Movement>();
        
    }

    // Update is called once per frame
    void Update()
    {
 
        
    }
    public void beginningTurn(){
        if(tilesfat > 0){
            tilesfat = 0;
        }
        movement.setOrigin();
        movement.setRange();
        hightlightReachableTile.UnhighlightReachable();
        clearTrail();
        hightlightReachableTile.HighlightReachable(gameObject);
        if(this.gameObject.tag == "Player"){
                GameObject.Find("Main Camera").GetComponent<CameraController>().trackPlayer(this.gameObject);
        }
        if(statupdate.getDictStats("fat") > 100){
            endingTurn(0);
        }
        
    }
    public void saveTurnStatData(int gameTurn){
        statupdate.startSaveStat();
        if(pastOrigin.ContainsKey(gameTurn)){
            pastOrigin[gameTurn] =  tilemap.WorldToCell(transform.position);    
        }
        else{
            pastOrigin.Add(gameTurn, tilemap.WorldToCell(transform.position));
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
            if(transform.position != tilemap.GetCellCenterWorld(movement.getOrigin())){
                tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position), true);
                transform.position = tilemap.GetCellCenterWorld( pastOrigin[i]);
            }
            
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

    
}
