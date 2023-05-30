
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
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AbilitiesData", order = 3)]
public class AbilitiesData : ScriptableObject
{
    public UnityEvent<string, GameObject> Leadership;
    public UnityEvent<string, GameObject> charge;
     public UnityEvent<string, GameObject> corruption;
    public UnityEvent<string, GameObject> cold;
    public UnityEvent<string, GameObject> assault;
    public UnityEvent<string, GameObject> death;
    public UnityEvent<string, GameObject, Vector3Int> WhirlWind_e;
    public UnityEvent<string, GameObject, Vector3Int> ForceBlast_e;
    public UnityEvent<string, GameObject, Vector3Int> PsychiStorm_e;
    public UnityEvent<string,GameObject, Vector3Int> PsychiStorm_ef;
    public UnityEvent<string, GameObject, Vector3Int> ForeSight_e;
    public UnityEvent<string, GameObject, Vector3Int> WaterStance_e;
    public UnityEvent<string, GameObject, Vector3Int> FireStance_e;
    public UnityEvent<string, GameObject, Vector3Int> Bubble_e;
    public UnityEvent<string, GameObject, Vector3Int> TimeStop_e;
    public UnityEvent<string, GameObject, Vector3Int> Restrict_e;
    public UnityEvent<string, GameObject, Vector3Int> Armageddon_e;
    public UnityEvent<string, GameObject, Vector3Int> Ripple_e;
    public UnityEvent<string, GameObject, Vector3Int> Meditate_e;
    public UnityEvent<string, GameObject, Vector3Int> CalmMind_e;
    public UnityEvent<string, GameObject, Vector3Int> Accelerate_e;
    public UnityEvent<string, GameObject, Vector3Int> BorrowedTime_e;
    TileManager tileM;
    TurnManager turnM;
    UI ui;
    int charge_bonus = 0;
    List<string> SkillLst = new List<string>(){"ForceBlast","PsychicStorm","ForeSight","WhirlWind","WaterStance","FireStance","Bubble","Restrict","TimeStop","Hasten","Armageddon","Ripple","Meditate","Ripple","Accelerate","BorrowedTime"};
    List<string> AbilitiesLst = new List<string>(){"LeaderShipAura","Charge","CorruptionAura","ColdAura","DeathAura","AssaultAura","Psychometry"};
    [SerializeField]
    UDictionary<string, UnityEvent<string, GameObject,Vector3Int>> Skills = new UDictionary<string, UnityEvent<string, GameObject, Vector3Int>>();
    [SerializeField]
    UDictionary<string, UnityEvent<string, GameObject,Vector3Int>> SkillEffects = new UDictionary<string, UnityEvent<string, GameObject, Vector3Int>>();
    [SerializeField]
    UDictionary<string, UnityEvent<string, GameObject>> Abilities = new UDictionary<string, UnityEvent<string, GameObject>>();

