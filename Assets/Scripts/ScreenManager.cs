using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Tilemaps;
using UnityEditor;

public class ScreenManager : MonoBehaviour
{
    Image SceneManagerImage;
    SceneLoader sceneLoader;
    public List<Button> buttons = new List<Button>();
    // Start is called before the first frame update

    public Tilemap tilemap;
    public Sprite sprite;

    private void Start()
    {
        SceneManagerImage = this.gameObject.GetComponent<Image>();

        sceneLoader = GetComponent<SceneLoader>();
        foreach(GameObject b in GameObject.FindGameObjectsWithTag("button")){
            buttons.Add(b.GetComponent<Button>());
        }
        buttons[0].onClick.AddListener(LoadMyScene);
        buttons[1].onClick.AddListener(CreateCharacterFile);
        buttons[2].onClick.AddListener(CreateMapFile);
    }

    public static void CreateCharacterFile() {
        InGameData Data = ScriptableObject.CreateInstance<InGameData>();
        Data.characterlst.Clear();
        Dictionary<string,string> PlData = new Dictionary<string, string>(){
            {"Type","Praetorian Guard"},{"Weapon","Light Glaive"},{"Shield","Kite Shield"},{"Armor","Praetorian Armor"},{"Buckler","kekw"},{"Mount","kekw"}
        };
        Dictionary<string,string> EnData = new Dictionary<string, string>(){
            {"Type","Imperial Conscript"},{"Weapon","Great Sword"},{"Shield","Tower Shield"},{"Armor","Assault Vest"},{"Buckler","Buckler"},{"Mount","War Horse"}
        };
        for(int i = 0; i < 4; i++){
            Data.characterlst.Add("Player"+i,PlData);
            Data.characterlst.Add("Enemy"+i,EnData);
        }
        AssetDatabase.CreateAsset(Data, "Assets/Scripts/InGameData.asset");

    }

    public static void CreateMapFile() {
        string filePath = Application.dataPath + "/Map.txt";
        if (File.Exists(filePath)) {
            File.Delete (filePath);
        }
        string input = "Never Gonna Give You Up,Never Gonna Let You Down";
        using StreamWriter file = new(filePath, append: true);
        file.WriteLineAsync(input);


        Debug.Log("pog");
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
