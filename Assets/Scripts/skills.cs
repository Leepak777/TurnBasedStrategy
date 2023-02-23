using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class skills : MonoBehaviour
{    
    public bool leadershipAura= true;         //0
    public bool actionSurge = true;            //1
    public bool inspiringPresence;      //2
    public bool intercept= true;              //3
    public bool protect;                //4
    public bool closeFormation;         //5
    public bool aiming;                 //6
    public bool charge;                 //7
    public bool disengage;              //8
    public bool rabble;                 //9
    public bool splash= true;                 //10
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
    GridGraph gridGraph;
    Tilemap tilemap;
    Movement movement;
    StatUpdate statupdate;
    TurnManager TM;
    bool gameTurn = true;
    bool characterTurn = false;
    bool characterTurn2 = false;
    Dictionary<string,float> leaderShipBuff;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        gridGraph = GameObject.Find("Tilemanager").GetComponent<GridGraph>();
        TM = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        movement = this.gameObject.GetComponent<Movement>();
        statupdate = this.gameObject.GetComponent<StatUpdate>();
        leaderShipBuff = new Dictionary<string,float>(){{"wd",1},{"pow",1},{"dex",1}};
    }

    void checkChTurnSkills(){
        if(leadershipAura){
            checkLeaderShipAura();
        }
        if(actionSurge){
            checkActionSurge();
        }

    }
    void checkGameTurnSkills(){
        if(intercept){
            checkIntercept();
        }
    }
    void checkDuringTurnSkills(){
        if(splash){
            checkSplash();
        }
    }

    void checkSplash(){
        if(movement.getTargetEnemy() != null){
            foreach(Node n in gridGraph.GetTilesInArea(tilemap.WorldToCell(movement.getTargetEnemy().transform.position),1)){
                if(n.occupant != null&& n.occupant.tag == movement.getTargetEnemy().tag){
                    n.occupant.GetComponent<StatUpdate>().flag = true;
                }
            }
        }
    }

    void opportunity(){
        string tag = "Enemy";
        if(this.gameObject.tag == "Enemy")
        {
            tag = "Player";
        }
        foreach(GameObject go in movement.getTaginArea(movement.getOrigin(),statupdate.getDictStats("mov"),tag)){
            StatUpdate checker = go.GetComponent<StatUpdate>();
            checker.Flagging();
            movement.AttackCheck(tag);
        }
    }

    void checkIntercept(){
        opportunity();
    }
    void checkCharge(){

    }
    void checkLeaderShipAura(){
        string tag = "Enemy";
        if(this.gameObject.tag == "Player")
        {
            tag = "Player";
        }
        foreach(GameObject go in movement.getTaginArea(movement.getOrigin(),3,tag)){
            StatUpdate checker = go.GetComponent<StatUpdate>();
                if(!checker.getbuff(0)){
                    checker.setbuff(0,true);
                    checker.modifyStat(leaderShipBuff,true);
                }
                if(checker.getbuff(0)){
                    checker.setbuff(0,false);
                    checker.modifyStat(leaderShipBuff,false);
                }
        }
    }
    void checkActionSurge(){
        StatUpdate checker = this.gameObject.GetComponent<StatUpdate>();
        if(TM.getTurnElasped() == 1){
            checker.multiplyStat(new Dictionary<string,float>(){{"mov",2}}, false);
        }
        if(as_turn == 0){
            checker.setbuff(1,true);
            checker.multiplyStat(new Dictionary<string,float>(){{"attack_num",2}}, true);
            
        }
        else if (as_turn == 2){
            if(checker.getbuff(1)){            
                checker.multiplyStat(new Dictionary<string,float>(){{"attack_num",2}}, false);
                checker.multiplyStat(new Dictionary<string,float>(){{"mov",2}}, true);
                checker.setbuff(1,false);
            }
        }
        as_turn++;
        if(as_turn >=5){
            as_turn = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(characterTurn2){
            checkDuringTurnSkills();
        }
        if(characterTurn){
            checkChTurnSkills();
            characterTurn = false;
        }
        if(TM.getActive() && gameTurn){
            checkGameTurnSkills();
            gameTurn = false;
            //Debug.Log("pog");
        }else if(!TM.getActive()){
            gameTurn = true;
        }
    }

    public void setUpdate(bool set){
        characterTurn = set;
        characterTurn2 = set;
    }

    public void setChTurn2(bool set){
        characterTurn2 = set;
    }

}
