using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AbilitiesData", order = 3)]
public class AbilitiesData : ScriptableObject
{
    public UnityEvent<GameObject> Leadership;
    public UnityEvent<GameObject> charge;
    public UnityEvent<GameObject, Vector3Int> WhirlWind_e;
    public UnityEvent<GameObject, Vector3Int> ForceBlast_e;
    public UnityEvent<GameObject, Vector3Int> PsychiStorm_e;
    public UnityEvent<GameObject, Vector3Int> PsychiStorm_ef;
    public UnityEvent<GameObject, Vector3Int> ForeSight_e;
    public UnityEvent<GameObject, Vector3Int> WaterStance_e;
    public UnityEvent<GameObject, Vector3Int> FireStance_e;
    TileManager tileM;
    TurnManager turnM;
    int charge_bonus = 0;
    void setTileM(){
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
    }
    void Awake(){
        setTileM();
        turnM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
    }
    public UnityEvent<GameObject> getEvent(string name){
        switch(name){
            case "LeadershipAura": 
            return Leadership;
            case "Charge":
            return charge;
        }
        return null;
    }

    public UnityEvent<GameObject, Vector3Int> getActiveSkill(string name){
        switch(name){
            case "WhirlWind": 
            return WhirlWind_e;
            case "ForceBlast":
            return ForceBlast_e;
            case "PsychicStorm":
            return PsychiStorm_e;
            case "ForeSight":
            return ForeSight_e;
        }
        return null;
    }
    public UnityEvent<GameObject, Vector3Int> getAreaEffect(string name){
        switch(name){
            case "PsychicStorm":
            return PsychiStorm_ef;
        }
        return null;
    }
    public void LeadersshipAura(GameObject play){
        setTileM();
        KeyValuePair<string,string> pair = new KeyValuePair<string, string>(play.name,"LeadershipAura");
        foreach(GameObject go in GameObject.FindGameObjectsWithTag(play.tag)){
            StatUpdate su = go.GetComponent<StatUpdate>();
            Vector3Int pos = tileM.WorldToCell(go.transform.position);
            if(tileM.inArea(tileM.WorldToCell(play.transform.position),pos,2)){
                if(su.addBuff("LeadershipAura", play.name,1)){
                    su.getStats().modifyStat("ma",(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.getStats().modifyStat("md",(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.getStats().modifyStat("ra",(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.getStats().modifyStat("rd",(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.getStats().modifyStat("mr",(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.addEffectStat(pair, new UDictionary<string, int>(){
                        {"ma",(int)(int)Math.Min(su.getDictStats("mid")/3,1)}
                        ,{"md",(int)Math.Min(su.getDictStats("mid")/3,1)},{"ra",(int)Math.Min(su.getDictStats("mid")/3,1)}
                        ,{"rd",(int)Math.Min(su.getDictStats("mid")/3,1)},{"mr",(int)Math.Min(su.getDictStats("mid")/3,1)}});

                }
            }
            else{
                if(su.removeBuff("LeadershipAura", play.name)){
                    su.getStats().modifyStat("ma",-su.getPreBonusStat(pair,"ma"));
                    su.getStats().modifyStat("md",-su.getPreBonusStat(pair,"md"));
                    su.getStats().modifyStat("ra",-su.getPreBonusStat(pair,"ra"));
                    su.getStats().modifyStat("rd",-su.getPreBonusStat(pair,"rd"));
                    su.getStats().modifyStat("mr",-su.getPreBonusStat(pair,"mr"));
                }
            }
        }
    }
    public void CorruptionAura(GameObject play){
        setTileM();
        KeyValuePair<string,string> pair = new KeyValuePair<string, string>(play.name,"CorruptionAura");
        string enemy_tag = "Enemy";
        if(play.tag == "Enemy"){enemy_tag = "Player";}
        foreach(GameObject go in GameObject.FindGameObjectsWithTag(enemy_tag)){
            StatUpdate su = go.GetComponent<StatUpdate>();
            Vector3Int pos = tileM.WorldToCell(go.transform.position);
            if(tileM.inArea(tileM.WorldToCell(play.transform.position),pos,2)){
                if(su.addBuff("CorruptionAura", play.name,1)){
                    su.getStats().modifyStat("ene",-(int)Math.Min(su.getDictStats("mid")/2,2));
                    su.getStats().modifyStat("mr",-(int)Math.Min(su.getDictStats("mid")/2,2));
                    su.getStats().modifyStat("stb",-(int)Math.Min(su.getDictStats("mid")/2,2));
                    su.getStats().setCostMul(2);
                    su.addEffectStat(pair, new UDictionary<string, int>(){
                        {"ene",-(int)Math.Min(su.getDictStats("mid")/2,2)}
                        ,{"mr",-(int)Math.Min(su.getDictStats("mid")/2,2)},{"stb",-(int)Math.Min(su.getDictStats("mid")/2,2)}});

                }
            }
            else{
                if(su.removeBuff("CorruptionAura", play.name)){
                    su.getStats().modifyStat("ene",-su.getPreBonusStat(pair,"ene"));
                    su.getStats().modifyStat("mr",-su.getPreBonusStat(pair,"mr"));
                    su.getStats().modifyStat("stb",-su.getPreBonusStat(pair,"stb"));
                    su.getStats().setCostMul(1);
                }
            }
        }
    }
    public void ColdAura(GameObject play){
        setTileM();
        KeyValuePair<string,string> pair = new KeyValuePair<string, string>(play.name,"ColdAura");
        string enemy_tag = "Enemy";
        if(play.tag == "Enemy"){enemy_tag = "Player";}
        foreach(GameObject go in GameObject.FindGameObjectsWithTag(enemy_tag)){
            StatUpdate su = go.GetComponent<StatUpdate>();
            Vector3Int pos = tileM.WorldToCell(go.transform.position);
            if(tileM.inArea(tileM.WorldToCell(play.transform.position),pos,2)){
                if(su.addBuff("ColdAura", play.name,1)){
                    su.getStats().modifyStat("ma",-(int)Math.Min(su.getDictStats("mid")/2,2));
                    su.getStats().modifyStat("md",-(int)Math.Min(su.getDictStats("mid")/2,2));
                    su.getStats().modifyStat("ra",-(int)Math.Min(su.getDictStats("mid")/2,2));
                    su.getStats().modifyStat("rd",-(int)Math.Min(su.getDictStats("mid")/2,2));
                    play.GetComponent<Teleport>().setRangeMul(0.5f);
                    su.addEffectStat(pair, new UDictionary<string, int>(){
                        {"ma",-(int)Math.Min(su.getDictStats("mid")/2,2)},{"rd",-(int)Math.Min(su.getDictStats("mid")/2,2)}
                        ,{"md",-(int)Math.Min(su.getDictStats("mid")/2,2)},{"ra",-(int)Math.Min(su.getDictStats("mid")/2,2)}});

                }
            }
            else{
                if(su.removeBuff("ColdAura", play.name)){
                    su.getStats().modifyStat("ma",-su.getPreBonusStat(pair,"ma"));
                    su.getStats().modifyStat("md",-su.getPreBonusStat(pair,"md"));
                    su.getStats().modifyStat("ra",-su.getPreBonusStat(pair,"ra"));
                    su.getStats().modifyStat("rd",-su.getPreBonusStat(pair,"rd"));
                    play.GetComponent<Teleport>().setRangeMul(1);
                }
            }
        }
    }
    public void DeathAura(GameObject play){
        setTileM();
        KeyValuePair<string,string> pair = new KeyValuePair<string, string>(play.name,"DeathAura");
        foreach(GameObject go in GameObject.FindGameObjectsWithTag(play.tag)){
            StatUpdate su = go.GetComponent<StatUpdate>();
            Vector3Int pos = tileM.WorldToCell(go.transform.position);
            if(tileM.inArea(tileM.WorldToCell(play.transform.position),pos,2)){
                if(su.addBuff("DeathAura", play.name,1)){
                    su.setBonus((int)Math.Min(su.getDictStats("mid")/3,1));
                    su.addEffectStat(pair, new UDictionary<string, int>(){{"bonus damage",(int)Math.Min(su.getDictStats("mid")/3,1)}});

                }
            }
            else{
                if(su.removeBuff("DeathAura", play.name)){
                    su.setBonus(-su.getPreBonusStat(pair,"bonus damage"));
                }
            }
        }
        string enemy_tag = "Enemy";
        if(play.tag == "Enemy"){enemy_tag = "Player";}
        foreach(GameObject go in GameObject.FindGameObjectsWithTag(enemy_tag)){
            StatUpdate su = go.GetComponent<StatUpdate>();
            Vector3Int pos = tileM.WorldToCell(go.transform.position);
            if(tileM.inArea(tileM.WorldToCell(play.transform.position),pos,3)){
                if(su.addBuff("DeathAura", play.name,1)){
                    su.getStats().modifyStat("fat",(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.getStats().modifyStat("hp",-(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.getStats().modifyStat("ene",-(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.getStats().modifyStat("stb",-(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.addEffectStat(pair, new UDictionary<string, int>(){
                       {"hp",-(int)Math.Min(su.getDictStats("mid")/3,1)},
                        {"stb",-(int)Math.Min(su.getDictStats("mid")/3,1)},{"ene",-(int)Math.Min(su.getDictStats("mid")/3,1)}});

                }
            }
            else{
                if(su.removeBuff("DeathAura", play.name)){
                    su.getStats().modifyStat("hp",-su.getPreBonusStat(pair,"hp"));
                    su.getStats().modifyStat("ene",-su.getPreBonusStat(pair,"ene"));
                    su.getStats().modifyStat("stb",-su.getPreBonusStat(pair,"stb"));
                }
            }
        }
    } 
    public void AssaultAura(GameObject play){
        setTileM();
        KeyValuePair<string,string> pair = new KeyValuePair<string, string>(play.name,"AssaultAura");
        foreach(GameObject go in GameObject.FindGameObjectsWithTag(play.tag)){
            StatUpdate su = go.GetComponent<StatUpdate>();
            Vector3Int pos = tileM.WorldToCell(go.transform.position);
            if(tileM.inArea(tileM.WorldToCell(play.transform.position),pos,2)){
                if(su.addBuff("AssaultAura", play.name,1)){
                    su.getStats().modifyStat("av",(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.getStats().modifyStat("sv",(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.addEffectStat(pair, new UDictionary<string, int>(){{"av",(int)Math.Min(su.getDictStats("mid")/3,1)},{"sv",(int)Math.Min(su.getDictStats("mid")/3,1)}});

                }
            }
            else{
                if(su.removeBuff("AssaultAura", play.name)){
                    su.getStats().modifyStat("av",-su.getPreBonusStat(pair,"av"));
                    su.getStats().modifyStat("sv",-su.getPreBonusStat(pair,"sv"));
                }
            }
        }
        string enemy_tag = "Enemy";
        if(play.tag == "Enemy"){enemy_tag = "Player";}
        foreach(GameObject go in GameObject.FindGameObjectsWithTag(enemy_tag)){
            StatUpdate su = go.GetComponent<StatUpdate>();
            Vector3Int pos = tileM.WorldToCell(go.transform.position);
            if(tileM.inArea(tileM.WorldToCell(play.transform.position),pos,3)){
                if(su.addBuff("AssaultAura", play.name,1)){
                    su.getStats().modifyStat("av",-(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.getStats().modifyStat("sv",-(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.addEffectStat(pair, new UDictionary<string, int>(){
                        {"av",-(int)Math.Min(su.getDictStats("mid")/3,1)},{"sv",-(int)Math.Min(su.getDictStats("mid")/3,1)}});

                }
            }
            else{
                if(su.removeBuff("AssaultAura", play.name)){
                    su.getStats().modifyStat("av",-su.getPreBonusStat(pair,"av"));
                    su.getStats().modifyStat("sv",-su.getPreBonusStat(pair,"sv"));
                }
            }
        }
    } 
    void Charge(GameObject play){
        setTileM();
        TurnManager tm = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        StatUpdate su = play.GetComponent<StatUpdate>();
        Teleport teleport = play.GetComponent<Teleport>();                         
        if(tm.getState() == 3){
            charge_bonus = teleport.gettrailCount();
            su.setBonus(charge_bonus);
        }
        else if(tm.getState() == 1 || tm.getState() == 2){
            su.setBonus(-charge_bonus);
            charge_bonus = 0;
        }
    }

    public void AreaDamage(GameObject play, Vector3Int center, int range, int Damage, string tag){
        foreach(Node n in tileM.GetTilesInArea(center,range)){
            if(n.occupant != null && n.occupant.tag != tag){
                StatUpdate ocstat = n.occupant.GetComponent<StatUpdate>();
                ocstat.TakeDamage(Damage);
            }
        }
    }
    public void AreaAttack(GameObject play, Vector3Int center, int range, string tag){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        foreach(Node n in tileM.GetTilesInArea(center,range)){
            if(n.occupant != null && n.occupant.tag != tag){
                ocstat.attackEn(n.occupant);
            }
        }
    }
    
    public void WhirlWind(GameObject play, Vector3Int target){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        AreaAttack(play, tileM.WorldToCell(play.transform.position),3,play.tag);
        ocstat.getStats().modifyStat("ene",-7*ocstat.getStats().getCostMul());
        ocstat.getStats().modifyStat("stb",-15*ocstat.getStats().getCostMul());
    }
    public void ForceBlast(GameObject play, Vector3Int target){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        float Damage = 10 + ocstat.getDictStats("mid")*2;
        int range = 3 + (int)ocstat.getDictStats("acu")/4;
        AreaDamage(play,target,range,(int)Damage,play.tag);
        ocstat.getStats().modifyStat("ene",-15*ocstat.getStats().getCostMul());
        ocstat.getStats().modifyStat("fat",-20*ocstat.getStats().getCostMul());
    }
    public void PsychicStorm(GameObject play, Vector3Int target){
        tileM.GetNodeFromWorld(target).effectFlag.Add("PsychicStorm", new KeyValuePair<GameObject, int>(play, 3));
        tileM.AddEffectLst(tileM.GetNodeFromWorld(target));
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        ocstat.getStats().modifyStat("ene",-25*ocstat.getStats().getCostMul());
        ocstat.getStats().modifyStat("stb",-30*ocstat.getStats().getCostMul());
    }

    public void PsychicStormEffect(GameObject play, Vector3Int target){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        AreaDamage(play, target, 3, (int)(20+ocstat.getDictStats("mid")-3), play.tag);
    }
    public void GenericSummon(GameObject play, Vector3Int target){
        //instantiate GameObject at target
    }
    public void Foresight(){
        if(!turnM.getUI().inForesight()){
            foresighStart();
            turnM.getUI().setForesight((true));
        }
        else{
            foresightEnd();
            turnM.getUI().setForesight((false));
        }
    }
    public void foresighStart(){
        turnM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        Debug.Log("Backed Up!");
        turnM.BackUpTurn();
        turnM.EnemyBackUP();
        turnM.PlayerBackUP();
    }
    public void foresightEnd(){
        Debug.Log("Reverted!");
        turnM.revertTurn();
        turnM.EnemyRevert();
        turnM.PlayerRevert();
    }

    public void WaterStance(GameObject play, Vector3Int target){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        if(!ocstat.isBuff("WaterStance", play.name)){
            ocstat.addBuff("WaterStance", play.name,1);
            ocstat.getStats().modifyStat("ene",-5);
            ocstat.getStats().modifyStat("stb",-5);
            ocstat.getStats().modifyStat("fat",-5);
            ocstat.getStats().modifyStat("pv",ocstat.getDictStats("acu"));
            ocstat.getStats().modifyStat("pr",ocstat.getDictStats("acu")+ocstat.getDictStats("dex"));
            ocstat.addEffectStat(new KeyValuePair<string, string>(play.name,"Water"), new UDictionary<string, int>(){{"pv",(int)ocstat.getDictStats("acu")},{"pr",(int)(ocstat.getDictStats("acu")+ocstat.getDictStats("dex"))}});
        }
        else{
            ocstat.removeBuff("WaterStance", play.name);
            play.GetComponent<Abilities>().addToCooldDown("WaterStance",3);
            ocstat.getStats().modifyStat("pv",-ocstat.getPreBonusStat(new KeyValuePair<string, string>(play.name,"WaterStance"),"pv"));
            ocstat.getStats().modifyStat("pr",-ocstat.getPreBonusStat(new KeyValuePair<string, string>(play.name,"WaterStance"),"pr"));
        }
        
    }
    public void FireStance(GameObject play, Vector3Int target){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        if(!ocstat.isBuff("FireStance", play.name)){
            ocstat.addBuff("FireStance", play.name,1);
            ocstat.getStats().modifyStat("ene",-5);
            ocstat.getStats().modifyStat("stb",-5);
            ocstat.getStats().modifyStat("hp",-5);
            ocstat.setBonus((int)ocstat.getDictStats("acu"));
            ocstat.getStats().modifyStat("acu",ocstat.getDictStats("acu")+ocstat.getDictStats("dex"));
            ocstat.addEffectStat(new KeyValuePair<string, string>(play.name,"FireStance"), new UDictionary<string, int>(){
                {"bonus damage",(int)ocstat.getDictStats("acu")},{"acu",(int)(ocstat.getDictStats("acu")+ocstat.getDictStats("dex"))}});
        }
        else{
            ocstat.removeBuff("FireStance", play.name);
            play.GetComponent<Abilities>().addToCooldDown("FireStance",3);
            ocstat.setBonus(-(int)ocstat.getPreBonusStat(new KeyValuePair<string, string>(play.name,"FireStance"),"bonus damage"));
            ocstat.getStats().modifyStat("acu",-ocstat.getPreBonusStat(new KeyValuePair<string, string>(play.name,"FireStance"),"acu"));
        }
        
    }
    
    public void Teleport(GameObject target, Vector3Int Loc){
        target.transform.position = tileM.GetCellCenterWorld(Loc);
    }

}
