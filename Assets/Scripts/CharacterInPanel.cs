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
    
    void createCharacter(string tag, KeyValuePair<string, UDictionary<string,string>> ch){
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
    
    public void LoadMyScene()
    {
        if(GameObject.Find("ChPanel").transform.childCount == 0){
            sceneLoader.LoadScene("GameScene");
        }
    }
}
