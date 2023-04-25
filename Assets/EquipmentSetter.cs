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
public class EquipmentSetter : MonoBehaviour
{
    string name_inEdit;
    int type_inEdit;
    public Dropdown eqtype;
    public Dropdown equipment;
    public InputField input;
    public List<string> type = new List<string>(){"none","weapon","armor","shield","buckler","mount"};
    public KeyValuePair<string, UDictionary<string, float>> Entry  = new KeyValuePair<string, UDictionary<string, float>>();
    public UDictionary<string, float> ChangeData = new UDictionary<string, float>();
    public Equipments eq;
    
    void Awake(){
        eqtype.ClearOptions();
        eqtype.AddOptions(type);
        ChangeData = new UDictionary<string, float>();
        Entry  = new KeyValuePair<string, UDictionary<string, float>>();
    }
    public void seteqdrop(int option){
        equipment.ClearOptions();
        List<string> lst = new List<string>();
        switch(option){
            case 1: lst = eq.weapon;break;
            case 2: lst = eq.armor;break;
            case 3: lst = eq.shield;break;
            case 4: lst = eq.buckler;break;
            case 5: lst = eq.mount;break;
        }
        equipment.AddOptions(lst);

    }
    public void neweq(){
        equipment.AddOptions(new List<string>(){"equipment"});
        equipment.value = equipment.options.Count-1;
    }
    public void removeeq(){
        equipment.options.Remove(equipment.options[equipment.value]);
        
        switch(eqtype.value){
            case 1:  eq.removeWeaponEntry(equipment.captionText.text);break;
            case 3:  eq.removeShieldEntry(equipment.captionText.text);break;
            case 2:  eq.removeArmorEntry(equipment.captionText.text);break;
            case 4:  eq.removeBucklerEntry(equipment.captionText.text);break;
            case 5:  eq.removeMountEntry(equipment.captionText.text);break;
        }
        equipment.value = 0;
        setInput(equipment.value);
        EditorUtility.SetDirty(eq);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public void setInput(int option){
        input.text = equipment.options[option].text;
        UDictionary<string, float> stats = new UDictionary<string, float>();
        switch(eqtype.value){
            case 1:  stats = eq.getWeaponStat(input.text);break;
            case 3:  stats = eq.getShieldStat(input.text);break;
            case 2:  stats = eq.getArmorStat(input.text);break;
            case 4:  stats = eq.getBucklerStat(input.text);break;
            case 5:  stats = eq.getMountStat(input.text);break;
        }       
        Entry = new KeyValuePair<string, UDictionary<string, float>>(input.text, stats);
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("statbox")){
            Destroy(go);
        }
        if(stats != null){
            AddStatBoxes(stats);
        }
        ChangeData = stats;
    }
    void AddStatBoxes(UDictionary<string, float> stats){
        
        for(int i = 0; i < stats.Count; i++){
            addInfo(stats.ElementAt(i));
        }
    }
    public void addInfo(KeyValuePair<string,float> stat){
        GameObject goParent = GameObject.Find("scrollPanel");
        GameObject prefab = Resources.Load<GameObject>("StatBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.name = stat.Key;
        Dropdown dd = player.transform.Find("Statname").GetComponent<Dropdown>();
        dd.ClearOptions();
        switch(eqtype.value){
            case 1:  dd.AddOptions(eq.weapon_stats);break;
            case 3:  dd.AddOptions(eq.shield_stats);break;
            case 2:  dd.AddOptions(eq.armor_stats);break;
            case 4:  dd.AddOptions(eq.buckler_stats);break;
            case 5:  dd.AddOptions(eq.mount_stats);break;
        }
        dd.value = dd.options.FindIndex(x => x.text == stat.Key);
        player.transform.Find("StatInput").GetComponent<InputField>().text = (stat.Value).ToString();
    }
    public void addBlank(){
        GameObject goParent = GameObject.Find("scrollPanel");
        GameObject prefab = Resources.Load<GameObject>("StatBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        Dropdown dd = player.transform.Find("Statname").GetComponent<Dropdown>();
        dd.ClearOptions();
        switch(eqtype.value){
            case 1:  dd.AddOptions(eq.weapon_stats);break;
            case 3:  dd.AddOptions(eq.shield_stats);break;
            case 2:  dd.AddOptions(eq.armor_stats);break;
            case 4:  dd.AddOptions(eq.buckler_stats);break;
            case 5:  dd.AddOptions(eq.mount_stats);break;
        }
    }
    public void setText(string name){
        equipment.options[equipment.value].text = name;
        equipment.captionText.text = name;
        switch(eqtype.value){
            case 1:  if(equipment.options.Count == eq.weapon.Count)eq.changeWeaponKey(equipment.value,name);break;
            case 3:  if(equipment.options.Count == eq.shield.Count)eq.changeShieldKey(equipment.value,name);break;
            case 2:  if(equipment.options.Count == eq.armor.Count)eq.changeArmorKey(equipment.value,name);break;
            case 4:  if(equipment.options.Count == eq.buckler.Count)eq.changeBucklerKey(equipment.value,name);break;
            case 5:  if(equipment.options.Count == eq.mount.Count)eq.changeMountKey(equipment.value,name);break;
        }
        EditorUtility.SetDirty(eq);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public void changedata(){
        UDictionary<string,float> newData = new UDictionary<string, float>();
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("statbox")){
            Dropdown dd = go.transform.Find("Statname").GetComponent<Dropdown>();
            string name = dd.options[dd.value].text;
            string value = go.transform.Find("StatInput").GetComponent<InputField>().text;
            if(newData.ContainsKey(name)){
                newData[name] = float.Parse(value);
            }
            else{
                newData.Add(new KeyValuePair<string,float>(name, float.Parse(value)));
            }
        }
        ChangeData = newData;
        switch(eqtype.value){
            case 1:  eq.addWeaponEntry(equipment.captionText.text,ChangeData);break;
            case 3:  eq.addShieldEntry(equipment.captionText.text,ChangeData);break;
            case 2:  eq.addArmorEntry(equipment.captionText.text,ChangeData);break;
            case 4:  eq.addBucklerEntry(equipment.captionText.text,ChangeData);break;
            case 5:  eq.addMountEntry(equipment.captionText.text,ChangeData);break;
        }
        EditorUtility.SetDirty(eq);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
