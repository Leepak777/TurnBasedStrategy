using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class skills : MonoBehaviour
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
    GridGraph gridGraph;
    Tilemap tilemap;
    Movement movement;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        gridGraph = GameObject.Find("Tilemanager").GetComponent<GridGraph>();
        movement = this.gameObject.GetComponent<Movement>();
        
    }

    void checkSkills(){
        if(leadershipAura){
            checkLeaderShip();
        }
        if(intercept){
            this.gameObject.GetComponent<StatUpdate>().setbuff(3,true);
            checkIntercept();
        }
        if(actionSurge){
            this.gameObject.GetComponent<StatUpdate>().setbuff(1,true);
        }
        if(splash){
            this.gameObject.GetComponent<StatUpdate>().setbuff(10,true);
        }
        if(charge){
            this.gameObject.GetComponent<StatUpdate>().setbuff(7,true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //movement = this.gameObject.GetComponent<Movement>();
        /*if(movement.getTurn()){
            checkSkills();
        }*/
    }

    void checkLeaderShip(){
            foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player")){
                Vector3Int targetPos = tilemap.WorldToCell(g.transform.position);
                if(movement.inArea(movement.getOrigin(),targetPos,movement.tilescheck) ){
                    if(!g.GetComponent<StatUpdate>().getbuff(0)){
                        g.GetComponent<StatUpdate>().setbuff(0,true);
                        g.GetComponent<StatUpdate>().Damage +=1;
                    }
                }
                else{
                    if(g.GetComponent<StatUpdate>().getbuff(0)){
                        g.GetComponent<StatUpdate>().setbuff(0,false);
                        g.GetComponent<StatUpdate>().Damage -= 1;
                    }
                }
            }
        
    }

    public void checkIntercept(){
            foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy")){
                Vector3Int targetPos = tilemap.WorldToCell(g.transform.position);
                if(movement.inArea(movement.getOrigin(),targetPos, movement.tilescheck) ){
                    if(!g.GetComponent<StatUpdate>().flag){
                        g.GetComponent<StatUpdate>().flag = true;
                    }
                }
                else{
                    if(g.GetComponent<StatUpdate>().flag){
                        g.GetComponent<StatUpdate>().flag = false;
                    }
                }
            }
        
    }
}
