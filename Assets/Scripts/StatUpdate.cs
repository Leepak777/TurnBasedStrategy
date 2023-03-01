using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;



public class StatUpdate : MonoBehaviour
{
    public UDictionary<string,float> stats = new UDictionary<string,float>();
    public void setUp(){
        //Raw stat
        stats.Add("pow",0);//power
        stats.Add("dex",0);//dexterity
        stats.Add("tou",0);//toughness
        stats.Add("acu",0);//accuity
        stats.Add("mid",0);//mind
        stats.Add("ma",0);//melee attack
        stats.Add("ra",0);//range attack
        stats.Add("md",0);//melee defence
        stats.Add("rd",0);//range defence
        stats.Add("sa",0);//spell attack
        stats.Add("mr",0);//magic resistance
        stats.Add("base_hp",0);
        stats.Add("base_mov",0);
        stats.Add("base_init",0);
        stats.Add("base_enc",0);
        stats.Add("base_ene",0);
        //weapon
        stats.Add("wd",0);//weapon damage
        stats.Add("rng",0);//range
        stats.Add("pscal",0);//pow scaling
        stats.Add("dscal",0);//dex scaling
        stats.Add("sp",0);//shield piercing
        stats.Add("ap",0);//armor piercing
        stats.Add("acc",0);//accuracy
        stats.Add("pr",0);//parry rating
        stats.Add("pv",0);//parry value
        stats.Add("w_enc",0);
        stats.Add("attack_num",0);
        //armor
        stats.Add("av",0);//armor value
        stats.Add("sv",0);//shield value
        stats.Add("mdb",0);//melee defence bonus
        stats.Add("rdb",0); //range defence bonus
        stats.Add("eq_mov",0);
        stats.Add("eq_init",0);
        stats.Add("eq_enc",0);
         //calculated stat
        stats.Add("hp",0);//health point
        stats.Add("ene",0);//energy
        stats.Add("fat",0);//fatigue
        stats.Add("stb",0);//stability
        stats.Add("maxstb",0); 
        stats.Add("mov",0);//movement range
        stats.Add("bite",0);//base initiative
        stats.Add("enc",0);//encumbrance
        stats.Add("size",0);//size
    } 
    public void setStats(Dictionary<string,float> input){
        foreach(KeyValuePair<string,float> s in input){
            if(stats.ContainsKey(s.Key)){
                stats[s.Key] += input[s.Key];
            }
        }
    }

    public void setCalStat(){
        stats["hp"] = stats["tou"]*5 + stats["pow"] + stats["base_hp"];
        stats["maxHealth"] = stats["hp"];
        stats["ene"] = stats["mid"] * 4 + stats["acu"] + stats["base_ene"];
        stats["fat"] = 0;
        stats["stb"] = stats["mid"]*5 +stats["acu"]*3 + stats["tou"] *2;
        stats["maxStb"] = stats["mid"] * 7 + stats["acu"] * 4 + stats["tou"] * 3;
        stats["mov"] = stats["base_mov"] + stats["pow"]/2 + stats["eq_mov"];
        stats["maxTiles"] = (int)stats["mov"];
        stats["enc"] = stats["w_enc"] + stats["eq_enc"] + stats["base_enc"] - stats["pow"]/4;
        stats["bite"] = stats["base_init"] + stats["dex"] + stats["acu"] + stats["eq_init"] - stats["enc"];
    }

    public bool meleeRoll(GameObject enemy){
        float player_roll = drn.getDRN() + stats["ma"]+ stats["acc"] + stats["pow"]/2 + stats["dex"] + stats["acu"]/2;
        StatUpdate en_stat = enemy.GetComponent<StatUpdate>();
        float enemy_roll = drn.getDRN() + en_stat.getDictStats("md") + en_stat.getDictStats("mdb") + en_stat.getDictStats("tou")/2 + en_stat.getDictStats("dex")/2 + en_stat.getDictStats("acu")/2;
        //Debug.Log("Player: "+player_roll);
        //Debug.Log("Enemy: " +enemy_roll);
        if(player_roll > enemy_roll){
            return true;
        }
        return false;
    }

    public bool rangeRoll(GameObject enemy){
        Vector3Int enpos = tilemap.WorldToCell(enemy.transform.position);
        Vector3Int playerpos = tilemap.WorldToCell(transform.position);
        float player_roll = drn.getDRN() + stats["ra"]+ stats["acc"] + stats["dex"] + stats["acu"]/2 - movement.GetDistance(enpos, playerpos);
        StatUpdate en_stat = enemy.GetComponent<StatUpdate>();
        float enemy_roll = drn.getDRN() + en_stat.getDictStats("rd") + en_stat.getDictStats("rdb") + en_stat.getDictStats("dex")/2 + en_stat.getDictStats("acu")/2;
        //Debug.Log("Player: "+player_roll);
        //Debug.Log("Enemy: " +enemy_roll);
        if(player_roll > enemy_roll){
            return true;
        }
        return false;
    }

    float currentHealth;
    public float maxHealth = 100f;
    public float Damage = 0;
    public HealthBar healthBar;
    TurnManager tm;
    public List<bool> buff =new List<bool>();
    public bool flag = false;
    public float bonus = 0;
    Tilemap tilemap;
    TileManager tileM;
    Movement movement;
    ActionCenter ac;
    DRN drn;
    int attackrange = 1;
    Text text;

