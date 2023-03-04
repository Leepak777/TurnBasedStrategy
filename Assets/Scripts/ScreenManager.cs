using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = System.Random;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;

public class ScreenManager : MonoBehaviour
{
    Image SceneManagerImage;
    SceneLoader sceneLoader;
    public static Random rnd = new Random();
    public List<Button> buttons = new List<Button>();
    public static List<string> types;
    // Start is called before the first frame update

    public Tilemap tilemap;
    public Sprite sprite;

    private void Start()
    {
        types = new List<string>(){"Praetorian Guard", "Imperial Legionary", "Imperial Conscript", "Mecenary", "Brigand"};
        SceneManagerImage = this.gameObject.GetComponent<Image>();

        sceneLoader = GetComponent<SceneLoader>();
        /*foreach(GameObject b in GameObject.FindGameObjectsWithTag("button")){
            buttons.Add(b.GetComponent<Button>());
        }*/
        buttons.Add(GameObject.Find("button1").GetComponent<Button>()); 
        buttons.Add(GameObject.Find("button2").GetComponent<Button>()); 
        buttons.Add(GameObject.Find("button3").GetComponent<Button>());
        buttons[0].onClick.AddListener(LoadMyScene);
        buttons[1].onClick.AddListener(CreateCharacterFile);
        buttons[2].onClick.AddListener(CreateMapFile);
    }

    public static void CreateCharacterFile() {
        string[] result = AssetDatabase.FindAssets("InGameData");
        InGameData Data = null;
        if (result.Length > 2)
        {
            Debug.LogError("More than 1 Asset founded");
            return;
        }
     
        if(result.Length == 0)
        {
            Debug.Log("Create new Asset");
            Data = ScriptableObject.CreateInstance<InGameData >();
            AssetDatabase.CreateAsset(Data, @"Assets/Scripts/InGameData.asset");
        }
        else
        {
            string path = AssetDatabase.GUIDToAssetPath(result[0]);
            Data= (InGameData )AssetDatabase.LoadAssetAtPath(path, typeof(InGameData));
            Debug.Log("Found Asset File !!!");
        }
        //InGameData Data = ScriptableObject.CreateInstance<InGameData>();
        Data.characterlst.Clear();
        UDictionary<string,string> PlData = new UDictionary<string, string>(){
            {"Type",types[rnd.Next(0,5)]},{"Weapon","Light Glaive"},{"Shield","Kite Shield"},{"Armor","Praetorian Armor"},{"Buckler","kekw"},{"Mount","War Horse"}
        };
        UDictionary<string,string> EnData = new UDictionary<string, string>(){
            {"Type",types[rnd.Next(0,5)]}/*,{"Weapon","Great Sword"},{"Shield","Tower Shield"},{"Armor","Assault Vest"},{"Buckler","Buckler"},{"Mount","War Horse"}*/
        };
        for(int i = 0; i < 4; i++){
            Data.characterlst.Add("Player"+i,PlData);
            
        }Data.characterlst.Add("Enemy"+0,EnData);
        EditorUtility.SetDirty(Data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    public static void CreateMapFile() {
        string[] result = AssetDatabase.FindAssets("InGameData");
             InGameData Data = null;
     
             if (result.Length > 2)
             {
                 Debug.LogError("More than 1 Asset founded");
                 return;
             }
     
             if(result.Length == 0)
             {
                 Debug.Log("Create new Asset");
                 Data = ScriptableObject.CreateInstance<InGameData >();
                 AssetDatabase.CreateAsset(Data, @"Assets/Scripts/InGameData.asset");
             }
             else
             {
                 string path = AssetDatabase.GUIDToAssetPath(result[0]);
                 Data= (InGameData )AssetDatabase.LoadAssetAtPath(path, typeof(InGameData ));
                 Debug.Log("Found Asset File !!!");
             }
        Data.map = "Never Gonna Give You Up";
        EditorUtility.SetDirty(Data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    void toGameScreen()
    {
        Debug.Log("pog");
        LoadMyScene();
    }
    void LoadMyScene()
    {
        sceneLoader.LoadScene("GameScene");
    }

    // Update is called once per frame
    void Update()
    {        
    }
}
