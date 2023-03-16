using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    SpriteRenderer Ghost_render;
    Tilemap tilemap;
    TileManager tileM;
    Teleport movement;
    // Start is called before the first frame update
    void Start()
    {   
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        movement = this.gameObject.GetComponentInParent<Teleport>();
        if(this.transform.parent.tag == "Enemy"){
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setGhost(){
        //if(transform.parent.tag == "Player"){
            if(!movement.getisMoving()){
                setOnOff(true);
                Vector3 shadowtarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int shadowtargetNode = tilemap.WorldToCell(shadowtarget);
                if(tileM.inArea(movement.getOrigin(),shadowtargetNode, (int)movement.getTilesCheck())){
                    Node locn = tileM.GetNodeFromWorld(shadowtargetNode);
                    if(locn.walkable){
                        Vector3Int loc = new Vector3Int((int)locn.worldPosition.x,(int)locn.worldPosition.y,0);
                        setLocation(loc);
                    }
                }
                else{
                    setOnOff(false);
                }
            }
            else if(this.enabled){
                setOnOff(false);
            }
        //}
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