    [SerializeField]
    UDictionary<string, UDictionary<string,string>> SkillAttributes = new UDictionary<string, UDictionary<string, string>>();
    [SerializeField]
    UDictionary<string, UDictionary<string,bool>> SkillBools = new UDictionary<string, UDictionary<string, bool>>();
    [SerializeField]
    UDictionary<string, UDictionary<string,string>> SkillCost = new UDictionary<string, UDictionary<string,string>>();
    [SerializeField]
    UDictionary<string, UDictionary<string,int>> SkillStats = new UDictionary<string, UDictionary<string,int>>();
    public void setTileM(){
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        turnM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        ui =  GameObject.Find("UICanvas").GetComponent<UI>();
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
        PsychiStorm_ef.AddListener(PsychicStormEffect);
        Leadership.AddListener(LeadersshipAura);
        charge.AddListener(Charge);
        corruption.AddListener(CorruptionAura);
        cold.AddListener(ColdAura);
        assault.AddListener(AssaultAura);
        death.AddListener(DeathAura);
        SkillLst = new List<string>(){"ForceBlast","PsychicStorm","ForeSight","WhirlWind","WaterStance","FireStance","Bubble","Restrict","TimeStop","Hasten","Armageddon","Ripple","Meditate","Ripple","Accelerate","BorrowedTime"};
        AbilitiesLst = new List<string>(){"LeaderShipAura","Charge","CorruptionAura","ColdAura","DeathAura","AssaultAura","Psychometry"};
        Skills = new UDictionary<string, UnityEvent<string, GameObject, Vector3Int>>(){
            {"ForceBlast",ForceBlast_e},{"PsychicStorm",PsychiStorm_e},{"WhirlWind",WhirlWind_e},{"ForeSight",ForeSight_e},{"Bubble",Bubble_e},{"WaterStance",WaterStance_e},{"FireStance",FireStance_e}
            ,{"TimeStop",TimeStop_e},{"Restrict",Restrict_e},{"Accelerate",Accelerate_e},{"BorrowedTime",BorrowedTime_e},{"Armageddon",Armageddon_e},{"Ripple",Ripple_e}};
        SkillEffects = new UDictionary<string, UnityEvent<string, GameObject, Vector3Int>>(){
            {"PsychicStorm",PsychiStorm_ef}
        };
        Abilities = new UDictionary<string, UnityEvent<string, GameObject>>(){
            {"LeadershipAura",Leadership},{"Charge",charge},{"CorruptionAura",corruption},{"ColdAura",cold},{"AssaultAura",assault},{"DeathAura",death}
        };
    }
    public List<string> getSkillList(){
        return SkillLst;
    }
    public List<string> getAbilList(){
        return AbilitiesLst;
    }
    public UnityEvent<string,GameObject> getEvent(string name){
        if(Abilities.ContainsKey(name)){
            return Abilities[name];
        }
        
        return null;
    }

