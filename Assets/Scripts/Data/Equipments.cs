using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Equipments", order = 1)]
public class Equipments : ScriptableObject
{

    public List<string> weapon = new List<string>(){"none","Light Glaive", "Gladius", "Power Sword", "Great Sword", "Pike","Mace","Pistol","Rifle"};
    public List<string> armor = new List<string>(){"none","Plate","Half Plate","Synthe Armor","Legionary Armor","Praetorian_Armor","Flak Suit","Assault Vest","Personal Shield MK I","Personal Shield MK II","Personal Shield MK III"};
    public List<string> shield = new List<string>(){"none","Kite Shield","Tower Shield"};
    public List<string> buckler = new List<string>(){"none","Buckler","Buckler2"};
    public List<string> mount = new List<string>(){"none","Pack Horse","War Horse"};
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
    
    void Awake(){
        Light_Glaive = new UDictionary<string, float>(){
            {"wd",6},{"pscal",1.3f},{"dscal",0.5f},{"ap",10},{"sp",90},
            {"rng",2},{"acc",1},{"mdb",-2},{"rdb",-1},{"w_enc",3},
            {"attack_num",1}
        };
        Gladius = new UDictionary<string, float>(){
            {"wd",5},{"pscal",1},{"dscal",0.5f},{"ap",25},{"sp",100},
            {"rng",1},{"acc",3},{"mdb",0},{"rdb",0},{"w_enc",2},
            {"attack_num",2}
        };
        Power_Sword = new UDictionary<string, float>(){
            {"wd",8},{"pscal",1},{"dscal",1},{"ap",0},{"sp",100},
            {"rng",1.5f},{"acc",2},{"mdb",1},{"rdb",0},{"w_enc",3},
            {"attack_num",1}
        };
        Great_Sword = new UDictionary<string, float>(){
            {"wd",9},{"pscal",1.3f},{"dscal",1},{"ap",0},{"sp",100},
            {"rng",1.5f},{"acc",2},{"mdb",1},{"rdb",0},{"w_enc",3},
            {"attack_num",1}
        };
        Pike = new UDictionary<string, float>(){
            {"wd",7},{"pscal",1.5f},{"dscal",0.2f},{"ap",25},{"sp",100},
            {"rng",2.3f},{"acc",1},{"mdb",-1},{"rdb",-1},{"w_enc",3},
            {"attack_num",1}
        };
        Mace = new UDictionary<string, float>(){
            {"wd",10},{"pscal",2},{"dscal",0},{"ap",50},{"sp",50},
            {"rng",1.5f},{"acc",2},{"mdb",-2},{"rdb",-1},{"w_enc",4},
            {"attack_num",1}
        };
        Pistol = new UDictionary<string, float>(){
            {"wd",12},{"pscal",0},{"dscal",1},{"ap",50},{"sp",0},
            {"rng",8},{"acc",2},{"mdb",0},{"rdb",0},{"w_enc",4},
            {"attack_num",1}
        };
        Rifle = new UDictionary<string, float>(){
            {"wd",16},{"pscal",0},{"dscal",1},{"ap",50},{"sp",0},
            {"rng",8},{"acc",2},{"mdb",0},{"rdb",0},{"w_enc",4},
            {"attack_num",1}
        };
        Pack_Horse = new UDictionary<string, float>(){
            {"base_hp",30},{"mdb",3},{"eq_mov",8},{"eq_init",+5},{"eq_enc",-6}
        };
        War_Horse = new UDictionary<string, float>(){
            {"base_hp",35},{"mdb",4},{"eq_mov",9},{"eq_init",+6},{"eq_enc",-4}
        };
        Plate = new UDictionary<string, float>(){
            {"av",16},{"sv",0},{"mdb",-2},{"rdb",-2},{"eq_mov",-1},
            {"eq_init",-3},{"eq_enc",6}
        };
        Half_Plate = new UDictionary<string, float>(){
            {"av",14},{"sv",0},{"mdb",-1},{"rdb",-1},{"eq_mov",-1},
            {"eq_init",-1},{"eq_enc",4}
        };
        Synthe_Armor = new UDictionary<string, float>(){
            {"av",13},{"sv",0},{"mdb",0},{"rdb",0},{"eq_mov",0},
            {"eq_init",0},{"eq_enc",2}
        };
        Legionary_Armor = new UDictionary<string, float>(){
            {"av",15},{"sv",0},{"mdb",-1},{"rdb",-2},{"eq_mov",0},
            {"eq_init",-1},{"eq_enc",4}
        };
        Praetorian_Armor = new UDictionary<string, float>(){
            {"av",18},{"sv",0},{"mdb",1},{"rdb",-3},{"eq_mov",-1},
            {"eq_init",-3},{"eq_enc",7}
        };
        Flak_Suit = new UDictionary<string, float>(){
            {"av",13},{"sv",0},{"mdb",-1},{"rdb",0},{"eq_mov",0},
            {"eq_init",0},{"eq_enc",2}
        };
        Assault_Vest = new UDictionary<string, float>(){
            {"av",16},{"sv",0},{"mdb",0},{"rdb",-1},{"eq_mov",-1},
            {"eq_init",-2},{"eq_enc",4}
        };
        Personal_Shield_MK_I = new UDictionary<string, float>(){
            {"av",0},{"sv",15},{"mdb",0},{"rdb",0},{"eq_mov",0},
            {"eq_init",0},{"eq_enc",1}
        };
        Personal_Shield_MK_II = new UDictionary<string, float>(){
            {"av",0},{"sv",17},{"mdb",0},{"rdb",0},{"eq_mov",0},
            {"eq_init",0},{"eq_enc",1}
        };
        Personal_Shield_MK_III = new UDictionary<string, float>(){
            {"av",0},{"sv",19},{"mdb",-1},{"rdb",-1},{"eq_mov",0},
            {"eq_init",-1},{"eq_enc",2}
        };
        Buckler = new UDictionary<string, float>(){
            {"pr",2},{"pv",19},{"mdb",0},{"rdb",0},{"acc",0},{"eq_enc",1}
        };
        Buckler2 = new UDictionary<string, float>(){
        {"pr",2},{"pv",19},{"mdb",-1},{"rdb",-2},{"acc",0},{"eq_enc",2}
        };
        Kite_Shield = new UDictionary<string, float>(){
            {"pr",4},{"pv",15},{"mdb",-2},{"rdb",-2},{"eq_init",-2},{"eq_enc",2}
        };
        Tower_Shield = new UDictionary<string, float>(){
        {"pr",6},{"pv",16},{"mdb",-2},{"rdb",-2},{"eq_init",-1},{"eq_enc",-3}
        };
        Weapon_lst = new UDictionary<string, UDictionary<string, float>>(){
            {"Light Glaive",Light_Glaive},{"Gladius",Gladius},{"Power Sword",Power_Sword},{"Great Sword",Great_Sword},{"Pike",Pike},{"Mace",Mace},{"Pistol",Pistol},{"Rifle",Rifle}
        };
        Buckler_lst = new UDictionary<string, UDictionary<string, float>>(){
            {"Buckler",Buckler},{"Buckler2",Buckler2}
        };   
        Armor_lst = new UDictionary<string, UDictionary<string, float>>(){
            {"Plate",Plate}, {"Half Plate",Half_Plate}, {"Synthe Armor",Synthe_Armor}, {"Legionary Armor",Legionary_Armor}, {"Praetorian Armor",Praetorian_Armor}, {"Flak Suit",Flak_Suit},{"Assault Vest",Assault_Vest}, {"Personal Shield MK I",Personal_Shield_MK_I},{"Personal Shield MK II",Personal_Shield_MK_II},{"Personal Shield MK III",Personal_Shield_MK_III}
        };    
        Mount_lst = new UDictionary<string, UDictionary<string, float>>(){
            {"Pack Horse",Pack_Horse},{"War Horse",War_Horse}
        };   
        Shield_lst = new UDictionary<string, UDictionary<string, float>>(){
            {"Tower Shield", Tower_Shield},{"Kite Shield", Kite_Shield}
        };      
    }
    UDictionary<string,float> Light_Glaive,Gladius,Power_Sword,Great_Sword,Pike,Mace,Pistol,Rifle = new UDictionary<string,float>();
    UDictionary<string,UDictionary<string,float>> Weapon_lst = new UDictionary<string, UDictionary<string, float>>();    
    public UDictionary<string,float> getWeaponStat(string weapon){
        if(Weapon_lst.ContainsKey(weapon)){
            return Weapon_lst[weapon];
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
    UDictionary<string,float> Kite_Shield,Tower_Shield= new UDictionary<string,float>();
    UDictionary<string,UDictionary<string,float>> Shield_lst = new UDictionary<string, UDictionary<string, float>>();    
    public UDictionary<string,float> getShieldStat(string shield){
        if(Shield_lst.ContainsKey(shield)){
            return Shield_lst[shield];
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
    UDictionary<string,float> Buckler,Buckler2= new UDictionary<string,float>();
    UDictionary<string,UDictionary<string,float>> Buckler_lst = new UDictionary<string, UDictionary<string, float>>();   
    public UDictionary<string,float> getBucklerStat(string buckler){    
        if(Buckler_lst.ContainsKey(buckler)){
            return Buckler_lst[buckler];
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
    UDictionary<string,float> Plate, Half_Plate, Synthe_Armor, Legionary_Armor, Praetorian_Armor, Flak_Suit,Assault_Vest, Personal_Shield_MK_I,Personal_Shield_MK_II,Personal_Shield_MK_III = new UDictionary<string,float>();
    UDictionary<string,UDictionary<string,float>> Armor_lst = new UDictionary<string, UDictionary<string, float>>(); 

    public UDictionary<string,float> getArmorStat(string armor){
        if(Armor_lst.ContainsKey(armor)){
            return Armor_lst[armor];
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
    UDictionary<string,float> Pack_Horse,War_Horse= new UDictionary<string,float>();
    UDictionary<string,UDictionary<string,float>> Mount_lst = new UDictionary<string, UDictionary<string, float>>();    

    public UDictionary<string,float> getMountStat(string mount){    
        if(Mount_lst.ContainsKey(mount)){
            return Mount_lst[mount];
        }
        return null;
    }
}
