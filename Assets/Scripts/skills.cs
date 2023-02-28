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
    TileManager tileM;
    Tilemap tilemap;
    Movement movement;
    StatUpdate statupdate;
    TurnManager TM;
    bool gameTurn = true;
    bool characterTurn = false;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        movement = this.gameObject.GetComponent<Movement>();
        statupdate = this.gameObject.GetComponent<StatUpdate>();
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
        string tag = "Enemy";
        if(this.gameObject.tag == "Enemy")
        {
            tag = "Player";
        }
        foreach(GameObject go in tileM.getTaginArea(movement.getOrigin(),statupdate.getDictStats("mov"),tag)){
            StatUpdate checker = go.GetComponent<StatUpdate>();
            checker.Flagging();
            movement.AttackCheck(tag);
        }
    }

    void checkBloodLust(int x){
        StatUpdate checker = this.gameObject.GetComponent<StatUpdate>();
        if(x == 0){
            //during turn
            if(movement.getTargetEnemy() == null){
                statupdate.setbuff(14,true);
                checker.modifyStat(new Dictionary<string,float>(){{"attack_num",1}}, true);
            }
        }
        if(x == 1){
            //start turn
            if(TM.getTurnElasped() == 1){
                checker.multiplyStat(new Dictionary<string,float>(){{"mov",2}}, false);
            }
        }
        if(x == 2){
            //end turn
            if(checker.getbuff(14) && TM.getTurnElasped() == 0){            
                checker.modifyStat(new Dictionary<string,float>(){{"attack_num",1}}, false);
                checker.multiplyStat(new Dictionary<string,float>(){{"mov",2}}, true);
                checker.setbuff(14,false);
            }
        }
    }

    void checkSplash(int x){
        if(x == 1){
            //during turn
            movement.setAttackArea(1);
        }
        if(x == 2){
           movement.setAttackArea(0);
        }
    }

    void checkIntercept(){
        opportunity();
    }
    void checkCharge(int x){
        if(x == 0){
            //during turn
            if(!movement.getisMoving() && movement.getTilesFat() > 0 && !statupdate.getbuff(7)){
                statupdate.setbuff(7,true);
                statupdate.setBonus(movement.getTilesFat());
                charge_bonus = movement.getTilesFat();
            }
            if(movement.getisMoving() && movement.getTilesFat() > 0 && statupdate.getbuff(7)){
                statupdate.setbuff(7,false);
                statupdate.setBonus(-movement.getTilesFat());
                charge_bonus = 0;
            }
        }
        if(x == 2){
            //end turn
            if(statupdate.getbuff(7) && charge_bonus > 0){
                statupdate.setbuff(7,false);
                statupdate.setBonus(-charge_bonus);
                charge_bonus = 0;
            }
        }
    }
    void checkLeaderShipAura(){
        string tag = "Enemy";
        if(this.gameObject.tag == "Player")
        {
            tag = "Player";
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag(tag)){
                StatUpdate checker = go.GetComponent<StatUpdate>();
                if(tileM.inArea(movement.getOrigin(), tilemap.WorldToCell(go.transform.position),3)){
                    if(!checker.getbuff(0)){
                        checker.setbuff(0,true);
                        checker.setBonus(1);
                    }
                }
                else{
                    if(checker.getbuff(0)){
                        checker.setbuff(0,false);
                        checker.setBonus(-1);
                    }
                }
        }
    }
    void checkActionSurge(int startEnd){
        StatUpdate checker = this.gameObject.GetComponent<StatUpdate>();
        if (startEnd == 1){
            if(as_turn == 0){
                checker.setbuff(1,true);
                checker.multiplyStat(new Dictionary<string,float>(){{"attack_num",2}}, true);
                
            }
        }
        else if (startEnd == 2){
            if(checker.getbuff(1) && TM.getTurnElasped() == 0){            
                checker.multiplyStat(new Dictionary<string,float>(){{"attack_num",2}}, false);
                checker.setbuff(1,false);
            }

            as_turn++;
            if(as_turn >=5){
                as_turn = 0;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //check when a turn start
        if(TM.getActive() && gameTurn){
            //check when character turn start
            if(TM.getCurrenPlay() == this.gameObject){
                checkChStartTurnSkills();
                characterTurn = true;
            }    
            //overall startturn check
            checkGameStartTurnSkills();
            gameTurn = false;
        }else if(!TM.getActive()){
            gameTurn = true;
            //character end turn check
            if(TM.getCurrenPlay() == this.gameObject){
                checkChEndTurnSkills();
                characterTurn = false;
            }
            //overall endturn check
            checkGameEndTurnSkills();
        }
        //check during character turn
        if(characterTurn){
            checkDuringTurnSkills();
        }
    }


}

