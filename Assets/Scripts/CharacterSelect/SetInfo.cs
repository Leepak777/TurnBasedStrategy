using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;
using Random = System.Random;
using UnityEditor;
using UnityEngine.UI;
public class SetInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CharacterStat Data = null;
        Data = ScriptableObject.CreateInstance<CharacterStat>();
        AssetDatabase.CreateAsset(Data, @"Assets/Scripts/Data/"+gameObject.name+".asset");
    }
    public Equipments equipments;
    public Types types;
    public CharacterStat GetAsset(string name){
        InGameData data = AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/Data/InGameData.asset");
        if(data.characterlst.ContainsKey(name)){
            CreateCharacterAsset(name, data.characterlst[name]);
            return AssetDatabase.LoadAssetAtPath<CharacterStat>("Assets/Scripts/Data/"+name+".asset");
        }
        else{
            return null;
        }
    }
    public void GetData(){
        string name = gameObject.name;
        InGameData data = AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/Data/InGameData.asset");
        if(data.characterlst.ContainsKey(name)){
            CreateCharacterAsset(name, data.characterlst[name]);
        }        
    }
    
    
    UDictionary<string,float> getAttributeStats(KeyValuePair<string,string> attribute){
        switch(attribute.Key){
            case "Type":    return types.getTypeStat(attribute.Value);
            case "Weapon":  return equipments.getWeaponStat(attribute.Value);
            case "Shield":  return equipments.getShieldStat(attribute.Value);
            case "Armor":   return equipments.getArmorStat(attribute.Value);
            case "Buckler": return equipments.getBucklerStat(attribute.Value);
            case "Mount":   return equipments.getMountStat(attribute.Value);
        }
        return null;
    }
    void setDataStats(CharacterStat character, UDictionary<string,string> attributes){
        foreach(KeyValuePair<string,string> attribute in attributes){
            if(getAttributeStats(attribute)!=null){
                character.addAttributes(attribute.Key, attribute.Value);
                character.setStats(getAttributeStats(attribute));
            }
        }
        character.setAbilities(types.getTypeAbilities(character.getAttribute("Type")));
    }
    
    public void CreateCharacterAsset(string go, UDictionary<string,string> ch) {    
        string[] result = AssetDatabase.FindAssets("/Data/"+go);
        CharacterStat Data = null;
        if (result.Length > 2)
        {
            Debug.LogError("More than 1 Asset founded");
            return;
        }
        if(result.Length == 0)
        {
            //Debug.Log("Create new Asset");
            Data = ScriptableObject.CreateInstance<CharacterStat>();
            AssetDatabase.CreateAsset(Data, @"Assets/Scripts/Data/"+go+".asset");
        }
        else
        {
            string path = AssetDatabase.GUIDToAssetPath(result[0]);
            Data= (CharacterStat )AssetDatabase.LoadAssetAtPath(path, typeof(CharacterStat ));
            Debug.Log("Found Asset File !!!");
        }
        Data.setUp();
        setDataStats(Data,ch);
        Data.setCalStat();
        EditorUtility.SetDirty(Data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    static void DeleteAssets(string name)
    {
        string targetSubstring = name; // Change this to your desired substring

        string[] assetPaths = AssetDatabase.GetAllAssetPaths(); // Get all asset paths in the project
        int count = 0;

        foreach (string path in assetPaths)
        {
            if (path.EndsWith(".asset") && path.Contains(targetSubstring))
            {
                AssetDatabase.DeleteAsset(path);
                count++;
            }
        }

        AssetDatabase.Refresh(); // Refresh the asset database to update the project window
        //Debug.Log("Deleted " + count + " .asset files with substring '" + targetSubstring + "'");
    }
    

}
