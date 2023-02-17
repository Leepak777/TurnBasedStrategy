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
    public GameObject currentPlay;
    private void Start()
    {
        // Initialize the turn order list and add the game objects to it
        turnOrder = new List<GameObject>();
        turnOrder2 = new List<GameObject>();
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player")){
            turnOrder.Add(g);
        }
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy")){
            turnOrder2.Add(g);
        }
        
        currentTurnIndex = 0;
        currentTurnIndex2 = 0;
        turnOrder[currentTurnIndex].GetComponent<Movement>().turn = true;
        turnOrder[currentTurnIndex].GetComponent<Movement>().moved = false;
    }

    private void Update()
    { 
        if(player){
            checkdead();
            
            if(turnOrder[currentTurnIndex] != null && turnOrder[currentTurnIndex].GetComponent<Movement>().moved && !turnOrder[currentTurnIndex].GetComponent<Movement>().turn){
                turnOrder[currentTurnIndex].GetComponent<StatUpdate>().tileFatigue(turnOrder[currentTurnIndex].GetComponent<Movement>().tilesfat);
                turnOrder[currentTurnIndex].GetComponent<Movement>().tilesfat = 0;
                turnOrder[currentTurnIndex].GetComponent<StatUpdate>().restoreFatigue();
                turnOrder[currentTurnIndex].GetComponent<StatUpdate>().checkFatigue();
                if(turnOrder[currentTurnIndex].GetComponent<StatUpdate>().getbuff(1)){
                    if(turnOrder[currentTurnIndex].GetComponent<skills>().as_turn == 0){
                        turnOrder[currentTurnIndex].GetComponent<Movement>().origin = false;
                        turnOrder[currentTurnIndex].GetComponent<skills>().as_turn++;
                    }
                    else{
                        currentTurnIndex++;
                        checkdead();
                        player = false;
                        turnOrder[currentTurnIndex].GetComponent<skills>().as_turn++;
                        if(turnOrder[currentTurnIndex].GetComponent<skills>().as_turn >=6){
                            turnOrder[currentTurnIndex].GetComponent<skills>().as_turn = 0;
                        }
                    }
                }
                else if( turnOrder[currentTurnIndex] != null && turnOrder[currentTurnIndex].GetComponent<StatUpdate>().getbuff(14)){
                    if(turnOrder[currentTurnIndex].GetComponent<skills>().bl_turn == 0){
                        turnOrder[currentTurnIndex].GetComponent<Movement>().origin = false;
                        turnOrder[currentTurnIndex].GetComponent<skills>().bl_turn++;
                    }
                    else{
                        turnOrder[currentTurnIndex].GetComponent<StatUpdate>().setbuff(14,false);
                        currentTurnIndex++;
                        player = false;
                        turnOrder[currentTurnIndex].GetComponent<skills>().bl_turn++;
                        if(turnOrder[currentTurnIndex].GetComponent<skills>().bl_turn >=1){
                            turnOrder[currentTurnIndex].GetComponent<skills>().bl_turn = 0;
                        }
                    }
                   
                }
                else {
                    currentTurnIndex++;
                    checkdead();
                    player = false;

                }
               
            }
            if(currentTurnIndex >= turnOrder.Count){
                currentTurnIndex = 0;
                reset1();
            }
            if(player && turnOrder[currentTurnIndex].GetComponent<StatUpdate>().fat < 100){
                turnOrder[currentTurnIndex].GetComponent<Movement>().turn = true;
                turnOrder[currentTurnIndex].GetComponent<Movement>().moved = false;
                currentPlay = turnOrder[currentTurnIndex];
            }
            else if(turnOrder[currentTurnIndex].GetComponent<StatUpdate>().fat >= 100){
                turnOrder[currentTurnIndex].GetComponent<StatUpdate>().tileFatigue(turnOrder[currentTurnIndex].GetComponent<Movement>().tilesfat);
                turnOrder[currentTurnIndex].GetComponent<Movement>().tilesfat = 0;
                turnOrder[currentTurnIndex].GetComponent<StatUpdate>().restoreFatigue();
                turnOrder[currentTurnIndex].GetComponent<StatUpdate>().checkFatigue();
                turnOrder[currentTurnIndex].GetComponent<Movement>().turn = false;
                turnOrder[currentTurnIndex].GetComponent<Movement>().moved = true;
                currentTurnIndex++;
                checkdead();
                player = false;
            }
        }
        else{
            checkdead2(); 
            
            if(turnOrder2[currentTurnIndex] != null &&turnOrder2[currentTurnIndex2].GetComponent<MovementAI>().moved && !turnOrder2[currentTurnIndex2].GetComponent<MovementAI>().turn){
                turnOrder2[currentTurnIndex2].GetComponent<MovementAI>().setPath = false;
                currentTurnIndex2++;
                checkdead2();
                player = true;
                foreach (GameObject g in turnOrder){
                    if(g!=null && g.GetComponent<StatUpdate>().getbuff(3)){
                        g.GetComponent<Movement>().AttackFlaged();
                    }
                }
            }
            if(currentTurnIndex2 >= turnOrder2.Count){
                currentTurnIndex2 = 0;
                reset2();
            }
            if(!player){
                turnOrder2[currentTurnIndex2].GetComponent<MovementAI>().turn = true;
                turnOrder2[currentTurnIndex2].GetComponent<MovementAI>().moved = false;
                
            }
        }
    }

    void checkdead(){
        if(currentTurnIndex >= turnOrder.Count || turnOrder[currentTurnIndex] == null){
                currentTurnIndex++;
                if(currentTurnIndex >= turnOrder.Count){
                    currentTurnIndex = 0;
                    reset1();
                }
            }
    }
    void checkdead2(){
        if(currentTurnIndex2 >= turnOrder2.Count || turnOrder2[currentTurnIndex2] == null){
                currentTurnIndex2++;
                if(currentTurnIndex2 >= turnOrder2.Count){
                    currentTurnIndex2 = 0;
                    reset2();
                }
            } 
    }

    private void reset1(){
        foreach(GameObject go in turnOrder){
            if(go != null){
            go.GetComponent<Movement>().turn = false;
            go.GetComponent<Movement>().moved = false;
            }
        }
    }
    private void reset2(){
        foreach(GameObject go in turnOrder2){
            if(go != null){
            go.GetComponent<MovementAI>().turn = false;
            go.GetComponent<MovementAI>().moved = false;
        }
        }
    }

    
}
