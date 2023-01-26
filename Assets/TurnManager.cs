using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<GameObject> turnOrder;
    private List<GameObject> turnOrder2;
    private int currentTurnIndex;
    private int currentTurnIndex2;
    private bool player = true;
    private void Start()
    {
        // Initialize the turn order list and add the game objects to it
        turnOrder = new List<GameObject>();
        turnOrder2 = new List<GameObject>();
        turnOrder.Add(GameObject.Find("black1"));
        turnOrder.Add(GameObject.Find("black2"));
        turnOrder2.Add(GameObject.Find("white1"));
        turnOrder2.Add(GameObject.Find("white2"));
        currentTurnIndex = 0;
        currentTurnIndex2 = 0;
        turnOrder[currentTurnIndex].GetComponent<Movement>().turn = true;
        turnOrder[currentTurnIndex].GetComponent<Movement>().moved = false;
    }

    private void Update()
    { 
        if(player){
            if(turnOrder[currentTurnIndex].GetComponent<Movement>().moved && !turnOrder[currentTurnIndex].GetComponent<Movement>().turn){
                currentTurnIndex++;
                player = false;
            }
            if(currentTurnIndex >= turnOrder.Count){
                currentTurnIndex = 0;
                reset1();
            }
            if(player){
                turnOrder[currentTurnIndex].GetComponent<Movement>().turn = true;
                turnOrder[currentTurnIndex].GetComponent<Movement>().moved = false;
            }
        }
        else{
            if(turnOrder2[currentTurnIndex2].GetComponent<MovementAI>().moved && !turnOrder2[currentTurnIndex2].GetComponent<MovementAI>().turn){
                turnOrder2[currentTurnIndex2].GetComponent<MovementAI>().setPath = false;
                currentTurnIndex2++;
                player = true;
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
    private void reset1(){
        foreach(GameObject go in turnOrder){
            go.GetComponent<Movement>().turn = false;
            go.GetComponent<Movement>().moved = false;
        }
    }
    private void reset2(){
        foreach(GameObject go in turnOrder2){
            go.GetComponent<MovementAI>().turn = false;
            go.GetComponent<MovementAI>().moved = false;
        }
    }

    
}
