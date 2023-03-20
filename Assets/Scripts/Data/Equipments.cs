using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Equipments", order = 1)]
public class Equipments : ScriptableObject
{
    //Hands
    /*
    0. wd
    1. pscal
    2. dscal
    3. ap
    4. sp
    5. rng
    6. acc
    7. mdb
    8. rdb
    9. enc
    10. #of attacks
    */
    //weapon
    UDictionary<string,float> Light_Glaive = new UDictionary<string, float>(){
        {"wd",6},{"pscal",1.3f},{"dscal",0.5f},{"ap",10},{"sp",90},
        {"rng",2},{"acc",1},{"mdb",-2},{"rdb",-1},{"w_enc",3},
        {"attack_num",1}
    };
    UDictionary<string,float> Gladius = new UDictionary<string, float>(){
        {"wd",5},{"pscal",1},{"dscal",0.5f},{"ap",25},{"sp",100},
        {"rng",1},{"acc",3},{"mdb",0},{"rdb",0},{"w_enc",2},
        {"attack_num",2}
    };
    UDictionary<string,float> Power_Sword = new UDictionary<string, float>(){
        {"wd",8},{"pscal",1},{"dscal",1},{"ap",0},{"sp",100},
        {"rng",1.5f},{"acc",2},{"mdb",1},{"rdb",0},{"w_enc",3},
        {"attack_num",1}
    };
    UDictionary<string,float> Great_Sword = new UDictionary<string, float>(){
        {"wd",9},{"pscal",1.3f},{"dscal",1},{"ap",0},{"sp",100},
        {"rng",1.5f},{"acc",2},{"mdb",1},{"rdb",0},{"w_enc",3},
        {"attack_num",1}
    };
    UDictionary<string,float> Pike = new UDictionary<string, float>(){
        {"wd",7},{"pscal",1.5f},{"dscal",0.2f},{"ap",25},{"sp",100},
        {"rng",2.3f},{"acc",1},{"mdb",-1},{"rdb",-1},{"w_enc",3},
        {"attack_num",1}
    };
    UDictionary<string,float> Mace = new UDictionary<string, float>(){
        {"wd",10},{"pscal",2},{"dscal",0},{"ap",50},{"sp",50},
        {"rng",1.5f},{"acc",2},{"mdb",-2},{"rdb",-1},{"w_enc",4},
        {"attack_num",1}
    };
    UDictionary<string,float> Pistol = new UDictionary<string, float>(){
        {"wd",12},{"pscal",0},{"dscal",1},{"ap",50},{"sp",0},
        {"rng",8},{"acc",2},{"mdb",0},{"rdb",0},{"w_enc",4},
        {"attack_num",1}
    };
    UDictionary<string,float> Rifle = new UDictionary<string, float>(){
        {"wd",16},{"pscal",0},{"dscal",1},{"ap",50},{"sp",0},
        {"rng",8},{"acc",2},{"mdb",0},{"rdb",0},{"w_enc",4},
        {"attack_num",1}
    };
    public UDictionary<string,float> getWeaponStat(string weapon){
        switch(weapon){
            case "Light Glaive":
                return Light_Glaive;
            case "Gladius":
                return Gladius;
            case "Power Sword":
                return Power_Sword;
            case "Great Sword":
                return Great_Sword;
            case "Pike":
                return Pike;
            case "Mace":
                return Mace;
            case "Pistol":
                return Pistol;
            case "Rifle":
                return Rifle;
        }
        return null;
    }
    //shields
    /*
    0. pr
    1. pv
    2. mdb
    3. rdb
    4. init
    5. enc
    */
    UDictionary<string,float> Kite_Shield = new UDictionary<string, float>(){
        {"pr",4},{"pv",15},{"mdb",-2},{"rdb",-2},{"eq_init",-2},{"eq_enc",2}
    };
    UDictionary<string,float> Tower_Shield = new UDictionary<string, float>(){
       {"pr",6},{"pv",16},{"mdb",-2},{"rdb",-2},{"eq_init",-1},{"eq_enc",-3}
    };
    public UDictionary<string,float> getShieldStat(string shield){
        switch(shield){
            case "Kite Shield":
                return Kite_Shield;
            case "Tower Shield":
                return Tower_Shield;
        }
        return null;

    }
    //buckler
    /*
    0. pr
    1. pv
    2. acc(buckler2)
    3. mdb
    4. rdb
    5. enc
    */
    UDictionary<string,float> Buckler = new UDictionary<string, float>(){
        {"pr",2},{"pv",19},{"mdb",0},{"rdb",0},{"acc",0},{"eq_enc",1}
    };
    UDictionary<string,float> Buckler2 = new UDictionary<string, float>(){
       {"pr",2},{"pv",19},{"mdb",-1},{"rdb",-2},{"acc",0},{"eq_enc",2}
    };
    public UDictionary<string,float> getBucklerStat(string buckler){    
        switch(buckler){
            case "Buckler":
                return Buckler;
            case "Buckler2":
                return Buckler2;
        }
        return null;

    }

