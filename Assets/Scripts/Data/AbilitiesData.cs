using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AbilitiesData", order = 3)]
public class AbilitiesData : ScriptableObject
{
    public UnityEvent<TileManager,GameObject> Leadership;
    public UnityEvent<TileManager,GameObject> getEvent(string name){
        switch(name){
            case "LeadershipAura": 
            return Leadership;
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


}
