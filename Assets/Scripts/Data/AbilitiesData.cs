
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;
using System.IO;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AbilitiesData", order = 3)]
public class AbilitiesData : ScriptableObject
{
    public UnityEvent<GameObject> Leadership;
    public UnityEvent<GameObject> charge;
     public UnityEvent<GameObject> corruption;
    public UnityEvent<GameObject> cold;
    public UnityEvent<GameObject> assault;
    public UnityEvent<GameObject> death;
    public UnityEvent<GameObject, Vector3Int> WhirlWind_e;
    public UnityEvent<GameObject, Vector3Int> ForceBlast_e;
    public UnityEvent<GameObject, Vector3Int> PsychiStorm_e;
    public UnityEvent<GameObject, Vector3Int> PsychiStorm_ef;
    public UnityEvent<GameObject, Vector3Int> ForeSight_e;
    public UnityEvent<GameObject, Vector3Int> WaterStance_e;
    public UnityEvent<GameObject, Vector3Int> FireStance_e;
    public UnityEvent<GameObject, Vector3Int> Bubble_e;
    public UnityEvent<GameObject, Vector3Int> TimeStop_e;
    public UnityEvent<GameObject, Vector3Int> Restrict_e;
    public UnityEvent<GameObject, Vector3Int> Armageddon_e;
    public UnityEvent<GameObject, Vector3Int> Ripple_e;
    public UnityEvent<GameObject, Vector3Int> Meditate_e;
    public UnityEvent<GameObject, Vector3Int> CalmMind_e;
    public UnityEvent<GameObject, Vector3Int> Accelerate_e;
    public UnityEvent<GameObject, Vector3Int> BorrowedTime_e;
    TileManager tileM;
    TurnManager turnM;
    int charge_bonus = 0;
    List<string> SkillLst = new List<string>(){"ForceBlast","PsychicStorm","ForeSight","WhirlWind","WaterStance","FireStance","Bubble","Restrict","TimeStop","Hasten","Armageddon","Ripple","Meditate","Ripple","Accelerate","BorrowedTime"};
    List<string> AbilitiesLst = new List<string>(){"LeaderShipAura","Charge","CorruptionAura","ColdAura","DeathAura","AssaultAura","Psychometry"};
    [SerializeField]
    UDictionary<string, UnityEvent<GameObject,Vector3Int>> Skills = new UDictionary<string, UnityEvent<GameObject, Vector3Int>>();
    [SerializeField]
    UDictionary<string, UnityEvent<GameObject,Vector3Int>> SkillEffects = new UDictionary<string, UnityEvent<GameObject, Vector3Int>>();
    [SerializeField]
    UDictionary<string, UnityEvent<GameObject>> Abilities = new UDictionary<string, UnityEvent<GameObject>>();

