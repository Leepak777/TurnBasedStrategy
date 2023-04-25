using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AbilitiesData", order = 3)]
public class AbilitiesData : ScriptableObject
{
    public UnityEvent<GameObject> Leadership;
    public UnityEvent<GameObject> charge;
    TileManager tileM;
    TurnManager turnM;
    int charge_bonus = 0;

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
    public void LeadersshipAura(GameObject play){
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

    public void AreaAttack(Vector3Int center, int range, int Damage){

    }


}
