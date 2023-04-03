using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    SpriteRenderer Ghost_render;
    TileManager tileM;
    Teleport movement;
    bool anchored = false;
    // Start is called before the first frame update
    void Start()
    {   
        getTileM();
        movement = this.gameObject.GetComponentInParent<Teleport>();
        if(this.transform.parent.tag == "Enemy"){
            Destroy(this.gameObject);
        }
    }
    public void getTileM(){
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setGhost(){
        if(!anchored){
            if(GameObject.Find("Panel") == null && GameObject.Find("AttackConfirm")== null ){
                Vector3 shadowtarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int shadowtargetNode = tileM.WorldToCell(shadowtarget);
                if(tileM.inArea(movement.getOrigin(),shadowtargetNode, movement.getTilesCheck()) ){
                    setOnOff(true);
                    Node locn = tileM.GetNodeFromWorld(shadowtargetNode);
                    if(locn.occupant==null){
                        Vector3Int loc = new Vector3Int((int)locn.worldPosition.x,(int)locn.worldPosition.y,0);
                        setLocation(loc);
                    }
                    if(shadowtargetNode == gameObject.GetComponentInParent<ActionCenter>().getMapPos()){
                        setOnOff(false);
                    }
                }
                else{
                    setOnOff(false);
                }
            }
            if(GameObject.Find("Canvas").GetComponent<EventTrig>().checkOnButton()){
                setOnOff(false);
            }
        }
        else{
           setOnOff(true); 
        }
    }
    public void setGhostTile(Vector3 mousePosition){
        if(GameObject.Find("Panel") == null && GameObject.Find("AttackConfirm")== null ){
            Vector3 shadowtarget = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3Int shadowtargetNode = tileM.WorldToCell(shadowtarget);
            shadowtargetNode = tileM.getCloestTile(shadowtargetNode,movement.getOrigin(),movement.getAttackRange(),movement.getTilesCheck());
            if(tileM.inArea(movement.getOrigin(),shadowtargetNode, movement.getTilesCheck()) ){
                setOnOff(true);
                Node locn = tileM.GetNodeFromWorld(shadowtargetNode);
                if(locn.occupant==null){
                    Vector3Int loc = new Vector3Int((int)locn.worldPosition.x,(int)locn.worldPosition.y,0);
                    setLocation(loc);
                }
                if(shadowtargetNode == gameObject.GetComponentInParent<ActionCenter>().getMapPos()){
                    setOnOff(false);
                }
            }
            else{
                setOnOff(false);
            }
        }
        if(GameObject.Find("Canvas").GetComponent<EventTrig>().checkOnButton()){
            setOnOff(false);
        }
        if(this.enabled){
            anchored = true;
        }
    }
    public void checkGhost(){
        if(tileM.WorldToCell(transform.position) !=  tileM.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition))){
            setOnOff(false);
        }
    }
    public void setAnchored(bool anchor){
        this.anchored = anchor;
    }
    public void setSprite(Sprite s){
        Ghost_render = this.gameObject.GetComponent<SpriteRenderer>();
        Ghost_render.sprite = s;
        Ghost_render.color = new Color(1f,1f,1f,.5f);
    }
    public void setLocation(Vector3Int pos){
        transform.position = pos;
    }
    public void setOnOff(bool active){
        this.enabled = active;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = active;
    }
}
