using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class HighlightReachableTiles : MonoBehaviour
{
    Tilemap tilemap;
    //int maxTiles;

    public Color highlightColor;
    public Color highlightColor2;
    private List<Vector3Int> reachableTiles = new List<Vector3Int>();
    private List<Vector3Int> EnemyTiles = new List<Vector3Int>();
    private List<Vector3Int> unreachableTiles = new List<Vector3Int>();
    TileManager tileM;
    public Vector3Int highlightorigin;

    void Start(){
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
        
                        
    }

  public void HighlightReachable()
    {   
        highlightColor.a = 0.5f;

        reachableTiles.Clear();
        Vector3Int currentPos = highlightorigin;//tilemap.WorldToCell(transform.position);
        foreach(Node node in tileM.GetTilesInArea(currentPos,this.gameObject.GetComponent<StatUpdate>().getMaxTiles())){
                Vector3Int tilePos = new Vector3Int((int)node.gridX , (int)node.gridY , 0);
                if (tilemap.HasTile(tilePos) ){
                    if(tileM.GetNodeFromWorld(tilePos)!= null && tileM.GetNodeFromWorld(tilePos).occupant == null)
                    {
                            // Save the original tile
                            var temp = tilemap.GetTile(tilePos);
                            // Highlight the tile
                            tilemap.SetTileFlags(tilePos, TileFlags.None);
                            tilemap.SetColor(tilePos, highlightColor);
                            reachableTiles.Add(tilePos);
                            
                            // put the original tile back
                            tilemap.SetTile(tilePos,temp);
                        //}
                    }
                }
        }
    }

    float GetDistance(Vector3Int A, Vector3Int B)
    {
        // Use Manhattan distance for tilemap
        return Mathf.Abs(A.x - B.x) + Mathf.Abs(A.y - B.y);
    }

    public void HighlightEnemy(){
        highlightColor2.a = 0.5f;
        //Debug.Log(this.gameObject.GetComponent<StatUpdate>().getAttackRange());
        EnemyTiles.Clear();
        Vector3Int currentPos = tilemap.WorldToCell(transform.position);
        foreach(Node node in tileM.GetTilesInArea(currentPos,this.gameObject.GetComponent<StatUpdate>().getAttackRange())){
            GameObject go = null;
            if(node.occupant!=null){
                if(node.occupant.tag == "Enemy"){
                    go = node.occupant;
                }
            }
            if(go!=null){
            Vector3Int tilePos = tilemap.WorldToCell(go.transform.position);
             var temp = tilemap.GetTile(tilePos);

            // Highlight the tile
            tilemap.SetTileFlags(tilePos, TileFlags.None);
            tilemap.SetColor(tilePos, highlightColor2);
            EnemyTiles.Add(tilePos);
                        
            // put the original tile back
            tilemap.SetTile(tilePos,temp);
            }
        }
    }

    
    public void UnhighlightEnemy(){
            for (int i = 0; i < EnemyTiles.Count; i++)
            {
                Vector3Int tilePos = EnemyTiles[i];
                tilemap.SetTileFlags(tilePos, TileFlags.None);
                tilemap.SetColor(tilePos, Color.white);
            }
            EnemyTiles.Clear();
        
    }

    public void UnhighlightReachable()
    {
        for (int i = 0; i < reachableTiles.Count; i++)
        {
            
            Vector3Int tilePos = reachableTiles[i];
            //if(tilemap.GetColor(tilePos) != Color.red){
                tilemap.SetTileFlags(tilePos, TileFlags.None);
                tilemap.SetColor(tilePos, Color.white);
        }
        reachableTiles.Clear();
        
    }


}