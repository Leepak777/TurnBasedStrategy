using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;
using Random = System.Random;
using UnityEditor;
using UnityEngine.UI;

public class txtReader : MonoBehaviour
{ 
    Dictionary<string,float> tst;
    Dictionary<string,float> wst;
    Dictionary<string,float> sst;
    Dictionary<string,float> ast;
    Dictionary<string,float> bst;
    Dictionary<string,float> mst;
    public Tilemap tilemap;
    Dictionary<string,Sprite> Daemons = new Dictionary<string,Sprite>();
    public Equipments equipments;
    public Types types;
    public InGameData data;
    Random rnd = new Random();
    TileManager tileM;
    // Start is called before the first frame update
    void Start()
    { 
        Sprite[] allsprites = Resources.LoadAll<Sprite>("Daemons");
        foreach(Sprite s in allsprites){
            //Debug.Log(s.name);
            Daemons.Add(s.name,s);
        }
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        setStage();
        
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
    void setStats(GameObject character, UDictionary<string,string> attributes){
        
        foreach(KeyValuePair<string,string> attribute in attributes){
            if(getAttributeStats(attribute)!=null){
                character.GetComponent<StatUpdate>().setStats(getAttributeStats(attribute));
            }
        }
       
    }

    public void setStage(){
        //List<string> lst = ReadInputFileAsList();
        data = AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/InGameData.asset");
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
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Player");
        GameObject.Find("Main Camera").GetComponent<CameraController>().initLoc(objectsWithTag[0]);

    }

    void createCharacter(string tag, KeyValuePair<string, UDictionary<string,string>> ch){
        GameObject prefab = Resources.Load<GameObject>("PlayerCh") as GameObject;
        prefab.name = ch.Key;
        GameObject player = Instantiate(prefab) as GameObject;
        player.tag = tag;
        player.transform.Find("NameIndicator").GetComponentInChildren<Text>().text = ch.Key;
        player.transform.SetParent(transform);
        player.GetComponent<SpriteRenderer>().sprite = Daemons.ElementAt(rnd.Next(0,Daemons.Count)).Value;
        player.GetComponentInChildren<Ghost>().setSprite(player.GetComponent<SpriteRenderer>().sprite);
        Vector3Int allocate = new Vector3Int(20+rnd.Next(1,7),12+rnd.Next(1,7),0);
        while(!tileM.GetNodeFromWorld(tilemap.WorldToCell(tilemap.GetCellCenterWorld(allocate))).walkable){
            allocate = new Vector3Int(20+rnd.Next(1,7),12+rnd.Next(1,7),0);
        }
        player.transform.position = tilemap.GetCellCenterWorld(allocate);
        tileM.setWalkable(player,tilemap.WorldToCell(player.transform.position),false);
        player.GetComponent<StatUpdate>().setUp();
        setStats(player,ch.Value);
        player.GetComponent<StatUpdate>().setCalStat();
    }
   

    public static List<string> ReadInputFileAsList() {
        string filePath = Application.dataPath + "/Character.txt";

        if (File.Exists(filePath)) {
            return File.ReadAllLines(filePath).ToList();
        } else {
            return new List<string>() { "File not found." };
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
