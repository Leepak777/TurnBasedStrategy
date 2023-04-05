using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;
using Random = System.Random;
using UnityEditor;
using UnityEngine.UI;

public class setInfoPanel : MonoBehaviour
{
    public GameObject type;
    public GameObject equipment;
    public GameObject stat;
    public Equipments equipments;
    public Types types;
    public string info_name;
    private string[] eqlst = {"Weapon","Shield","Armor","Buckler","Mount"};
    SetInfo setter;
    void Start(){

    }
    public void updateInfo(){
        settext(info_name);
    }
    public void settext(string name){
        info_name = name;
        setter = GameObject.Find(name).GetComponent<SetInfo>();
        Text goType = type.GetComponent<Text>();
        Text goEquipment = equipment.GetComponent<Text>();
        Text goStat = stat.gameObject.GetComponent<Text>();
        CharacterStat chStat = setter.GetAsset(name);
        if(chStat == null){
            return;
        }
        goType.text = chStat.getAttribute("Type");
        goEquipment.text="";
        if(goEquipment.text == ""){
            foreach(string str in eqlst){
                if(chStat.getAttribute(str) != null){
                    goEquipment.text += str+": \n"+ chStat.getAttribute(str) +"\n"; 
                }
            }
        }
        goStat.text = "";
        List<string> lst = new List<string>();
        if(chStat.abilities.Count > 0){
            goStat.text = chStat.abilities[0] +"\n";
            lst.Add(chStat.abilities[0]);
            for(int i = 1; i < chStat.abilities.Count; i++){
                if(!lst.Contains(chStat.abilities[i])){
                    goStat.text += chStat.abilities[i]+"\n";
                    lst.Add(chStat.abilities[i]);
                }   
            }
        }
        goStat.text += "HP: "+chStat.getStat("maxHealth") + "\n";
        goStat.text += "Damage: \n" + chStat.getBaseDamage() + "\n";
        goStat.text += "Protection: \n" + chStat.getProtection() + "\n";
        
    }
}
