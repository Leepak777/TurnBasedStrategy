using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PopEvent : MonoBehaviour
{
    public UnityEvent<GameObject, GameObject> setPos;
    public GameObject go;
    public GameObject popwindow;
    private string[] eqlst = {"Weapon","Shield","Armor","Buckler","Mount"};
    public GameObject type;
    public GameObject equipment;
    public GameObject stat;
    public GameObject target;
    TileManager tileM;

    void Start(){
        if(popwindow==null){
            popwindow = FindInActiveObjectByName("InfoPanel");
            popwindow.SetActive(false);
        }
        tileM = GameObject.Find("Tilemanager").GetComponentInChildren<TileManager>();
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
    public void togglePanel(GameObject pos, GameObject go){
        if(popwindow.activeInHierarchy){
            popwindow.SetActive(false);
        }
        else{
            popwindow.SetActive(true);
        }
    }
    public void toggleP(){
        if(popwindow.activeInHierarchy){
            popwindow.SetActive(false);
        }
        else{
            popwindow.SetActive(true);
        }
        if(SceneManager.GetActiveScene().name == "MapSelection"){
            if(transform.parent.GetComponent<PositionSetup>().checkisDragging()){
                popwindow.SetActive(false);
            }
        }
    }
    public void goAttack(){
        go.GetComponentInChildren<CharacterEvents>().onPlayerAttack.Invoke(target);
    }
    public void setLoc(GameObject target, GameObject go){
        this.target = target;
        this.go = go;
        if(popwindow.name == "AttackConfirm"){
            setAttackConfirmContent();
        }
        if(popwindow.name =="InfoPanel"){
            setText();
        }
        
    
    }
    void setAttackConfirmContent(){
            Text PlayerStat = popwindow.transform.Find("PlayerInfo").GetComponent<Text>();
            Text EnemyStat = popwindow.transform.Find("EnemyInfo").GetComponent<Text>();
            GameObject enemy = target;
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
        if(tileM == null){
            tileM = GameObject.Find("Tilemanager").GetComponentInChildren<TileManager>();
        }
        Vector3 newpos = tileM.GetCellCenterWorld(tileM.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        Vector3 modpos = new Vector3(0,128,0);
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
        goEquipment.text ="";
        if(goEquipment.text == ""){
            foreach(string str in eqlst){
                if(chStat.getAttribute(str) != null){
                    goEquipment.text += str+": \n"+ chStat.getAttribute(str) +"\n"; 
                }
            }
        }
        goStat.text = "";
        foreach(KeyValuePair<string,string> pair in chStat.getAbilities()){
            goStat.text += pair.Key+"\n";
        }
        goStat.text += "HP: "+go.GetComponent<StatUpdate>().getCurrentHealth() + " / "+ chStat.getStat("maxHealth") + "\n";
        goStat.text += "Damage: \n" + chStat.getBaseDamage() + "\n";
        goStat.text += "Protection: \n" + chStat.getProtection() + "\n";
    }
}
