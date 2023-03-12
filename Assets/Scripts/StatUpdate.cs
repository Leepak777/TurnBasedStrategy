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
    public float currentHealth;
    public float maxHealth = 100f;
    public int Damage = 0;
    public HealthBar healthBar;
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
    Dictionary<int,float> pastHP = new Dictionary<int, float>();
    Dictionary<int,float> pastFat = new Dictionary<int, float>();
    CharacterStat stats;

    void Start()
    {   
        //stats = AssetDatabase.LoadAssetAtPath<CharacterStat>("Assets/Scripts/Data/"+gameObject.name+".asset");
        ac = this.gameObject.GetComponent<ActionCenter>();
        drn = DRN.getInstance();
        movement = this.gameObject.GetComponent<Movement>(); 
        text = this.gameObject.transform.Find("DamageIndicator").GetComponentInChildren<Text>();
        maxHealth = stats.getStat("maxHealth");
        currentHealth = maxHealth;
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        healthBar = this.gameObject.GetComponentInChildren<HealthBar>();
        for(int i = 0; i < 18; i++){
            buff.Add(false);
        }
        
    }
    public bool meleeRoll(GameObject enemy){
        float player_roll = drn.getDRN() + stats.getMeleeAttackRoll();
        StatUpdate en_stat = enemy.GetComponent<StatUpdate>();
        float enemy_roll = drn.getDRN() + en_stat.getStats().getMeleeDefenceRoll();
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
        float player_roll = drn.getDRN() + stats.getRangeAttackRoll(movement.GetDistance(enpos, playerpos));
        StatUpdate en_stat = enemy.GetComponent<StatUpdate>();
        float enemy_roll = drn.getDRN() + en_stat.getStats().getRangeDefenceRoll();
        //Debug.Log("Player: "+player_roll);
        //Debug.Log("Enemy: " +enemy_roll);
        if(player_roll > enemy_roll){
            return true;
        }
        return false;
    }
    
    public void attackEn(GameObject targetEnemy){
        Vector3Int enpos = tilemap.WorldToCell(targetEnemy.transform.position);
        Vector3Int playerpos = tilemap.WorldToCell(transform.position);
        for(int i = 0; i < stats.getStat("attack_num"); i++){
            bool drn_check;
            if(movement.IsAdjacent(enpos,playerpos)){
                drn_check = meleeRoll(targetEnemy);
            }
            else{
                drn_check = rangeRoll(targetEnemy);
            }
            if(drn_check){
                Damage = (int)(drn.getDRN() + stats.getBaseDamage() + bonus);
                attackingFatigue();
                //targetEnemy.GetComponent<StatUpdate>().TakeDamage(Damage);
                targetEnemy.GetComponent<CharacterEvents>().onDamage.Invoke(Damage);
            }
        }
        
    }

    public void startSaveStat(int gameTurn){
        if(pastFat.ContainsKey(gameTurn) && pastHP.ContainsKey(gameTurn)){
            pastFat[gameTurn] = stats.getStat("fat");
            pastHP[gameTurn] = currentHealth;
        }
        else{
            pastFat.Add(gameTurn, stats.getStat("fat"));
            pastHP.Add(gameTurn, currentHealth);
        }
    }
    public void revertStat(int i){
        if(pastHP.Count > 0){
            Debug.Log(pastHP[i]);
            currentHealth = pastHP[i];
            //pastHP.Remove(i);
            healthBar.UpdateHealth();
        }
        stats.setStat("fat", pastFat[i]);
        //pastFat.Remove(i);
    }
    public void showText(){
        text.enabled = true;
        Invoke("disableText",2f);
    }
    void disableText(){
        text.enabled = false;
    }
    public void TakeDamage(int damage)
    {
        int protection = drn.getDRN() + stats.getProtection();
        //Debug.Log("Damage: "+damage);
        //Debug.Log("Protection: " +protection);
        
        damage -= protection;
        
        if(damage <= 0){
            damage = 0;
        }
        text.text = ""+damage;
        showText();
        //Debug.Log("hit!" + damage);
        currentHealth -= (float)damage;
        if(currentHealth < 0){
            currentHealth = 0;
        }
        healthBar.UpdateHealth();
        flag = false;
        if(currentHealth <= 0){
            tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position),true);
            this.gameObject.SetActive(false);
        }
        attackedFatigue();
    }
    
    public void setStatAsset(CharacterStat s){
        stats = s;
    }
    public CharacterStat getStats(){
        return stats;
    }

    public void updateHealthBar(){
        healthBar.UpdateHealth();
    }
    
    public void Flagging(){
        this.flag = true;
    }

    public float getDictStats(string name){
        return stats.getStat(name);
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
        
        return (int)stats.getStat("mov")/4 +1;
    }

    public void setMaxTiles(int x){
        stats.setStat("mov", (float)x);
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
        return (int)stats.getStat("rng");
    }

    public void attackingFatigue(){
        stats.modifyStat("fat", stats.getStat("enc"));
    }

    public void attackedFatigue(){
        stats.modifyStat("fat",1);
    }

    public void tileFatigue(int tiles){
        stats.modifyStat("fat", tiles);
        //Debug.Log(this.gameObject.tag + "Fatigue: "+ stats["fat"]);

    }

    public void restoreFatigue(){
        stats.modifyStat("fat", -stats.getStat("tou"));
        //Debug.Log("Fatigue: "+fat);
        //Debug.Log("Toughness: "+tou);
        if(stats.getStat("fat") <= 0){
            stats.setStat("fat", 0);
        }
    }

    public void checkFatigue(int tiles){
        tileFatigue(tiles);
        restoreFatigue();
        if(stats.getStat("fat") >= 75){
            stats.modifyStat("rd", -stats.getStat("fat")/5);
            stats.modifyStat("md",-stats.getStat("fat")/10);
            stats.modifyStat("ma",-stats.getStat("fat")/20);
            stats.modifyStat("ra",-stats.getStat("fat")/30);
        }
        if(stats.getStat("fat") >= 100){
            stats.modifyStat("stb",-stats.getStat("fat")-100);
            stats.modifyStat("fat",100);   
        }

    }

    public void destablize(){
        
    }

    public void stabilityCheck(){

    }


}
