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
        string filePath = Application.dataPath + "/Character.txt";
        if (File.Exists(filePath)) {
            File.Delete (filePath);
        }
        string input = "Poggers,Praetorian Guard,Light Glaive,Kite Shield,Praetorian Armor,Buckler,War Horse";
        string input2 = "早上好中國,Imperial Conscript,Great Sword,Tower Shield,Assault Vest,kekw,kekw";
        using StreamWriter file = new(filePath, append: true);
        file.WriteLineAsync(input);
        file.WriteLineAsync(input2);

        Debug.Log("pog");
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
