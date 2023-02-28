using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class TileN
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;
    public GameObject occupant;

    public TileN(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }
}

public class TileManager : MonoBehaviour
{
    Tilemap tilemap;
    public LayerMask unwalkableMask;

    public Color highlightColor;
    public Color highlightColor2;

    public TileN[,] grid;
    public TileN[,] Grid { get { return grid; } }
    private Vector3Int gridSize;
    public Vector3Int GridSize { get { return gridSize; } }
    private List<TileN> reachableTiles;
    void Start()
    {
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        Debug.Log(tilemap.size);
        gridSize = tilemap.size;
        grid = new TileN[gridSize.x, gridSize.y];
        reachableTiles = new List<TileN>();
        CreateGrid();
        //printGrid();
    }
    
    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                grid[x, y] = new TileN(true, tilemap.GetCellCenterWorld(tilePos), x, y);
                reachableTiles.Add(grid[x,y]);
            }
        }
    }
    public void printGrid(){
            for(int i = 0; i < gridSize.x; i++){
                for(int j = 0; j < gridSize.y; j++){
                    //Debug.Log(grid[i,j].gridX+", "+grid[i,j].gridY +": "+ grid[i,j].walkable);
                    Debug.Log(grid[i,j].worldPosition);
                    Vector3Int tilePos = new Vector3Int(i, j, 0);

                        var temp = tilemap.GetTile(tilePos);

                        // Highlight the tile
                        tilemap.SetTileFlags(tilePos, TileFlags.None);
                        tilemap.SetColor(tilePos, highlightColor);
                        
                        // put the original tile back
                        tilemap.SetTile(tilePos,temp);
        
                }
            }
    }

    void Update(){
         
    }

    public void setWalkable(GameObject Ch, Vector3Int world,bool walkable){
        if(walkable && !GetTileNFromWorld(world).walkable){
            GetTileNFromWorld(world).walkable = walkable;
            GetTileNFromWorld(world).occupant = null;
        }
        else if(!walkable && GetTileNFromWorld(world).walkable){
            GetTileNFromWorld(world).walkable = walkable;
            GetTileNFromWorld(world).occupant = Ch;
        }

    }
    public void resetOccupant(Vector3Int world){
        GetTileNFromWorld(world).occupant = null;
    }
    public TileN GetTileNFromIndex(int x, int y)
    {
     return grid[x+gridSize.x/2 , y + gridSize.y/2];   
    }
    public TileN GetTileNFromWorld(Vector3Int world){
        //Vector3Int tilePos = new Vector3Int(world.x+gridSize.x/2, world.y + gridSize.y/2, 0);
        Vector3 center = tilemap.GetCellCenterWorld(world);

        foreach(TileN TileN in grid){
            if(TileN.worldPosition.x == center.x && TileN.worldPosition.y == center.y){
                return TileN;
            }
        }
        return null;
    }
    public Vector3Int GetWorldFromTileN(TileN n){
        return new Vector3Int((int)n.worldPosition.x,(int)n.worldPosition.y, 0);
    }
    
    public List<TileN> GetReachableTiles()
    {
        return reachableTiles;
    }



    public List<TileN> GetTilesInArea(Vector3Int center, float range){
        List<TileN> Area = new List<TileN>();
        for(int x = (int)-range; x <= (int)range; x++){
            for(int y = (int)-range; y <= (int)range; y++){
                if(Mathf.Sqrt(x*x + y*y) <= range){
                    Vector3Int target = new Vector3Int(center.x+x, center.y+y,center.z);
                    if(GetTileNFromWorld(target)!=null){
                        Area.Add(GetTileNFromWorld(target));
                    }
                }
            }    
        }
        return Area;
    }

  

}