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
    public UnityEvent<GameObject, Vector3Int> ForeSight_e;
    TileManager tileM;
    TurnManager turnM;
    int charge_bonus = 0;
    void setTileM(){
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
    }
    void Awake(){
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
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
    public void LeadersshipAura(GameObject play){
        setTileM();
        foreach(GameObject go in GameObject.FindGameObjectsWithTag(play.tag)){
            StatUpdate su = go.GetComponent<StatUpdate>();
            Vector3Int pos = tileM.WorldToCell(go.transform.position);
            if(tileM.inArea(tileM.WorldToCell(play.transform.position),pos,3)){
                if(su.addBuff("LeadershipAura", play.name)){
                    su.setBonus(1);
                }
            }
            else{
                if(su.removeBuff("LeadershipAura", play.name)){
                    su.setBonus(-1);
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
        AreaAttack(play, target,3,play.tag);
        ocstat.getStats().modifyStat("ene",-7);
        ocstat.getStats().modifyStat("stb",-15);
    }
    public void ForceBlast(GameObject play, Vector3Int target){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        float Damage = 10 + ocstat.getDictStats("mid")*2;
        int range = 3 + (int)ocstat.getDictStats("acu")/4;
        AreaDamage(play,target,range,(int)Damage,play.tag);
        ocstat.getStats().modifyStat("ene",-15);
        ocstat.getStats().modifyStat("fat",-20);
        Debug.Log(Damage);
    }
    public void PsychicStorm(GameObject play, Vector3Int target){
        tileM.GetNodeFromWorld(target).effectFlag.Add("PsychicStorm", new KeyValuePair<GameObject, int>(play, 3));
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        ocstat.getStats().modifyStat("ene",-25);
        ocstat.getStats().modifyStat("stb",-30);
    }

    public void PsychicStormEffect(GameObject play, Vector3Int target){
        StatUpdate ocstat = play.GetComponent<StatUpdate>();
        AreaDamage(play, target, 3, (int)(20+ocstat.getDictStats("mid")-3), play.tag);
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
        turnM.EnemyBackUP();
        turnM.PlayerBackUP();
    }
    public void foresightEnd(){
        turnM.EnemyRevert();
        turnM.PlayerRevert();
    }




}
