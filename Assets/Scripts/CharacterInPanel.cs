using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;
using Random = System.Random;
using UnityEditor;
using UnityEngine.UI;

public class CharacterInPanel : MonoBehaviour
{
    // Start is called before the first frame update
    Dictionary<string,Sprite> Daemons = new Dictionary<string,Sprite>();
    public InGameData data;
    Random rnd = new Random();
    TileManager tileM;
    SceneLoader sceneLoader;
    public Equipments equipments;
    public Types types;

    void Start()
    {
        data = AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/Data/InGameData.asset");
        Sprite[] allsprites = Resources.LoadAll<Sprite>("Daemons");
        foreach(Sprite s in allsprites){
            //Debug.Log(s.name);
            Daemons.Add(s.name,s);
        }
        setStage();
        sceneLoader = GetComponent<SceneLoader>();

    }
    //words
    /*
    0. Player/Enemy
    1. Type
    2. Weapon
    3. shield
    4. armor
    5. buckler
    6. mount
    */
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
       
    }
    void createCharacter(string tag, KeyValuePair<string, UDictionary<string,string>> ch){
        CreateCharacterAsset(ch.Key,ch.Value);
        GameObject prefab = Resources.Load<GameObject>("ChDemo") as GameObject;
        prefab.name = ch.Key;
        GameObject player = Instantiate(prefab) as GameObject;
        player.name = ch.Key;
        player.tag = tag;
        player.transform.Find("NameIndicator").GetComponentInChildren<Text>().text = ch.Key;
        player.transform.SetParent(transform);
        player.GetComponent<SpriteRenderer>().sprite = data.sprites[ch.Key];
    }

    public void setStage(){
        //List<string> lst = ReadInputFileAsList();
        
        UDictionary<string, UDictionary<string,string>> chlst = data.characterlst;
        foreach(KeyValuePair<string, UDictionary<string,string>> ch in chlst){
            //string[] words = lst[i].Split(',');
            if(ch.Key[0] == 'P'){
                createCharacter("Player",ch);   
            }
            else if(ch.Key[0] == 'E'){
                createCharacter("Enemy",ch);
            }
        }
        
        
    }
    public void CreateCharacterAsset(string go, UDictionary<string,string> ch) {    
        DeleteAssets(go);
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
    public void LoadMyScene()
    {
        if(GameObject.Find("ChPanel").transform.childCount == 0){
            sceneLoader.LoadScene("GameScene");
        }
    }

    [MenuItem("Tools/Delete Assets with Substring")]
    static void DeleteAssets(string name)
    {
        string targetSubstring = name; // Change this to your desired substring

        string[] guids = AssetDatabase.FindAssets("t:Object"); // Find all assets in the project
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains(targetSubstring))
            {
                AssetDatabase.DeleteAsset(path);
                count++;
            }
        }

        AssetDatabase.Refresh(); // Refresh the asset database to update the project window
        Debug.Log("Deleted " + count + " assets with substring '" + targetSubstring + "'");
    }
}
