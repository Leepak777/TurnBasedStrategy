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
    public UDictionary<string,int> CoolDown = new UDictionary<string, int>();
    UDictionary<KeyValuePair<int,KeyValuePair<GameObject,Vector3Int>>, int> castList = new UDictionary<KeyValuePair<int, KeyValuePair<GameObject, Vector3Int>>, int>();
    UDictionary<string,float> skillAttributesFloat = new UDictionary<string, float>();
    UDictionary<string,bool> skillAttributesBool = new UDictionary<string, bool>();
    CharacterStat stats;
    AbilitiesData abilitiesData;
    public int choice = 0;
    int targets = 0;
    Vector3Int targetLoc;
    List<Vector3Int> targetV = new List<Vector3Int>();
    Dropdown skillDrop;
    UI ui;
    public void GameCheck(){
        foreach(UnityEvent<GameObject> e in UniversalCheck){
            if(e != null){
                e.Invoke(gameObject);
            }
        }
    }
    public void CharacterCheck(){
        foreach(UnityEvent<GameObject> e in IndividualCheck){
            if(e != null){
                e.Invoke(gameObject);
            }
        }
    }
    public void getManagers(){
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
    }
    void Start()
    {
        ui = GameObject.Find("UICanvas").GetComponent<UI>();
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
        Skillname.Add("Skills");
        
        activeSkill.Add(new UnityEvent<GameObject,Vector3Int>());
        foreach(KeyValuePair<string,int> name in stats.getSkills()){
            Skillname.Add(name.Key);
            activeSkill.Add(abilitiesData.getActiveSkill(name.Key));
        }
        skillDrop = GameObject.Find("ActiveSkill").GetComponent<Dropdown>();
    }

    public List<string> getSkillNames(){
        return Skillname;
    }

    public void ActiveSkillCheck(int choice){
        Debug.Log("Choice: " + choice);
        this.choice = choice;
        Debug.Log(Skillname[choice]);
        if(!stgameObject.GetComponent<StatUpdate>().isTimeStop()){
            switch(Skillname[choice]){
                case "ForceBlast":
                    CheckCoolDown(Skillname[choice]);break;
                case "PsychicStorm":
                    CheckCoolDown(Skillname[choice]);break;
                case "ForeSight":CheckCoolDown(Skillname[choice]);break;
                case "WhirlWind":CheckCoolDown(Skillname[choice]);break;
                case "WaterStance":CheckCoolDown(Skillname[choice]);break;
                case "FireStance":CheckCoolDown(Skillname[choice]);break;
                case "Bubble":CheckCoolDown(Skillname[choice]);break;
                case "Skills":unTargetSkill(); return;
            }
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
    public void unTargetSkill(){
        gameObject.GetComponentInChildren<CharacterEvents>().unHighLightArea.Invoke();
        gameObject.GetComponentInChildren<CharacterEvents>().HighLightReachable.Invoke();
        gameObject.GetComponent<ActionCenter>().setCasting(false);
    }
    public void ActivateSkill(string name){
        if(!CoolDown.ContainsKey(name)){
            activeSkill[choice].Invoke(gameObject, Vector3Int.zero);
        }
    }
    public void resetTargeting(){
        targetV.Clear();
        targets = 0;
        gameObject.GetComponentInChildren<CharacterEvents>().unHighLightArea.Invoke();
        gameObject.GetComponentInChildren<CharacterEvents>().HighLightReachable.Invoke();
    }
    public void targeting(Vector3Int loc){
        Vector3Int playLoc = tileM.WorldToCell(transform.position);
        if(tileM.GetDistance(playLoc,loc) <= skillAttributesFloat["CastRange"] && targets < (int)skillAttributesFloat["TargetNum"]){
            targetLoc = loc;
            //gameObject.GetComponentInChildren<CharacterEvents>().unHighLightArea.Invoke();
            //gameObject.GetComponentInChildren<CharacterEvents>().HighLightReachable.Invoke();
            targetV.Add(loc);
            gameObject.GetComponentInChildren<CharacterEvents>().HighLightArea.Invoke(loc,(int)skillAttributesFloat["Radius"]);
            targets++;
            if(skillAttributesBool["characterTarget"] && tileM.GetNodeFromWorld(targetV[0]).occupant == null){
                resetTargeting();
            }
            if(skillAttributesBool["sameTag"] && tileM.GetNodeFromWorld(targetV[0]).occupant.tag != gameObject.tag){
                resetTargeting();
            }
        }
        else{
            resetTargeting();
        }
    }
    public void updateCasttime(){
        for(int i = 0; i < castList.Count; i++){
            //KeyValuePair<int,KeyValuePair<GameObject,Vector3Int>>, int
            KeyValuePair<KeyValuePair<int,KeyValuePair<GameObject,Vector3Int>>, int> pair = castList.ElementAt(i);
            KeyValuePair<int,KeyValuePair<GameObject,Vector3Int>> k = pair.Key;
            int dec = pair.Value-1;
            castList.Remove(k);
            if(dec <= 0){
                activeSkill[pair.Key.Key].Invoke(pair.Key.Value.Key,pair.Key.Value.Value);
            }
            else{
                castList.Add(k,dec);
            }
        }
    }
    public void SpellCast(){
        if(gameObject.GetComponent<ActionCenter>().isCasting()){
            gameObject.GetComponentInChildren<CharacterEvents>().unHighLightArea.Invoke();
            gameObject.GetComponentInChildren<CharacterEvents>().HighLightReachable.Invoke();
            if(targetV.Count == 1){
                if((int)skillAttributesFloat["CastTime"] == 0){
                    activeSkill[choice].Invoke(gameObject, targetLoc);
                }
                else{
                    castList.Add(new KeyValuePair<int,KeyValuePair<GameObject,Vector3Int>>(choice, new KeyValuePair<GameObject, Vector3Int>(gameObject, targetLoc)),(int)skillAttributesFloat["CastTime"]);
                    }
                }
            else if(skillAttributesBool["characterTarget"]){
                //activeSkill[choice].Invoke(tileM.GetNodeFromWorld(targetV[0]).occupant, targetV[1]);
                if((int)skillAttributesFloat["CastTime"] == 0){
                    activeSkill[choice].Invoke(tileM.GetNodeFromWorld(targetV[0]).occupant, targetV[1]);
                }
                else{
                castList.Add(
                    new KeyValuePair<int,KeyValuePair<GameObject,Vector3Int>>(choice, 
                    new KeyValuePair<GameObject, Vector3Int>(tileM.GetNodeFromWorld(targetV[0]).occupant, targetV[1])),(int)skillAttributesFloat["CastTime"]);
                }
            }
            else{
                if((int)skillAttributesFloat["CastTime"] == 0){
                    activeSkill[choice].Invoke(gameObject, Vector3Int.zero);
                }
                else{
                    castList.Add(new KeyValuePair<int,KeyValuePair<GameObject,Vector3Int>>(choice, new KeyValuePair<GameObject, Vector3Int>(gameObject, Vector3Int.zero)),(int)skillAttributesFloat["CastTime"]);
                }
            }
            //characterTarget = false;
            gameObject.GetComponent<ActionCenter>().setCasting(false);
            targets = 0;
            abilitiesData.computeCost(gameObject, Skillname[choice]);
        }
        if(Skillname[choice] != "ForeSight" && !ui.inForesight()){
            skillDrop.value = 0;
        }
    }

    public void CheckCoolDown(string name){
        switch(name){
            case "ForceBlast":
                if(!CoolDown.ContainsKey("ForceBlast")){
                    skillAttributesFloat = abilitiesData.getSkillsFloats(name,stats); 
                    skillAttributesBool = abilitiesData.getSkillBool(name,stats); 
                    TargetingSkill();
                    CoolDown.Add(name,5);    
                }
                else{
                    Debug.Log("In CoolDown");
                }
            break;
            case "PsychicStorm":
                if(!CoolDown.ContainsKey("PsychicStorm")){
                    skillAttributesFloat = abilitiesData.getSkillsFloats(name,stats); 
                    skillAttributesBool = abilitiesData.getSkillBool(name,stats); 
                    TargetingSkill();
                    CoolDown.Add(name,7);    
                }
                else{
                    Debug.Log("In CoolDown");
                }
            break;
            case "ForeSight":
                if(!CoolDown.ContainsKey("ForeSight") && !GameObject.Find("UICanvas").GetComponent<UI>().inForesight()){
                    skillAttributesFloat = abilitiesData.getSkillsFloats(name,stats); 
                    skillAttributesBool = abilitiesData.getSkillBool(name,stats); 
                    CoolDown.Add(name,Math.Max((int)(5 - stats.getStat("acu")/2),0));   
                    TargetingSkill(); 
                    //activeSkill[choice].Invoke(gameObject, Vector3Int.zero);
                }
                else if(CoolDown.ContainsKey("ForeSight") && GameObject.Find("UICanvas").GetComponent<UI>().inForesight()){
                    skillAttributesFloat = abilitiesData.getSkillsFloats(name,stats); 
                    skillAttributesBool = abilitiesData.getSkillBool(name,stats); 
                    TargetingSkill();
                }
                else{
                    Debug.Log("In CoolDown");
                }
            break;
            case "WhirldWind":
                if(!CoolDown.ContainsKey("WhirldWind")){
                    skillAttributesFloat = abilitiesData.getSkillsFloats(name,stats); 
                    skillAttributesBool = abilitiesData.getSkillBool(name,stats); 
                    CoolDown.Add(name,3);    
                }
                else{
                    Debug.Log("In CoolDown");
                }
            break;
            case "WaterStance":
                if(!CoolDown.ContainsKey("WaterStance")){
                    skillAttributesFloat = abilitiesData.getSkillsFloats(name,stats); 
                    skillAttributesBool = abilitiesData.getSkillBool(name,stats); 
                    CoolDown.Add(name,3);
                    TargetingSkill();
                    //activeSkill[choice].Invoke(gameObject, Vector3Int.zero);
                }
                else{
                    Debug.Log("In CoolDown");
                }
            break;
            case "FireStance":
                if(!CoolDown.ContainsKey("FireStance")){
                    skillAttributesFloat = abilitiesData.getSkillsFloats(name,stats); 
                    skillAttributesBool = abilitiesData.getSkillBool(name,stats); 
                    CoolDown.Add(name,3);
                    TargetingSkill();
                    //activeSkill[choice].Invoke(gameObject, Vector3Int.zero);
                }
                else{
                    Debug.Log("In CoolDown");
                }
            break;
            case "Bubble":
                if(!CoolDown.ContainsKey("Bubble")){
                    skillAttributesFloat = abilitiesData.getSkillsFloats(name,stats); 
                    skillAttributesBool = abilitiesData.getSkillBool(name,stats); 
                    TargetingSkill();
                    //activeSkill[choice].Invoke(gameObject, Vector3Int.zero);
                }
                else{
                    Debug.Log("In CoolDown");
                }
            break;
        }
    }
}
