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

    void Start()
    {
        Sprite[] allsprites = Resources.LoadAll<Sprite>("Daemons");
        foreach(Sprite s in allsprites){
            //Debug.Log(s.name);
            Daemons.Add(s.name,s);
        }
        sceneLoader = GetComponent<SceneLoader>();
        setStage();
    }

    void createCharacter(string tag, KeyValuePair<string, UDictionary<string,string>> ch, int pos){
        GameObject prefab = Resources.Load<GameObject>("ChDemo") as GameObject;
        prefab.name = ch.Key;
        GameObject player = Instantiate(prefab) as GameObject;
        player.name = ch.Key;
        player.tag = tag;
        player.transform.Find("NameIndicator").GetComponentInChildren<Text>().text = ch.Key;
        player.transform.SetParent(transform);
        player.GetComponent<SpriteRenderer>().sprite = Daemons.ElementAt(rnd.Next(0,Daemons.Count)).Value;
        Vector3 panelPos = transform.position;
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        Vector3 allocate = new Vector3Int((int)(rect.rect.xMin + pos * 64) , (int)(rect.rect.yMin - 169),0);
        player.transform.position = allocate;
        player.GetComponent<PositionSetup>().setOrigin(player.transform.position);
    }

    public void setStage(){
        //List<string> lst = ReadInputFileAsList();
        data = AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/Data/InGameData.asset");
        UDictionary<string, UDictionary<string,string>> chlst = data.characterlst;
        int pos = 0;
        foreach(KeyValuePair<string, UDictionary<string,string>> ch in chlst){
            //string[] words = lst[i].Split(',');
            if(ch.Key[0] == 'P'){
                createCharacter("Player",ch,pos);   
            }
            else if(ch.Key[0] == 'E'){
                createCharacter("Enemy",ch,pos);
            }
            pos++;
        }
        
        
    }

    public void LoadMyScene()
    {
        sceneLoader.LoadScene("GameScene");
    }
}
