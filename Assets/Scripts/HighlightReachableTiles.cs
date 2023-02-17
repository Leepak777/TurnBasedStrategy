using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class HighlightReachableTiles : MonoBehaviour
{
    Tilemap tilemap;
    int maxTiles;
    public Color highlightColor;
    public Color highlightColor2;
    private List<Vector3Int> reachableTiles = new List<Vector3Int>();
    private List<Vector3Int> EnemyTiles = new List<Vector3Int>();
    private List<Vector3Int> unreachableTiles = new List<Vector3Int>();
    GridGraph gridGraph;
    public Vector3Int highlightorigin;

    void Start(){
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        gridGraph = GameObject.Find("Tilemanager").GetComponent<GridGraph>();
        maxTiles = this.gameObject.GetComponent<StatUpdate>().getMaxTiles();
                        
    }

  public void HighlightReachable()
    {   
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        if(gridGraph == null){
            gridGraph = GameObject.Find("Tilemanager").GetComponent<GridGraph>();
        }
        highlightColor.a = 0.5f;

        reachableTiles.Clear();
        // maxTiles = this.gameObject.GetComponent<Movement>().tilescheck;
        Vector3Int currentPos = highlightorigin;//tilemap.WorldToCell(transform.position);
        for (int x = -maxTiles; x <= maxTiles; x++)
        {
            for (int y = -maxTiles; y <= maxTiles; y++)
            {
                Vector3Int tilePos = new Vector3Int(currentPos.x + x, currentPos.y + y, currentPos.z);
                if (tilemap.HasTile(tilePos) ){
                    if(  gridGraph.GetNodeFromWorld(tilePos)!= null && gridGraph.GetNodeFromWorld(tilePos).walkable)
                    {
                        int distance = Mathf.Abs(tilePos.x - currentPos.x) + Mathf.Abs(tilePos.y - currentPos.y);
                        if (distance <= maxTiles)
                        {
                            // Save the original tile
                            var temp = tilemap.GetTile(tilePos);

                            // Highlight the tile
                            tilemap.SetTileFlags(tilePos, TileFlags.None);
                            tilemap.SetColor(tilePos, highlightColor);
                            reachableTiles.Add(tilePos);
                            
                            // put the original tile back
                            tilemap.SetTile(tilePos,temp);
                        }
                    }
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

        EnemyTiles.Clear();
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject go in objectsWithTag){
            Vector3Int tilePos = tilemap.WorldToCell(go.transform.position);
            if(GetDistance(tilePos, tilemap.WorldToCell(transform.position)) <= this.gameObject.GetComponent<StatUpdate>().getAttackRange() ){
            
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

    public bool EnemyInRange(){
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject go in objectsWithTag){
            Vector3Int tilePos = tilemap.WorldToCell(go.transform.position);
            if(GetDistance(tilePos, tilemap.WorldToCell(transform.position)) <= this.gameObject.GetComponent<StatUpdate>().getAttackRange() ){
                return true;
            }
        }
        return false;
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
            tilemap.SetTileFlags(tilePos, TileFlags.None);
            tilemap.SetColor(tilePos, Color.white);
        }
        reachableTiles.Clear();
    }
}