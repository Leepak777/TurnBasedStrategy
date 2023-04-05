using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;



public class TurnManager : MonoBehaviour
{
    public List<GameObject> turnOrder;
    public List<GameObject> turnOrder2;
    private int currentTurnIndex;
    private int currentTurnIndex2;
    public bool player = true;
    bool Active = false;
    public int gamestate = 0;
    private int turnElasped;
    private int gameTurn = 1;
    public UI ui;
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
        ui.setCurrentPlay(turnOrder[currentTurnIndex]);
        //currentPlay = turnOrder[currentTurnIndex];
        //reset2();
    }

    
    public void updateIndex()
    {
        if (this.player)
        {
            currentTurnIndex++;
            if (currentTurnIndex >= turnOrder.Count)
            {
                currentTurnIndex = 0;
                 
            }
            while(!turnOrder[currentTurnIndex].activeInHierarchy){
                currentTurnIndex++;
                if(currentTurnIndex>=turnOrder.Count){
                    currentTurnIndex =0;
                }
            }
            this.player = false;
            ui.setCurrentPlay(turnOrder2[currentTurnIndex2]);
        }
        else
        {
            currentTurnIndex2++;
            if (currentTurnIndex2 >= turnOrder2.Count)
            {
                currentTurnIndex2 = 0;
                
            }
            while(!turnOrder2[currentTurnIndex2].activeInHierarchy){
                currentTurnIndex2++;
                if(currentTurnIndex2>=turnOrder2.Count){
                    currentTurnIndex2 =0;
                }
            }
            this.player = true; 
            ui.setCurrentPlay(turnOrder[currentTurnIndex]);
        }
    }
    public void startTurnSavePlayer(){
        foreach(GameObject go in turnOrder){
            go.GetComponentInChildren<CharacterEvents>().saveStat.Invoke(gameTurn);
        }
    }
    public void startTurnSaveEnemy(){
        foreach(GameObject go in turnOrder2){
            go.GetComponentInChildren<CharacterEvents>().saveStat.Invoke(gameTurn);
        }  
    }
    public void gameEndCheck(){
        bool PlayerDied = true;
        bool EnemyDied = true;
        foreach(GameObject go in turnOrder){
            if(go.activeInHierarchy){
                PlayerDied = false;
            }
        }
        foreach(GameObject go in turnOrder2){
            if(go.activeInHierarchy){
                EnemyDied = false;
            }
        }
        if(PlayerDied || EnemyDied){
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
    public int getState(){
        return gamestate;
    }
    public void setGameState(int gamestate){
        this.gamestate = gamestate;
    }
    public int getGameTurn(){
        return gameTurn;
    }
    public void incGameTurn(){
        gameTurn++;
    }
    public void endTurn(){
        gamestate = 1;
    }
}
