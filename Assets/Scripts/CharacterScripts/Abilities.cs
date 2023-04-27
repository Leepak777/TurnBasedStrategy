using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;
using System.IO;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Abilities : MonoBehaviour
{
    TileManager tileM;
    public List<UnityEvent<GameObject>> UniversalCheck = new List<UnityEvent<GameObject>>();
    public List<UnityEvent<GameObject>> IndividualCheck = new List<UnityEvent<GameObject>>();
    public List<UnityEvent<GameObject,Vector3Int>> activeSkill = new List<UnityEvent<GameObject,Vector3Int>>();
    public List<string> Skillname = new List<string>();
    UDictionary<string,int> CoolDown = new UDictionary<string, int>();
    CharacterStat stats;
    AbilitiesData abilitiesData;
    int choice = 0;
    int radius = 0;
    int CastRange = 0;
    Vector3Int targetLoc;
    public void GameCheck(){
        foreach(UnityEvent<GameObject> e in UniversalCheck){
            e.Invoke(gameObject);
        }
    }
    public void CharacterCheck(){
        foreach(UnityEvent<GameObject> e in IndividualCheck){
            e.Invoke(gameObject);
        }
    }
    void Start()
    {
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        stats = AssetDatabase.LoadAssetAtPath<CharacterStat>("Assets/Scripts/Data/"+gameObject.name+".asset");
        abilitiesData = AssetDatabase.LoadAssetAtPath<AbilitiesData>("Assets/Scripts/Data/AbilitiesData.asset");
        foreach(KeyValuePair<string,string> ability in stats.getAbilities()){
            UnityEvent<GameObject> e = abilitiesData.getEvent(ability.Key);
            switch(ability.Value){
                case"Universal":
                    UniversalCheck.Add(e);
                    break;
                case "Individual":
                    IndividualCheck.Add(e);
                    break;
            }
        }
        Skillname.Add("Skill");
        Skillname = stats.getSkills();
        activeSkill.Add(new UnityEvent<GameObject,Vector3Int>());
        foreach(string name in stats.getSkills()){
            activeSkill.Add(abilitiesData.getActiveSkill(name));
        }
    }

    public List<string> getSkillNames(){
        return Skillname;
    }

    public void ActiveSkillCheck(int choice){
        this.choice = choice;
        switch(Skillname[choice]){
            case "ForceBlast":
                CheckCoolDown(Skillname[choice]);break;
            case "PsychicStorm":
                CheckCoolDown(Skillname[choice]);break;
            case "ForeSight":ActivateSkill(Skillname[choice]);break;
            case "WhirlWind":ActivateSkill(Skillname[choice]);break;
            case "WaterStance":ActivateSkill(Skillname[choice]);break;
        }
    }
    public void CheckCoolDown(string name){
        switch(name){
            case "ForceBlast":
                if(!CoolDown.ContainsKey("ForceBlast")){
                    radius = 3; 
                    CastRange = 3 + (int) stats.getStat("acu")/4; 
                    TargetingSkill();
                    CoolDown.Add(name,5);    
                }
                else{
                    Debug.Log("In CoolDown");
                }
            break;
            case "PsychicStorm":
                if(!CoolDown.ContainsKey("PsychicStorm")){
                    radius = 3; 
                    CastRange = 5 + (int) stats.getStat("acu")/4; 
                    TargetingSkill();
                    CoolDown.Add(name,7);    
                }
                else{
                    Debug.Log("In CoolDown");
                }
            break;
        }
    }
    public void addToCooldDown(string name,int turns){
        CoolDown.Add(name,turns);
    }
    public void DecCoolDown(){
        for(int i = 0; i < CoolDown.Count; i++){
            CoolDown[CoolDown.ElementAt(i).Key]--;
            if(CoolDown.ElementAt(i).Value <= 0){
                CoolDown.Remove(CoolDown.ElementAt(i));
            }
        }
    }
    public void TargetingSkill(){
        gameObject.GetComponent<ActionCenter>().setCasting(true);
    }
    public void ActivateSkill(string name){
        if(!CoolDown.ContainsKey(name)){
            activeSkill[choice].Invoke(gameObject, Vector3Int.zero);
        }
    }

    public void targeting(Vector3Int loc){
        Vector3Int playLoc = tileM.WorldToCell(transform.position);
        if(tileM.GetDistance(playLoc,loc) <= CastRange){
            targetLoc = loc;
            gameObject.GetComponentInChildren<CharacterEvents>().unHighLightArea.Invoke();
            gameObject.GetComponentInChildren<CharacterEvents>().HighLightReachable.Invoke();
            gameObject.GetComponentInChildren<CharacterEvents>().HighLightArea.Invoke(loc,radius);
        }
        else{
            gameObject.GetComponentInChildren<CharacterEvents>().unHighLightArea.Invoke();
            gameObject.GetComponentInChildren<CharacterEvents>().HighLightReachable.Invoke();
        }
    }
    public void SpellCast(){
        if(gameObject.GetComponent<ActionCenter>().isCasting()){
            gameObject.GetComponentInChildren<CharacterEvents>().unHighLightArea.Invoke();
            gameObject.GetComponentInChildren<CharacterEvents>().HighLightReachable.Invoke();
            activeSkill[choice].Invoke(gameObject, targetLoc);
            gameObject.GetComponent<ActionCenter>().setCasting(false);
        }
    }
}
