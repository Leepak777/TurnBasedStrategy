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
    int gamestate = 0;
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
        currentPlay = turnOrder[currentTurnIndex];
        //reset2();
    }

    void Update()
    {
        /*if (player)
        {
            UpdatePlayerTurn();
        }
        else
        {
            UpdateEnemyTurn();
        }*/
        ActionCenter ac = currentPlay.GetComponent<ActionCenter>();
        switch(gamestate){
            case 0://start
                ac.inovkeEvent(0);
                gamestate = 2;
                break;
            case 1://end
                ac.inovkeEvent(1);
                currentPlay.GetComponent<StatUpdate>().checkFatigue(ac.getTilesFat());
                currentPlay.GetComponent<StatUpdate>().setDamage(0);
                updateTurn(ac);
                Active = false;
                gamestate = 0;
                break;
            case 2://during
                ac.inovkeEvent(2);
                break;
        }
        

    }

    /*void UpdatePlayerTurn()
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
                turn = true;
            }
            else if (checkFat(turnOrder[currentTurnIndex]) && currentPlay != turnOrder[currentTurnIndex])
            {
                Active = true;
                currentPlay = turnOrder[currentTurnIndex];
                currentPlay.GetComponent<ActionCenter>().inovkeEvent(0);

            }
            else if (checkFat(turnOrder[currentTurnIndex]))
            {
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
        if(turnOrder.Count == 0 || turnOrder2.Count == 0){
           #if UNITY_EDITOR
           UnityEditor.EditorApplication.isPlaying = false;
           #elif UNITY_WEBPLAYER
           Application.OpenURL(webplayerQuitURL);
           #else
           Application.Quit();
           #endif
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
                turn = true;
            }
            else if (checkFat(turnOrder2[currentTurnIndex2]) && currentPlay != turnOrder2[currentTurnIndex2])
            {
                currentPlay = turnOrder2[currentTurnIndex2];
                currentPlay.GetComponent<ActionCenter>().inovkeEvent(0);
                Active = true;
            }
            else if (!checkFat(turnOrder2[currentTurnIndex2]))
            {
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
        if(turnOrder.Count == 0 || turnOrder2.Count == 0){
           #if UNITY_EDITOR
           UnityEditor.EditorApplication.isPlaying = false;
           #elif UNITY_WEBPLAYER
           Application.OpenURL(webplayerQuitURL);
           #else
           Application.Quit();
           #endif
        }
    }*/

    bool checkFat(GameObject go){
        return go.GetComponent<StatUpdate>().getDictStats("fat") < 100;
    }


    private void updateTurn(ActionCenter ac)
    {
        turnElasped++;
        if(turnElasped >= currentPlay.GetComponent<StatUpdate>().getDictStats("attack_num")){
            updateIndex();
            turnElasped = 0;
        }
        else{
            gamestate = 0;    
        }
    }

    private void updateIndex()
    {
        if (this.player)
        {
            currentTurnIndex++;
            if (currentTurnIndex >= turnOrder.Count)
            {
                currentTurnIndex = 0;
                 
            }
            
            this.player = false;
            currentPlay = turnOrder2[currentTurnIndex2];// switch to enemy's turn
        }
        else
        {
            currentTurnIndex2++;
            if (currentTurnIndex2 >= turnOrder2.Count)
            {
                currentTurnIndex2 = 0;
                
            }
            this.player = true; 
            currentPlay = turnOrder[currentTurnIndex];// switch to player's turn
        }
    }

    public void removefromLst(GameObject go){
        if(go.tag == "Player"){
            turnOrder.Remove(go);
        }
        if(go.tag == "Enemy"){
            turnOrder2.Remove(go);
        }
        if(turnOrder.Count == 0 || turnOrder2.Count == 0){
           #if UNITY_EDITOR
           UnityEditor.EditorApplication.isPlaying = false;
           #elif UNITY_WEBPLAYER
           Application.OpenURL(webplayerQuitURL);
           #else
           Application.Quit();
           #endif
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
    public void setGameState(int gamestate){
        this.gamestate = gamestate;
    }
}
