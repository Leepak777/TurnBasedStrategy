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


public class InitCharacter : MonoBehaviour
{ 
    Dictionary<string,Sprite> Daemons = new Dictionary<string,Sprite>();
    public InGameData data;
    Random rnd = new Random();
    TileManager tileM;
    // Start is called before the first frame update
    void Start()
    { 
        data = AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/Data/InGameData.asset");
        Sprite[] allsprites = Resources.LoadAll<Sprite>("Daemons");
        foreach(Sprite s in allsprites){
            //Debug.Log(s.name);
            Daemons.Add(s.name,s);
        }
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
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
    

    public void setStage(){
        //List<string> lst = ReadInputFileAsList();
        
        UDictionary<string, UDictionary<string,string>> chlst = data.characterlst;
        foreach(KeyValuePair<string, UDictionary<string,string>> ch in chlst){
            //string[] words = lst[i].Split(',');
            if(ch.Key[0] == 'P'){
                createCharacter("Player",ch, data.positions[ch.Key]);   
            }
            else if(ch.Key[0] == 'E'){
                createCharacter("Enemy",ch, data.positions[ch.Key]);
            }
        }
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Player");
        GameObject.Find("Main Camera").GetComponent<CameraController>().initLoc(objectsWithTag[0]);
        
    }

    void createCharacter(string tag, KeyValuePair<string, UDictionary<string,string>> ch, Vector3Int pos){
        GameObject prefab = Resources.Load<GameObject>("PlayerCh") as GameObject;
        prefab.name = ch.Key;
        GameObject player = Instantiate(prefab) as GameObject;
        player.name = ch.Key;
        player.tag = tag;
        player.transform.Find("NameIndicator").GetComponentInChildren<Text>().text = ch.Key;
        player.transform.SetParent(transform);
        player.GetComponent<SpriteRenderer>().sprite = data.sprites[ch.Key];
        player.GetComponentInChildren<Ghost>().setSprite(player.GetComponent<SpriteRenderer>().sprite);
        Vector3Int allocate = pos;//new Vector3Int(pos.y, pos.x, pos.z);
        player.transform.position = tileM.GetCellCenterWorld(allocate);
        tileM.setWalkable(player,tileM.WorldToCell(player.transform.position),false);
        player.GetComponent<ActionCenter>().saveTurnStatData(0);
        player.GetComponent<ActionCenter>().updatePos();
        player.GetComponentInChildren<CharacterEvents>().onCreate.Invoke();
    }
   

    
}
