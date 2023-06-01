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
    public TypesSkills TYS;
    public AbilitiesData ad;
    [SerializeField]
    UDictionary<string,Sprite> sprites = new UDictionary<string,Sprite>();
    Sprite selected;
    ScriptableObjectManager som = new ScriptableObjectManager("Assets/Scripts/Data");
    void Awake(){
        Sprite[] allsprites = Resources.LoadAll<Sprite>("CharacterSprites/Crystal_Knight");
        foreach(Sprite s in allsprites){
            //Debug.Log(s.name);
            sprites.Add(s.name,s);
        }
        allsprites = Resources.LoadAll<Sprite>("CharacterSprites/Daemons");
        foreach(Sprite s in allsprites){
            //Debug.Log(s.name);
            sprites.Add(s.name,s);
        }
        type.ClearOptions();
        //type.AddOptions(new List<string>(){"none"});
        type.AddOptions(ty.type);
        ChangeData = new UDictionary<string, float>();
        Entry  = new KeyValuePair<string, UDictionary<string, float>>();
        /*EditorUtility.SetDirty(ty);
        EditorUtility.SetDirty(TYS);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();*/
        som.CreateAndSaveScriptableObject(ty,"Types.asset");
        som.CreateAndSaveScriptableObject(TYS,"TypesSkills.asset");
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
        /*EditorUtility.SetDirty(ty);
        EditorUtility.SetDirty(TYS);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();*/
        som.CreateAndSaveScriptableObject(ty,"Types.asset");
        som.CreateAndSaveScriptableObject(TYS,"TypesSkills.asset");
    }
    public void setInput(int option){
        
        input.text = type.options[option].text;
        UDictionary<string, float> stats = new UDictionary<string, float>();
        stats = ty.getTypeStat(input.text); 
        Entry = new KeyValuePair<string, UDictionary<string, float>>(input.text, stats);
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("statbox")){
            Destroy(go);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Skillbox")){
            Destroy(go);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Abilitiesbox")){
            Destroy(go);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("SpriteBox")){
            Destroy(go);
        }
        if(stats != null){
            AddStatBoxes(stats);
        }
        
        if(TYS.getTypeSkills(input.text) != null){
            AddSkillBoxes(TYS.getTypeSkills(input.text));
        }
        if(TYS.getTypeAbilities(input.text) != null){
            AddAbilitiesBoxes(TYS.getTypeAbilities(input.text));
        }
        addSprite();
        ChangeData = stats;
    }
    void AddStatBoxes(UDictionary<string, float> stats){
        
        for(int i = 0; i < stats.Count; i++){
            addInfo(stats.ElementAt(i));
        }
    }
    void AddSkillBoxes(UDictionary<string,int> stats){
        
        for(int i = 0; i < stats.Count; i++){
            addSkill(input.text,stats.ElementAt(i).Key);
        }
    }
    void AddAbilitiesBoxes(UDictionary<string,string> stats){
        
        for(int i = 0; i < stats.Count; i++){
            addAbility(input.text,stats.ElementAt(i).Key);
        }
    }

    public void addSprite(){
        foreach(KeyValuePair<string,Sprite> pair in sprites){
            GameObject goParent = GameObject.Find("SpritePanel");
            GameObject prefab = Resources.Load<GameObject>("ChSprites") as GameObject;
            GameObject player = Instantiate(prefab) as GameObject;
            player.transform.SetParent(goParent.transform);
            player.GetComponent<Image>().sprite = pair.Value;
            player.name = pair.Key;
            if(input.text == player.name){
                player.GetComponent<Selectable>().Select();
                player.GetComponent<BoxFunc>().setSprite();
            }
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
        for(int i = 0; i < ty.type_stats.Count; i++){
            if(ty.type_stats.ElementAt(i)!= null && ty.type_stats.ElementAt(i) == stat.Key){
                dd.value = i;
            }
        }
        //dd.value = dd.options.FindIndex(x => x.text == stat.Key);
        player.transform.Find("StatInput").GetComponent<InputField>().text = (stat.Value).ToString();
        player.GetComponent<BoxFunc>().statslst = ty.type_stats;
        player.GetComponent<BoxFunc>().setfullname(stat.Key);
    }
    public void addBlank(){
        GameObject goParent = GameObject.Find("scrollPanel");
        GameObject prefab = Resources.Load<GameObject>("StatBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        Dropdown dd = player.transform.Find("Statname").GetComponent<Dropdown>();
        dd.ClearOptions();
        dd.AddOptions(ty.type_stats);
        player.GetComponent<BoxFunc>().updateBox();
    }

    public void addSkill(string name, string skill){
        GameObject goParent = GameObject.Find("scrollPanel_Skill");
        GameObject prefab = Resources.Load<GameObject>("SkillBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.name = skill;
        Dropdown dd = player.transform.Find("Statname").GetComponent<Dropdown>();
        dd.ClearOptions();
        dd.AddOptions(ad.getSkillList());
        dd.value = dd.options.FindIndex(x => x.text == skill);
    }
    public void addBlankSkill(){
        GameObject goParent = GameObject.Find("scrollPanel_Skill");
        GameObject prefab = Resources.Load<GameObject>("SkillBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        Dropdown dd = player.transform.Find("Statname").GetComponent<Dropdown>();
        dd.ClearOptions();
        dd.AddOptions(ad.getSkillList());
    }
    public void addAbility(string name, string skill){
        GameObject goParent = GameObject.Find("scrollPanel_Abil");
        GameObject prefab = Resources.Load<GameObject>("AbilitiesBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        player.name = skill;
        Dropdown dd = player.transform.Find("Statname").GetComponent<Dropdown>();
        dd.ClearOptions();
        dd.AddOptions(ad.getAbilList());
        dd.value = dd.options.FindIndex(x => x.text == skill);
    }
    public void addBlankAbilite(){
        GameObject goParent = GameObject.Find("scrollPanel_Abil");
        GameObject prefab = Resources.Load<GameObject>("AbilitiesBox") as GameObject;
        GameObject player = Instantiate(prefab) as GameObject;
        player.transform.SetParent(goParent.transform);
        Dropdown dd = player.transform.Find("Statname").GetComponent<Dropdown>();
        dd.ClearOptions();
        dd.AddOptions(ad.getAbilList());
    }

    public void setText(string name){
        type.options[type.value].text = name;
        type.captionText.text = name;
        if(type.options.Count == ty.type.Count){
            ty.changeTypeKey(type.value,name);
            TYS.changeTypeKey(type.value,name);
        }
        /*EditorUtility.SetDirty(ty);
        EditorUtility.SetDirty(TYS);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();*/
        som.CreateAndSaveScriptableObject(ty,"Types.asset");
        som.CreateAndSaveScriptableObject(TYS,"TypesSkills.asset");
    }
    public void SetSprite(Sprite s){
        selected = s;
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
        UDictionary<string,int> Skills = new UDictionary<string,int>();
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Skillbox")){
            Dropdown dd = go.transform.Find("Statname").GetComponent<Dropdown>();
            string name = dd.options[dd.value].text;
            if(!Skills.ContainsKey(name)){
                Skills.Add(name,0);
            }
        }
        UDictionary<string,string> Abilities = new UDictionary<string,string>();
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Abilitiesbox")){
            Dropdown dd = go.transform.Find("Statname").GetComponent<Dropdown>();
            string name = dd.options[dd.value].text;
            if(!Abilities.ContainsKey(name)){
                Abilities.Add(name,ad.getAbilType(name));
            }
        }

        ChangeData = newData;
        ty.addTypeEntry(type.captionText.text,ChangeData);
        TYS.addTypeAbilities(type.captionText.text,Abilities);
        TYS.addTypeSkill(type.captionText.text,Skills);
        ty.setTypeSprite(type.captionText.text, selected);
        /*EditorUtility.SetDirty(ty);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.SetDirty(TYS);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();*/
        som.CreateAndSaveScriptableObject(ty,"Types.asset");
        som.CreateAndSaveScriptableObject(TYS,"TypesSkills.asset");
    }
}
