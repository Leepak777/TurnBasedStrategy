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
        if (turnOrder[currentTurnIndex] != null)
        {
            var currentMovement = turnOrder[currentTurnIndex].GetComponent<Movement>();

            if (currentMovement.moved && !currentMovement.turn)
            {
                turnOrder[currentTurnIndex].GetComponent<StatUpdate>().checkFatigue(currentMovement.getTilesFat());
                turnOrder[currentTurnIndex].GetComponent<StatUpdate>().setDamage(0);
                updateTurn(currentMovement);
                Active = false;
            }
            else if (checkFat(turnOrder[currentTurnIndex]))
            {
                turnOrder[currentTurnIndex].GetComponent<Movement>().startTurn();
                Active = true;
                currentPlay = turnOrder[currentTurnIndex];
            }
            else if (checkFat(turnOrder[currentTurnIndex]))
            {
                currentMovement.endTurn();
                turnOrder[currentTurnIndex].GetComponent<StatUpdate>().setDamage(0);
                Active = false;
            }
        }
    }

    void UpdateEnemyTurn()
    {
        if (turnOrder2[currentTurnIndex2] != null)
        {
            var currentMovement = turnOrder2[currentTurnIndex2].GetComponent<Movement>();

            if (currentMovement.moved && !currentMovement.turn)
            {
                turnOrder2[currentTurnIndex2].GetComponent<StatUpdate>().checkFatigue(currentMovement.getTilesFat());
                turnOrder2[currentTurnIndex2].GetComponent<StatUpdate>().setDamage(0);
                currentMovement.setPath = false;
                updateTurn(currentMovement);
                Active = false;
            }
            else if (checkFat(turnOrder2[currentTurnIndex2]))
            {
                turnOrder2[currentTurnIndex2].GetComponent<Movement>().startTurn();
                currentPlay = turnOrder2[currentTurnIndex2];
                Active = true;
            }
            else if (checkFat(turnOrder2[currentTurnIndex2]))
            {
                currentMovement.endTurn();
                turnOrder2[currentTurnIndex2].GetComponent<StatUpdate>().setDamage(0);
                Active = false;
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

    private void updateTurn(Movement currentMovement)
    {
        turnElasped++;
        if(turnElasped >= currentPlay.GetComponent<StatUpdate>().getDictStats("attack_num")){
            updateIndex();
            turnElasped = 0;
        }
        else{
            currentMovement.resetTurn();    
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
                go.GetComponent<Movement>().resetTurn();
            }
        }
    }

    private void reset2()
    {
        foreach (GameObject go in turnOrder2)
        {
            if (go != null)
            {
                go.GetComponent<Movement>().resetTurn();
            }
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
