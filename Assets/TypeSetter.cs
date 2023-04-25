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
public class TypeSetter : MonoBehaviour
{
    string name_inEdit;
    int type_inEdit;
    public Dropdown type;
    public InputField input;
    public KeyValuePair<string, UDictionary<string, float>> Entry  = new KeyValuePair<string, UDictionary<string, float>>();
    public UDictionary<string, float> ChangeData = new UDictionary<string, float>();
    public Types ty;
    
    void Awake(){
        type.ClearOptions();
        type.AddOptions(ty.type);
        ChangeData = new UDictionary<string, float>();
        Entry  = new KeyValuePair<string, UDictionary<string, float>>();
    }

    public void neweq(){
        type.AddOptions(new List<string>(){"type"});
        type.value = type.options.Count-1;
    }
    public void removeeq(){
        type.options.Remove(type.options[type.value]);
        ty.removeTypeEntry(type.captionText.text);
        type.value = 0;
        setInput(type.value);
        EditorUtility.SetDirty(ty);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public void setInput(int option){
        input.text = type.options[option].text;
        UDictionary<string, float> stats = new UDictionary<string, float>();
        stats = ty.getTypeStat(input.text); 
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
        dd.AddOptions(ty.type_stats);
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
        dd.AddOptions(ty.type_stats);
    }
    public void setText(string name){
        type.options[type.value].text = name;
        type.captionText.text = name;
        if(type.options.Count == ty.type.Count){
            ty.changeTypeKey(type.value,name);
        }
        EditorUtility.SetDirty(ty);
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
        ty.addTypeEntry(type.captionText.text,ChangeData);
        EditorUtility.SetDirty(ty);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
