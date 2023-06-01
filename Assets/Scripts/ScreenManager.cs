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
        buttons.Add(GameObject.Find("button3").GetComponent<Button>());
        buttons[0].onClick.AddListener(LoadMyScene);
        buttons[2].onClick.AddListener(CreateMapFile);
    }

    

    public static void CreateMapFile() {
        ScriptableObjectManager som = new ScriptableObjectManager("Assets/Scripts/Data/");

        string[] result = som.FindFilesByName("InGameData");
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
                 som.CreateAndSaveScriptableObject(Data, "InGameData.asset");
             }
             else
             {
                 Data= som.LoadScriptableObject<InGameData>("InGameData.asset");
                 Debug.Log("Found Asset File !!!");
             }
        Data.map = "Never Gonna Give You Up";
        /*EditorUtility.SetDirty(Data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();*/
        som.CreateAndSaveScriptableObject(Data,"InGameData.asset");
    }


    void toGameScreen()
    {
        Debug.Log("pog");
        LoadMyScene();
    }
    void LoadMyScene()
    {
        sceneLoader.LoadScene("MapSelection");
    }

    // Update is called once per frame
    void Update()
    {        
    }
}
