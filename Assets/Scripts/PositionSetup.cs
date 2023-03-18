using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;
using Random = System.Random;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class PositionSetup : MonoBehaviour
{
    public TileManager tileM; 
    // Start is called before the first frame update
    public UnityEvent<Vector3Int> unhighlightTile;
    public UnityEvent<Vector3Int> highlightTile;
    Vector3 originNode;
    public InGameData data;
    void Awake()
    { 
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        originNode = Camera.main.WorldToScreenPoint(transform.position);
        data = AssetDatabase.LoadAssetAtPath<InGameData>("Assets/Scripts/Data/InGameData.asset");
    }
    public void setOrigin(Vector3 pos){
        originNode = Camera.main.WorldToScreenPoint(pos);
    }
    void Update(){
        
    }
    void highlightMap(){
        for (int x = 0; x < tileM.GridSize.x; x++)
        {
            for (int y = 0; y < tileM.GridSize.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                highlightTile.Invoke(pos);
            }
        }
    }
    public void moveWithMouse(){
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = tileM.GetCellCenterWorld(tileM.WorldToCell(pos));
        RectTransform r = GameObject.Find("ChPanel").GetComponent<RectTransform>();
        if(RectTransformUtility.RectangleContainsScreenPoint(r, Input.mousePosition, Camera.main)){
            gameObject.transform.SetParent(GameObject.Find("ChPanel").transform);
            //transform.position = Camera.main.ScreenToWorldPoint(originNode);
        }
        else{
            gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
        }
    }
    
    public void highlightCurrent(){
        if(transform.parent.gameObject.name != "ChPanel"){
            highlightTile.Invoke(tileM.WorldToCell(transform.position));
            tileM.setWalkable(this.gameObject,tileM.WorldToCell(transform.position),true); 
        }
    }
    public void unhighlightCurrent(){
        if(!tileM.GetNodeFromWorld(tileM.WorldToCell(transform.position)).walkable){
            gameObject.transform.SetParent(GameObject.Find("ChPanel").transform);
            transform.position = Camera.main.ScreenToWorldPoint(originNode);
            return;
        }
        if(transform.parent.gameObject.name != "ChPanel"){
            unhighlightTile.Invoke(tileM.WorldToCell(transform.position));
            tileM.setWalkable(this.gameObject,tileM.WorldToCell(transform.position),false);
            addEntry();
        }
        else{
            transform.position = Camera.main.ScreenToWorldPoint(originNode);
        }
    }

    public void addEntry(){
        if(data.positions.ContainsKey(gameObject.name)){
            data.positions[gameObject.name] = tileM.WorldToCell(transform.position);    
        }
        else{
            data.positions.Add(gameObject.name, tileM.WorldToCell(transform.position));
        }
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    

}
