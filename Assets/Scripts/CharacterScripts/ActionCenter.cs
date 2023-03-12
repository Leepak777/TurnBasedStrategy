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
    Tilemap tilemap;
    //bool gameTurn = true;
    public int tilesfat = 0;
    TileManager tileM;
    private Dictionary<int,Vector3Int> pastOrigin = new Dictionary<int, Vector3Int>();
    void Awake()
    {
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
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
        //remove
        if(gameObject.GetComponent<StatUpdate>().getDictStats("fat") > 100){
            this.gameObject.GetComponent<CharacterEvents>().onEnd.Invoke(1);
        }
    }
    
    public void endingTurn(int i){
        //To-DO: Added skill check for skills that update each Character turn
        if(i == 0){
            if(this.gameObject.tag == "Enemy"){
                this.gameObject.GetComponent<CharacterEvents>().onEnemyAttack.Invoke();
            }
        }
    }

    public void duringTurn(){
        if(gameObject.tag == "Player"){
            if(GetMouseButtonDown(0)){
                if(!gameObject.GetComponent<Attack>().isAttacking()){
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
            if(!this.gameObject.activeInHierarchy){
                this.gameObject.SetActive(true);
                
            }
            tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position),false);
        }

    }

    public void notmoving(){
        if(this.gameObject.activeInHierarchy){
            tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position), false);
        }
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
    public void setTilesFat(int tilesfat){
        this.tilesfat = tilesfat;
    }
    public int getTilesFat(){
        return tilesfat;
    }




    
}
