using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class HighlightReachableTiles : MonoBehaviour
{
    public Tilemap tilemap;
    public int maxTiles = 10;
    public Color highlightColor;
    public Color highlightColor2;
    private List<Vector3Int> reachableTiles = new List<Vector3Int>();
    private List<Vector3Int> unreachableTiles = new List<Vector3Int>();
    public GridGraph gridGraph;


  public void HighlightReachable()
    {
        highlightColor.a = 0.5f;

        reachableTiles.Clear();
        Vector3Int currentPos = tilemap.WorldToCell(transform.position);
        for (int x = -maxTiles; x <= maxTiles; x++)
        {
            for (int y = -maxTiles; y <= maxTiles; y++)
            {
                Vector3Int tilePos = new Vector3Int(currentPos.x + x, currentPos.y + y, currentPos.z);
                if (tilemap.HasTile(tilePos) && gridGraph.GetNodeFromWorld(tilePos).walkable)
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