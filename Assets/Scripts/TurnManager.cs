using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class startTurnEvent : UnityEvent{

}

public class endTurnEvent : UnityEvent{
    
}

public class duringTurnEvent : UnityEvent{
    
}
public class undoEvent :UnityEvent{

}

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
    startTurnEvent start;
    endTurnEvent end;
    duringTurnEvent during;
    undoEvent undo;
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
        start = new startTurnEvent();
        end = new endTurnEvent();
        during = new duringTurnEvent();
        undo = new undoEvent();
        start.AddListener(startEvent);
        end.AddListener(endEvent);
        during.AddListener(duringEvent);
        undo.AddListener(undoTurn);
        //reset2();
    }

    void Update()
    {
        ActionCenter ac = currentPlay.GetComponent<ActionCenter>();
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
    void startEvent(){
        //To-DO: Added skill check for skills that update each game turn
        currentPlay.GetComponent<ActionCenter>().beginningTurn();
        if(currentPlay.tag == "Player"){        
            startTurnSavePlayer();
            //startTurnSaveEnemy();
        /*}
        else{*/
            startTurnSaveEnemy();
        }
        GameObject.Find("Main Camera").GetComponent<CameraController>().trackPlayer(currentPlay);  
        gamestate = 2;
    }
    void endEvent(){
        //To-DO: Added skill check for skills that update each game turn
        currentPlay.GetComponent<ActionCenter>().endingTurn(0);
        updateTurn(currentPlay.GetComponent<ActionCenter>());
        gamestate = 0;
        if(currentPlay.tag == "Player"){
            gameTurn++;
        }
    }
    void duringEvent(){
        currentPlay.GetComponent<ActionCenter>().duringTurn();
        if(!currentPlay.GetComponent<Movement>().getisMoving() && currentPlay.tag == "Enemy"){
            endTurn();
        }
    }


    private void updateTurn(ActionCenter ac)
    {
        updateIndex();
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
    public void resetTurn(){
        gamestate = 0;
        currentPlay.GetComponent<ActionCenter>().undoTurn(gameTurn);
         
    }
    public void undoTurn(){
        if(gameTurn > 1){
            gameTurn--;
            gamestate = 0;
            currentPlay.GetComponent<ActionCenter>().endingTurn(1);
            currentTurnIndex2--;
            currentTurnIndex--;
            if(currentTurnIndex<0){currentTurnIndex = turnOrder.Count-1;}
            if(currentTurnIndex2<0){currentTurnIndex2 = turnOrder2.Count-1;}
            currentPlay = turnOrder[currentTurnIndex];
            foreach(GameObject go in turnOrder){
                    go.GetComponent<ActionCenter>().undoTurn(gameTurn);
            }
            foreach(GameObject go in turnOrder2){
                go.GetComponent<ActionCenter>().undoTurn(gameTurn);
            }     
        }
    }
    public void startTurnSavePlayer(){
        foreach(GameObject go in turnOrder){
                go.GetComponent<ActionCenter>().saveTurnStatData(gameTurn);
            }
    }
    public void startTurnSaveEnemy(){
        foreach(GameObject go in turnOrder2){
            go.GetComponent<ActionCenter>().saveTurnStatData(gameTurn);
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
    public UnityEvent getEvent(int i){
        switch(i){
            case 0:
                return start;
            case 1:
                return end;
            case 2:
                return during;
            case 3:
                return undo;
        }
        return null;
    }
}
