using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Types", order = 2)]
public class Types:ScriptableObject
{
    public List<string> type = new List<string>(){"none","Praetorian Guard", "Imperial Legionary", "Imperial Conscript", "Mercenary", "Brigand"};
    /*
    0. pow
    1. dex
    2. tou
    3. acu
    4. mid
    5. base HP
    6. base ene
    7. base mov
    8. base init
    9. base enc
    10. ma
    11. ra
    12. sa
    13. md
    14. rd
    15. mr
    */
    UDictionary<string,float> Praetorian_Guard,Imperial_Legionary,Imperial_Conscript,Mercenary,Brigand = new UDictionary<string, float>();
    UDictionary<string,UDictionary<string,float>> Type_lst = new UDictionary<string, UDictionary<string, float>>();
    void Awake(){
        setBaseTy();
        Type_lst = new UDictionary<string, UDictionary<string, float>>(){
        {"Praetorian Guard", Praetorian_Guard}, {"Imperial Legionary", Imperial_Legionary},{"Imperial Conscript",Imperial_Conscript}
        ,{"Mercenary",Mercenary},{"Brigand",Brigand}
        };
    }
    public UDictionary<string,float> getTypeStat(string type){
        if(Type_lst.ContainsKey(type)){
            return Type_lst[type];
        }
        return null;
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
        switch(type){
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
        }
        return null;
    }
    public void addEntry(string name, UDictionary<string,float> stats){
        Type_lst.Add(name,stats);
    }
    public void removeEntry(string name){
        Type_lst.Remove(name);
    }
    void setBaseTy(){
        Praetorian_Guard = new UDictionary<string, float>(){
            {"pow",8},{"dex",6},{"tou",7},{"acu",4},{"mid",3},
            {"base_hp",10},{"base_ene",0},{"base_mov",1},{"base_init",10},{"base_enc",0},
            {"ma",15},{"ra",9},{"sa",3},{"md",16},{"rd",16},{"mr",13}
        };
        Imperial_Legionary = new UDictionary<string, float>(){
            {"pow",7},{"dex",5},{"tou",6},{"acu",2},{"mid",3},
            {"base_hp",10},{"base_ene",0},{"base_mov",1},{"base_init",10},{"base_enc",0},
            {"ma",13},{"ra",9},{"sa",3},{"md",15},{"rd",14},{"mr",10}
        };
        Imperial_Conscript = new UDictionary<string, float>(){
            {"pow",5},{"dex",5},{"tou",5},{"acu",1},{"mid",2},
            {"base_hp",0},{"base_ene",0},{"base_mov",1},{"base_init",10},{"base_enc",0},
            {"ma",10},{"ra",7},{"sa",3},{"md",10},{"rd",10},{"mr",10}
        };
        Mercenary = new UDictionary<string, float>(){
            {"pow",6},{"dex",6},{"tou",5},{"acu",2},{"mid",2},
            {"base_hp",5},{"base_ene",0},{"base_mov",1},{"base_init",11},{"base_enc",0},
            {"ma",12},{"ra",11},{"sa",3},{"md",13},{"rd",12},{"mr",10}
        };
        Brigand = new UDictionary<string, float>(){
            {"pow",4},{"dex",5},{"tou",4},{"acu",1},{"mid",2},
            {"base_hp",0},{"base_ene",0},{"base_mov",2},{"base_init",10},{"base_enc",0},
            {"ma",9},{"ra",11},{"sa",3},{"md",9},{"rd",9},{"mr",10}
        };
    }
}