    public void setTileM(){
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        turnM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
    }
    void Awake(){
        reset();
        //setTileM();
    }
    void Start(){
        setTileM();
    }
    public void reset(){
        ForceBlast_e.AddListener(ForceBlast);
        PsychiStorm_e.AddListener(PsychicStorm);
        WhirlWind_e.AddListener(WhirlWind);
        ForeSight_e.AddListener(Foresight);
        Bubble_e.AddListener(Bubble);
        TimeStop_e.AddListener(TimeStop);
        Restrict_e.AddListener(Restrict);
        Accelerate_e.AddListener(Accelerate);
        BorrowedTime_e.AddListener(BorrowedTime);
        Armageddon_e.AddListener(Armageddon);
        FireStance_e.AddListener(FireStance);
        WaterStance_e.AddListener(WaterStance);
        Ripple_e.AddListener(Ripple);
        PsychiStorm_ef.AddListener(PsychicStorm);
        Leadership.AddListener(LeadersshipAura);
        charge.AddListener(Charge);
        corruption.AddListener(CorruptionAura);
        cold.AddListener(ColdAura);
        assault.AddListener(AssaultAura);
        death.AddListener(DeathAura);
        SkillLst = new List<string>(){"ForceBlast","PsychicStorm","ForeSight","WhirlWind","WaterStance","FireStance","Bubble","Restrict","TimeStop","Hasten","Armageddon","Ripple","Meditate","Ripple","Accelerate","BorrowedTime"};
        AbilitiesLst = new List<string>(){"LeaderShipAura","Charge","CorruptionAura","ColdAura","DeathAura","AssaultAura","Psychometry"};
        Skills = new UDictionary<string, UnityEvent<GameObject, Vector3Int>>(){
            {"ForceBlast",ForceBlast_e},{"PsychicStorm",PsychiStorm_e},{"WhirlWind",WhirlWind_e},{"ForeSight",ForeSight_e},{"Bubble",Bubble_e},{"WaterStance",WaterStance_e},{"FireStance",FireStance_e}
            ,{"TimeStop",TimeStop_e},{"Restrict",Restrict_e},{"Accelerate",Accelerate_e},{"BorrowedTime",BorrowedTime_e},{"Armageddon",Armageddon_e},{"Ripple",Ripple_e}};
        SkillEffects = new UDictionary<string, UnityEvent<GameObject, Vector3Int>>(){
            {"PsychicStorm",PsychiStorm_ef}
        };
        Abilities = new UDictionary<string, UnityEvent<GameObject>>(){
            {"LeadershipAura",Leadership},{"Charge",charge},{"CorruptionAura",corruption},{"ColdAura",cold},{"AssaultAura",assault},{"DeathAura",death}
        };
    }
    public List<string> getSkillList(){
        return SkillLst;
    }
    public List<string> getAbilList(){
        return AbilitiesLst;
    }
    public UnityEvent<GameObject> getEvent(string name){
        if(Abilities.ContainsKey(name)){
            return Abilities[name];
        }
        /*switch(name){
            case "LeadershipAura": 
            return Leadership;
            case "Charge":
            return charge;
            case "CorruptionAura":
            return corruption;
            case "ColdAura":
            return cold;
        }*/
        return null;
    }

