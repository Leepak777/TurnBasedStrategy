using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSimulation 
{
    DRN drn;
    public AttackSimulation(DRN drn){
        this.drn = drn;
    }
    public KeyValuePair<bool,KeyValuePair<int,int>> SimulateAttack(GameObject attack, GameObject defense){
        TileManager tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        StatUpdate attack_stat = attack.GetComponent<StatUpdate>();
        StatUpdate defense_stat = defense.GetComponent<StatUpdate>();
        bool roll = attack_stat.rangeRoll(defense);
        int ATK;
        int DEF;
        if(tileM.IsAdjacent(attack.GetComponent<ActionCenter>().getMapPos(),defense.GetComponent<ActionCenter>().getMapPos())){
            roll = attack_stat.meleeRoll(defense);
        }
        ATK = (int)(drn.getDRN() + attack_stat.getStats().getBaseDamage() + attack_stat.getBonus());
        DEF = drn.getDRN() + attack_stat.getStats().getProtection();
        
        return new KeyValuePair<bool, KeyValuePair<int,int>>(roll, new KeyValuePair<int, int>(ATK,DEF));
    }

    public KeyValuePair<float,float> atkPossibility(GameObject attack, GameObject defense){
        int sucess = 0;
        int Damage_dealt = 0;
        int Damage_deducted = 0;
        for(int i = 0; i < 100; i ++){
            KeyValuePair<bool,KeyValuePair<int,int>> sim = SimulateAttack(attack, defense);
            if(sim.Key && sim.Value.Key > sim.Value.Value){
                sucess++;
                Damage_dealt += sim.Value.Key;
                Damage_deducted += sim.Value.Value;
            }
        }
        return new KeyValuePair<float, float>(sucess, (Damage_dealt-Damage_deducted)/sucess);
    }
}
