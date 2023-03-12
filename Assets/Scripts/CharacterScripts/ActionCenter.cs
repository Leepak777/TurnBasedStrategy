using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aoiti.Pathfinding; //import the pathfinding library
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class ActionCenter : MonoBehaviour
{
    // Start is called before the first frame update
    HighlightReachableTiles hightlightReachableTile;
    Tilemap tilemap;
    //bool gameTurn = true;
    public int tilesfat = 0;
    GameObject targetEnemy;
    Movement movement;
    StatUpdate statupdate;
    TileManager tileM;
    Attack atk;
    public HealthBar healthBar;
        
    private List<Vector3Int> Trail = new List<Vector3Int>();
    private Dictionary<int,Vector3Int> pastOrigin = new Dictionary<int, Vector3Int>();
    void Awake()
    {
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        hightlightReachableTile = this.gameObject.GetComponent<HighlightReachableTiles>();
        statupdate = this.gameObject.GetComponent<StatUpdate>();
        atk = this.gameObject.GetComponent<Attack>();
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        movement = this.gameObject.GetComponent<Movement>();
    }
    public bool GetMouseButtonDown(int button)
    {
        if(GameObject.Find("AttackPrompt").GetComponent<AttackPrompt>().checkOnButton()){
            return false;
        }
        return Input.GetMouseButtonDown(button);
    }
    // Update is called once per frame
    void Update()
    {
 
        
    }
    public void beginningTurn(){
        //To-DO: Added skill check for skills that update each Character turn
        if(tilesfat > 0){
            tilesfat = 0;
        }
        if(statupdate.getDictStats("fat") > 100){
            endingTurn(0);
        }
    }
    
    public void endingTurn(int i){
        //To-DO: Added skill check for skills that update each Character turn
        if(i == 0){
            movement.setPath = false;
            if(this.gameObject.tag == "Enemy"){
                this.gameObject.GetComponent<CharacterEvents>().onEnemyAttack.Invoke();
            }
            statupdate.checkFatigue(tilesfat);
            statupdate.setDamage(0);
            tilesfat = 0;
        }
    }

    public void duringTurn(){
        if(gameObject.tag == "Player"){
            if(GetMouseButtonDown(0)){
                if(!atk.isAttacking()){
                    this.gameObject.GetComponent<CharacterEvents>().onPlayerMove.Invoke(Input.mousePosition);
                }
                else{
                    this.gameObject.GetComponent<CharacterEvents>().onPlayerAttack.Invoke(Input.mousePosition);
                }
            }
        }
        else{
            this.gameObject.GetComponent<CharacterEvents>().onEnemyMove.Invoke();
        }

        this.gameObject.GetComponent<CharacterEvents>().onMoving.Invoke();

    }

    public void undoTurn(int i){
        //To-DO: Check what kind of buff undoed 
        if(!this.gameObject.activeInHierarchy){
            i--;
        }
        if(pastOrigin.ContainsKey(i)){
            Vector3 ogPos = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));
            transform.position = tilemap.GetCellCenterWorld(pastOrigin[i]);
            if(transform.position != ogPos){
                tileM.setWalkable(this.gameObject,tilemap.WorldToCell(ogPos), true);   
            }
            //pastOrigin.Remove(i);
            statupdate.revertStat(i);
            if(!this.gameObject.activeInHierarchy){
                this.gameObject.SetActive(true);
                statupdate.updateHealthBar();
                
            }
            tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position),false);
        }

    }

    public void notmoving(){
        if(this.gameObject.activeInHierarchy){
            tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position), false);
            hightlightReachableTile.UnhighlightReachable();
            hightlightReachableTile.UnhighlightEnemy();
            clearTrail();
        }
    }

    public void onMove(){
        hightlightReachableTile.UnhighlightReachable();
        if(Trail.Count > 0){
            clearTrail();
        }
    }

    public void onStop(){
        hightlightReachableTile.UnhighlightReachable();
        hightlightReachableTile.HighlightReachable();
    }

    public void saveTurnStatData(int gameTurn){
        if(this.gameObject.activeInHierarchy || gameTurn == 0){
            //statupdate.startSaveStat(gameTurn);
            if(pastOrigin.ContainsKey(gameTurn)){
                pastOrigin[gameTurn] =  tilemap.WorldToCell(transform.position);    
            }
            else{
                pastOrigin.Add(gameTurn, tilemap.WorldToCell(transform.position));
            }
        }
    }

    public void addTrail(Vector3Int tile){
        hightlightReachableTile.highlight(tile);
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
    public void clearTrail(){
        hightlightReachableTile.UnhighlightTrail(Trail);
        Trail.Clear();
    }

    
}