    public UnityEvent<GameObject, Vector3Int> getActiveSkill(string name){
        if(Skills.ContainsKey(name)){
            return Skills[name];
        }
        /*switch(name){
            case "WhirlWind": 
            return WhirlWind_e;
            case "ForceBlast":
            return ForceBlast_e;
            case "PsychicStorm":
            return PsychiStorm_e;
            case "ForeSight":
            return ForeSight_e;
            case "Bubble":
            return Bubble_e;
            case "TimeStop":
            return TimeStop_e;
            case "Restrict":
            return Restrict_e;
            case "Accelerate":
            return Accelerate_e;
            case "BorrowedTime":
            return BorrowedTime_e;
        }*/
        return null;
    }
    public UnityEvent<GameObject, Vector3Int> getAreaEffect(string name){
        if(SkillEffects.ContainsKey(name)){
            return SkillEffects[name];
        }
        /*switch(name){
            case "PsychicStorm":
            return PsychiStorm_ef;
        }*/
        return null;
    }
    public string getAbilType(string name){
        switch(name){
            case "LeadershipAura": return "Universal";
            case "CorruptionAura": return "Universal";
            case "ColdAura": return "Universal";
            case "DeathAura": return "Universal";
            case "AssaultAura": return "Universal";
            case "Charge": return "Individual";
            case "Psychometry": return "Universal";
        }
        return "Null";
    }
    public void LeadersshipAura(GameObject play){
        setTileM();
        KeyValuePair<string,string> pair = new KeyValuePair<string, string>(play.name,"LeadershipAura");
        foreach(GameObject go in GameObject.FindGameObjectsWithTag(play.tag)){
            StatUpdate su = go.GetComponent<StatUpdate>();
            Vector3Int pos = tileM.WorldToCell(go.transform.position);
            if(tileM.inArea(tileM.WorldToCell(play.transform.position),pos,2)){
                if(!su.isTimeStop()&& su.addBuff("LeadershipAura", play.name,-1)){
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
                if(!su.isTimeStop()&&su.removeBuff("LeadershipAura", play.name)){
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
                if(!su.isTimeStop()&&su.addBuff("CorruptionAura", play.name,-1)){
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
                if(!su.isTimeStop()&&su.removeBuff("CorruptionAura", play.name)){
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
                if(!su.isTimeStop()&&su.addBuff("ColdAura", play.name,-1)){
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
                if(!su.isTimeStop()&&su.removeBuff("ColdAura", play.name)){
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
                if(!su.isTimeStop()&&su.addBuff("DeathAura", play.name,-1)){
                    su.setBonus((int)Math.Min(su.getDictStats("mid")/3,1));
                    su.addEffectStat(pair, new UDictionary<string, int>(){{"bonus damage",(int)Math.Min(su.getDictStats("mid")/3,1)}});

                }
            }
            else{
                if(!su.isTimeStop()&&su.removeBuff("DeathAura", play.name)){
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
                if(!su.isTimeStop()&&su.addBuff("DeathAura", play.name,-1)){
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
                if(!su.isTimeStop()&&su.removeBuff("DeathAura", play.name)){
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
                if(!su.isTimeStop()&&su.addBuff("AssaultAura", play.name,-1)){
                    su.getStats().modifyStat("av",(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.getStats().modifyStat("sv",(int)Math.Min(su.getDictStats("mid")/3,1));
                    su.addEffectStat(pair, new UDictionary<string, int>(){{"av",(int)Math.Min(su.getDictStats("mid")/3,1)},{"sv",(int)Math.Min(su.getDictStats("mid")/3,1)}});

                }
            }
            else{
                if(!su.isTimeStop()&&su.removeBuff("AssaultAura", play.name)){
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
                if(su.addBuff("AssaultAura", play.name,-1)){
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
    public void Charge(GameObject play){
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
                if(!ocstat.isTimeStop()){
                    ocstat.TakeDamage(Damage);
                }
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
    public void AreaAllAttack(GameObject play, Vector3Int center, int range, string tag){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        foreach(Node n in tileM.GetTilesInArea(center,range)){
            if(n.occupant != null && n.occupant != play){
                ocstat.attackEn(n.occupant);
            }
        }
    }
    public void AreaAllDamage(GameObject play, Vector3Int center, int range, int Damage, string effect,int duration){
        foreach(Node n in tileM.GetTilesInArea(center,range)){
            if(n.occupant != null && n.occupant != play){
                StatUpdate ocstat = n.occupant.GetComponent<StatUpdate>();
                if(!ocstat.isTimeStop()){
                    ocstat.TakeDamage(Damage);
                    if(effect != ""){
                        ocstat.addStartBuff(effect, play.name,1);
                    }
                }
            }
        }
    }
    
    public void WhirlWind(GameObject play, Vector3Int target){
        Debug.Log("WhirlWind");
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        AreaAttack(play, tileM.WorldToCell(play.transform.position),3,play.tag);
    }
    public void ForceBlast(GameObject play, Vector3Int target){
        Debug.Log("ForceBlast");
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        float Damage = 10 + ocstat.getDictStats("mid")*2;
        int range = 3 + (int)ocstat.getDictStats("acu")/4;
        AreaDamage(play,target,range,(int)Damage,play.tag);
    }
    public void PsychicStorm(GameObject play, Vector3Int target){
        tileM.GetNodeFromWorld(target).effectFlag.Add("PsychicStorm", new KeyValuePair<GameObject, int>(play, 3));
        tileM.AddEffectLst(tileM.GetNodeFromWorld(target));
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
    }

    public void PsychicStormEffect(GameObject play, Vector3Int target){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        AreaDamage(play, target, 3, (int)(20+ocstat.getDictStats("mid")-3), play.tag);
    }
    public void GenericSummon(GameObject play, Vector3Int target){
        //instantiate GameObject at target
    }
    public void Foresight(GameObject go, Vector3Int v){
        Debug.Log("ForeSight");
        if(!turnM.getUI().inForesight()){
            foresighStart();
            turnM.getUI().setForesight((true));
            turnM.getUI().preattack = turnM.getUI().attackTime;
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
        turnM.CurrentSaveOrigin();
    }
    public void foresightEnd(){
        Debug.Log("Reverted!");
        turnM.revertTurn();
        turnM.EnemyRevert();
        turnM.PlayerRevert();
        turnM.CurrentRevertOrigin();
        GameObject.Find("ActiveSkill").GetComponent<Dropdown>().value = 0;
    }

    public void WaterStance(GameObject play, Vector3Int target){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        if(!ocstat.isBuff("WaterStance", play.name)){
            ocstat.addBuff("WaterStance", play.name,-1);
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
            ocstat.addBuff("FireStance", play.name,-1);
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
    public void Bubble(GameObject target, Vector3Int Loc){
        Node n = tileM.GetNodeFromWorld(Loc);
        StatUpdate ocstat = n.occupant.GetComponent<StatUpdate>();
        if(!ocstat.isTimeStop()&&ocstat.addBuff("Bubble", target.name,-1)){  
            GameObject prefab = Resources.Load<GameObject>("Bubble") as GameObject;
            GameObject player = Instantiate(prefab) as GameObject;
            player.transform.SetParent(n.occupant.transform);
            player.name = n.occupant.name+"_Bubble";
            player.GetComponent<BubbleBar>().bubbleGiver = target.name;
        }
        
    }

    public void TimeStop(GameObject target, Vector3Int Loc){
        foreach(Node n in tileM.GetTilesInArea(Loc,3)){
            if(n.occupant != null){
                StatUpdate nstat = n.occupant.GetComponent<StatUpdate>();
                nstat.addStartBuff("TimeStop",target.name,2);
            }
        }
    }
    public void Hasten(GameObject user, Vector3Int Loc){
        Node n = tileM.GetNodeFromWorld(Loc);
        if(!n.occupant == null){
            n.occupant.GetComponent<Abilities>().DecCoolDown();
            StatUpdate nstat = n.occupant.GetComponent<StatUpdate>();
            nstat.addStartBuff("Hasten",user.name,-1);
        }
    }

    public void Restrict(GameObject user, Vector3Int Loc){
        Node n = tileM.GetNodeFromWorld(Loc);
        if(!n.occupant == null){
            n.occupant.GetComponent<Abilities>().DecCoolDown();
            StatUpdate nstat = n.occupant.GetComponent<StatUpdate>();
            nstat.addStartBuff("Restrict",user.name,-1);
            user.GetComponent<StatUpdate>().addBuff("Restrict",n.occupant.name,-1);
        }
    }

    public void Ripple(GameObject play, Vector3Int Loc){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        AreaAllDamage(play, tileM.WorldToCell(play.transform.position),5,20+(int)ocstat.getDictStats("mid"),"Restrict",1);
    }
    public void Armageddon(GameObject play, Vector3Int Loc){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        AreaAllDamage(play, Loc,7,30+(int)ocstat.getDictStats("mid"),"",0);
        AreaAllDamage(play, Loc,7,30+(int)ocstat.getDictStats("mid"),"",0);
        AreaAllDamage(play, Loc,7,30+(int)ocstat.getDictStats("mid"),"",0);
    }

    public void Meditate(GameObject play, Vector3Int Loc){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        CharacterStat chStat = ocstat.getStats();
        chStat.modifyStat("ene",10);
        chStat.modifyStat("stb",10);
        chStat.modifyStat("fat",-10);
    }
    public void CalmMind(GameObject play, Vector3Int Loc){
        Node n = tileM.GetNodeFromWorld(Loc);
        if(n.occupant != null){
            Meditate(n.occupant, Vector3Int.zero);
        }
    }

    public void Accelerate(GameObject play, Vector3Int Loc){
        Node n = tileM.GetNodeFromWorld(Loc);
        StatUpdate ocstat = n.occupant.GetComponent<StatUpdate>();
        CharacterStat chStat = ocstat.getStats();
        KeyValuePair<string,string> pair = new KeyValuePair<string, string>(play.name,"Accelerate");
        if(ocstat.addBuff("Accelerate", play.name, 2)){
            chStat.modifyStat("attack_num",1);
            chStat.modifyStat("mov",chStat.getStat("base_mov"));
            ocstat.addEffectStat(pair, new UDictionary<string, int>(){{"attack_num",1},{"mov",(int)chStat.getStat("base_mov")}});
        }
    }

    public void BorrowedTime(GameObject play, Vector3Int Loc){
        Node n = tileM.GetNodeFromWorld(Loc);
        if(n.occupant!=null){
            Abilities ab = n.occupant.GetComponent<Abilities>();
            StatUpdate ocstat = n.occupant.GetComponent<StatUpdate>();
            CharacterStat chStat = ocstat.getStats();
            KeyValuePair<string,string> pair = new KeyValuePair<string, string>(play.name,"BorrowedTime");
            if(ocstat.addBuff("BorrowedTIme", play.name, 3)){
                chStat.modifyStat("cooldowndec",1);
                chStat.modifyStat("costmul",1);
                ocstat.addEffectStat(pair, new UDictionary<string, int>(){{"cooldowndec",1},{"costmul",1}});
                ab.DecCoolDown();
                ab.updateCasttime();
            }
        }
    }

    public UDictionary<string,float> getSkillsFloats(string name, StatUpdate statU){
        CharacterStat stats = statU.getStats();
        int hastenCast = statU.isHasten();
        
        switch(name){
            case "ForceBlast":
                return new UDictionary<string, float>(){{"Radius",3},{"CastRange",3 + (int) stats.getStat("acu")/4},{"TargetNum",1},{"CastTime",0}};
                break;
            case "PsychicStorm":
                return new UDictionary<string, float>(){{"Radius",3},{"CastRange",5 + (int) stats.getStat("acu")/4},{"TargetNum",1},{"CastTime",2-hastenCast}};
                break;
            case "WhirlWind":
                return new UDictionary<string, float>(){{"Radius",0},{"CastRange",0},{"TargetNum",0},{"CastTime",1-hastenCast}};
                break;
            case "WaterStance":
               return new UDictionary<string, float>(){{"Radius",0},{"CastRange",0},{"TargetNum",0},{"CastTime",0}};
                break;
            case "FireStance":
                return new UDictionary<string, float>(){{"Radius",0},{"CastRange",0},{"TargetNum",0},{"CastTime",0}};
                break;
            case "Bubble":
                return new UDictionary<string, float>(){{"Radius",0},{"CastRange",9999},{"TargetNum",1},{"CastTime",0}};
            case "ForeSight":
                return new UDictionary<string, float>(){{"Radius",0},{"CastRange",9999},{"TargetNum",1},{"CastTime",0}};
                break;
            case "TimeStop":
                return new UDictionary<string, float>(){{"Radius",3},{"CastRange",7+(int)stats.getStat("acu")},{"TargetNum",1},{"CastTime",0}};
                break;
            case "Restrict":
                return new UDictionary<string, float>(){{"Radius",0},{"CastRange",5},{"TargetNum",1},{"CastTime",1-hastenCast}};
                break;
            case "Armageddon":
                return new UDictionary<string, float>(){{"Radius",7},{"CastRange",7+(int)stats.getStat("acu")},{"TargetNum",1},{"CastTime",3-hastenCast}};
                break;
            case "Ripple":
                return new UDictionary<string, float>(){{"Radius",5},{"CastRange",0},{"TargetNum",0},{"CastTime",2-hastenCast}};
                break;
            case "Meditate":
                return new UDictionary<string, float>(){{"Radius",0},{"CastRange",0},{"TargetNum",0},{"CastTime",1-hastenCast}};
                break;
            case "CalmMind":
                return new UDictionary<string, float>(){{"Radius",0},{"CastRange",(int)stats.getStat("acu")},{"TargetNum",0},{"CastTime",1-hastenCast}};
                break;
            case "Accelerate":
                return new UDictionary<string, float>(){{"Radius",0},{"CastRange",7},{"TargetNum",0},{"CastTime",0}};
                break;
            case "BorrowedTime":
                return new UDictionary<string, float>(){{"Radius",0},{"CastRange",0},{"TargetNum",0},{"CastTime",0}};
                break;
        }
        return new UDictionary<string, float>(){{"Radius",0},{"CastRange",0},{"TargetNum",0},{"CastTime",0}};
    }
    public UDictionary<string,bool> getSkillBool(string name,CharacterStat chstsat){
        switch(name){
            case "ForceBlast":
                return new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}};
                break;
            case "PsychicStorm":
                return new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}};
                break;
            case "WhirlWind":
                return new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}};
                break;
            case "WaterStance":
               return new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}};
                break;
            case "FireStance":
                return new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}};
                break;
            case "ForeSight":
                return new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}};
                break;
            case "Bubble":
                return new UDictionary<string, bool>(){{"characterTarget",true},{"sameTag",true}};
                break;
            case "TimeStop":
                return new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}};
                break;
            case "Restrict":
                return new UDictionary<string, bool>(){{"characterTarget",true},{"sameTag",false}};
                break;
            case "Accelerate":
                return new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}};
                break;
            case "BorrowedTime":
                return new UDictionary<string, bool>(){{"characterTarget",true},{"sameTag",true}};
                break;
        }
        return new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}};
    }
    public void computeCost(GameObject play, string name){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        CharacterStat chStat = ocstat.getStats();
        switch(name){
            case "WhirlWind":
                chStat.modifyStat("ene",-7*ocstat.getStats().getCostMul());
                chStat.modifyStat("stb",-15*ocstat.getStats().getCostMul());
                break;
            case "ForceBlast":
                chStat.modifyStat("ene",-15*ocstat.getStats().getCostMul());
                chStat.modifyStat("fat",20*ocstat.getStats().getCostMul());
                break;
            case "PsychicStorm":
                chStat.modifyStat("ene",-25*ocstat.getStats().getCostMul());
                chStat.modifyStat("stb",-30*ocstat.getStats().getCostMul());
                break;
            case "WaterStance":
                chStat.modifyStat("ene",-5);
                chStat.modifyStat("stb",-5);
                chStat.modifyStat("fat",5);
                chStat.modifyStat("pv",ocstat.getDictStats("acu"));
                chStat.modifyStat("pr",ocstat.getDictStats("acu")+ocstat.getDictStats("dex"));
                ocstat.addEffectStat(new KeyValuePair<string, string>(play.name,"Water"), new UDictionary<string, int>(){{"pv",(int)ocstat.getDictStats("acu")},{"pr",(int)(ocstat.getDictStats("acu")+ocstat.getDictStats("dex"))}});
                break;
            case "FireStance":
                chStat.modifyStat("ene",-5);
                chStat.modifyStat("stb",-5);
                chStat.modifyStat("hp",-5);
                ocstat.setBonus((int)ocstat.getDictStats("acu"));
                chStat.modifyStat("acu",ocstat.getDictStats("acu")+ocstat.getDictStats("dex"));
                ocstat.addEffectStat(new KeyValuePair<string, string>(play.name,"FireStance"), new UDictionary<string, int>(){
                    {"bonus damage",(int)ocstat.getDictStats("acu")},{"acu",(int)(ocstat.getDictStats("acu")+ocstat.getDictStats("dex"))}});
                break;
            case "Bubble":
                ocstat.getStats().modifyStat("ene",-10);
                break;
            case "TimeStop":
                ocstat.getStats().modifyStat("ene",-10);
                ocstat.getStats().modifyStat("stb",-10);
                break;
            case "Restrict":
                ocstat.getStats().modifyStat("ene",-10);
                ocstat.getStats().modifyStat("stb",-5);
                break;
            case "Hasten":
                ocstat.getStats().modifyStat("ene",-5);
                ocstat.getStats().modifyStat("stb",-5);
                break;
            case "Armageddon":
                ocstat.getStats().modifyStat("ene",-50);
                ocstat.getStats().modifyStat("stb",-100);
                ocstat.getStats().modifyStat("fat",100);
                break;
            case "Ripple":
                ocstat.getStats().modifyStat("ene",-20);
                ocstat.getStats().modifyStat("stb",-30);
                break;
            case "CalmMind":
                ocstat.getStats().modifyStat("ene",-10);
                break;
            case "Accelerate":
                ocstat.getStats().modifyStat("ene",-10);
                ocstat.getStats().modifyStat("stb",-5);
                break;
            case "BorrowedTime":
                ocstat.getStats().modifyStat("ene",-5);
                break;
        }
    }
}
