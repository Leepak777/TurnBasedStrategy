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
    public float bonus = 0;
    TileManager tileM;
    UDictionary<KeyValuePair<string,string>,int> buffs = new UDictionary<KeyValuePair<string, string>, int>();
    UDictionary<KeyValuePair<string,string>,UDictionary<string,int>> effect_Bonus = new UDictionary<KeyValuePair<string,string>,UDictionary<string, int>>();
    public UDictionary<string,float> backup = new UDictionary<string,float>();
    DRN drn;
    Text text;
    float backupHP = 0;
    Vector3 backupLoc = new Vector3();
    public CharacterStat stats;
    AttackSimulation atkSim;
    void Start()
    {   
        DeleteAssets(gameObject.name);
        //stats = AssetDatabase.LoadAssetAtPath<CharacterStat>("Assets/Scripts/Data/"+gameObject.name+".asset");
        CharacterStat baseData = (AssetDatabase.LoadAssetAtPath<CharacterStat>("Assets/Scripts/Data/"+gameObject.name+"(base).asset"));
        //if(baseData!=null){Debug.Log("pog");stats.fetchBase(baseData);}
        stats = Instantiate(baseData);
        AssetDatabase.CreateAsset(stats, "Assets/Scripts/Data/"+gameObject.name+".asset");
        AssetDatabase.SaveAssets();
        maxHealth = stats.getStat("maxHealth");
        currentHealth = maxHealth;
        drn = DRN.getInstance();
        if(SceneManager.GetActiveScene().name != "MapSelection"){
            text = this.gameObject.transform.Find("DamageIndicator").GetComponentInChildren<Text>();
        }
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
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
       
            bool drn_check;
            if(tileM.IsAdjacent(enpos,playerpos)){
                drn_check = meleeRoll(targetEnemy);
            }
            else{
                drn_check = rangeRoll(targetEnemy);
            }
            if(drn_check){
                Damage = (int)(drn.getDRN() + stats.getBaseDamage() + bonus);
                attackingFatigue();
                targetEnemy.GetComponentInChildren<CharacterEvents>().onDamage.Invoke(Damage);
            }
        
        
    }
    public void BuffMaintainCheck(){
        for(int i= 0; i<buffs.Count;i++){
            if(buffs.ElementAt(i).Key.Key == "WaterStance"){
                currentHealth += stats.getStat("acu");
                stats.modifyStat("ene", Math.Max(10-stats.getStat("mid") , 3)*stats.getCostMul());
                stats.modifyStat("fat", Math.Max(10-stats.getStat("tou") , 3)*stats.getCostMul());
                stats.modifyStat("stb", Math.Max(10-stats.getStat("acu") , 3)*stats.getCostMul());
            }
            if(buffs.ElementAt(i).Key.Key == "FireStance"){
                stats.modifyStat("fat", stats.getStat("acu")) ;
                stats.modifyStat("ene", Math.Max(10-stats.getStat("mid") , 3)*stats.getCostMul());
                stats.modifyStat("hp", Math.Max(10-stats.getStat("tou") , 3)*stats.getCostMul());
                stats.modifyStat("stb", Math.Max(10-stats.getStat("acu") , 3)*stats.getCostMul());
            }
        }
    }
    public void saveStat(){
        backupHP = currentHealth;
        backupLoc = transform.position;
        backup = stats.getStatLst();
    }
    public void revert(){
        currentHealth = backupHP;
        tileM.setWalkable(gameObject, tileM.WorldToCell(transform.position),true);
        transform.position = backupLoc; 
        tileM.setWalkable(gameObject, tileM.WorldToCell(transform.position),false);
        stats.revertStat(backup);
        gameObject.GetComponentInChildren<HealthBar>().UpdateHealth();
        gameObject.GetComponentInChildren<CharacterEvents>().unHighLightRechable.Invoke();
        if(currentHealth > 0 && !gameObject.activeInHierarchy){
            gameObject.SetActive(true);
        }
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
        Debug.Log("Damage:"+damageReceived+"Protection: " +protection);
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


    public float getBonus(){
        return bonus;
    }
    public void setBonus(int bonus){
        this.bonus += bonus;
    }
    public AttackSimulation getAttackSim(){
        return atkSim;
    }
    public int getMaxTiles(){
        if(stats == null){
            stats = AssetDatabase.LoadAssetAtPath<CharacterStat>("Assets/Scripts/Data/"+gameObject.name+".asset");
        }
        return (int)stats.getStat("mov")/4 +1;
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
        return stats.getStat("rng");
    }

    public void attackingFatigue(){
        stats.modifyStat("fat", stats.getStat("enc"));
    }
    public bool addBuff(string buff, string character, int duration){
        KeyValuePair<string,string> pair = new KeyValuePair<string,string>(buff,character);
        if(!buffs.ContainsKey(pair)){
            buffs.Add(pair,duration);
            return true;
        }
        return false;
        
    }
    public bool removeBuff(string buff, string character){
        KeyValuePair<string,string> pair = new KeyValuePair<string,string>(buff,character);
        if(buffs.ContainsKey(pair)){
            buffs.Remove(pair);
            return true;
        }
        return false;
        
    }
    public void addEffectStat(KeyValuePair<string,string> pair, UDictionary<string,int> dict){
        effect_Bonus.Add(pair,dict);
    }
    public int getPreBonusStat(KeyValuePair<string,string> name,string targetStat){
        int PreBonus = effect_Bonus[name][targetStat];
        effect_Bonus[name].Remove(targetStat);
        return PreBonus;
    }
    public bool isBuff(string name,string character){
        return buffs.ContainsKey(new KeyValuePair<string,string>(name,character));
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

    public void getTileM(){
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
    }

    static void DeleteAssets(string name)
    {
        string targetSubstring = name; // Change this to your desired substring

        string[] assetPaths = AssetDatabase.GetAllAssetPaths(); // Get all asset paths in the project
        int count = 0;

        foreach (string path in assetPaths)
        {
            if (!path.EndsWith("(base).asset") && path.Contains(targetSubstring))
            {
                AssetDatabase.DeleteAsset(path);
                count++;
            }
        }

        AssetDatabase.Refresh(); // Refresh the asset database to update the project window
        //Debug.Log("Deleted " + count + " .asset files with substring '" + targetSubstring + "'");
    }
}
