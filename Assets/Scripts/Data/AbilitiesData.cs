
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
    UDictionary<string, UDictionary<string,float>> SkillAttributes = new UDictionary<string, UDictionary<string, float>>();
    [SerializeField]
    UDictionary<string, UDictionary<string,bool>> SkillBools = new UDictionary<string, UDictionary<string, bool>>();
    [SerializeField]
    UDictionary<string, UDictionary<string,float>> SkillCost = new UDictionary<string, UDictionary<string,float>>();
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
    public UDictionary<string,float> getSkillAtt(string name){
        if(SkillAttributes.ContainsKey(name)){
        return SkillAttributes[name];}
        return null;
    }
     public UDictionary<string,int> getSkillStat(string name){
        if(SkillStats.ContainsKey(name)){
        return SkillStats[name];}
        return null;
    }
     public UDictionary<string,float> getSkillCost(string name){
        if(SkillCost.ContainsKey(name)){
        return SkillCost[name];}
        return null;
    }
    public UDictionary<string,bool> getSkillBool(string name){
        if(SkillCost.ContainsKey(name)){
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
    public void AreaDamage(GameObject play, Vector3Int center, int range, int Damage, string name,string effect,int duration){
        string tag = "";
        if(SkillAttributes.ContainsKey(name)){
            if(SkillAttributes[name]["All"] == 1){
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
        if(SkillAttributes.ContainsKey(name)){
            if(SkillAttributes[name]["All"] == 1){
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
        if(SkillAttributes.ContainsKey(name)){
            if(SkillAttributes[name]["All"] == 1){
                tag = play.tag;
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
                            ocstat.addStartBuff(name, play.name,(int)SkillAttributes[name]["Duration"]);
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
                            ocstat.addStartBuff(name, play.name,(int)SkillAttributes[name]["Duration"]);
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
            if(SkillAttributes[Skillname]["Type"] == 0 ||SkillAttributes[Skillname]["Type"] == 1 ){
                GeneralAreaDamage(play,center,(int)SkillAttributes[Skillname]["Radius"],Skillname);
            }
            else if(SkillAttributes[Skillname]["Type"] == 2){
                GeneralAreaEffect(play,center,(int)SkillAttributes[Skillname]["Radius"],Skillname);
            }
            else{
                tileM.GetNodeFromWorld(center).effectFlag.Add(Skillname, new KeyValuePair<GameObject, int>(play, (int)SkillAttributes[Skillname]["Duration"]));
                tileM.AddEffectLst(tileM.GetNodeFromWorld(center));
            }
        }
    }
    public void GeneralAreaDamage(GameObject play, Vector3Int center, int range, string name){
        string effect = "";
        int duration = 0;
        if(SkillAttributes[name].ContainsKey("Effect") && SkillAttributes[name].ContainsKey("Duration")){
            effect = getEffect((int)SkillAttributes[name]["Effect"]);
            duration = (int) SkillAttributes[name]["Duration"];
        }
        if(SkillAttributes[name]["Type"] == 0){
            AreaDamage(play, center, range,(int)SkillAttributes[name]["Damage"],name,effect,duration);
        }
        else if(SkillAttributes[name]["Type"] == 1){
            AreaAttack(play, center, range,name,"",duration);
        }
    }

    public void GeneralAreaEffect(string name, GameObject play, Vector3Int target){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        AreaDamage(play, target, 3, (int)SkillAttributes[name]["Damage"],name, "",0);
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

    public void addEntry(int type, string name, UDictionary<string,float> attribute,UDictionary<string,float> cost,UDictionary<string,int> stat,UDictionary<string,bool> bools){
        if(type == 0){AbilitiesLst.Add(name);}
        else{SkillLst.Add(name);}
        SkillAttributes.Add(name,attribute);
        SkillCost.Add(name,cost);
        SkillStats.Add(name,stat);
        SkillBools.Add(name,bools);
    }
    public void removeEntry(string name){
        if(AbilitiesLst.Contains(name)){AbilitiesLst.Remove(name);}
        else{SkillLst.Remove(name);}
        if(SkillAttributes.ContainsKey(name)){SkillAttirbutes.Remove(name);}
        if(SkillCost.ContainsKey(name)){SkillCost.Remove(name);}
        if(SkillStats.ContainsKey(name)){SkillStats.Remove(name);}
        if(SkillBools.ContainsKey(name)){SkillBools.Remove(name);}
    }
    
}
