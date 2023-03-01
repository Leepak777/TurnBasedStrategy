using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<GameObject> turnOrder;
    public List<GameObject> turnOrder2;
    private int currentTurnIndex;
    private int currentTurnIndex2;
    public bool player = true;
    bool Active = false;
    public GameObject currentPlay;
    private int turnElasped;

    void Start()
    {
        turnOrder = new List<GameObject>();
        turnOrder2 = new List<GameObject>();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
            turnOrder.Add(g);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
            turnOrder2.Add(g);
        currentTurnIndex = 0;
        currentTurnIndex2 = 0;
        //reset2();
    }

    void Update()
    {
        if (player)
        {
            UpdatePlayerTurn();
        }
        else
        {
            UpdateEnemyTurn();
        }


    }

    void UpdatePlayerTurn()
    {
        if (currentTurnIndex < turnOrder.Count && turnOrder[currentTurnIndex] != null)
        {
            var ac = turnOrder[currentTurnIndex].GetComponent<ActionCenter>();

            if (ac.moved && !ac.turn)
            {
                turnOrder[currentTurnIndex].GetComponent<StatUpdate>().checkFatigue(ac.getTilesFat());
                turnOrder[currentTurnIndex].GetComponent<StatUpdate>().setDamage(0);
                updateTurn(ac);
                Active = false;
            }
            else if (checkFat(turnOrder[currentTurnIndex]))
            {
                turnOrder[currentTurnIndex].GetComponent<ActionCenter>().startTurn();
                Active = true;
                currentPlay = turnOrder[currentTurnIndex];
            }
            else if (checkFat(turnOrder[currentTurnIndex])&& !ac.turn)
            {
                ac.endTurn();
                turnOrder[currentTurnIndex].GetComponent<StatUpdate>().setDamage(0);
                updateTurn(ac);
                Active = false;
            }
        }
        else if(turnOrder.Count > 0){
            if(currentTurnIndex >= turnOrder.Count){
                    currentTurnIndex = 0;
            }
            while(turnOrder[currentTurnIndex] == null){
                currentTurnIndex++;
                if(currentTurnIndex >= turnOrder.Count){
                    currentTurnIndex = 0;
                }
            }
        }
    }

    void UpdateEnemyTurn()
    {
        if (currentTurnIndex2 < turnOrder2.Count && turnOrder2[currentTurnIndex2] != null)
        {
            var ac = turnOrder2[currentTurnIndex2].GetComponent<ActionCenter>();

            if (ac.moved && !ac.turn)
            {
                turnOrder2[currentTurnIndex2].GetComponent<StatUpdate>().checkFatigue(ac.getTilesFat());
                turnOrder2[currentTurnIndex2].GetComponent<StatUpdate>().setDamage(0);
                turnOrder2[currentTurnIndex2].GetComponent<Movement>().setPath = false;
                updateTurn(ac);
                Active = false;
            }
            else if (checkFat(turnOrder2[currentTurnIndex2]) && !ac.turn)
            {
                turnOrder2[currentTurnIndex2].GetComponent<ActionCenter>().startTurn();
                currentPlay = turnOrder2[currentTurnIndex2];
                Active = true;
            }
            else if (!checkFat(turnOrder2[currentTurnIndex2]))
            {
                ac.endTurn();
                turnOrder2[currentTurnIndex2].GetComponent<StatUpdate>().setDamage(0);
                updateTurn(ac);
                Active = false;
            }
        }
        else if(turnOrder2.Count > 0){
            if(currentTurnIndex2 >= turnOrder2.Count){
                    currentTurnIndex2 = 0;
            }
            while(turnOrder2[currentTurnIndex2] == null){
                currentTurnIndex2++;
                if(currentTurnIndex2 >= turnOrder2.Count){
                    currentTurnIndex2 = 0;
                }
            }
        }
    }

    bool checkFat(GameObject go){
        return go.GetComponent<StatUpdate>().getDictStats("fat") < 100;
    }


    void checkText()
    {
        if(player){
        foreach(GameObject go in turnOrder){
            if (go.GetComponent<StatUpdate>().getTextEnabled())
            {
                go.GetComponent<StatUpdate>().setTextActive(false);
            }
        }
        }
        else{
        foreach(GameObject go in turnOrder2){
            if (go.GetComponent<StatUpdate>().getTextEnabled())
            {
                go.GetComponent<StatUpdate>().setTextActive(false);
            }
        }}
    }

    private void updateTurn(ActionCenter ac)
    {
        turnElasped++;
        if(turnElasped >= currentPlay.GetComponent<StatUpdate>().getDictStats("attack_num")){
            updateIndex();
            turnElasped = 0;
        }
        else{
            ac.resetTurn();    
        }
    }

    private void updateIndex()
    {
        if (this.player)
        {
            currentTurnIndex++;
            if (currentTurnIndex >= turnOrder.Count)
            {
                reset1();
                currentTurnIndex = 0;
                 
            }this.player = false;// switch to enemy's turn
        }
        else
        {
            currentTurnIndex2++;
            if (currentTurnIndex2 >= turnOrder2.Count)
            {
                reset2();
                currentTurnIndex2 = 0;
                
            }this.player = true; // switch to player's turn
        }
    }


    private void reset1()
    {
        foreach (GameObject go in turnOrder)
        {
            if (go != null)
            {
                go.GetComponent<ActionCenter>().resetTurn();
            }
        }
    }

    private void reset2()
    {
        foreach (GameObject go in turnOrder2)
        {
            if (go != null)
            {
                go.GetComponent<ActionCenter>().resetTurn();
            }
        }
    }
    public void removefromLst(GameObject go){
        if(go.tag == "Player"){
            turnOrder.Remove(go);
        }
        if(go.tag == "Enemy"){
            turnOrder2.Remove(go);
        }
    }
    public bool getActive(){
        return Active;
    }

    public int getTurnElasped(){
        return turnElasped;
    }
    public void setTurnElasped(int x){
        turnElasped += x;
    }
    public GameObject getCurrenPlay(){
        return currentPlay;
    }
}
