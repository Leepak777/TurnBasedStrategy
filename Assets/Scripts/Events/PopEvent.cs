using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PopEvent : MonoBehaviour
{
    public UnityEvent<Vector3, GameObject> setPos;
    public GameObject go;
    public GameObject popwindow;
    private string[] eqlst = {"Weapon","Shield","Armor","Buckler","Mount"};
    public GameObject type;
    public GameObject equipment;
    public GameObject stat;

    Vector3 target;
    Tilemap tilemap;

    public void togglePanel(){
        popwindow.SetActive(!popwindow.activeInHierarchy);
    }
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
        if(popwindow.name == "Panel"){
            modpos.x *= 2;
        }
        popwindow.transform.position = newpos + modpos;
        this.go = go;
        
    }

    public void setText(){
        Text goType = type.GetComponent<Text>();
        Text goEquipment = equipment.GetComponent<Text>();
        Text goStat = stat.GetComponent<Text>();
        CharacterStat chStat = go.GetComponent<StatUpdate>().getStats();
        goType.text = chStat.getAttribute("Type");
        foreach(string str in eqlst){
            if(chStat.getAttribute(str) != null){
            goEquipment.text += str+": \n"+ chStat.getAttribute(str) +"\n"; 
            }
        }
        goStat.text = "HP: "+go.GetComponent<StatUpdate>().getCurrentHealth() + " / "+ chStat.getStat("maxHealth") + "\n";
        goStat.text += "Damage: \n" + chStat.getBaseDamage() + "\n";
        goStat.text += "Protection: \n" + chStat.getProtection() + "\n";
    }
}
