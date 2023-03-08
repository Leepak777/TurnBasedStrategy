using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

using System;
using System.Linq;
 
public class Node
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;
    public GameObject occupant;
    public Tile tile;

    public Node(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY,Tile tile)
    {
        this.tile = tile;
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

    public Node[,] grid;
    public Node[,] Grid { get { return grid; } }
    private Vector3Int gridSize;
    public Vector3Int GridSize { get { return gridSize; } }
    private List<Node> reachableTiles;
    Dictionary<string,Tile> tiles = new Dictionary<string,Tile>();
    Dictionary<string,Tile> UI = new Dictionary<string,Tile>();
    Dictionary<string,Tile> Water = new Dictionary<string,Tile>();
    Dictionary<string,Tile> House = new Dictionary<string,Tile>();
    void Start()
    {
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        Debug.Log(tilemap.size);
        gridSize = tilemap.size;
        grid = new Node[gridSize.x, gridSize.y];
        reachableTiles = new List<Node>();
        tiles = GameObject.Find("Canvas").GetComponentInChildren<SetMap>().getTiles();
        UI = GameObject.Find("Canvas").GetComponentInChildren<SetMap>().getUI();
        Water = GameObject.Find("Canvas").GetComponentInChildren<SetMap>().getWater();
        House = GameObject.Find("Canvas").GetComponentInChildren<SetMap>().getHouse();
        CreateGrid();
        //printGrid();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
            g.GetComponent<Movement>().setTilemap(tilemap);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
            g.GetComponent<Movement>().setTilemap(tilemap);
    }
    
    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                grid[x, y] = new Node(true, tilemap.GetCellCenterWorld(tilePos), x, y, tilemap.GetTile<Tile>(tilePos));
                reachableTiles.Add(grid[x,y]);
            }
        }
        foreach(Node n in grid){
            setTileSize(n.tile,n.gridX,n.gridY);
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
    void setTileSize(Tile tile,int x, int  y){
        if(House.Any(kvp => kvp.Value.Equals(tile))){
            grid[x,y].walkable = false;
            int width = (int)(tile.sprite.bounds.extents.x);
            int height = (int)(tile.sprite.bounds.extents.y);
            for(int i = x-width; i <= x+width;i++){
                for(int j = y-height; j <= y+height; j++){
                    //if(grid[i,j] != null){
                        grid[i,j].walkable = false;
                    //}
                }
            }
    
        } 
    }

    public void setWalkable(GameObject Ch, Vector3Int world,bool walkable){
        if(walkable && !GetNodeFromWorld(world).walkable){
            GetNodeFromWorld(world).walkable = walkable;
            GetNodeFromWorld(world).occupant = null;
        }
        else if(!walkable && GetNodeFromWorld(world).walkable){
            GetNodeFromWorld(world).walkable = walkable;
            GetNodeFromWorld(world).occupant = Ch;
        }

    }
    public void resetOccupant(Vector3Int world){
        GetNodeFromWorld(world).occupant = null;
    }
    public Node GetNodeFromIndex(int x, int y)
    {
     return grid[x+gridSize.x/2 , y + gridSize.y/2];   
    }
    public Node GetNodeFromWorld(Vector3Int world){
        //Vector3Int tilePos = new Vector3Int(world.x+gridSize.x/2, world.y + gridSize.y/2, 0);
        Vector3 center = tilemap.GetCellCenterWorld(world);

        foreach(Node node in grid){
            if(node.worldPosition.x == center.x && node.worldPosition.y == center.y){
                return node;
            }
        }
        return null;
    }
    public Vector3Int GetWorldFromNode(Node n){
        return new Vector3Int((int)n.worldPosition.x,(int)n.worldPosition.y, 0);
    }
    
    public List<Node> GetReachableTiles()
    {
        return reachableTiles;
    }

    public List<Node> GetTilesInArea(Vector3Int center, float range){
        List<Node> Area = new List<Node>();
        /*int top =   (int) Math.Ceiling(center.y - range);
        int bottom  = (int) Math.Floor(center.y + range);
        for(int y= top; y <= bottom; y++){
            int dy = y - center.y;
            float dx = Mathf.Sqrt(range*range - dy*dy);
            int left = (int) Math.Ceiling(center.x - dx);
            int right = (int) Math.Floor(center.x + dx);
            for(int x = left; x <= right; x++){
                Vector3Int target = new Vector3Int(x, y,center.z);
                if(GetNodeFromWorld(target)!=null){
                    Area.Add(GetNodeFromWorld(target));
                }
            }
        }*/
        for(float x = -range; x <= range; x++){
            for(float y = -range; y <= range; y++){
                if(Mathf.Sqrt(x*x + y*y) < (range+0.5f)){
                    Vector3Int target = new Vector3Int((int)(center.x+x),(int)( center.y+y),center.z);
                    if(GetNodeFromWorld(target)!=null){
                        Area.Add(GetNodeFromWorld(target));
                    }
                }
            }    
        }
        return Area;
    }

      public List<GameObject> getTaginArea(Vector3Int start, float range, string tag){
        List<GameObject> tags = new List<GameObject>(); 
        foreach(Node n in GetTilesInArea(start,range)){
            if(n.occupant != null && n.occupant.tag == tag){
                tags.Add(n.occupant);
            }
        }
        return tags;
    }

    public bool inArea(Vector3Int start,Vector3Int target, float range){
        foreach(Node n in GetTilesInArea(start,range)){
            if(n.gridX == target.x  && n.gridY == target.y){
                return true;
            }
        }
        return false;
    }

    public Vector3Int inAreaTile(Vector3Int start,Vector3Int target, float range){
        foreach(Node n in GetTilesInArea(start,range)){
            if(n.gridX == target.x  && n.gridY == target.y){
                return new Vector3Int((int)n.worldPosition.x, (int) n.worldPosition.y, 0);
            }
        }
        return start;
    }

    public GameObject getClosestPlayer(string tag, Vector3 currentpos){
        int mindis = int.MaxValue;   
        GameObject close = null; 
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);            
        Vector3Int curpos = tilemap.WorldToCell(currentpos);
        foreach(GameObject n in objectsWithTag){
            //Debug.Log(n.occupant.name);
            //Debug.Log(Mathf.Abs((int)(n.worldPosition.x - transform.position.x)) + Mathf.Abs((int)(n.worldPosition.y - transform.position.y)));
            Vector3Int cellpos = tilemap.WorldToCell(n.transform.position);
            int distance = (int)GetDistance(curpos,cellpos);
            if(distance< mindis){
                mindis = distance;
                close = n;
            }  
        }
        return close;
    }

     public KeyValuePair<GameObject,Vector3Int> getClosestReachablePlayer(string tag, Vector3Int currentpos, float attackrange, int movrange){
        int mindis = int.MaxValue;   
        GameObject close = null; 
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);            
        //Vector3Int curpos = tilemap.WorldToCell(currentpos);
        Vector3Int targetNode = currentpos;
        foreach(GameObject n in objectsWithTag){
            //Debug.Log(n.occupant.name);
            //Debug.Log(Mathf.Abs((int)(n.worldPosition.x - transform.position.x)) + Mathf.Abs((int)(n.worldPosition.y - transform.position.y)));
            Vector3Int cellpos = tilemap.WorldToCell(n.transform.position);
            Vector3Int destination = getClosestTiletoObject(n,  currentpos, attackrange, movrange);
            int distance = (int)GetDistance(cellpos,destination);
            if(distance< mindis){
                mindis = distance;
                close = n;
                targetNode = destination;
            }  
        }
        return new KeyValuePair<GameObject, Vector3Int>(close,targetNode);
    }

    public Vector3Int getClosestTiletoObject(GameObject go, Vector3Int originNode, float attackrange, int movrange){
        GameObject player = go;
        Node ans = null;
        int mindis = int.MaxValue;   
        int tiledis = int.MaxValue; 
        Vector3Int cellpos = tilemap.WorldToCell(player.transform.position);
        foreach(Node n in GetTilesInArea(originNode,movrange)){
            Vector3Int target = tilemap.WorldToCell(new Vector3Int((int)n.worldPosition.x,(int)n.worldPosition.y,0));
            int distance = (int)GetDistance(cellpos,target);
            int distance2 = (int)GetDistance(originNode,target);
            if(n.walkable && distance + distance2 < mindis ){
                mindis = distance + distance2;
                ans = n;
            }
        }
        tiledis = int.MaxValue;
        foreach(Node n in GetTilesInArea(originNode,movrange)){
            Vector3Int target = tilemap.WorldToCell(new Vector3Int((int)n.worldPosition.x,(int)n.worldPosition.y,0));
            int distance = (int)GetDistance(cellpos,target);
            int distance2 = (int)GetDistance(originNode,target);
            if(n.walkable && distance == (int)attackrange && distance2 < tiledis){
                tiledis = distance2;
                ans = n;
            }
            
        }
        if(ans == null){
            return originNode;
        }
        return tilemap.WorldToCell(new Vector3Int((int)ans.worldPosition.x, (int) ans.worldPosition.y, 0));
    }

    public void flagEnemyArea(GameObject enemy, string tag,int range){
        enemy.GetComponent<StatUpdate>().Flagging();
        foreach(Node n in GetTilesInArea(tilemap.WorldToCell(enemy.transform.position),range)){
                if(n.occupant != null&& n.occupant.tag == tag){
                    n.occupant.GetComponent<StatUpdate>().Flagging();
                }
        }
    }
    public bool EnemyInRange(string tag, int attackrange, GameObject go){
        Vector3Int currentPos = tilemap.WorldToCell(go.transform.position);
        foreach(Node node in GetTilesInArea(currentPos,attackrange)){
            if(node.occupant!=null){
                if(node.occupant.tag == tag){
                    return true;
                }
            }
        }
        return false;
    }
    public float GetDistance(Vector3Int A, Vector3Int B)
    {
        // Use Manhattan distance for tilemap
        return Mathf.Abs(A.x - B.x) + Mathf.Abs(A.y - B.y);
    }

    public Tilemap getTileMap(){
        return tilemap;
    }
}