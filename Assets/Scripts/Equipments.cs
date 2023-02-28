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
    float[] Light_Glaive = {6,1.3f,0.5f,10,90,2,1,-2,-1,3,1};
    float[] Gladius = {5,1,0.5f,25,100,1,3,0,0,2,2};
    float[] Power_Sword = {8,1,1,0,100,1.5f,2,1,0,3,1};
    float[] Great_Sword = {9,1.3f,1,0,100,1.5f,2,1,0,3,1};
    float[] Pike = {7,1.5f,0.2f,25,100,2.3f,1,-1,-1,3,1};
    float[] Mace = {10,2,0,50,50,1.5f,2,-2,-1,4,1};
    float[] Pistol = {12,0,1,50,0,8,2,0,0,4,1};
    float[] Rifle = {16,0,1,50,0,8,2,0,0,4,1};
    public float[] getWeaponStat(string weapon){
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
    float[] Kite_Shield = {4,15,-2,-2,-2,2};
    float[] Tower_Shield = {6,16,-2,-2,-1,-3,3};
    public float[] getShieldStat(string shield){
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
    float[] Buckler = {2,19,0,0,0,1};
    float[] Buckler2 = {2,19,-1,-2,0,2};
    public float[] getBucklerStat(string buckler){    
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
    float[] Plate = {16,0,-2,-2,-1,-3,6};
    float[] Half_Plate = {14,0,-1,-1,-1,-1,4};
    float[] Synthe_Armor = {13,0,0,0,0,0,2};
    float[] Legionary_Armor = {15,0,-1,-2,0,-1,4};
    float[] Praetorian_Armor = {18,0,1,-3,-1,-3,7};
    float[] Flak_Suit = {13,0,-1,0,0,0,2};
    float[] Assault_Vest = {16,0,0,-1,-1,-2,4};
    float[] Personal_Shield_MK_I = {0,15,0,0,0,0,1};
    float[] Personal_Shield_MK_II = {0,17,0,0,0,0,1};
    float[] Personal_Shield_MK_III = {0,19,-1,-1,0,-1,2};
    public float[] getArmorStat(string armor){
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
    float[] Pack_Horse = {30,3,8,+5,-6};
    float[] War_Horse = {35,4,9,+6,-4};
    public float[] getMountStat(string mount){    
        switch(mount){
            case "Pack Horse":
                return Pack_Horse;
            case "War Horse":
                return War_Horse;
        }
        return null;
    }
}
