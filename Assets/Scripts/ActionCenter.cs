using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aoiti.Pathfinding; //import the pathfinding library
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class ActionCenter : MonoBehaviour
{
    // Start is called before the first frame update
    HighlightReachableTiles hightlightReachableTile;
    Tilemap tilemap;
    public bool turn = false;
    public bool moved = false;
    public bool dead = false;
    bool gameTurn = true;
    public int tilesfat = 0;
    GameObject targetEnemy;
    Movement movement;
    StatUpdate statupdate;
    TileManager tileM;
    TurnManager TM;
    Attack atk;
    void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        hightlightReachableTile = this.gameObject.GetComponent<HighlightReachableTiles>();
        statupdate = this.gameObject.GetComponent<StatUpdate>();
        movement = this.gameObject.GetComponent<Movement>();
        atk = this.gameObject.GetComponent<Attack>();
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        Node locn = tileM.GetNodeFromWorld(tilemap.WorldToCell(transform.position));
        Vector3Int loc = new Vector3Int((int)locn.worldPosition.x,(int)locn.worldPosition.y,0);
        this.gameObject.GetComponentInChildren<Ghost>().setLocation(loc);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TM.getActive() && gameTurn){
            //check when character turn start
            if(TM.getCurrenPlay() == this.gameObject){
                movement.setOrigin();
                movement.setRange();
            }    
            //overall startturn check
            gameTurn = false;
        }else if(!TM.getActive()){
            gameTurn = true;
            //character end turn check
            if(TM.getCurrenPlay() == this.gameObject){
                if(this.gameObject.tag == "Enemy"){
                    atk.Attacking("Player");
                }
                notmoving();
            }
            //overall endturn check
            
        }
        //targetEnemy = null;
        /*Debug.Log("turn"+turn );
        Debug.Log("!moved"+!moved );
        Debug.Log("!atk.isAttacking()"+!atk.isAttacking() );*/
        if(turn && !moved && !atk.isAttacking())
        {
            movement.moving();
            highlight();
        }
        
           
        
    }
    public void notmoving(){
            //origin = false;
            tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position), false);
            hightlightReachableTile.UnhighlightReachable();
            hightlightReachableTile.UnhighlightEnemy();
    }
    public void highlight(){
         if (movement.getisMoving() )
        {
            hightlightReachableTile.UnhighlightReachable();
        }
        else
        {        
            hightlightReachableTile.UnhighlightReachable();
            hightlightReachableTile.HighlightReachable();
        }
    }
    public bool getTurn(){
        return turn;
    }

    public void startTurn(){
        if(!turn){
            tilesfat = 0;
            turn = true;
            moved = false;
        }
    }
    public void resetTurn(){
        tilesfat = 0;
        turn = false;
        moved = false;
    }

    public void endTurn(){
        tilesfat = 0;
        turn = false;
        moved = true;
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
}
