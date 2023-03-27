using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    TileManager tileM;
    public int attackArea = 0;
    public float attackrange;
    void Start()
    {
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        attackrange = this.gameObject.GetComponent<StatUpdate>().getAttackRange();
    }
    //Enemy Attack cloest Player, call after Enemy moved
    public void EnemyAttack(){
        GameObject targetPlayer = tileM.getClosestPlayer("Player", transform.position);
        Vector3Int targetNode = tileM.WorldToCell(targetPlayer.transform.position);
        //tileM.flagEnemyArea(targetPlayer,"Player",attackArea);
        if(tileM.inArea(tileM.WorldToCell(transform.position),tileM.WorldToCell(targetPlayer.transform.position),attackrange)){
            this.gameObject.GetComponent<CharacterEvents>().onAttacking.Invoke(targetPlayer);
            this.gameObject.GetComponent<CharacterEvents>().onUnHighLight.Invoke();
        }
    }
    //Player Attack enemy on mouseposition, call after click attack button in popup
    //click enemy=>popup=>click attack button=>this
    public void PlayerAttack(Vector3 mousePosition){
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int targetNode = tileM.WorldToCell(target);
        if(tileM.GetNodeFromWorld(targetNode).occupant != null){
            GameObject go = tileM.GetNodeFromWorld(targetNode).occupant;
            this.gameObject.GetComponent<CharacterEvents>().onAttacking.Invoke(go);
            this.gameObject.GetComponent<CharacterEvents>().onUnHighLight.Invoke();
       }
    }

    public void setAttackArea(int i){
        attackArea = i;
    }

}
