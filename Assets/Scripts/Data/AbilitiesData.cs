using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AbilitiesData", order = 3)]
public class AbilitiesData : ScriptableObject
{
    public UnityEvent<TileManager,GameObject> Leadership;
    public UnityEvent<TileManager,GameObject> charge;
    int charge_bonus = 0;
    public UnityEvent<TileManager,GameObject> getEvent(string name){
        switch(name){
            case "LeadershipAura": 
            return Leadership;
            case "Charge":
            return charge;
        }
        return null;
    }
    public void LeadersshipAura(TileManager tileM, GameObject play){
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
    public void Charge(TileManager tileM, GameObject play){
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


}
