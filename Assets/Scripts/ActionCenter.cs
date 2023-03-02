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

public class endTurnEvent : UnityEvent{
    
}

public class duringTurnEvent : UnityEvent{
    
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
    void Start()
    {
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        hightlightReachableTile = this.gameObject.GetComponent<HighlightReachableTiles>();
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
        if(start != null){
            start.AddListener(startTurn);
        }
        if(end != null){
            end.AddListener(endingTurn);
        }
        if(during != null){
            during.AddListener(duringTurn);
        }
    }

    // Update is called once per frame
    void Update()
    {
 
        
    }
    public void beginningTurn(){
        movement.setOrigin();
        movement.setRange();
        hightlightReachableTile.UnhighlightReachable();
        if(this.gameObject.tag == "Player"){
                GameObject.Find("Main Camera").GetComponent<CameraController>().trackPlayer(TM.getCurrenPlay());
        }
    }

    public void endingTurn(){
        if(this.gameObject.tag == "Enemy"){
            atk.Attacking("Player");
        }
        notmoving();
    }

    public void duringTurn(){
        ghost.setGhost();
        movement.moving();
        highlight();
    }

    public void notmoving(){
            //origin = false;
            tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position), false);
            hightlightReachableTile.UnhighlightReachable();
            hightlightReachableTile.UnhighlightEnemy();
    }
    public void highlight(){
         if (movement.getisMoving())
        {
            hightlightReachableTile.UnhighlightReachable();
        }
        else
        {        
            hightlightReachableTile.UnhighlightReachable();
            hightlightReachableTile.HighlightReachable();
        }
    }


    public void startTurn(){
        tilesfat = 0;
        beginningTurn();
    }
    public void resetTurn(){
        tilesfat = 0;
    }

    public void endTurn(){
        tilesfat = 0;
        TM.setGameState(1);
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

    public void inovkeEvent(int i){
        switch(i){
            case 0:
                start.Invoke();
                break;
            case 1:
                end.Invoke();
                break;
            case 2:
                during.Invoke();
                break;
        }
    }
}
