using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TypesSkills", order = 2)]
public class TypesSkills:ScriptableObject
{
    [SerializeField]
    UDictionary<string,UDictionary<string,string>> Types_Abil = new UDictionary<string, UDictionary<string, string>>();
    [SerializeField]
    UDictionary<string,UDictionary<string,int>> Types_Skil = new UDictionary<string, UDictionary<string,int>>();
    public void reset(){
        Types_Abil = new UDictionary<string, UDictionary<string,string>>(){
        {"Praetorian Guard", Praetorian_Guard_Abilities}, {"Imperial Legionary", Imperial_Legionary_Abilities},{"Imperial Conscript",Imperial_Conscript_Abilities}
        ,{"Mercenary",Mercenary_Abilities},{"Brigand",Brigand_Abilities}
        };
        Types_Skil = new UDictionary<string, UDictionary<string,int>>(){
        {"Praetorian Guard", Praetorian_Guard_Skill}, {"Imperial Legionary", Imperial_Legionary_Skill},{"Imperial Conscript",Imperial_Conscript_Skill}
        ,{"Mercenary",Mercenary_Skill},{"Brigand",Brigand_Skill}
        };
    }
    public void changeTypeKey(int index, string name){
        UDictionary<string,int> skills = Types_Skil.ElementAt(index-1).Value;
        UDictionary<string,string> abilities = Types_Abil.ElementAt(index-1).Value; 
        removeSkillEntry(Types_Skil.ElementAt(index-1).Key);
        removeAbilEntry(Types_Abil.ElementAt(index-1).Key);
        addSkillEntry(name,skills);
        addAbilEntry(name,abilities);
    }
    public void addTypeSkill(string name, UDictionary<string,int> stats){
       
        if(Types_Skil.ContainsKey(name)){
            Types_Skil[name] = stats;
        }
        else{
            Types_Skil.Add(name,stats);
        }
        
        /*foreach(KeyValuePair<string,int> s in Types_Skil[name] ){
            Debug.Log(s.Key);
        }*/
    }
    public void addTypeAbilities(string name, UDictionary<string,string> stats){
        if(Types_Abil.ContainsKey(name)){
            Types_Abil[name] = stats;
        }
        else{
            Types_Abil.Add(name,stats);
        }
        /*foreach(KeyValuePair<string,string> s in Types_Abil[name] ){
            Debug.Log(s.Key);
        }*/
    }
    public UDictionary<string,string> Praetorian_Guard_Abilities = new UDictionary<string,string>(){
        {"LeadershipAura","Universal"},{"Charge","Individual"}
    };
    public UDictionary<string,string> Imperial_Legionary_Abilities = new UDictionary<string,string>(){
        {"LeadershipAura","Universal"},{"Charge","Individual"}
    };
    public UDictionary<string,string> Imperial_Conscript_Abilities = new UDictionary<string,string>(){
        {"LeadershipAura","Universal"},{"Charge","Individual"}
    };
    public UDictionary<string,string> Mercenary_Abilities = new UDictionary<string,string>(){
        {"LeadershipAura","Universal"},{"Charge","Individual"}
    };
    public UDictionary<string,string> Brigand_Abilities = new UDictionary<string,string>(){
        {"LeadershipAura","Universal"},{"Charge","Individual"}
    };
    public UDictionary<string,string> getTypeAbilities(string type){

        if(Types_Abil.ContainsKey(type)){return Types_Abil[type];}
        /*switch(type){
            case "Praetorian Guard":
                return Praetorian_Guard_Abilities;
            case "Imperial Legionary":
                return Imperial_Legionary_Abilities;
            case "Imperial Conscript":
                return Imperial_Conscript_Abilities;
            case "Mercenary":
                return Mercenary_Abilities;
            case "Brigand":
                return Brigand_Abilities;
        }*/
        return null;
    }
    public void addAbilEntry(string name, UDictionary<string,string> stats){
        Types_Abil.Add(name,stats);
    }
    public void removeAbilEntry(string name){
        Types_Abil.Remove(name);
    }
    public void addSkillEntry(string name, UDictionary<string,int> stats){
        Types_Skil.Add(name,stats);
    }
    public void removeSkillEntry(string name){
        Types_Skil.Remove(name);
    }

    public UDictionary<string,int> Praetorian_Guard_Skill = new UDictionary<string,int>(){
        {"WhirlWind",0},{"ForceBlast",0},{"ForeSight",0}
    };
    public UDictionary<string,int> Imperial_Legionary_Skill = new UDictionary<string,int>(){
        {"PsychicStorm",0},{"ForceBlast",0},{"ForeSight",0}
    };
    public UDictionary<string,int> Imperial_Conscript_Skill = new UDictionary<string,int>(){
        {"WhirlWind",0},{"PsychicStorm",0},{"ForeSight",0}
    };
    public UDictionary<string,int> Mercenary_Skill = new UDictionary<string,int>(){
        {"WhirlWind",0},{"PsychicStorm",0},{"ForceBlast",0},{"ForeSight",0}
    };
    public UDictionary<string,int> Brigand_Skill = new UDictionary<string,int>(){
        {"WhirlWind",0},{"PsychicStorm",0},{"ForceBlast",0},{"ForeSight",0}
    };
    public UDictionary<string,int> getTypeSkills(string type){
        if(Types_Skil.ContainsKey(type)){return Types_Skil[type];}
        /*switch(type){
            case "Praetorian Guard":
                return Types_Skil[type];
            case "Imperial Legionary":
                return Types_Skil[type];
            case "Imperial Conscript":
                return Types_Skil[type];
            case "Mercenary":
                return Types_Skil[type];
            case "Brigand":
                return Types_Skil[type];
            default: return Types_Skil[type];
        }*/
        return null;
    }
    
}
