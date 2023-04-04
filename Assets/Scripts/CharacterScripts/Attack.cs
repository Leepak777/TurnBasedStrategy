using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    TileManager tileM;
    public float attackrange;
    void Start()
    {
        getTileM();
        attackrange = this.gameObject.GetComponent<StatUpdate>().getAttackRange();
    }
    //Enemy Attack cloest Player, call after Enemy moved
    public void EnemyAttack(){
        GameObject targetPlayer = tileM.getClosestPlayer("Player", transform.position);
        Vector3Int targetNode = tileM.WorldToCell(targetPlayer.transform.position);
        //tileM.flagEnemyArea(targetPlayer,"Player",attackArea);
        if(tileM.inArea(tileM.WorldToCell(transform.position),tileM.WorldToCell(targetPlayer.transform.position),attackrange)){
            this.gameObject.GetComponentInChildren<CharacterEvents>().onAttacking.Invoke(targetPlayer);
            this.gameObject.GetComponentInChildren<CharacterEvents>().onUnHighLight.Invoke();
        }
    }
    //Player Attack enemy on mouseposition, call after click attack button in popup
    //click enemy=>popup=>click attack button=>this
    public void PlayerAttack(GameObject target){
        if(target != null){
            GameObject go = target;
            this.gameObject.GetComponentInChildren<CharacterEvents>().onAttacking.Invoke(go);
            this.gameObject.GetComponentInChildren<CharacterEvents>().onUnHighLight.Invoke();
       }
    }

    public void getTileM(){
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
    }

}
