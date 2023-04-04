using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ActionCenter : MonoBehaviour
{
    // Start is called before the first frame update
    //bool gameTurn = true;
    public int tilesfat = 0;
    TileManager tileM;
    private Dictionary<int,Vector3Int> pastOrigin = new Dictionary<int, Vector3Int>();
    Vector3Int nodePos;
    Vector3Int target;
    Node tmNode;
    Vector3 worldPos;
    bool attacking = false;
    bool moving = false;
    void Awake()
    {
        getTileM();
    }

    //update current position of character, just made it incase we need it
    public void updatePos(){
        if(ifmoved()){
            nodePos = tileM.WorldToCell(transform.position);
            tmNode = tileM.GetNodeFromWorld(nodePos);
            worldPos = tileM.GetCellCenterWorld(nodePos);
            if(attacking){
                gameObject.GetComponentInChildren<CharacterEvents>().onUnHighLight.Invoke();
                gameObject.GetComponentInChildren<CharacterEvents>().onHighLight.Invoke();
            }
            /*Debug.Log("tileM Position"+nodePos);
            Debug.Log("TileManager Node"+tmNode);
            Debug.Log("World Position"+worldPos);*/
        }
    }
    //check if character moved after click or movement, if not can skip some checks
    public bool ifmoved(){
        return tileM.WorldToCell(transform.position) != nodePos;
    }
    //checks at beginning of turn, decoupled it so rn its empty lol
    public void beginningTurn(){
        attacking = false;
        if(tilesfat > 0){
            tilesfat = 0;
        }
        //remove
        
    }
    //checks at end of turn, also that's when enemy attack,
    // takes int bc reset and undo ends the current play turn before going to the previous character, can removeit not that we don't use undo
    public void endingTurn(int i){
        //To-DO: Added skill check for skills that update each Character turn
        if(i == 0){
            if(this.gameObject.tag == "Enemy"){
                this.gameObject.GetComponentInChildren<CharacterEvents>().onEnemyAttack.Invoke();
            }
        }

    }
    
    //checks during turn 
    public void duringTurn(){
        if(gameObject.tag == "Enemy"){
            this.gameObject.GetComponentInChildren<CharacterEvents>().onEnemyMove.Invoke();
        }

    }
    // invoke events onClick, either movement or attack
    public void onClick(){
        if(GameObject.Find("Canvas").GetComponent<EventTrig>().checkOnButton()){
            return;
        }
        if(GameObject.Find("InfoPanel") == null && GameObject.Find("AttackConfirm")== null){
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Node n = tileM.GetNodeFromWorld(tileM.WorldToCell(pos));
            if(tileM.WorldToCell(pos) == tileM.WorldToCell(transform.position) 
            ||(!tileM.inArea(gameObject.GetComponent<Teleport>().getOrigin(),tileM.WorldToCell(pos), gameObject.GetComponent<Teleport>().getTilesCheck())&& tileM.GetNodeFromWorld(tileM.WorldToCell(pos)).occupant==null)
            ||(tileM.GetNodeFromWorld(tileM.WorldToCell(pos)).occupant!=null && tileM.IsAdjacent(tileM.WorldToCell(transform.position),tileM.WorldToCell(pos))&& !attacking)){
                target = Vector3Int.zero;
                this.gameObject.GetComponentInChildren<CharacterEvents>().onReset.Invoke();
                return;
            }
            if(target != tileM.WorldToCell(pos) && ((!attacking )|| (attacking && !tileM.IsAdjacent(tileM.WorldToCell(pos),tileM.WorldToCell(transform.position))))){
                target = tileM.WorldToCell(pos);
                this.gameObject.GetComponentInChildren<CharacterEvents>().setTargetTile.Invoke(Input.mousePosition);
            }
            
            else{
                if(n != null && n.occupant != null && n.occupant.tag =="Enemy")
                {
                    if(attacking && tileM.inArea(tileM.WorldToCell(transform.position),tileM.WorldToCell(n.occupant.transform.position),(int)gameObject.GetComponent<StatUpdate>().getAttackRange())){
                        //tileM.flagEnemyArea(go,"Enemy",attackArea);
                        GameObject.Find("PopUpEvent").GetComponent<PopEvent>().setPos.Invoke(n.occupant,gameObject);
                        attacking = false;
                    } 
                    else{
                        this.gameObject.GetComponentInChildren<CharacterEvents>().onPlayerMove.Invoke(Input.mousePosition);    
                    }               
                }
                else if(tileM.WorldToCell(transform.position) != tileM.WorldToCell(pos)){
                    this.gameObject.GetComponentInChildren<CharacterEvents>().onPlayerMove.Invoke(Input.mousePosition);
                    moving = true;
                }
                if(!attacking){
                    target = Vector3Int.zero;
                }
            }
        }
    }

    public void notmoving(){
        if(this.gameObject.activeInHierarchy){
            tileM.setWalkable(this.gameObject,tileM.WorldToCell(transform.position), false);
        }
    }
    //originally keep track of stats throughout turn for undo, can still keep for other features
    public void saveTurnStatData(int gameTurn){
        if(this.gameObject.activeInHierarchy || gameTurn == 0){
            //statupdate.startSaveStat(gameTurn);
            if(pastOrigin.ContainsKey(gameTurn)){
                pastOrigin[gameTurn] =  tileM.WorldToCell(transform.position);    
            }
            else{
                pastOrigin.Add(gameTurn, tileM.WorldToCell(transform.position));
            }
        }
    }
    //sense when character is being right clicked, show the stat panel 
    void OnGUI()
    {
        Event e = Event.current;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Node n = tileM.GetNodeFromWorld(tileM.WorldToCell(pos));
        if (e.type == EventType.MouseUp && e.button  == 1 && n.occupant == gameObject && !moving)
        {
            gameObject.GetComponentInChildren<PopEvent>().setPos.Invoke(n.occupant,gameObject); 
        }
        else if(moving){
            moving = false;
        }
        
        
    }
    public void getTileM(){
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
    }
    
    public void setTilesFat(int tilesfat){
        this.tilesfat = tilesfat;
    }
    public int getTilesFat(){
        return tilesfat;
    }
    public Vector3Int getMapPos(){
        return nodePos;
    }
    public Node getMapNode(){
        return tmNode;
    }
    public Vector3 getWorldPos(){
        return worldPos;
    }
    public bool isAttacking(){
        return attacking;
    }
    public void setAttacking()
    {
        this.attacking = !this.attacking;
    }
}
