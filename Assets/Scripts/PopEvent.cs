using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
public class PopEvent : MonoBehaviour
{
    public UnityEvent<Vector3, GameObject> setPos;
    GameObject go;
    Vector3 target;
    Tilemap tilemap;

    public void goAttack(){
        go.GetComponent<CharacterEvents>().onPlayerAttack.Invoke(target);
    }
    public void setLoc(Vector3 pos, GameObject go){
        if(tilemap == null){
            tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        }
        this.target = pos;
        Vector3 newpos = tilemap.GetCellCenterWorld(tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        Vector3 modpos = new Vector3(64,0,0);
        if(GameObject.Find("Main Camera").transform.position.x < GameObject.Find("Canvas").transform.position.x){
            modpos = new Vector3(-64,0,0);
        }
        this.transform.position = newpos + modpos;
        this.go = go;
        
    }
}