    public void attackEn(GameObject targetEnemy){
        Vector3Int enpos = tilemap.WorldToCell(targetEnemy.transform.position);
        Vector3Int playerpos = tilemap.WorldToCell(transform.position);
        bool drn_check;
        if(movement.IsAdjacent(enpos,playerpos)){
            drn_check = meleeRoll(targetEnemy);
        }
        else{
            drn_check = rangeRoll(targetEnemy);
        }
        if(drn_check){
            Damage = drn.getDRN() + stats["wd"] + stats["pscal"] * stats["pow"] + stats["dscal"]*stats["dex"] + bonus;
            attackingFatigue();
            targetEnemy.GetComponent<StatUpdate>().TakeDamage(Damage);
            targetEnemy.GetComponent<StatUpdate>().attackedFatigue();
        }
        
    }



    void Start()
    {   
        ac = this.gameObject.GetComponent<ActionCenter>();
        drn = this.gameObject.GetComponent<DRN>();
        movement = this.gameObject.GetComponent<Movement>(); 
        text = this.gameObject.GetComponentInChildren<Text>();
        currentHealth = maxHealth;
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        tm = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        //healthBar = this.gameObject.GetComponentInChildren<HealthBar>();
        for(int i = 0; i < 18; i++){
            buff.Add(false);
        }
        
    }
    void Update(){
        

        
        
    }

    void showText(){
        text.enabled = true;
        Invoke("disableText",2f);
    }
    void disableText(){
        text.enabled = false;
    }
    public void TakeDamage(float damage)
    {
        
        float protection = drn.getDRN() + stats["sv"] * (100-stats["sp"])/100 + stats["av"]*(100 -stats["ap"])/100 + stats["tou"]/4;
        //Debug.Log("Damage: "+damage);
        //Debug.Log("Protection: " +protection);
        
        damage -= protection;
        
        if(damage <= 0){
            damage = 0;
        }
        text.text = ""+damage;
        showText();
        //Debug.Log("hit!" + damage);
        currentHealth -= damage;
        if(currentHealth < 0){
            currentHealth = 0;
        }
        healthBar.UpdateHealth(currentHealth);
        flag = false;
        if(currentHealth <= 0){
            tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position),true);
            tm.removefromLst(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void modifyStat(Dictionary<string,float> lst, bool buff){
        if(buff){
            foreach(KeyValuePair<string, float> s in lst){
                stats[s.Key] += lst[s.Key];
            }
        }
        else{
            foreach(KeyValuePair<string, float > s in lst){
                stats[s.Key] -= lst[s.Key];
            }
        }
    }

    public void multiplyStat(Dictionary<string,float> lst, bool buff){
        if(buff){
            foreach(KeyValuePair<string, float> s in lst){
                stats[s.Key] *= lst[s.Key];
            }
        }
        else{
            foreach(KeyValuePair<string, float > s in lst){
                stats[s.Key] /= lst[s.Key];
            }
        }
    }
    
    public void Flagging(){
        this.flag = true;
    }

    public float getDictStats(string name){
        return stats[name];
    }

    public bool getbuff(int i){
        return buff[i];
    }

    public void setbuff(int i, bool b){
        buff[i] = b;
    }

    public void setDamage(int i){
        Damage = i;
    }
    public void saddDamage(int i){
        Damage += i;
    }

    public void setBonus(int i ){
        bonus += i;
    }

    public bool getTextEnabled(){
        return text.enabled;
    }

    public void setTextActive(bool active){
        text.enabled = active;
    }

    public int getMaxTiles(){
        
        return (int)stats["mov"]/4 +1;
    }

    public void setMaxTiles(int x){
        stats["mov"] = (float)x;
    }

    public float getMaxHealth(){
        return maxHealth;
    }

    public void setMaxHealth(float x){
        maxHealth = x;
    }
    
    public void setAttackRange(int x){
        attackrange = x;
    }
    public int getAttackRange(){
        return (int)stats["rng"];
    }

    public void attackingFatigue(){
        stats["fat"] += stats["enc"];
    }

    public void attackedFatigue(){
        stats["fat"]++;
    }

    public void tileFatigue(int tiles){
        
        stats["fat"] += tiles;
        //Debug.Log(this.gameObject.tag + "Fatigue: "+ stats["fat"]);

    }

    public void restoreFatigue(){
        stats["fat"] -= stats["tou"];
        //Debug.Log("Fatigue: "+fat);
        //Debug.Log("Toughness: "+tou);
    if(stats["fat"] <= 0){
            stats["fat"] = 0;
        }
    }

    public void checkFatigue(int tiles){
        tileFatigue(tiles);
        restoreFatigue();
        if(stats["fat"] >= 75){
            stats["rd"] -= stats["fat"] / 5;
            stats["md"] -= stats["fat"] / 10;
            stats["ma"] -= stats["fat"] / 20;
            stats["ra"] -= stats["fat"] / 30;
        }
        if(stats["fat"] >= 100){
            stats["stb"] -= stats["fat"] - 100;
            stats["fat"] = 100;
        }

    }

    public void destablize(){
        
    }

    public void stabilityCheck(){

    }


}
