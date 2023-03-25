using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System;
using System.Linq;
using UnityEditor;
using Random = System.Random;


public class SetMap : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile tile;
    public List<string> mapData;
    public Sprite[] allsprites;
    public InGameData data;

    Dictionary<string,Sprite> tileSprite = new Dictionary<string,Sprite>();
    Dictionary<string,Tile> tiles = new Dictionary<string,Tile>();
    Dictionary<string,Tile> UI = new Dictionary<string,Tile>();
    Dictionary<string,Tile> Water = new Dictionary<string,Tile>();
    Dictionary<string,Tile> House = new Dictionary<string,Tile>();
    Random rnd = new Random();

    
    void Awake()
    {
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        // Load sprite from resources
        //allsprites = Resources.LoadAll<Sprite>("TerrainAssets");
        allsprites = Resources.LoadAll<Sprite>("EarthHexXY");
        // Load text file and store its contents
        //mapData = ReadInputFileAsList();

        // Create tile asset
        foreach(Sprite s in allsprites){
            //Debug.Log(s.name);
            tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = s;
            tile.name = s.name;
            tileSprite.Add(s.name,s);
            tiles.Add(s.name,tile);
        }
        foreach(Sprite s in  Resources.LoadAll<Sprite>("UI_")){
            //Debug.Log(s.name);
            tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = s;
            tile.name = s.name;
            tileSprite.Add(s.name,s);
            UI.Add(s.name,tile);
        }
        foreach(Sprite s in  Resources.LoadAll<Sprite>("Water")){
            //Debug.Log(s.name);
            tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = s;
            tile.name = s.name;
            tileSprite.Add(s.name,s);
            Water.Add(s.name,tile);
            
        }
        foreach(Sprite s in  Resources.LoadAll<Sprite>("House")){
            //Debug.Log(s.name);
            tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = s;
            tile.name = s.name;
            tileSprite.Add(s.name,s);
            House.Add(s.name,tile);
        }
        
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
        int x_size = (int)rec.sizeDelta.y / 16;
        int y_size = (int)rec.sizeDelta.x / 16;
        if(x_size%2 != 0){
            x_size--;
        }
        if(y_size%2 != 0){
            y_size--;
        }
        Vector3Int size = new Vector3Int((int)(x_size/1.420), (int)(y_size/1.420), 0);
        tilemap.size = size;
        // Calculate total number of tiles
        int tileCount = size.x * size.y;

        // Create array of tiles


        // Set tiles on tilemap
        Vector3Int position = Vector3Int.zero;
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Color c = Color.white; 
                c.a = 0.1f;
                Vector3Int tilePos = position + new Vector3Int(i, j, 0);
                tilemap.SetTile(position + new Vector3Int(i, j, 0), tiles["EarthHexXY"]);
                tilemap.SetTileFlags(tilePos, TileFlags.None);
                tilemap.SetColor(tilePos, c);
            }
        }
        tilemap.SetTile(position + new Vector3Int(15 , 20 , 0), House["House_1"]);
        

        // Set scale and position of tilemap
        tilemap.transform.localScale = new Vector3(16, 16, 0);
        tilemap.transform.position = new Vector3(rec.anchoredPosition.x - rec.sizeDelta.x/2, rec.anchoredPosition.y- rec.sizeDelta.y/2, 0);

    }



    public void setStage()
    {
        data = AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/Data/InGameData.asset");
        if (data.map == "Never Gonna Give You Up")
        {
            createTilemap();
        }
        
    }
    public Dictionary<string,Tile> getTiles(){
        return tiles;
    }
    public Dictionary<string,Tile> getWater(){
        return Water;
    }
    public Dictionary<string,Tile> getUI(){
        return UI;
    }
    public Dictionary<string,Tile> getHouse(){
        return House;
    }

}
