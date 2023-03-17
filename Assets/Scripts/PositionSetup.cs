using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PositionSetup : MonoBehaviour
{
    public TileManager tileM; 
    // Start is called before the first frame update
    public UnityEvent<Vector3Int> unhighlightTile;
    public UnityEvent<Vector3Int> highlightTile;
    
    void Start()
    { 
        
    }

    void Update(){
        Vector3Int mousePos = tileM.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if(tileM.HasTile(mousePos)){
            //unhighlightTile.Invoke(mousePos);
        } 
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
    
}
