using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSimulation 
{
    DRN drn;
    public AttackSimulation(DRN drn){
        this.drn = drn;
    }
    public KeyValuePair<bool,int> SimulateAttack(GameObject attack, GameObject defense){
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
        int damage = ATK - DEF;
        if(damage <0){
            damage = 0;
        }
        return new KeyValuePair<bool, int>(roll, damage);
    }

    public KeyValuePair<float,float> atkPossibility(GameObject attack, GameObject defense){
        int sucess = 0;
        int Damage_dealt = 0;
        for(int i = 0; i < 100; i ++){
            KeyValuePair<bool,int> sim = SimulateAttack(attack, defense);
            if(sim.Key){
                sucess++;
                Damage_dealt += sim.Value;
            }
        }
        Debug.Log("Damage dealt: " + Damage_dealt + ", Sucess rate: " +sucess);
        return new KeyValuePair<float, float>(sucess, Damage_dealt);
    }
}
