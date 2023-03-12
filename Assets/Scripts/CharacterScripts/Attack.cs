using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    HighlightReachableTiles hightlightReachableTile;
    TileManager tileM;
    ActionCenter ac;
    Movement movement;
    Tilemap tilemap;
    public bool attacking= false;
    public int attackArea = 0;
    public int attackrange;
    void Start()
    {
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        ac = this.gameObject.GetComponent<ActionCenter>();
        hightlightReachableTile = ac.getHighLight();
        movement = this.gameObject.GetComponent<Movement>();
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
        ac.setTargetEnemy(targetPlayer);
        tileM.flagEnemyArea(targetPlayer,"Player",attackArea);
        AttackCheck(targetPlayer);
        return;
    }
    public void PlayerAttack(Vector3 mousePosition){
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int targetNode = tilemap.WorldToCell(target);
        if(tileM.GetNodeFromWorld(targetNode).occupant != null){
            GameObject go = tileM.GetNodeFromWorld(targetNode).occupant;
            if(tileM.inArea(tilemap.WorldToCell(transform.position),tilemap.WorldToCell(go.transform.position),attackrange)){
                ac.setTargetEnemy(go);
                //tileM.flagEnemyArea(go,"Enemy",attackArea);
                AttackCheck(go);
           }              
       }
        
    }
    public void Attacking(string tag){
        if(!attacking && tileM.EnemyInRange(tag, attackrange, this.gameObject)){
            hightlightReachableTile.HighlightEnemy();
            attacking = true;
        }
        else{
            attacking = false;
            hightlightReachableTile.UnhighlightEnemy();
        }
    }
    public void AttackCheck(GameObject en){
        //foreach(GameObject en in GameObject.FindGameObjectsWithTag(tag)){
        //    if(en.GetComponent<StatUpdate>().flag){
                this.gameObject.GetComponent<StatUpdate>().attackEn(en);
        //    }
        //}
        attacking = false;
            
        GameObject.Find("TurnManager").GetComponent<TurnManager>().endTurn();
        
    }
    public void setAttackArea(int i){
        attackArea = i;
    }

    public bool isAttacking(){
        return attacking;
    }
}
