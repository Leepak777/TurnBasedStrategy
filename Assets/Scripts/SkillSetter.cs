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
    UDictionary<string, UDictionary<string,float>> SkillAttributes = new UDictionary<string, UDictionary<string, float>>();
    [SerializeField]
    UDictionary<string, UDictionary<string,bool>> SkillBools = new UDictionary<string, UDictionary<string, bool>>();
    [SerializeField]
    UDictionary<string, UDictionary<string,float>> SkillCost = new UDictionary<string, UDictionary<string,float>>();
    [SerializeField]
    UDictionary<string, UDictionary<string,int>> SkillStats = new UDictionary<string, UDictionary<string,int>>();
    List<string> type_lst = new List<string>();
    void Awake(){
        type_lst = new List<string>(){"none","Active","Passive"};
        type.ClearOptions();
        type.AddOptions(type_lst);
        type.value = 0;

    }
    public void setSkilldrop(int option){
        skills.ClearOptions();
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
        type.AddOptions(new List<string>(){"Skill"});
        type.value = type.options.Count-1;
    }
    public void removeeq(){
        type.options.Remove(type.options[type.value]);
        ad.removeEntry(type.captionText.text);
        type.value = 0;
        //setInput(type.value);
        EditorUtility.SetDirty(ad);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public void setInput(int option){
        input.text = skills.options[option].text;
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
        if(stats != null){addStats(stats);}
        if(bools != null){addBools(bools);}
        if(attribute != null){addAttributes(attribute);}
        if(cost != null){addCosts(cost);}

    }
    public void addStats(UDictionary<string, int> stats){
        foreach(KeyValuePair<string,int> pair in stats){
            addStat(pair.Key,pair.Value);
        }
    }
    public void addBools(UDictionary<string, bool> bools){
        foreach(KeyValuePair<string,bool> pair in bools){
            addBool(pair.Key,pair.Value);
        }
    }
    public void addAttributes(UDictionary<string, string> attributes){
        foreach(KeyValuePair<string,string> pair in attributes){
            addAttribute(pair.Key,pair.Value);
        }
    }
    public void addCosts(UDictionary<string, string> costs){
        foreach(KeyValuePair<string,string> pair in costs){
            addCost(pair.Key,pair.Value);
        }
    }
    public void addStat(string name, int value){

    }
    public void addBool(string name, bool value){
        GameObject goParent = GameObject.Find("scrollPanelBools");
        GameObject prefab = Resources.Load<GameObject>("BoolBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.GetComponent<BoxFunc>().setToggle(value);
        player.name = name;
        player.GetComponent<BoxFunc>().setDDBoolList(name);
    }
    public void addCost(string name, string value){
        GameObject goParent = GameObject.Find("scrollPanelCost");
        GameObject prefab = Resources.Load<GameObject>("StatBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.name = name;
        player.GetComponent<BoxFunc>().setDDCostList(name);
        player.GetComponent<BoxFunc>().setTxtValS(value);
    }
    public void addAttribute(string name, string value){
        GameObject goParent = GameObject.Find("scrollPanelAttributes");
        GameObject prefab = Resources.Load<GameObject>("StatBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.name = name;
        player.GetComponent<BoxFunc>().setDDAttributeList(name);
        player.GetComponent<BoxFunc>().setTxtValS(value);
    }
    public void addBlankBool(){
        GameObject goParent = GameObject.Find("scrollPanelBools");
        GameObject prefab = Resources.Load<GameObject>("BoolBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.GetComponent<BoxFunc>().setToggle(false);
        player.GetComponent<BoxFunc>().setDDBoolList(name);
    }
    public void addBlankCost(){
        GameObject goParent = GameObject.Find("scrollPanelCost");
        GameObject prefab = Resources.Load<GameObject>("StatBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.GetComponent<BoxFunc>().setDDCostList(name);
    }
    public void addBlankAttribute(){
        GameObject goParent = GameObject.Find("scrollPanelAttributes");
        GameObject prefab = Resources.Load<GameObject>("StatBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.GetComponent<BoxFunc>().setDDAttributeList(name);
    }
    public void reset(){
        ad.reset();
        ad.resetSkillAtt();
        ad.resetSkillBools();
        ad.resetSkillCost();
        EditorUtility.SetDirty(ad);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }
    public void save(){
        EditorUtility.SetDirty(ad);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    
}
