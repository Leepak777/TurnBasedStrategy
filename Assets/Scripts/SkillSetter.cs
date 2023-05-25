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
    void Awake(){
        type.ClearOptions();
        type.AddOptions(new List<string>(){"none","Active","Passive"});

    }
    public void newSkill(){
        type.AddOptions(new List<string>(){"Skill"});
        type.value = type.options.Count-1;
    }
    public void removeeq(){
        type.options.Remove(type.options[type.value]);
        ad.removeTypeEntry(type.captionText.text);
        type.value = 0;
        //setInput(type.value);
        EditorUtility.SetDirty(ad);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
}
