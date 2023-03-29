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
    TileManager tileM;

    void Start(){
        if(popwindow==null){
            popwindow = FindInActiveObjectByName("InfoPanel");
            popwindow.SetActive(false);
        }
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
            tileM = GameObject.Find("Tilemanager").GetComponentInChildren<TileManager>();
        }
        this.target = pos;
        this.go = go;
        if(popwindow.name == "InfoPanel"){
            Vector3 newpos = tilemap.GetCellCenterWorld(tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            Vector3 modpos = new Vector3(64,0,0);
            if(GameObject.Find("Main Camera").transform.position.x < GameObject.Find("Canvas").transform.position.x){
                modpos = new Vector3(-64,0,0);
            }
            modpos.y *= 2;
            popwindow.transform.position = newpos + modpos;

        }
        else if(popwindow.name == "AttackConfirm"){
            setAttackConfirmContent();

        }
        
        
    }
    void setAttackConfirmContent(){
            Debug.Log("pog");
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(target);
            Text PlayerStat = popwindow.transform.Find("PlayerInfo").GetComponent<Text>();
            Text EnemyStat = popwindow.transform.Find("EnemyInfo").GetComponent<Text>();
            GameObject enemy = tileM.GetNodeFromWorld(tilemap.WorldToCell(targetPos)).occupant;
            CharacterStat chStat = go.GetComponent<StatUpdate>().getStats();
            CharacterStat enStat = enemy.GetComponent<StatUpdate>().getStats();
            PlayerStat.text = chStat.getAttribute("Type")+"\n";
            PlayerStat.text += "HP: "+go.GetComponent<StatUpdate>().getCurrentHealth() + " / "+ chStat.getStat("maxHealth") + "\n";
            PlayerStat.text += "Damage: \n" + chStat.getBaseDamage() + "\n";
            PlayerStat.text += "Protection: \n" + chStat.getProtection() + "\n";
            EnemyStat.text = enStat.getAttribute("Type")+"\n";
            EnemyStat.text += "HP: "+enemy.GetComponent<StatUpdate>().getCurrentHealth() + " / "+ enStat.getStat("maxHealth") + "\n";
            EnemyStat.text += "Damage: \n" + enStat.getBaseDamage() + "\n";
            EnemyStat.text += "Protection: \n" + enStat.getProtection() + "\n";
            Image PlayerImage = popwindow.transform.Find("PlayerImage").GetComponent<Image>();
            Image EnemyImage = popwindow.transform.Find("EnemyImage").GetComponent<Image>();
            PlayerImage.sprite = go.GetComponent<SpriteRenderer>().sprite;
            EnemyImage.sprite = enemy.GetComponent<SpriteRenderer>().sprite;
            KeyValuePair<float,float> predict = go.GetComponent<StatUpdate>().getAttackSim().atkPossibility(go,enemy);
            Text prediction = popwindow.transform.Find("Prediction").GetComponent<Text>();
            prediction.text = "Sucess rate: " +predict.Key + ", Damage: " + predict.Value;
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
