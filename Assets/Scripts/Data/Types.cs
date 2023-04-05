using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Types", order = 2)]
public class Types:ScriptableObject
{
    public List<string> type = new List<string>(){"none","Praetorian Guard", "Imperial Legionary", "Imperial Conscript", "Mecenary", "Brigand"};
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
    UDictionary<string,float> Praetorian_Guard = new UDictionary<string, float>(){
        {"pow",8},{"dex",6},{"tou",7},{"acu",4},{"mid",3},
        {"base_hp",10},{"base_ene",0},{"base_mov",1},{"base_init",10},{"base_enc",0},
        {"ma",15},{"ra",9},{"sa",3},{"md",16},{"rd",16},{"mr",13}
    };
    UDictionary<string,float> Imperial_Legionary = new UDictionary<string, float>(){
        {"pow",7},{"dex",5},{"tou",6},{"acu",2},{"mid",3},
        {"base_hp",10},{"base_ene",0},{"base_mov",1},{"base_init",10},{"base_enc",0},
        {"ma",13},{"ra",9},{"sa",3},{"md",15},{"rd",14},{"mr",10}
    };
    UDictionary<string,float> Imperial_Conscript = new UDictionary<string, float>(){
        {"pow",5},{"dex",5},{"tou",5},{"acu",1},{"mid",2},
        {"base_hp",0},{"base_ene",0},{"base_mov",1},{"base_init",10},{"base_enc",0},
        {"ma",10},{"ra",7},{"sa",3},{"md",10},{"rd",10},{"mr",10}
    };
    UDictionary<string,float> Mecenary = new UDictionary<string, float>(){
        {"pow",6},{"dex",6},{"tou",5},{"acu",2},{"mid",2},
        {"base_hp",5},{"base_ene",0},{"base_mov",1},{"base_init",11},{"base_enc",0},
        {"ma",12},{"ra",11},{"sa",3},{"md",13},{"rd",12},{"mr",10}
    };
    UDictionary<string,float> Brigand = new UDictionary<string, float>(){
        {"pow",4},{"dex",5},{"tou",4},{"acu",1},{"mid",2},
        {"base_hp",0},{"base_ene",0},{"base_mov",2},{"base_init",10},{"base_enc",0},
        {"ma",9},{"ra",11},{"sa",3},{"md",9},{"rd",9},{"mr",10}
    };
    public UDictionary<string,float> getTypeStat(string type){
        switch(type){
            case "Praetorian Guard":
                return Praetorian_Guard;
            case "Imperial Legionary":
                return Imperial_Legionary;
            case "Imperial Conscript":
                return Imperial_Conscript;
            case "Mecenary":
                return Mecenary;
            case "Brigand":
                return Brigand;
        }
        return null;
    }

    public List<string> Praetorian_Guard_Abilities = new List<string>(){
        "LeadershipAura"
    };
    public List<string> Imperial_Legionary_Abilities = new List<string>(){
        "LeadershipAura"
    };
    public List<string> Imperial_Conscript_Abilities = new List<string>(){
        "LeadershipAura"
    };
    public List<string> Mecenary_Abilities = new List<string>(){
        "LeadershipAura"
    };
    public List<string> Brigand_Abilities = new List<string>(){
        "LeadershipAura"
    };
    public List<string> getTypeAbilities(string type){
        switch(type){
            case "Praetorian Guard":
                return Praetorian_Guard_Abilities;
            case "Imperial Legionary":
                return Imperial_Legionary_Abilities;
            case "Imperial Conscript":
                return Imperial_Conscript_Abilities;
            case "Mecenary":
                return Mecenary_Abilities;
            case "Brigand":
                return Brigand_Abilities;
        }
        return null;
    }

    public List<KeyValuePair<string,string>> getAbilitiesList(string ability){
        List<KeyValuePair<string,string>> result = new List<KeyValuePair<string,string>>();
        switch (ability){
            case "LeadershipAura":
                result.AddRange(new[]{new KeyValuePair<string, string>(ability,"start"),new KeyValuePair<string, string>(ability,"end"),new KeyValuePair<string, string>(ability,"move"),new KeyValuePair<string, string>(ability,"stop")});
                break;
        }
        return result;
    }
}
