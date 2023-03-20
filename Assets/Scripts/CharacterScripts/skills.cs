using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class Skills : MonoBehaviour
{    
    public bool leadershipAura;         //0
    public bool actionSurge;            //1
    public bool inspiringPresence;      //2
    public bool intercept;              //3
    public bool protect;                //4
    public bool closeFormation;         //5
    public bool aiming;                 //6
    public bool charge;                 //7
    public bool disengage;              //8
    public bool rabble;                 //9
    public bool splash;                 //10
    public bool cleave;                 //11
    public bool regenerate;             //12
    public bool packTatic;              //13
    public bool bloodlust;              //14
    public bool overwhelm;              //15
    public bool crush;                  //16
    public bool agileForm;              //17
    public bool defensiveMatrix;        //18
    
    public int bl_turn = 0;
    public int as_turn = 0;
    public int charge_bonus = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void checkChStartTurnSkills(){
        if(splash){
            checkSplash(1);
        }
        if(actionSurge){
            checkActionSurge(1);
        }
        if(bloodlust){
            checkBloodLust(1);
        }

    }
    void checkChEndTurnSkills(){
        if(actionSurge){
            checkActionSurge(2);
        }
        if(bloodlust){
            checkBloodLust(2);
        }
        if(splash){
            checkSplash(2);
        }
    }
    void checkGameStartTurnSkills(){
        if(leadershipAura){
            checkLeaderShipAura();
        }
    }

    void checkGameEndTurnSkills(){
        if(intercept){
            checkIntercept();
        }
    }
    void checkDuringTurnSkills(){
        if(bloodlust){
            checkBloodLust(0);
        }
        if(charge){
            checkCharge(0);
        }
    }

    void opportunity(){
        
    }

    void checkBloodLust(int x){
       
    }

    void checkSplash(int x){
        
    }

    void checkIntercept(){
    }
    void checkCharge(int x){
        
    }
    void checkLeaderShipAura(){
        
    }
    void checkActionSurge(int startEnd){
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

