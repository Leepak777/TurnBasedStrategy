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
public class SkillSetter : MonoBehaviour
{
    string name_inEdit;
    public Dropdown type;
    public Dropdown skills;
    public InputField input;

    public AbilitiesData ad;
    [SerializeField]
    UDictionary<string,string> SkillAttributes = new UDictionary<string, string>();
    [SerializeField]
    UDictionary<string,bool> SkillBools = new UDictionary<string, bool>();
    [SerializeField]
    UDictionary<string,string> SkillCost = new UDictionary<string,string>();
    [SerializeField]
    UDictionary<string,int> SkillStats = new UDictionary<string,int>();
    List<string> type_lst = new List<string>();
    void Awake(){
        type_lst = new List<string>(){"none","Active","Passive"};
        type.ClearOptions();
        type.AddOptions(type_lst);
        type.value = 0;

    }
    public void setSkilldrop(int option){
        skills.ClearOptions();
        skills.AddOptions(new List<string>(){"none"});
        List<string> lst = new List<string>();
        if(option == 1){
            lst = ad.getSkillList();
        }
        else if (option == 2){
            lst = ad.getAbilList();
        }
        skills.AddOptions(lst);

    }
    public void newSkill(){
        skills.AddOptions(new List<string>(){"Skill"});
        skills.value = skills.options.Count-1;
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("statbox")){
            Destroy(go);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("SkillAtt")){
            Destroy(go);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("SkillCost")){
            Destroy(go);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("SkillStat")){
            Destroy(go);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("SkillBool")){
            Destroy(go);
        }
    }
    public void removeeq(){
        skills.options.Remove(skills.options[skills.value]);
        ad.removeEntry(skills.captionText.text);
        skills.value = 0;
        //setInput(type.value);
        EditorUtility.SetDirty(ad);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public void setInput(int option){
        input.text = skills.options[option].text;
        Debug.Log(input.text);
        UDictionary<string, int> stats = new UDictionary<string, int>();
        stats = ad.getSkillStat(input.text); 
        UDictionary<string, bool> bools = new UDictionary<string, bool>();
        bools = ad.getSkillBool(input.text); 
        UDictionary<string, string> attribute = new UDictionary<string, string>();
        attribute = ad.getSkillAtt(input.text); 
        UDictionary<string, string> cost = new UDictionary<string, string>();
        cost = ad.getSkillCost(input.text); 
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("statbox")){
            Destroy(go);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("SkillAtt")){
            Destroy(go);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("SkillCost")){
            Destroy(go);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("SkillStat")){
            Destroy(go);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("SkillBool")){
            Destroy(go);
        }
        if(stats != null){Debug.Log(stats);addStats(stats);}
        if(bools != null){Debug.Log(bools);addBools(bools);}
        if(attribute != null){Debug.Log(attribute);addAttributes(attribute);}
        if(cost != null){Debug.Log(cost);addCosts(cost);}

    }
    public void addStats(UDictionary<string, int> stats){
        foreach(KeyValuePair<string,int> pair in stats){
            Debug.Log(pair.Key+","+pair.Value);
            addStat(pair.Key,pair.Value);
        }
    }
    public void addBools(UDictionary<string, bool> bools){
        foreach(KeyValuePair<string,bool> pair in bools){
            Debug.Log(pair.Key+","+pair.Value);
            addBool(pair.Key,pair.Value);
        }
    }
    public void addAttributes(UDictionary<string, string> attributes){
        foreach(KeyValuePair<string,string> pair in attributes){
            Debug.Log(pair.Key+","+pair.Value);
            addAttribute(pair.Key,pair.Value);
        }
    }
    public void addCosts(UDictionary<string, string> costs){
        foreach(KeyValuePair<string,string> pair in costs){
            addCost(pair.Key,pair.Value);
        }
    }
    public void addBool(string name, bool value){
        GameObject goParent = GameObject.Find("scrollPanelBools");
        GameObject prefab = Resources.Load<GameObject>("BoolBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.tag = "SkillBool";
        player.transform.SetParent(goParent.transform);
        player.GetComponent<BoxFunc>().setToggle(value);
        player.name = name;
        player.GetComponent<BoxFunc>().setDDBoolList(name);
        player.GetComponent<BoxFunc>().updateBox();
    }
    public void addCost(string name, string value){
        GameObject goParent = GameObject.Find("scrollPanelCost");
        GameObject prefab = Resources.Load<GameObject>("StatBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.tag = "SkillCost";
        player.name = name;
        player.GetComponent<BoxFunc>().setDDCostList(name);
        player.GetComponent<BoxFunc>().setTxtValS(value);
        player.GetComponent<BoxFunc>().updateBox();
    }
    public void addAttribute(string name, string value){
        GameObject goParent = GameObject.Find("scrollPanelAttributes");
        GameObject prefab = Resources.Load<GameObject>("StatBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.tag = "SkillAtt";
        player.name = name;
        player.GetComponent<BoxFunc>().setDDAttributeList(name);
        player.GetComponent<BoxFunc>().setTxtValS(value);
        if(name != "Type"){
            player.GetComponent<BoxFunc>().updateBox();
        }
    }
    public void addStat(string name, int value){
        GameObject goParent = GameObject.Find("scrollPanelStats");
        GameObject prefab = Resources.Load<GameObject>("StatBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.tag = "SkillStat";
        player.name = name;
        player.GetComponent<BoxFunc>().setDDCostList(name);
        player.GetComponent<BoxFunc>().setTxtVal(value);
        player.GetComponent<BoxFunc>().updateBox();
    }
    public void addBlankBool(){
        GameObject goParent = GameObject.Find("scrollPanelBools");
        GameObject prefab = Resources.Load<GameObject>("BoolBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.tag = "SkillBool";
        player.GetComponent<BoxFunc>().setToggle(false);
        player.GetComponent<BoxFunc>().setDDBoolList(name);
        player.GetComponent<BoxFunc>().updateBox();
    }
    public void addBlankCost(){
        GameObject goParent = GameObject.Find("scrollPanelCost");
        GameObject prefab = Resources.Load<GameObject>("StatBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.GetComponent<BoxFunc>().setDDCostList("");
        player.tag = "SkillCost";
        player.GetComponent<BoxFunc>().updateBox();
    }
    public void addBlankAttribute(){
        GameObject goParent = GameObject.Find("scrollPanelAttributes");
        GameObject prefab = Resources.Load<GameObject>("StatBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.GetComponent<BoxFunc>().setDDAttributeList("");
        player.tag = "SkillAtt";
        player.GetComponent<BoxFunc>().updateBox();
    }
    public void addBlankStats(){
        GameObject goParent = GameObject.Find("scrollPanelStat");
        GameObject prefab = Resources.Load<GameObject>("StatBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.tag = "SkillStat";
        player.name = name;
        player.GetComponent<BoxFunc>().setDDCostList("");
        player.GetComponent<BoxFunc>().updateBox();
    }
    public void setText(string name){
        skills.options[skills.value].text = name;
        skills.captionText.text = name;
        save();
    }
    public void reset(){
        ad.reset();
        ad.resetSkillAtt();
        ad.resetSkillBools();
        ad.resetSkillCost();
        save();

    }
    public void save(){
        EditorUtility.SetDirty(ad);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public KeyValuePair<string,string> readStringBox(GameObject box){
        InputField i = box.GetComponentInChildren<InputField>();
        Dropdown dd = box.GetComponentInChildren<Dropdown>();
        return new KeyValuePair<string, string>(dd.captionText.text,i.text);
    }
    public KeyValuePair<string,bool> readBoolBox(GameObject box){
        Toggle i = box.GetComponentInChildren<Toggle>();
        Dropdown dd = box.GetComponentInChildren<Dropdown>();
        return new KeyValuePair<string, bool>(dd.captionText.text,i.isOn);
    }
    public KeyValuePair<string,int> readIntBox(GameObject box){
        InputField i = box.GetComponentInChildren<InputField>();
        Dropdown dd = box.GetComponentInChildren<Dropdown>();
        return new KeyValuePair<string, int>(dd.captionText.text,int.Parse(i.text));
    }
    public void confirm(){
        SkillAttributes.Clear();
        SkillCost.Clear();
        SkillStats.Clear();
        SkillBools.Clear();
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("SkillAtt")){
            KeyValuePair<string,string> pair = readStringBox(go);
            Debug.Log(pair.Key+","+pair.Value);
            SkillAttributes.Add(pair.Key,pair.Value);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("SkillCost")){
            KeyValuePair<string,string> pair = readStringBox(go);
            Debug.Log(pair.Key+","+pair.Value);
            SkillCost.Add(pair.Key,pair.Value);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("SkillStat")){
            KeyValuePair<string,int> pair = readIntBox(go);
            Debug.Log(pair.Key+","+pair.Value);
            SkillStats.Add(pair.Key,pair.Value);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("SkillBool")){
            KeyValuePair<string,bool> pair = readBoolBox(go);
            Debug.Log(pair.Key+","+pair.Value);
            SkillBools.Add(pair.Key,pair.Value);
        }
        ad.addEntry(type.value,input.text,SkillAttributes,SkillCost,SkillStats,SkillBools);
        save();
    }

    
}
