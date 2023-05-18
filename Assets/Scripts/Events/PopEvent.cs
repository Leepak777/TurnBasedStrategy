using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.IO;
using UnityEngine.AI;
using Random = System.Random;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
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
    public UI ui;
    Random rnd;

    void Start(){
        rnd = new Random((int)Time.time*1000);
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
            //popwindow.SetActive(false);
            //DeleteStat();
        }
        else{
            DeleteStat();
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
            DeleteStat();
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
            /*PlayerStat.text += "HP: "+go.GetComponent<StatUpdate>().getCurrentHealth() + " / "+ chStat.getStat("maxHealth") + "\n";
            PlayerStat.text += "Damage: \n" + chStat.getBaseDamage() + "\n";
            PlayerStat.text += "Protection: \n" + chStat.getProtection() + "\n";*/
            EnemyStat.text = enStat.getAttribute("Type")+"\n";
            /*EnemyStat.text += "HP: "+enemy.GetComponent<StatUpdate>().getCurrentHealth() + " / "+ enStat.getStat("maxHealth") + "\n";
            EnemyStat.text += "Damage: \n" + enStat.getBaseDamage() + "\n";
            EnemyStat.text += "Protection: \n" + enStat.getProtection() + "\n";*/
            Image PlayerImage = popwindow.transform.Find("PlayerImage").GetComponent<Image>();
            Image EnemyImage = popwindow.transform.Find("EnemyImage").GetComponent<Image>();
            PlayerImage.sprite = go.GetComponent<SpriteRenderer>().sprite;
            EnemyImage.sprite = enemy.GetComponent<SpriteRenderer>().sprite;
            /*KeyValuePair<float,float> predict = go.GetComponent<StatUpdate>().getAttackSim().atkPossibility(go,enemy);
            Text prediction = popwindow.transform.Find("Prediction").GetComponent<Text>();
            prediction.text = "Sucess rate: " +predict.Key + ", Damage: " + predict.Value;*/
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

    public void self(){
        Text goType = popwindow.transform.Find("TypeI").GetComponentInChildren<Text>();
        Text goEquipment = popwindow.transform.Find("Equipment").GetComponent<Text>();
        //Text goStat = popwindow.transform.Find("Stats").GetComponent<Text>();
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
       /* goStat.text = "";
        foreach(KeyValuePair<string,string> pair in chStat.getAbilities()){
            goStat.text += pair.Key+"\n";
        }
        goStat.text += "HP: "+go.GetComponent<StatUpdate>().getCurrentHealth() + " / "+ chStat.getStat("maxHealth") + "\n";
        goStat.text += "Damage: \n" + chStat.getBaseDamage() + "\n";
        goStat.text += "Protection: \n" + chStat.getProtection() + "\n";*/
    }
    public void mapSelectInfo(){
        go = this.transform.parent.gameObject;
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
    public void setTypeEQText(CharacterStat chStat){
        Text goType = popwindow.transform.Find("TypeI").GetComponentInChildren<Text>();
        Text goEquipment = popwindow.transform.Find("Equipment").GetComponent<Text>();
        goType.text = chStat.getAttribute("Type");
        goEquipment.text ="";
        if(goEquipment.text == ""){
            foreach(string str in eqlst){
                if(chStat.getAttribute(str) != null){
                    goEquipment.text += str+": \n"+ chStat.getAttribute(str) +"\n"; 
                }
            }
        }
    }
    public void addInfo(string stat, string value){
        GameObject goParent = GameObject.Find("StatPanel");
        GameObject prefab = Resources.Load<GameObject>("Stat") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.name = stat;
        player.GetComponentInChildren<Text>().text = stat +": "+value;
    }
    public void DeleteStat(){
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Stat")){
            Destroy(go);
        }
    }
    public void setText(){
        //
        if(ui == null){ui = GameObject.Find("UICanvas").GetComponent<UI>();}
        GameObject current = ui.getCurrentPlay();
        Debug.Log(current.name +","+target.name);
        Text goType = popwindow.transform.Find("TypeI").GetComponentInChildren<Text>();
        Text goEquipment = popwindow.transform.Find("Equipment").GetComponent<Text>();
        //Text goStat = popwindow.transform.Find("ScrollStat").GetComponentInChildren<Text>();
        //current: currentplay, target: target object
        
        StatUpdate currentSU = current.GetComponent<StatUpdate>();
        StatUpdate targetSU= target.GetComponent<StatUpdate>();

        CharacterStat currentStat = currentSU.getStats();
        CharacterStat targetStat = targetSU.getStats();
        KeyValuePair<float,float> predict = go.GetComponent<StatUpdate>().getAttackSim().atkPossibility(current,target);
        string prediction = "" +predict.Key;
        if(current.name == target.name){
            self();
            getInfo(targetStat,currentStat,1);
        }
        else if(current.tag == target.tag){
            setTypeEQText(targetStat);
            getInfo(targetStat,currentStat,1);
        }
        else if(!currentStat.getAbilities().ContainsKey("Psychometry")){
            goType.text = "Access Denied";
            goEquipment.text = "";
            //goStat.text = "";
        }
        else{
            float diff = currentStat.stats["acu"] - targetStat.stats["acu"];
            //Debug.Log(currentStat.stats["acu"] + " - " + targetStat.stats["acu"]+"="+diff);
            if(diff < -5){
             //goStat.text = 
             setTypeEQText(targetStat);
             getInfo(targetStat,currentStat,-1);
            }
            else if(diff >= -5 && diff <= -3){
             //goStat.text = 
             setTypeEQText(targetStat);
             getInfo(targetStat,currentStat,2);
            }
            else if(diff >= -2 && diff <= -1){
                goType.text = "Access Denied";
                goEquipment.text = "";
                //goStat.text = "";
            }
            else if(diff == 0){
                //whether >50% attack sucess, stats higher or lower, hp > or <  50%
                //goStat.text = 
                setTypeEQText(targetStat);
                if(predict.Key > 50){prediction = ">50%";}
                else{prediction = "<50%";}
                addInfo("Sucess Rate",prediction);
                getInfo(targetStat,currentStat,0);
            }
            else if(diff <= 2 && diff >= 1){
                //goStat.text = 
                setTypeEQText(targetStat);
                if(predict.Key < 20){prediction = "very unlikely";}
                else if(predict.Key >= 20 && predict.Key <40){prediction = "unlikely";}
                else if(predict.Key >= 40 && predict.Key <60){prediction = "even";}
                else if(predict.Key >= 60 && predict.Key <80){prediction = "likely";}
                else if(predict.Key >= 80 && predict.Key <100){prediction = "very likely";}
                addInfo("Sucess Rate",prediction);
                getInfo(targetStat,currentStat,20);
            }
            else if(diff <= 4 && diff >= 3){
                //goStat.text = 
                setTypeEQText(targetStat);
                addInfo("Sucess Rate",gethitChance(predict.Key, 10));
                getInfo(targetStat,currentStat,10);
            }
            else if(diff >= 5 && diff <= 6){
                //goStat.text = 
                setTypeEQText(targetStat);
                addInfo("Sucess Rate",gethitChance(predict.Key, 5));
                getInfo(targetStat,currentStat,5);
            }
            else if(diff > 6){
             //goStat.text = 
                setTypeEQText(targetStat);
                addInfo("Sucess Rate",prediction);
                getInfo(targetStat,currentStat,1);
            }

        }
    }

    
    void getInfo(CharacterStat targetStat, CharacterStat currentStat, int x){
        string lst = "";
        foreach(KeyValuePair<string, float> pair in targetStat.stats){
            string showValue = pair.Value * x +"\n";
            if(x == 5){
                showValue = getPercent(pair.Value, currentStat.stats[pair.Key], 0.05f, -0.95f)+"\n";
            }
            if(x == 10){
                showValue = getPercent(pair.Value, currentStat.stats[pair.Key], 0.1f, -0.9f)+"\n";
            }
            if(x == 20){
                showValue = getPercent(pair.Value, currentStat.stats[pair.Key], 0.20f, -0.80f)+"\n";
            }
            if(x == 2){
                showValue = rnd.Next(0, (int)Math.Max(pair.Value, currentStat.stats[pair.Key]))+"\n";
            }
            if(x == 0){
                if(pair.Value< currentStat.stats[pair.Key]){
                    showValue = "Less"+"\n";
                }
                else{
                    showValue = "More"+"\n";
                }
                //showValue = /*getPercent(pair.Value, currentStat.stats[pair.Key], 1f, 0f)*/+"\n";
            }
            //lst += pair.Key + " : " + showValue;
            //Debug.Log(pair.Key+":"+showValue);
            if(getfullname(pair.Key) != "none"){
                addInfo(getfullname(pair.Key),showValue);
            }
            else{
                addInfo(pair.Key,showValue);
            }
        }
        //return lst;

    }
    string gethitChance(float chance, float percent){
        for(float i = 0; i < 100; i+=percent){
            if(chance >= i && chance < i+percent){
                return "~"+i+"-"+(i+percent)+"%";
            }
        }
        return null;
    }
    string getPercent(float targetV, float currentV, float percent, float start){
        float diff = currentV - targetV;
        start = 0;
        while(start < 100){
            //Debug.Log(Math.Abs(diff/currentV)*100 + ","+","+currentV+","+start +","+(start+(percent*100)));
            if(currentV == 0){
                if(diff<0){return "Less";}
                else{return "More";}
            }
            if(Math.Abs(diff/currentV)*100 >= start && Math.Abs(diff/currentV)*100 <= start+(percent*100)){
                if(diff <0){
                    return "<"+start+"-"+(start+(percent*100));
                }
                else{
                    return ">" + start+"-"+(start+(percent*100));
                }
            }
                start += (percent*100);
        }        
        return null;
    }
    public string getfullname(string i){
        string full = "none";
        switch(i){
            case "wd": full = "Weapon Damage";break;
            case "pow": full = "Power";break;
            case "dex": full = "Dexterity";break;
            case "tou": full = "Toughness";break;
            case "acu": full = "Acuity";break;
            case "mid": full = "Mind";break;
            case "hp": full = "Hit Point";break;
            case "ene": full = "Energy";break;
            case "enc": full = "Encumbrance";break;
            case "fat": full = "Fatigue";break;
            case "stb": full = "Stability";break;
            case "ma": full = "Melee Attack";break;
            case "ra": full = "Range Attack";break;
            case "md": full = "Melee Defence";break;
            case "rd": full = "Ranged Defence";break;
            case "sa": full = "Spell Attack";break;
            case "mr": full = "Magic Resistance";break;
            case "rng": full = "Range";break;
            case "pscal": full = "Pow Scaling";break;
            case "dscal": full = "Dex Scaling";break;
            case "sp": full = "Shield Piercing";break;
            case "ap": full = "Armor Piercing";break;
            case "acc": full = "Accuracy Bonus";break;
            case "av": full = "Armor Value";break;
            case "sv": full = "Shield Value";break;
            case "mdb": full = "Melee Defence Bonus";break;
            case "rdb": full = "Ranged Defence Bonus";break;
            case "pr": full = "Parry Rating";break;
            case "pv": full = "Parry Value";break;
            case "mov": full = "Movement range";break;
            case "bit": full = "Base Initiative";break;
            case "base_hp": full = "Base HP";break;
            case "base_ene": full = "Base Energy";break;
            case "base_mov": full = "Base Movement range";break;
            case "base_init": full = "Base Initiative";break;
            case "base_enc": full = "Base Encumbrance";break;
            case "attack_num": full = "Attack times";break;
            default: full = "none";break;
        }
        return full;
    }
    

}