    public UnityEvent<string, GameObject, Vector3Int> getActiveSkill(string name){
        if(Skills.ContainsKey(name)){
            return Skills[name];
        }
        
        return null;
    }
    public UnityEvent<string, GameObject, Vector3Int> getAreaEffect(string name){
        if(SkillEffects.ContainsKey(name)){
            return SkillEffects[name];
        }
        return null;
    }
    public UDictionary<string,string> getSkillAtt(string name){
       if(SkillAttributes.ContainsKey(name)){
        return SkillAttributes[name];}
        return null;
    }
     public UDictionary<string,int> getSkillStat(string name){
        if(SkillStats.ContainsKey(name)){
        return SkillStats[name];}
        return null;
    }
     public UDictionary<string,string> getSkillCost(string name){
        if(SkillCost.ContainsKey(name)){
        return SkillCost[name];}
        return null;
    }
    public UDictionary<string,bool> getSkillBool(string name){
        if(SkillBools.ContainsKey(name)){
        return SkillBools[name];}
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
    public void LeadersshipAura(string name, GameObject play){
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
    public void CorruptionAura(string name,GameObject play){
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
    public void ColdAura(string name,GameObject play){
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
    public void DeathAura(string name,GameObject play){
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
    public void AssaultAura(string name,GameObject play){
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
    public void Charge(string name,GameObject play){
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
    public void WhirlWind(string name, GameObject play, Vector3Int target){
        Debug.Log("WhirlWind");
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        AreaAttack(play, tileM.WorldToCell(play.transform.position),3,"WhirlWind","",0);
    }
    public void ForceBlast(string name, GameObject play, Vector3Int target){
        Debug.Log("ForceBlast");
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        float Damage = 10 + ocstat.getDictStats("mid")*2;
        int range = 3 + (int)ocstat.getDictStats("acu")/4;
        AreaDamage(play,target,range,(int)Damage,"ForceBlast","",0);
    }
    public void PsychicStorm(string name, GameObject play, Vector3Int target){
        tileM.GetNodeFromWorld(target).effectFlag.Add("PsychicStorm", new KeyValuePair<GameObject, int>(play, 3));
        tileM.AddEffectLst(tileM.GetNodeFromWorld(target));
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
    }

    public void PsychicStormEffect(string name, GameObject play, Vector3Int target){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        AreaDamage(play, target, 3, (int)(20+ocstat.getDictStats("mid")-3),"PsychicStorm", "",0);
    }
    public void GenericSummon(string name, GameObject play, Vector3Int target){
        //instantiate GameObject at target
    }
    public void Foresight(string name, GameObject go, Vector3Int v){
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
        turnM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        Debug.Log("Reverted!");
        turnM.revertTurn();
        turnM.EnemyRevert();
        turnM.PlayerRevert();
        turnM.CurrentRevertOrigin();
        GameObject.Find("ActiveSkill").GetComponent<Dropdown>().value = 0;
    }

    public void WaterStance(string name, GameObject play, Vector3Int target){
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
    public void FireStance(string name, GameObject play, Vector3Int target){
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
    
    public void Teleport(string name, GameObject target, Vector3Int Loc){
        target.transform.position = tileM.GetCellCenterWorld(Loc);
    }
    public void Bubble(string name, GameObject target, Vector3Int Loc){
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

    public void TimeStop(string name, GameObject target, Vector3Int Loc){
        foreach(Node n in tileM.GetTilesInArea(Loc,3)){
            if(n.occupant != null){
                StatUpdate nstat = n.occupant.GetComponent<StatUpdate>();
                nstat.addStartBuff("TimeStop",target.name,2);
            }
        }
    }
    public void Hasten(string name, GameObject user, Vector3Int Loc){
        Node n = tileM.GetNodeFromWorld(Loc);
        if(!n.occupant == null){
            n.occupant.GetComponent<Abilities>().DecCoolDown();
            StatUpdate nstat = n.occupant.GetComponent<StatUpdate>();
            nstat.addStartBuff("Hasten",user.name,-1);
        }
    }

    public void Restrict(string name, GameObject user, Vector3Int Loc){
        Node n = tileM.GetNodeFromWorld(Loc);
        if(!n.occupant == null){
            n.occupant.GetComponent<Abilities>().DecCoolDown();
            StatUpdate nstat = n.occupant.GetComponent<StatUpdate>();
            nstat.addStartBuff("Restrict",user.name,-1);
            user.GetComponent<StatUpdate>().addBuff("Restrict",n.occupant.name,-1);
        }
    }

    public void Ripple(string name, GameObject play, Vector3Int Loc){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        AreaDamage(play, tileM.WorldToCell(play.transform.position),5,20+(int)ocstat.getDictStats("mid"),"Ripple","Restrict",1);
    }
    public void Armageddon(string name, GameObject play, Vector3Int Loc){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        AreaDamage(play, Loc,7,30+(int)ocstat.getDictStats("mid"),"Armageddon","",0);
        AreaDamage(play, Loc,7,30+(int)ocstat.getDictStats("mid"),"Armageddon","",0);
        AreaDamage(play, Loc,7,30+(int)ocstat.getDictStats("mid"),"Armageddon","",0);
    }

    public void Meditate(string name, GameObject play, Vector3Int Loc){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        CharacterStat chStat = ocstat.getStats();
        chStat.modifyStat("ene",10);
        chStat.modifyStat("stb",10);
        chStat.modifyStat("fat",-10);
    }
    public void CalmMind(string name, GameObject play, Vector3Int Loc){
        Node n = tileM.GetNodeFromWorld(Loc);
        if(n.occupant != null){
            Meditate(name, n.occupant, Vector3Int.zero);
        }
    }

    public void Accelerate(string name, GameObject play, Vector3Int Loc){
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

    public void BorrowedTime(string name, GameObject play, Vector3Int Loc){
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
    public static int CalculateExpression(string expression, CharacterStat stats)
    {
        // Define the regular expression pattern to match substrings between 'a' and 'z'
        string pattern = @"([a-z]+)";

        // Create a regular expression object
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

        // Match all substrings in the expression that fall within the range of 'a' to 'z'
        MatchCollection matches = regex.Matches(expression);

        // Iterate through the matches and replace each substring with its corresponding value
        foreach (Match match in matches)
        {
            string substring = match.Value;

            // Retrieve the value of the substring using the stats.getStat method
            int substringValue = (int)stats.getStat(substring);

            // Replace the substring with its value in the expression
            expression = expression.Replace(substring, substringValue.ToString());
        }

        // Evaluate the expression using the DataTable class from System.Data
        System.Data.DataTable dataTable = new System.Data.DataTable();
        var result = dataTable.Compute(expression, "");

        // Convert the result to an integer and return it
        return Convert.ToInt32(result);
    }
    public UDictionary<string,float> getSkillsFloats(string name, StatUpdate statU){
        CharacterStat stats = statU.getStats();
        if(SkillAttributes.ContainsKey(name)){
            UDictionary<string,string> pre = SkillAttributes[name];
            UDictionary<string,float> result = new UDictionary<string, float>();
            foreach(KeyValuePair<string,string> pair in pre){
                result.Add(pair.Key,CalculateExpression(pair.Value,stats));
            }
            return result;
        }
        
        switch(name){
            case "ForceBlast":
                return new UDictionary<string, float>(){{"Radius",3},{"CastRange",3 + (int) stats.getStat("acu")/4},{"TargetNum",1},{"CastTime",0}};
                break;
            case "PsychicStorm":
                return new UDictionary<string, float>(){{"Radius",3},{"CastRange",5 + (int) stats.getStat("acu")/4},{"TargetNum",1},{"CastTime",2}};
                break;
            case "WhirlWind":
                return new UDictionary<string, float>(){{"Radius",0},{"CastRange",0},{"TargetNum",0},{"CastTime",1}};
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
                return new UDictionary<string, float>(){{"Radius",0},{"CastRange",5},{"TargetNum",1},{"CastTime",1}};
                break;
            case "Armageddon":
                return new UDictionary<string, float>(){{"Radius",7},{"CastRange",7+(int)stats.getStat("acu")},{"TargetNum",1},{"CastTime",3}};
                break;
            case "Ripple":
                return new UDictionary<string, float>(){{"Radius",5},{"CastRange",0},{"TargetNum",0},{"CastTime",2}};
                break;
            case "Meditate":
                return new UDictionary<string, float>(){{"Radius",0},{"CastRange",0},{"TargetNum",0},{"CastTime",1}};
                break;
            case "CalmMind":
                return new UDictionary<string, float>(){{"Radius",0},{"CastRange",(int)stats.getStat("acu")},{"TargetNum",0},{"CastTime",1}};
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
        if(SkillBools.ContainsKey(name)){return SkillBools[name];}
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
        if(SkillCost.ContainsKey(name)){
            UDictionary<string,string> pre = SkillCost[name];
            foreach(KeyValuePair<string,string> pair in pre){
                chStat.modifyStat(pair.Key,(int)CalculateExpression(pair.Value,chStat)*ocstat.getStats().getCostMul());
            }
            if(name == "FireStance"){
                    ocstat.setBonus((int)ocstat.getDictStats("acu"));
                    ocstat.addEffectStat(new KeyValuePair<string, string>(play.name,"FireStance"), new UDictionary<string, int>(){
                        {"bonus damage",(int)ocstat.getDictStats("acu")},{"acu",(int)(ocstat.getDictStats("acu")+ocstat.getDictStats("dex"))}});
            }
            if(name == "WaterStance"){
                ocstat.addEffectStat(new KeyValuePair<string, string>(play.name,"Water"), new UDictionary<string, int>(){{"pv",(int)ocstat.getDictStats("acu")},{"pr",(int)(ocstat.getDictStats("acu")+ocstat.getDictStats("dex"))}});
            }
        }
        
    }
    public void AreaDamage(GameObject play, Vector3Int center, int range, int Damage, string name,string effect,int duration){
        string tag = "";
        if(SkillBools.ContainsKey(name) && SkillBools[name].ContainsKey("All")){
            if(SkillBools[name]["All"] == false){
                tag = play.tag;
            }
        }
        if(tag == ""){
            foreach(Node n in tileM.GetTilesInArea(center,range)){
                if(n.occupant != null && n.occupant != play){
                    StatUpdate ocstat = n.occupant.GetComponent<StatUpdate>();
                    if(!ocstat.isTimeStop()){
                        ocstat.TakeDamage(Damage);
                        if(effect !=""){
                        ocstat.addStartBuff(effect, play.name,duration);
                        }
                    }
                }
            }
        }
        else{
            foreach(Node n in tileM.GetTilesInArea(center,range)){
                if(n.occupant != null && n.occupant.tag != tag){
                    StatUpdate ocstat = n.occupant.GetComponent<StatUpdate>();
                    if(!ocstat.isTimeStop()){
                        ocstat.TakeDamage(Damage);
                        if(effect !=""){
                        ocstat.addStartBuff(effect, play.name,duration);
                        }
                    }
                }
            }
        }
    }
    public void AreaAttack(GameObject play, Vector3Int center, int range, string name,string effect,int duration){
        string tag = "";
        if(SkillBools.ContainsKey(name) && SkillBools[name].ContainsKey("All")){
            if(SkillBools[name]["All"] == false){
                tag = play.tag;
            }
        }
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        if(tag == ""){
            foreach(Node n in tileM.GetTilesInArea(center,range)){
                if(n.occupant != null && n.occupant != play){
                    StatUpdate targetstat = n.occupant.GetComponent<StatUpdate>();
                    if(!targetstat.isTimeStop()){
                        ocstat.attackEn(n.occupant);
                        if(effect !=""){
                            targetstat.addStartBuff(effect, play.name,duration);
                        }
                    }
                }
            }
        }
        else{
            foreach(Node n in tileM.GetTilesInArea(center,range)){
                if(n.occupant != null && n.occupant.tag != tag){
                    StatUpdate targetstat = n.occupant.GetComponent<StatUpdate>();
                    if(!targetstat.isTimeStop()){
                        ocstat.attackEn(n.occupant);
                        if(effect !=""){
                            targetstat.addStartBuff(effect, play.name,duration);
                        }
                    }
                }
            }
        }
    }
    
    public void GeneralAreaEffect(GameObject play, Vector3Int center, int range, string name){
        string tag = "";
        if(SkillBools.ContainsKey(name) && SkillBools[name].ContainsKey("All")){
            if(SkillBools[name]["All"] == false){
                tag = play.tag;
            }
        }
        if(SkillBools.ContainsKey(name) && SkillBools[name].ContainsKey("characterTarget")){
            if(!SkillBools[name]["characterTarget"] && center == Vector3Int.zero){
                center =tileM.WorldToCell( play.transform.position);
            }
        }
        if(tag ==""){
            foreach(Node n in tileM.GetTilesInArea(center,range)){
                if(n.occupant != null && n.occupant != play){
                    StatUpdate ocstat = n.occupant.GetComponent<StatUpdate>();
                    if(!ocstat.isTimeStop() &&SkillStats.ContainsKey(name)){
                        
                            foreach(KeyValuePair<string,int> p in SkillStats[name]){
                                if(p.Key == "bonus damage"){
                                    ocstat.setBonus(p.Value);
                                }
                                else{
                                    ocstat.getStats().modifyStat(p.Key,p.Value);
                                }
                            }
                            ocstat.addEffectStat(new KeyValuePair<string,string>(play.name,name),SkillStats[name]);
                            ocstat.addStartBuff(name, play.name,(int)CalculateExpression(SkillAttributes[name]["Duration"],play.GetComponent<StatUpdate>().getStats()));
                        }
                        
                    
                }
            }
        }
        else{
            foreach(Node n in tileM.GetTilesInArea(center,range)){
                if(n.occupant != null && n.occupant.tag != tag){
                    StatUpdate ocstat = n.occupant.GetComponent<StatUpdate>();
                    if(!ocstat.isTimeStop() &&SkillStats.ContainsKey(name)){
                            foreach(KeyValuePair<string,int> p in SkillStats[name]){
                                if(p.Key == "bonus damage"){
                                    ocstat.setBonus(p.Value);
                                }
                                else{
                                    ocstat.getStats().modifyStat(p.Key,p.Value);
                                }
                            }
                            ocstat.addEffectStat(new KeyValuePair<string,string>(play.name,name),SkillStats[name]);
                            ocstat.addStartBuff(name, play.name,(int)CalculateExpression(SkillAttributes[name]["Duration"],play.GetComponent<StatUpdate>().getStats()));
                        }
                        
                    
                }
            }
        }
    }
    //Attributes: Type, CoolDown, Radius, CastTime, CastRange, TargetNum, Duration, Effect,Damage
    //bools: CharacterTarget, sameTag, All
    public void GeneralSkill(string Skillname, GameObject play, Vector3Int center){
        //string Skillname = turnM.getUI().getCurrentPlay().GetComponent<Abilities>().getCurrentSkill();
        if(SkillAttributes.ContainsKey(Skillname)){
            if(SkillAttributes[Skillname]["Type"] == "0" ||SkillAttributes[Skillname]["Type"] == "1" ){
                GeneralAreaDamage(play,center,(int)CalculateExpression(SkillAttributes[Skillname]["Radius"],play.GetComponent<StatUpdate>().getStats()),Skillname);
            }
            else if(SkillAttributes[Skillname]["Type"] == "2"){
                GeneralAreaEffect(play,center,(int)CalculateExpression(SkillAttributes[Skillname]["Radius"],play.GetComponent<StatUpdate>().getStats()),Skillname);
            }
            else{
                tileM.GetNodeFromWorld(center).effectFlag.Add(Skillname, new KeyValuePair<GameObject, int>(play, (int)CalculateExpression(SkillAttributes[Skillname]["Duration"],play.GetComponent<StatUpdate>().getStats())));
                tileM.AddEffectLst(tileM.GetNodeFromWorld(center));
            }
        }
    }
    public void GeneralAreaDamage(GameObject play, Vector3Int center, int range, string name){
        string effect = "";
        int duration = 0;
        if(SkillAttributes[name].ContainsKey("Effect") && SkillAttributes[name].ContainsKey("Duration")){
            effect = getEffect((int)CalculateExpression(SkillAttributes[name]["Effect"],play.GetComponent<StatUpdate>().getStats()));
            duration = (int) CalculateExpression(SkillAttributes[name]["Duration"],play.GetComponent<StatUpdate>().getStats());
        }
        if(SkillAttributes[name]["Type"] == "0"){
            AreaDamage(play, center, range,(int)CalculateExpression(SkillAttributes[name]["Damage"],play.GetComponent<StatUpdate>().getStats()),name,effect,duration);
        }
        else if(SkillAttributes[name]["Type"] == "1"){
            AreaAttack(play, center, range,name,"",duration);
        }
    }

    public void GeneralAreaEffect(string name, GameObject play, Vector3Int target){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        AreaDamage(play, target, 3, (int)CalculateExpression(SkillAttributes[name]["Damage"],play.GetComponent<StatUpdate>().getStats()),name, "",0);
    }
    public void GeneralPassive(string name, GameObject play){
        setTileM();
        KeyValuePair<string,string> pair = new KeyValuePair<string, string>(play.name,name);
        string enemy_tag = "Enemy";
        if(play.tag == "Enemy"){enemy_tag = "Player";}
        foreach(GameObject go in GameObject.FindGameObjectsWithTag(enemy_tag)){
            StatUpdate su = go.GetComponent<StatUpdate>();
            Vector3Int pos = tileM.WorldToCell(go.transform.position);
            if(tileM.inArea(tileM.WorldToCell(play.transform.position),pos,2)){
                if(!su.isTimeStop()&&su.addBuff(name, play.name,-1)){
                    foreach(KeyValuePair<string,int> spair in SkillStats[name]){
                        su.getStats().modifyStat(spair.Key,spair.Value);
                    }
                    su.addEffectStat(pair, SkillStats[name]);

                }
            }
            else{
                if(!su.isTimeStop()&&su.removeBuff(name, play.name)){
                    foreach(KeyValuePair<string,int> spair in SkillStats[name]){
                        su.getStats().modifyStat(spair.Key,-spair.Value);
                    }
                }
            }
        }
    }
    public string getEffect(int i){
        switch(i){
            case 0: return "TimeStop";
            case 1: return "Restrict";
        }
        return "";
    }

    public void addEntry(int type, string name, UDictionary<string,string> attribute,UDictionary<string,string> cost,UDictionary<string,int> stat,UDictionary<string,bool> bools){
        removeEntry(name);
        if(type == 0){
            AbilitiesLst.Add(name);
            UnityEvent<string,GameObject> a = new UnityEvent<string, GameObject>();
            a.AddListener(GeneralPassive);
            Abilities.Add(name,a);
        }
        else{
            SkillLst.Add(name);
            UnityEvent<string,GameObject,Vector3Int> e = new UnityEvent<string, GameObject,Vector3Int>();
            e.AddListener(GeneralSkill);
            Skills.Add(name,e);
        }
        SkillAttributes.Add(name,attribute);
        SkillCost.Add(name,cost);
        SkillStats.Add(name,stat);
        SkillBools.Add(name,bools);
    }
    public void removeEntry(string name){
        if(Skills.ContainsKey(name)){Skills.Remove(name);}
        else{Abilities.Remove(name);}
        if(AbilitiesLst.Contains(name)){AbilitiesLst.Remove(name);}
        else{SkillLst.Remove(name);}
        if(SkillAttributes.ContainsKey(name)){SkillAttributes.Remove(name);}
        if(SkillCost.ContainsKey(name)){SkillCost.Remove(name);}
        if(SkillStats.ContainsKey(name)){SkillStats.Remove(name);}
        if(SkillBools.ContainsKey(name)){SkillBools.Remove(name);}
    }

    public void resetSkillAtt(){
        SkillAttributes.Clear();
        SkillAttributes.Add( "ForceBlast", 
        new UDictionary<string, string>(){{"Radius","3"},{"CastRange","3+acu/4"},{"TargetNum","1"},{"CastTime","0"}});
        SkillAttributes.Add( "PsychicStorm", 
        new UDictionary<string, string>(){{"Radius","3"},{"CastRange","5+acu/4"},{"TargetNum","1"},{"CastTime","2"}});        
        SkillAttributes.Add( "WhirlWind", 
        new UDictionary<string, string>(){{"Radius","0"},{"CastRange","0"},{"TargetNum","0"},{"CastTime","1"}});
        SkillAttributes.Add( "WaterStance", 
        new UDictionary<string, string>(){{"Radius","0"},{"CastRange","0"},{"TargetNum","0"},{"CastTime","0"}});
        SkillAttributes.Add( "FireStance", 
        new UDictionary<string, string>(){{"Radius","0"},{"CastRange","0"},{"TargetNum","0"},{"CastTime","0"}});
        SkillAttributes.Add( "Bubble", 
        new UDictionary<string, string>(){{"Radius","0"},{"CastRange","9999"},{"TargetNum","1"},{"CastTime","0"}});
        SkillAttributes.Add( "ForeSight", 
        new UDictionary<string, string>(){{"Radius","0"},{"CastRange","9999"},{"TargetNum","1"},{"CastTime","0"}});
        SkillAttributes.Add( "TimeStop", 
        new UDictionary<string, string>(){{"Radius","3"},{"CastRange","7+acu"},{"TargetNum","1"},{"CastTime","0"}});
        SkillAttributes.Add( "Restrict", 
        new UDictionary<string, string>(){{"Radius","0"},{"CastRange","5"},{"TargetNum","1"},{"CastTime","1"}});
        SkillAttributes.Add( "Armageddon", 
        new UDictionary<string, string>(){{"Radius","7"},{"CastRange","7+acu"},{"TargetNum","1"},{"CastTime","3"}});
        SkillAttributes.Add( "Ripple", 
        new UDictionary<string, string>(){{"Radius","5"},{"CastRange","0"},{"TargetNum","0"},{"CastTime","2"}});
        SkillAttributes.Add( "Meditate", 
        new UDictionary<string, string>(){{"Radius","0"},{"CastRange","0"},{"TargetNum","0"},{"CastTime","1"}});
        SkillAttributes.Add( "CalmMind", 
        new UDictionary<string, string>(){{"Radius","0"},{"CastRange","acu"},{"TargetNum","0"},{"CastTime","1"}});
        SkillAttributes.Add( "Accelerate", 
        new UDictionary<string, string>(){{"Radius","0"},{"CastRange","7"},{"TargetNum","0"},{"CastTime","0"}});
        SkillAttributes.Add( "BorrowedTime", 
        new UDictionary<string, string>(){{"Radius","0"},{"CastRange","0"},{"TargetNum","0"},{"CastTime","0"}});
    }
    public void resetSkillBools(){
        SkillBools.Clear();
        SkillBools.Add( "ForceBlast",
                new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}});
            SkillBools.Add( "PsychicStorm",
                new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}});
            SkillBools.Add( "WhirlWind",
                new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}});
            SkillBools.Add( "WaterStance",
               new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}});
            SkillBools.Add( "FireStance",
                new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}});
            SkillBools.Add( "ForeSight",
                new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}});
            SkillBools.Add( "Bubble",
                new UDictionary<string, bool>(){{"characterTarget",true},{"sameTag",true}});
            SkillBools.Add( "TimeStop",
                new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}});
            SkillBools.Add( "Restrict",
                new UDictionary<string, bool>(){{"characterTarget",true},{"sameTag",false}});
            SkillBools.Add( "Accelerate",
                new UDictionary<string, bool>(){{"characterTarget",false},{"sameTag",false}});
            SkillBools.Add( "BorrowedTime",
                new UDictionary<string, bool>(){{"characterTarget",true},{"sameTag",true}});
    }
    public void resetSkillCost(){
        SkillCost.Clear();
        SkillCost.Add( "ForceBlast", 
        new UDictionary<string, string>(){{"ene","-15"},{"fat","20"}});
        SkillCost.Add( "PsychicStorm", 
        new UDictionary<string, string>(){{"ene","-25"},{"stb","-30"}});        
        SkillCost.Add( "WhirlWind", 
        new UDictionary<string, string>(){{"ene","-7"},{"fat","15"}});
        SkillCost.Add( "WaterStance", 
        new UDictionary<string, string>(){{"ene","-5"},{"stb","-5"},{"fat","5"},{"pv","acu"},{"pr","acu"}});
        SkillCost.Add( "FireStance", 
        new UDictionary<string, string>(){{"ene","-5"},{"stb","-5"},{"fat","5"},{"acu","acu+dex"}});
        SkillCost.Add( "Bubble", 
        new UDictionary<string, string>(){{"ene","-10"}});
        SkillCost.Add( "TimeStop", 
        new UDictionary<string, string>(){{"ene","-10"},{"stb","-10"}});
        SkillCost.Add( "Restrict", 
        new UDictionary<string, string>(){{"ene","-10"},{"stb","-5"}});
        SkillCost.Add( "Armageddon", 
        new UDictionary<string, string>(){{"ene","-50"},{"stb","-100"},{"fat","100"}});
        SkillCost.Add( "Ripple", 
        new UDictionary<string, string>(){{"ene","-20"},{"stb","-30"}});
        SkillCost.Add( "CalmMind", 
        new UDictionary<string, string>(){{"ene","-10"}});
        SkillCost.Add( "Accelerate", 
        new UDictionary<string, string>(){{"ene","-10"},{"stb","-5"}});
        SkillCost.Add( "BorrowedTime", 
        new UDictionary<string, string>(){{"ene","-5"}});
    }
}
