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
    UDictionary<KeyValuePair<string,string>,int> endbuffs = new UDictionary<KeyValuePair<string, string>, int>();
    UDictionary<KeyValuePair<string,string>,int> startbuffs = new UDictionary<KeyValuePair<string, string>, int>();
    UDictionary<KeyValuePair<string,string>,UDictionary<string,int>> effect_Bonus = new UDictionary<KeyValuePair<string,string>,UDictionary<string, int>>();
    public UDictionary<string,float> backup = new UDictionary<string,float>();
    DRN drn;
    Text text;
    float backupHP = 0;
    bool skip = false;
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
            //drn_check = true;
            if(drn_check && !targetEnemy.GetComponent<StatUpdate>().isTimeStop()){
                Damage = (int)(drn.getDRN() + stats.getBaseDamage() + bonus);
                attackingFatigue();
                targetEnemy.GetComponentInChildren<CharacterEvents>().onDamage.Invoke(Damage);
            }
        
        
    }
    
    public void BuffMaintainCheck(KeyValuePair<KeyValuePair<string,string>,int> pair){
        switch(pair.Key.Key){
            case "WaterStance":
                currentHealth += stats.getStat("acu");
                stats.modifyStat("ene", Math.Max(10-stats.getStat("mid") , 3)*stats.getCostMul());
                stats.modifyStat("fat", Math.Max(10-stats.getStat("tou") , 3)*stats.getCostMul());
                stats.modifyStat("stb", Math.Max(10-stats.getStat("acu") , 3)*stats.getCostMul());
                break;
            case "FireStance":
                stats.modifyStat("fat", stats.getStat("acu")) ;
                stats.modifyStat("ene", Math.Max(10-stats.getStat("mid") , 3)*stats.getCostMul());
                stats.modifyStat("hp", Math.Max(10-stats.getStat("tou") , 3)*stats.getCostMul());
                stats.modifyStat("stb", Math.Max(10-stats.getStat("acu") , 3)*stats.getCostMul());
                break;
            case "Bubble":
                stats.modifyStat("ene", -4) ;
                break;
            case "Restrict":
                stats.modifyStat("ene",-4);
                stats.modifyStat("stb",-2);
                break;
        }
    }
    public void EndbuffDuration(){
        Dictionary<KeyValuePair<string,string>,int> toAdd = new Dictionary<KeyValuePair<string, string>,int>();
        List<KeyValuePair<string,string>> toRemove = new List<KeyValuePair<string, string>>();
        if(!skip){
        for(int i= 0; i<endbuffs.Count;i++){
            KeyValuePair<KeyValuePair<string,string>,int> pair =endbuffs.ElementAt(i);
            KeyValuePair<string,string> keypair = pair.Key;
            int value = pair.Value-1;
            if(value > 0){
                toAdd.Add(keypair,value);
                BuffMaintainCheck(pair);
            }
            if(value == 0){
                toRemove.Add(keypair);
            }
            
        }}
        foreach(KeyValuePair<KeyValuePair<string,string>,int> pair in toAdd){
            endbuffs.Remove(pair.Key);
            endbuffs.Add(pair);
        }
        foreach(KeyValuePair<string,string> pair in toRemove){
            RevertEffect(pair);
            endbuffs.Remove(pair);
        }
    }
    public void StartbuffDuration(){
        skip = false;
        Dictionary<KeyValuePair<string,string>,int> toAdd = new Dictionary<KeyValuePair<string, string>,int>();
        List<KeyValuePair<string,string>> toRemove = new List<KeyValuePair<string, string>>();
        foreach(KeyValuePair<KeyValuePair<string,string>,int> pair in startbuffs){
            if(pair.Key.Key == "TimeStop"){
                int value = pair.Value-1;
                if(value > 0){
                    toAdd.Add(pair.Key,value);
                }
                if(value == 0){
                    toRemove.Add(pair.Key);
                }
                skip = true;
            }
        }
        if(!skip){
        for(int i= 0; i<startbuffs.Count;i++){
            KeyValuePair<KeyValuePair<string,string>,int> pair =startbuffs.ElementAt(i);
            KeyValuePair<string,string> keypair = pair.Key;
            int value = pair.Value-1;
            if(value > 0){
                toAdd.Add(keypair,value);
                if(pair.Key.Key != "Restrict"){
                    BuffMaintainCheck(pair);
                }
            }
            if(pair.Value == 0){
                toRemove.Add(keypair);
            }
            
        }}
        foreach(KeyValuePair<KeyValuePair<string,string>,int> pair in toAdd){
            startbuffs.Remove(pair.Key);
            startbuffs.Add(pair);
        }
        foreach(KeyValuePair<string,string> pair in toRemove){
            RevertEffect(pair);
            startbuffs.Remove(pair);
        }
    }
    public void RevertEffect(KeyValuePair<string,string> pair){
        KeyValuePair<string,string> newpair = new KeyValuePair<string, string>(pair.Value,pair.Key);
        if(pair.Key == "BorrowedTime"){
            addStartBuff("TimeStop",gameObject.name,2);
        }
        if(effect_Bonus.ContainsKey(newpair)){
            foreach(KeyValuePair<string, int> p in effect_Bonus[newpair]){
                stats.modifyStat(p.Key,-p.Value);
            }
            effect_Bonus.Remove(newpair);
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
        BubbleBar Bubble = gameObject.GetComponentInChildren<BubbleBar>();
        
        int remainindamage = 0;
        if( Bubble != null){
            remainindamage = damageReceived - (int)Bubble.currentHealth;
            //Debug.Log(Bubble.currentHealth +"-"+damageReceived);
            Bubble.currentHealth -= (float)damageReceived;
            Bubble.UpdateHealth();
            if(remainindamage <=0 ){
                damageReceived = 0;
            }
            else{
                removeBuff("Bubble",Bubble.bubbleGiver);
                Destroy(GameObject.Find(gameObject.name+"_Bubble"));
                damageReceived = remainindamage;
            }
        }
        
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
    public bool isTimeStop(){
        foreach(KeyValuePair<KeyValuePair<string,string>,int> pair in startbuffs){
            if(pair.Key.Key == "TimeStop"){
                return true;
            }
        }
        return false;
    }
    public bool isRestrict(){
        foreach(KeyValuePair<KeyValuePair<string,string>,int> pair in startbuffs){
            if(pair.Key.Key == "Restrict"){
                return true;
            }
        }
        return false;
    }
    public void RestrictRemoveCheck(){
        List<KeyValuePair<string,string>> toRemove = new List<KeyValuePair<string, string>>();
        foreach(KeyValuePair<KeyValuePair<string,string>,int> pair in startbuffs){
            if(pair.Key.Key == "Restrict"){
                if(stats.getSpellCR() > 13){
                    toRemove.Add(pair.Key);
                }
            }
        }
        foreach(KeyValuePair<string,string> pair in toRemove){
            startbuffs.Remove(pair);
        }
    }
    public KeyValuePair<string,bool> usingRestrict(){
        foreach(KeyValuePair<KeyValuePair<string,string>,int> pair in endbuffs){
            if(pair.Key.Key == "Restrict"){
                return new KeyValuePair<string, bool>(pair.Key.Value, true);
            }
        }
        return new KeyValuePair<string, bool>("",false);
    }
    public int isHasten(){
        int num = 0;
        UDictionary<KeyValuePair<string,string>,int> toremove = new UDictionary<KeyValuePair<string, string>, int>();
        foreach(KeyValuePair<KeyValuePair<string,string>,int> pair in startbuffs){
            if(pair.Key.Key == "Hasten"){
                num++;
                toremove.Add(pair);
            }
        }
        foreach(KeyValuePair<KeyValuePair<string,string>,int> pair in toremove){
            startbuffs.Remove(pair);
        }
        return num;
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
        if(!endbuffs.ContainsKey(pair)){
            endbuffs.Add(pair,duration);
            return true;
        }
        return false;
        
    }
    public bool removeBuff(string buff, string character){
        KeyValuePair<string,string> pair = new KeyValuePair<string,string>(buff,character);
        if(endbuffs.ContainsKey(pair)){
            endbuffs.Remove(pair);
            return true;
        }
        return false;
        
    }
    public bool addStartBuff(string buff, string character, int duration){
        KeyValuePair<string,string> pair = new KeyValuePair<string,string>(buff,character);
        if(!startbuffs.ContainsKey(pair)){
            startbuffs.Add(pair,duration);
            return true;
        }
        return false;
        
    }
    public bool removeStartBuff(string buff, string character){
        KeyValuePair<string,string> pair = new KeyValuePair<string,string>(buff,character);
        if(startbuffs.ContainsKey(pair)){
            startbuffs.Remove(pair);
            return true;
        }
        return false;
        
    }
    public void addEffectStat(KeyValuePair<string,string> pair, UDictionary<string,int> dict){
        if(!effect_Bonus.ContainsKey(pair)){
            effect_Bonus.Add(pair,dict);
        }
    }
    public int getPreBonusStat(KeyValuePair<string,string> name,string targetStat){
        if(effect_Bonus[name].ContainsKey(targetStat)){
            int PreBonus = effect_Bonus[name][targetStat];
            effect_Bonus[name].Remove(targetStat);
            return PreBonus;
        }
        else{
            return 0;
        }
    }
    public bool isBuff(string name,string character){
        return endbuffs.ContainsKey(new KeyValuePair<string,string>(name,character)) || startbuffs.ContainsKey(new KeyValuePair<string,string>(name,character)) ;
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
