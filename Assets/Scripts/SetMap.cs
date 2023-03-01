using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System;
using System.Linq;
using UnityEditor;


public class SetMap : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile tile;
    public List<string> mapData;
    public Sprite[] allsprites;
    public InGameData data;

    Dictionary<string,Sprite> tileSprite = new Dictionary<string,Sprite>();

    
    void Awake()
    {
        // Load sprite from resources
        allsprites = Resources.LoadAll<Sprite>("TerrainAssets");

        // Load text file and store its contents
        //mapData = ReadInputFileAsList();

        // Create tile asset
        tile = ScriptableObject.CreateInstance<Tile>();
        foreach(Sprite s in allsprites){
            //Debug.Log(s.name);
            tileSprite.Add(s.name,s);
        }
        tile.sprite = tileSprite["TerrainAssets_9"];
        setStage();
        
    }

    public void createTilemap()
    {
        // Check if a Tilemap component is attached to the GameObject
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found");
            return;
        }

        // Cache RectTransform and Vector3Int
        RectTransform rec = GameObject.Find("Canvas").GetComponent<RectTransform>();
        int x_size = (int)rec.sizeDelta.x / 32;
        int y_size = (int)rec.sizeDelta.y / 32;
        if(x_size%2 != 0){
            x_size++;
        }
        if(y_size%2 != 0){
            y_size++;
        }
        Vector3Int size = new Vector3Int(x_size, y_size, 0);
        tilemap.size = size;
        // Calculate total number of tiles
        int tileCount = size.x * size.y;

        // Create array of tiles
        Tile[] tiles = new Tile[tileCount];
        for (int i = 0; i < tileCount; i++)
        {
            tiles[i] = tile;
        }

        // Set tiles on tilemap
        Vector3Int position = Vector3Int.zero;
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                tilemap.SetTile(position + new Vector3Int(i, j, 0), tile);
            }
        }

        // Set scale and position of tilemap
        tilemap.transform.localScale = new Vector3(32, 32, 0);
        tilemap.transform.position = new Vector3(rec.anchoredPosition.x - rec.sizeDelta.x/2, rec.anchoredPosition.y- rec.sizeDelta.y/2, 0);

    }



    public void setStage()
    {
        data = AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/InGameData.asset");
        if (data.map == "Never Gonna Give You Up")
        {
            createTilemap();
        }
        
    }

     public static List<string> ReadInputFileAsList()
        {
            string filePath = Application.dataPath + "/Map.txt";
            List<string> result = new List<string>();

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        result.Add(reader.ReadLine());
                    }
                }
            }
            else
            {
                result.Add("File not found.");
            }

            return result;
        }


}
