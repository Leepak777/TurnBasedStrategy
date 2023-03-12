using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    TileManager tileM;
    Tilemap tilemap;
    public bool attacking= false;
    public int attackArea = 0;
    public int attackrange;
    void Start()
    {
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        attackrange = this.gameObject.GetComponent<StatUpdate>().getAttackRange();
    }


    // Update is called once per frame
    void Update()
    {
        
        
    }
    public void EnemyAttack(){
        GameObject targetPlayer = tileM.getClosestPlayer("Player", transform.position);
        Vector3Int targetNode = tilemap.WorldToCell(targetPlayer.transform.position);
        tileM.flagEnemyArea(targetPlayer,"Player",attackArea);
        this.gameObject.GetComponent<CharacterEvents>().onAttacking.Invoke(targetPlayer);
        return;
    }
    public void PlayerAttack(Vector3 mousePosition){
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int targetNode = tilemap.WorldToCell(target);
        if(tileM.GetNodeFromWorld(targetNode).occupant != null){
            GameObject go = tileM.GetNodeFromWorld(targetNode).occupant;
            if(tileM.inArea(tilemap.WorldToCell(transform.position),tilemap.WorldToCell(go.transform.position),attackrange)){
                //tileM.flagEnemyArea(go,"Enemy",attackArea);
                this.gameObject.GetComponent<CharacterEvents>().onAttacking.Invoke(go);
           }              
       }
        
    }
    public void Attacking(string tag){
        if(!attacking && tileM.EnemyInRange(tag, attackrange, this.gameObject)){
            this.gameObject.GetComponent<CharacterEvents>().onAttackTrue.Invoke();
        }
        else{
            this.gameObject.GetComponent<CharacterEvents>().onAttackFalse.Invoke();
        }
    }
    public void setAttackArea(int i){
        attackArea = i;
    }
    public void setAttacking(bool attacking){
        this.attacking = attacking;
    }
    public bool isAttacking(){
        return attacking;
    }
}
