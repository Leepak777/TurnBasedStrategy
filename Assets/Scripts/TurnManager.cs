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
    int gamestate = 0;
    public GameObject currentPlay;
    private int turnElasped;
    private int gameTurn = 1;
    public UnityEvent start;
    public UnityEvent end;
    public UnityEvent during;
    public UnityEvent undo;
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
        switch(gamestate){
            case 0://start
                start.Invoke();
                break;
            case 1://end
                end.Invoke();
                break;
            case 2://during
                during.Invoke();
                break;
        }
    }
    public void startEvent(){
        //To-DO: Added skill check for skills that update each game turn
        currentPlay.GetComponentInChildren<CharacterEvents>().onStart.Invoke();
        if(currentPlay.tag == "Player"){        
            startTurnSavePlayer();
            startTurnSaveEnemy();
        }
        gamestate = 2;
    }
    public void endEvent(){
        //To-DO: Added skill check for skills that update each game turn
        currentPlay.GetComponentInChildren<CharacterEvents>().onEnd.Invoke(0);
        gamestate = 0;
        if(currentPlay.tag == "Player"){
            gameTurn++;
        }
    }
    public void duringEvent(){
        if(currentPlay.tag == "Enemy"){
            Invoke("duringAction",1.5f);
        }
        else{
            duringAction();
        }
    }
    public void duringAction(){
        currentPlay.GetComponentInChildren<CharacterEvents>().onDuring.Invoke();
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
            currentPlay = turnOrder2[currentTurnIndex2];// switch to enemy's turn
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
            currentPlay = turnOrder[currentTurnIndex];// switch to player's turn
        }
    }
    
    public void currentPlayAttack(){
        currentPlay.GetComponentInChildren<CharacterEvents>().onSetAttack.Invoke();
    }
    public void currentPlayHighlighten(){
        if(currentPlay.GetComponent<ActionCenter>().isAttacking()){
            currentPlay.GetComponentInChildren<CharacterEvents>().onHighLight.Invoke();
        }
        else{
        currentPlay.GetComponentInChildren<CharacterEvents>().onUnHighLight.Invoke();
        }
    }
    public void currentPlayClick(){
        currentPlay.GetComponentInChildren<CharacterEvents>().onClick.Invoke();
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
    public GameObject getCurrenPlay(){
        return currentPlay;
    }
    public void setGameState(int gamestate){
        this.gamestate = gamestate;
    }
    public int getGameTurn(){
        return gameTurn;
    }
    public void endTurn(){
        gamestate = 1;
    }
}
