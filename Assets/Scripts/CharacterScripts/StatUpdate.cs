using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
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
    public List<bool> buff =new List<bool>();
    public float bonus = 0;
    TileManager tileM;
    DRN drn;
    Text text;
    Dictionary<int,float> pastHP = new Dictionary<int, float>();
    Dictionary<int,float> pastFat = new Dictionary<int, float>();
    CharacterStat stats;
    AttackSimulation atkSim;
    void Start()
    {   
        stats = AssetDatabase.LoadAssetAtPath<CharacterStat>("Assets/Scripts/Data/"+gameObject.name+".asset");
        maxHealth = stats.getStat("maxHealth");
        currentHealth = maxHealth;
        drn = DRN.getInstance();
        if(SceneManager.GetActiveScene().name != "MapSelection"){
            text = this.gameObject.transform.Find("DamageIndicator").GetComponentInChildren<Text>();
        }
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        for(int i = 0; i < 18; i++){
            buff.Add(false);
        }
        atkSim = new AttackSimulation(drn);
    }
    public bool meleeRoll(GameObject enemy){
        float player_roll = drn.getDRN() + stats.getMeleeAttackRoll();
        StatUpdate en_stat = enemy.GetComponent<StatUpdate>();
        float enemy_roll = drn.getDRN() + en_stat.getStats().getMeleeDefenceRoll();
        //Debug.Log("Player: "+player_roll);
        //Debug.Log("Enemy: " +enemy_roll);
        return player_roll > enemy_roll;
    }

    public bool rangeRoll(GameObject enemy){
        Vector3Int enpos = tileM.WorldToCell(enemy.transform.position);
        Vector3Int playerpos = tileM.WorldToCell(transform.position);
        float player_roll = drn.getDRN() + stats.getRangeAttackRoll(tileM.GetDistance(enpos, playerpos));
        StatUpdate en_stat = enemy.GetComponent<StatUpdate>();
        float enemy_roll = drn.getDRN() + en_stat.getStats().getRangeDefenceRoll();
        //Debug.Log("Player: "+player_roll);
        //Debug.Log("Enemy: " +enemy_roll);
        return player_roll > enemy_roll;
    }
    
    public void attackEn(GameObject targetEnemy){
        Vector3Int enpos = tileM.WorldToCell(targetEnemy.transform.position);
        Vector3Int playerpos = tileM.WorldToCell(transform.position);
        for(int i = 0; i < stats.getStat("attack_num"); i++){
            bool drn_check;
            if(tileM.IsAdjacent(enpos,playerpos)){
                drn_check = meleeRoll(targetEnemy);
            }
            else{
                drn_check = rangeRoll(targetEnemy);
            }
            if(drn_check){
                //Debug.Log("Roll Sucess!");
                Damage = (int)(drn.getDRN() + stats.getBaseDamage() + bonus);
                attackingFatigue();
                targetEnemy.GetComponentInChildren<CharacterEvents>().onDamage.Invoke(Damage);
            }
            else{
                //Debug.Log("Roll Fail!");
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
        if(pastFat.ContainsKey(i)){
            if(pastHP.Count > 0){
                currentHealth = pastHP[i];
                //pastHP.Remove(i);
            }
            stats.setStat("fat", pastFat[i]);
        }
        //pastFat.Remove(i);
    }
    public void showText(){
        text.enabled = true;
        Invoke("disableText",2f);
    }
    void disableText(){
        text.enabled = false;
    }
    public void TakeDamage(int damageReceived)
    {
        int protection = drn.getDRN() + stats.getProtection();

        
        damageReceived -= protection;

        if(damageReceived <= 0){
            damageReceived = 0;
        }
        text.text = ""+damageReceived;
        showText();
        //Debug.Log("hit!" + damage);
        currentHealth -= (float)damageReceived;
        if(currentHealth < 0){
            currentHealth = 0;
        }
        if(currentHealth <= 0){
            tileM.setWalkable(this.gameObject,tileM.WorldToCell(transform.position),true);
            this.gameObject.SetActive(false);
        }
        attackedFatigue();
    }

    public CharacterStat getStats(){
        return stats;
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
    public float getBonus(){
        return bonus;
    }

    public bool getTextEnabled(){
        return text.enabled;
    }
    public AttackSimulation getAttackSim(){
        return atkSim;
    }
    public void setTextActive(bool active){
        text.enabled = active;
    }

    public int getMaxTiles(){
        if(stats == null){
            stats = AssetDatabase.LoadAssetAtPath<CharacterStat>("Assets/Scripts/Data/"+gameObject.name+".asset");
        }
        return (int)stats.getStat("mov")/4 +1;
    }

    public void setMaxTiles(int x){
        stats.setStat("mov", (float)x);
    }

    public float getMaxHealth(){
        return maxHealth;
    }
    public float getCurrentHealth(){
        return currentHealth;
    }
    public void setMaxHealth(float x){
        maxHealth = x;
    }
    public float getAttackRange(){
        return 1;//stats.getStat("rng");
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

    public void checkFatigue(){
        tileFatigue(gameObject.GetComponent<Teleport>().gettrailCount());
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
    public void startFatCheck(){
        if(stats.getStat("fat") > 100){
            this.gameObject.GetComponentInChildren<CharacterEvents>().onEnd.Invoke(1);
        }
    }
    public void destablize(){
        
    }

    public void stabilityCheck(){

    }


}
