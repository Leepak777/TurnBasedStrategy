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

    void Start(){
        popwindow = FindInActiveObjectByName("InfoPanel");
        popwindow.SetActive(false);
    }
    GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
    public void togglePanel(Vector3 pos, GameObject go){
        if(popwindow.activeInHierarchy){
            popwindow.SetActive(false);
        }
        else{
            popwindow.SetActive(true);
        }
        if(this.go != go){
            popwindow.SetActive(true);
        }
    }
    public void goAttack(){
        go.GetComponent<CharacterEvents>().onPlayerAttack.Invoke(target);
    }
    public void setLoc(Vector3 pos, GameObject go){
        if(tilemap == null){
            tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        }
        this.target = pos;
        
        if(popwindow.name == "InfoPanel"){
            Vector3 newpos = tilemap.GetCellCenterWorld(tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            Vector3 modpos = new Vector3(64,0,0);
            if(GameObject.Find("Main Camera").transform.position.x < GameObject.Find("Canvas").transform.position.x){
                modpos = new Vector3(-64,0,0);
            }
            modpos.y *= 2;
            popwindow.transform.position = newpos + modpos;

        }
        this.go = go;
        
    }
    public void setLocMenu(){
        if(tilemap == null){
            tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        }
        this.target = go.transform.position;
        Vector3 newpos = tilemap.GetCellCenterWorld(tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        Vector3 modpos = new Vector3(0,128,0);
        /*if(GameObject.Find("Main Camera").transform.position.x < GameObject.Find("Canvas").transform.position.x){
            modpos = new Vector3(-64,0,0);
        }*/
        if(popwindow.name == "Panel"){
            modpos.x *= 2;
        }

        popwindow.transform.position = newpos + modpos;
        
        
    }

    public void setText(){
        Text goType = popwindow.transform.Find("Type").GetComponent<Text>();
        Text goEquipment = popwindow.transform.Find("Equipment").GetComponent<Text>();
        Text goStat = popwindow.transform.Find("Stats").GetComponent<Text>();
        CharacterStat chStat = go.GetComponent<StatUpdate>().getStats();
        goType.text = chStat.getAttribute("Type");
        if(goEquipment.text == ""){
            foreach(string str in eqlst){
                if(chStat.getAttribute(str) != null){
                    goEquipment.text += str+": \n"+ chStat.getAttribute(str) +"\n"; 
                }
            }
        }
        goStat.text = "HP: "+go.GetComponent<StatUpdate>().getCurrentHealth() + " / "+ chStat.getStat("maxHealth") + "\n";
        goStat.text += "Damage: \n" + chStat.getBaseDamage() + "\n";
        goStat.text += "Protection: \n" + chStat.getProtection() + "\n";
    }
}
