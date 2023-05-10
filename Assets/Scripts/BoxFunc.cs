using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;
using Random = System.Random;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;

public class BoxFunc : MonoBehaviour
{
    public Text fullname;

    public string stattype;
    public List<string> statslst;
    public Dropdown dd;
    void Start(){
    }
    public void RemoveBox(){
       Destroy(gameObject);
    }
    public void updateBox(){
        setfullname(dd.options[dd.value].text);
    }
    public void updateText(int i){
        setfullname(dd.options[i].text);
    }
    public void setfullname(string i){
        string full = "none";
        switch(i){
            case "wd": full = "Weapon Damage";break;
            case "pow": full = "Power";break;
            case "dex": full = "Dexterity";break;
            case "tou": full = "Toughness";break;
            case "acu": full = "Acuity";break;
            case "mid": full = "Mind";break;
            case "hp": full = "Hit Point";break;
            case "ene": full = "Energy";break;
            case "enc": full = "Encumbrance";break;
            case "fat": full = "Fatigue";break;
            case "stb": full = "Stability";break;
            case "ma": full = "Melee Attack";break;
            case "ra": full = "Range Attack";break;
            case "md": full = "Melee Defence";break;
            case "rd": full = "Ranged Defence";break;
            case "sa": full = "Spell Attack";break;
            case "mr": full = "Magic Resistance";break;
            case "rng": full = "Range";break;
            case "pscal": full = "Pow Scaling";break;
            case "dscal": full = "Dex Scaling";break;
            case "sp": full = "Shield Piercing";break;
            case "ap": full = "Armor Piercing";break;
            case "acc": full = "Accuracy Bonus";break;
            case "av": full = "Armor Value";break;
            case "sv": full = "Shield Value";break;
            case "mdb": full = "Melee Defence Bonus";break;
            case "rdb": full = "Ranged Defence Bonus";break;
            case "pr": full = "Parry Rating";break;
            case "pv": full = "Parry Value";break;
            case "mov": full = "Movement range";break;
            case "bit": full = "Base Initiative";break;
            case "base_hp": full = "Base HP";break;
            case "base_ene": full = "Base Energy";break;
            case "base_mov": full = "Base Movement range";break;
            case "base_init": full = "Base Initiative";break;
            case "base_enc": full = "Base Encumbrance";break;
            case "attack_num": full = "Attack times";break;
            default: full = "none";break;
        }
        fullname.text = full;
    }
}
