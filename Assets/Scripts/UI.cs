
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;
using Random = System.Random;
using UnityEditor;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    public GameObject currentPlay;
    public UnityEvent start;
    public UnityEvent end;
    public bool skip = false;
    public UnityEvent move;
    public UnityEvent stop;
    public TurnManager tm;
    public AbilitiesData ad; 
    public bool foresight = false;
    public Dropdown skillLst;
    public bool Casting = false;
    public int attackTime = 0;
    public int preattack = 0;
    public StatUpdate statupdate;

    void Update()
    {
        switch(tm.getState()){
            case 0://start
                start.Invoke();
                break;
            case 1://end
                end.Invoke();
                break;
            case 2://move
                move.Invoke();
                break;
            case 3://stop
                stop.Invoke();
                break;
        }
    }
    public void startEvent(){
        //To-DO: Added skill check for skills that update each game turn
        skillLst.ClearOptions();
        skillLst.AddOptions(currentPlay.GetComponent<Abilities>().getSkillNames());
        currentPlay.GetComponentInChildren<CharacterEvents>().onStart.Invoke();
        statupdate = currentPlay.GetComponent<StatUpdate>();
        attackTime = 0;
        /*if(currentPlay.tag == "Player"){        
            tm.PlayerBackUP();
            tm.EnemyBackUP();
        }*/
        tm.EndEffectDurationCheck();
        areaEffectCheck();
        tm.setGameState(2);
    }
    public void ActivateSkill(int choice){
        currentPlay.GetComponent<Abilities>().ActiveSkillCheck(choice);
    }
    public void CastSkill(){
        currentPlay.GetComponent<Abilities>().SpellCast();
    }
    public void endEvent(){
        //To-DO: Added skill check for skills that update each game turn
        if(!foresight){
            currentPlay.GetComponentInChildren<CharacterEvents>().onEnd.Invoke(0);
            tm.setGameState(0);
            if(currentPlay.tag == "Player"){
                tm.incGameTurn();
            }
        }
        tm.BubbleCheck();
        tm.EndEffectDurationCheck();
    }
    public void areaEffectCheck(){
        List<Node> lst = GameObject.Find("Tilemanager").GetComponent<TileManager>().getEffectlst();
        for(int i = 0; i < lst.Count; i++){
            UDictionary<string,KeyValuePair<GameObject,int>> effectlst = lst[i].effectFlag;
            Debug.Log(lst[i]);
            for(int j = 0; j < effectlst.Count; j++){
                lst[i].decEffectDuration(effectlst.ElementAt(j).Key);
                ad.getAreaEffect(effectlst.ElementAt(j).Key).Invoke(effectlst.ElementAt(j).Value.Key, new Vector3Int(lst[j].gridX,lst[j].gridY,lst[j].gridX));
                
            }
        }
    }
    public void duringEvent(){
        if(currentPlay.tag == "Enemy" && !skip){
            Invoke("duringAction",1.5f);
            skip = true;
        }
        else if (currentPlay.tag != "Enemy"){
            duringAction();
        }
        
    }
    public void duringAction(){
        currentPlay.GetComponentInChildren<CharacterEvents>().onDuring.Invoke();
        skip = false;
        
    }
    public void stopEvent(){
        if(currentPlay.tag == "Enemy"){
            if(attackTime < statupdate.getDictStats("attack_num")){
                currentPlay.GetComponentInChildren<CharacterEvents>().onEnemyAttack.Invoke();
                attackTime++;
                tm.setGameState(2);
            }
            else{
                tm.setGameState(1);
            }
        }
        
    }
    public void EndForeSight(){
        ad.ForeSight_e.Invoke(currentPlay, Vector3Int.zero);
    }
    public bool inForesight(){
        return foresight;
    }
    public void setForesight(bool activate){
        foresight = activate;
        if(!foresight){
            tm.setGameState(2);
            currentPlay.GetComponentInChildren<CharacterEvents>().unHighLightRechable.Invoke();
            currentPlay.GetComponentInChildren<CharacterEvents>().HighLightReachable.Invoke();
            attackTime = preattack;
        }
    }
    public void setCurrentPlay(GameObject go){
        this.currentPlay = go;
    }

    public void currentPlayAttack(){
        if(attackTime < statupdate.getDictStats("attack_num")){
            currentPlay.GetComponentInChildren<CharacterEvents>().onSetAttack.Invoke();
            attackTime++;
        }
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
    public GameObject getCurrentPlay(){
        return currentPlay;
    }
}
