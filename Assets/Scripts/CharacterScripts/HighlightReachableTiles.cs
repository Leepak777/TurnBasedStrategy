using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class HighlightReachableTiles : MonoBehaviour
{
    //int maxTiles;

    Color highlightColor = Color.blue;
    Color highlightColor2 = Color.red;
    private List<Vector3Int> reachableTiles = new List<Vector3Int>();
    private List<Vector3Int> EnemyTiles = new List<Vector3Int>();
    public TileManager tileM;
    void Start(){
        tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
    }
    //highlight movement area
    public void HighlightReachable()
    {   
        highlightColor.a = 0.5f;
        GameObject character = this.gameObject;
        reachableTiles.Clear();
        Vector3Int currentPos = character.GetComponent<Teleport>().getOrigin();//tileM.WorldToCell(transform.position);
        foreach(Node node in tileM.GetTilesInArea(currentPos,(character.GetComponent<Teleport>().getTilesCheck()))){
                Vector3Int tilePos = new Vector3Int((int)node.gridX , (int)node.gridY , 0);
                if (tileM.HasTile(tilePos) && node.walkable&& !character.GetComponent<Teleport>().getTrail().Contains(tilePos)){
                    //if(tileM.GetNodeFromWorld(tilePos)!= null && tileM.GetNodeFromWorld(tilePos).occupant == null && !character.GetComponent<Teleport>().getTrail().Contains(tilePos))
                    //{
                            // Save the original tile
                            var temp = tileM.GetTile(tilePos);
                            // Highlight the tile
                            tileM.SetTileFlags(tilePos, TileFlags.None);
                            tileM.SetColor(tilePos, highlightColor);
                            reachableTiles.Add(tilePos);
                            
                            // put the original tile back
                            tileM.SetTile(tilePos,temp);
                        //}
                    //}
                }
        }
    }
    //highlight tile
    public void highlight(Vector3Int tile){
        Color c = Color.red; 
        c.a = 0.5f;
        tileM.SetTileFlags(tile, TileFlags.None);
        tileM.SetColor(tile, c);

    }
    //unhighlight tile
    public void unhighlight(Vector3Int tile){
        Color c = Color.white; 
        c.a = 0.1f;
        tileM.SetTileFlags(tile, TileFlags.None);
        tileM.SetColor(tile, c);

    }
    //highlight enemy in range
    public void HighlightEnemy(){
        GameObject character = this.gameObject;
        highlightColor2.a = 0.5f;
        //Debug.Log(this.gameObject.GetComponent<StatUpdate>().getAttackRange());
        EnemyTiles.Clear();
        Vector3Int currentPos = tileM.WorldToCell(character.transform.position);
        foreach(Node node in tileM.GetTilesInArea(currentPos,(int)character.GetComponent<StatUpdate>().getAttackRange())){
            GameObject go = null;
            if(node.occupant!=null){
                if(node.occupant.tag == "Enemy"){
                    go = node.occupant;
                }
            }
            if(go!=null){
            Vector3Int tilePos = tileM.WorldToCell(go.transform.position);
             var temp = tileM.GetTile(tilePos);

            // Highlight the tile
            tileM.SetTileFlags(tilePos, TileFlags.None);
            tileM.SetColor(tilePos, highlightColor2);
            EnemyTiles.Add(tilePos);
                        
            // put the original tile back
            tileM.SetTile(tilePos,temp);
            }
        }
    }

    //unhighligh enemy
    public void UnhighlightEnemy(){
            for (int i = 0; i < EnemyTiles.Count; i++)
            {
                Color c = Color.white; 
                c.a = 0.1f;
                Vector3Int tilePos = EnemyTiles[i];
                tileM.SetTileFlags(tilePos, TileFlags.None);
                tileM.SetColor(tilePos, c);
            }
            EnemyTiles.Clear();
        
    }

    public void UnhighlightReachable()
    {
        for (int i = 0; i < reachableTiles.Count; i++)
        {
            Color c = Color.white; 
            c.a = 0.1f;
            Vector3Int tilePos = reachableTiles[i];
            tileM.SetTileFlags(tilePos, TileFlags.None);
            tileM.SetColor(tilePos, c);
            
        }
        reachableTiles.Clear();
        
    }

    public void UnhighlightMoveTrail()
    {
        List<Vector3Int> trail = gameObject.GetComponent<Teleport>().getTrail();
        trail.Add(gameObject.GetComponent<Teleport>().getOrigin());
        for (int i = 0; i < trail.Count; i++)
        {
            Color c = Color.white; 
            c.a = 0.1f;
            Vector3Int tilePos = trail[i];
            tileM.SetTileFlags(tilePos, TileFlags.None);
            tileM.SetColor(tilePos, c);
            
        }        
    }
    //hightlight trail after movement
    public void highlightMoveTrail()
    {
        Vector3Int tile;
        List<Vector3Int> trail = gameObject.GetComponent<Teleport>().getTrail();
        trail.Add(gameObject.GetComponent<Teleport>().getOrigin());
        for (int i = 0; i < trail.Count; i++)
        {
            Color c = Color.red; 
            c.a = 0.5f;
            tile = trail[i];
            tileM.SetTileFlags(tile, TileFlags.None);
            tileM.SetColor(tile, c);
            
        }        
    }

}