    //Body
    /*
    0. av
    1. sv
    2. mdb
    3. rdb
    4. mov
    5. init
    6. enc
    */
    UDictionary<string,float> Plate = new UDictionary<string, float>(){
        {"av",16},{"sv",0},{"mdb",-2},{"rdb",-2},{"eq_mov",-1},
        {"eq_init",-3},{"eq_enc",6}
    };
    UDictionary<string,float> Half_Plate = new UDictionary<string, float>(){
        {"av",14},{"sv",0},{"mdb",-1},{"rdb",-1},{"eq_mov",-1},
        {"eq_init",-1},{"eq_enc",4}
    };
    UDictionary<string,float> Synthe_Armor = new UDictionary<string, float>(){
        {"av",13},{"sv",0},{"mdb",0},{"rdb",0},{"eq_mov",0},
        {"eq_init",0},{"eq_enc",2}
    };
    UDictionary<string,float> Legionary_Armor = new UDictionary<string, float>(){
        {"av",15},{"sv",0},{"mdb",-1},{"rdb",-2},{"eq_mov",0},
        {"eq_init",-1},{"eq_enc",4}
    };
    UDictionary<string,float> Praetorian_Armor = new UDictionary<string, float>(){
        {"av",18},{"sv",0},{"mdb",1},{"rdb",-3},{"eq_mov",-1},
        {"eq_init",-3},{"eq_enc",7}
    };
    UDictionary<string,float> Flak_Suit = new UDictionary<string, float>(){
        {"av",13},{"sv",0},{"mdb",-1},{"rdb",0},{"eq_mov",0},
        {"eq_init",0},{"eq_enc",2}
    };
    UDictionary<string,float> Assault_Vest = new UDictionary<string, float>(){
        {"av",16},{"sv",0},{"mdb",0},{"rdb",-1},{"eq_mov",-1},
        {"eq_init",-2},{"eq_enc",4}
    };
    UDictionary<string,float> Personal_Shield_MK_I = new UDictionary<string, float>(){
        {"av",0},{"sv",15},{"mdb",0},{"rdb",0},{"eq_mov",0},
        {"eq_init",0},{"eq_enc",1}
    };
    UDictionary<string,float> Personal_Shield_MK_II = new UDictionary<string, float>(){
        {"av",0},{"sv",17},{"mdb",0},{"rdb",0},{"eq_mov",0},
        {"eq_init",0},{"eq_enc",1}
    };
    UDictionary<string,float> Personal_Shield_MK_III = new UDictionary<string, float>(){
        {"av",0},{"sv",19},{"mdb",-1},{"rdb",-1},{"eq_mov",0},
        {"eq_init",-1},{"eq_enc",2}
    };
    public UDictionary<string,float> getArmorStat(string armor){
        switch(armor){
            case "Plate":
                return Plate;
            case "Half Plate":
                return Half_Plate;
            case "Synthe Armor":
                return Synthe_Armor;
            case "Legionary Armor":
                return Legionary_Armor;
            case "Praetorian_Armor":
                return Praetorian_Armor;
            case "Flak Suit":
                return Flak_Suit;
            case "Assault Vest":
                return Assault_Vest;
            case "Personal Shield MK I":
                return Personal_Shield_MK_I;
            case "Personal Shield MK II":
                return Personal_Shield_MK_II;
            case "Personal Shield MK III":
                return Personal_Shield_MK_III;
        }
        return null;

    }

    //Mount
    /*
    0. hp
    1. mdr
    2. mov
    3. init 
    4. enc
    */
    UDictionary<string,float> Pack_Horse = new UDictionary<string, float>(){
        {"base_hp",30},{"mdb",3},{"eq_mov",8},{"eq_init",+5},{"eq_enc",-6}
    };
    UDictionary<string,float> War_Horse = new UDictionary<string, float>(){
        {"base_hp",35},{"mdb",4},{"eq_mov",9},{"eq_init",+6},{"eq_enc",-4}
    };
    public UDictionary<string,float> getMountStat(string mount){    
        switch(mount){
            case "Pack Horse":
                return Pack_Horse;
            case "War Horse":
                return War_Horse;
        }
        return null;
    }
}
