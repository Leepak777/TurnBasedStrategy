using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : ScriptableObject
{
    public UDictionary<string,float> stats = new UDictionary<string,float>();
    public UDictionary<string,string> attributes = new UDictionary<string, string>();
    public UDictionary<string,string> abilities = new UDictionary<string, string>();
    public UDictionary<string,int> skills = new UDictionary<string, int>();
    public Sprite sprite;
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
        stats.Add("attack_num",1);
        stats.Add("cooldowndec",0);
        stats.Add("costmul",0);
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
    public void fetchBase(CharacterStat baseData){
        stats = baseData.stats;
        attributes = baseData.attributes;
        abilities = baseData.abilities;
        skills = baseData.skills;
    }
    public void setStats(UDictionary<string,float> input){
        foreach(KeyValuePair<string,float> s in input){
            if(stats.ContainsKey(s.Key)){
                stats[s.Key] += input[s.Key];
            }
        }
    }
    public void setCostMul(int num){
        stats["costmul"] = num;
    }
    public int getCostMul(){
        return (int)stats["costmul"];
    }
    public void setAbilities(UDictionary<string,string> lst){
        abilities = lst;
    }
    public UDictionary<string,string> getAbilities(){
        return abilities;
    }
    public UDictionary<string,int> getSkills(){
        return skills;
    }
    public void setSkills(UDictionary<string,int> lst){
        skills = lst;
    }
    public void addAttributes(string key, string value){
        attributes.Add(key,value);
    }
    public void revertStat(UDictionary<string,float> backup){
        stats = backup;
    }
    public void setCalStat(){
        stats["hp"] = stats["tou"]*5 + stats["pow"] + stats["base_hp"];
        stats["maxHealth"] = stats["hp"];
        stats["ene"] = stats["mid"] * 4 + stats["acu"] + stats["base_ene"];
        stats["fat"] = 0;
        stats["stb"] = stats["mid"]*5 +stats["acu"]*3 + stats["tou"] *2;
        stats["maxstb"] = stats["mid"] * 7 + stats["acu"] * 4 + stats["tou"] * 3;
        stats["mov"] = stats["base_mov"] + stats["pow"]/2 + stats["eq_mov"];
        stats["maxTiles"] = (int)stats["mov"];
        stats["enc"] = stats["w_enc"] + stats["eq_enc"] + stats["base_enc"] - stats["pow"]/4;
        stats["bite"] = stats["base_init"] + stats["dex"] + stats["acu"] + stats["eq_init"] - stats["enc"];
    }
    public UDictionary<string,float> getStatLst(){
        return stats;
    }
    public float getStat(string s){
        return stats[s];
    }
    public string getAttribute(string s){
        if(attributes.ContainsKey(s)){
            return attributes[s];
        }
        return null;
    }

    public void setStat(string key, float value){
        stats[key] = value; 
    }

    public void modifyStat(string key, float value){
        stats[key] += value;
    }

    public float getMeleeAttackRoll(){
        return  stats["ma"]+ stats["acc"] + stats["pow"]/2 + stats["dex"] + stats["acu"]/2;
    }
    public float getMeleeDefenceRoll(){
        return  stats["md"]+ stats["mdb"] + stats["tou"]/2 + stats["dex"]/2 + stats["acu"]/2;
    }
    public float getRangeAttackRoll(float distance){
        return  stats["ra"]+ stats["acc"] + stats["dex"] + stats["acu"]/2 - distance;
    }
    public float getRangeDefenceRoll(){
        return  stats["rd"]+ stats["rdb"] + stats["dex"]/2 + stats["acu"]/2;
    }
    public float getSpellCR(){
        return stats["sa"]+stats["acu"]+stats["mid"];
    }
    public int getBaseDamage(){
        return (int)(stats["wd"] + stats["pscal"] * stats["pow"] + stats["dscal"]*stats["dex"]);
    }
    public int getProtection(){
        return (int)(stats["sv"] * (100-stats["sp"])/100 + stats["av"]*(100 -stats["ap"])/100 + stats["tou"]);
    }
}
