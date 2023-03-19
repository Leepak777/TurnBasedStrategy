using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
    public UnityEvent<Vector3Int> highlightTile;
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
        if(SceneManager.GetActiveScene().name == "MapSelection"){
            GameObject.Find("HighLight").GetComponentInChildren<PositionSetup>().tileM = this;
            GameObject.Find("HighLight").GetComponentInChildren<HighlightReachableTiles>().tileM = this;
            highlightMap();
        }
        else{
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
                g.GetComponent<Teleport>().settileM(this);
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
                g.GetComponent<Teleport>().settileM(this);
        }
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
                    /*Vector3Int tilePos = new Vector3Int(i, j, 0);

                        var temp = tilemap.GetTile(tilePos);

                        // Highlight the tile
                        tilemap.SetTileFlags(tilePos, TileFlags.None);
                        tilemap.SetColor(tilePos, highlightColor);
                        
                        // put the original tile back
                        tilemap.SetTile(tilePos,temp);*/
        
                }
            }
    }

    
    void setTileSize(Tile tile,int x, int  y){
        if(House.Any(kvp => kvp.Value.Equals(tile))){
            grid[x,y].walkable = false;
            int width = (int)(tile.sprite.bounds.extents.x);
            int height = (int)(tile.sprite.bounds.extents.y);
            for(int i = x-width; i <= x+width;i++){
                for(int j = y-height; j <= y+height; j++){
                    if(grid[i,j] != null){
                        grid[i,j].walkable = false;
                    }
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
        for(float x = -range; x <= range; x++){
            for(float y = -range; y <= range; y++){
                if((int)(x*x + y*y) <= (int)(range*range)){
                    Vector3Int target = new Vector3Int((int)(center.x+x),(int)(center.y+y),center.z);
                    if(GetNodeFromWorld(target)!=null){
                        Area.Add(GetNodeFromWorld(target));
                    }
                }
            }    
        }
        if((int) range > 1){
            Vector3Int top = new Vector3Int((int)(center.x),(int)(center.y+range),center.z);
            Vector3Int bottom = new Vector3Int((int)(center.x),(int)(center.y-range),center.z);
            Vector3Int left = new Vector3Int((int)(center.x+range),(int)(center.y),center.z);
            Vector3Int right = new Vector3Int((int)(center.x-range),(int)(center.y),center.z);
            Area.Remove(GetNodeFromWorld(top));
            Area.Remove(GetNodeFromWorld(bottom));
            Area.Remove(GetNodeFromWorld(left));
            Area.Remove(GetNodeFromWorld(right));
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

    public bool entityInRange(GameObject attack, GameObject defense, float range){
        Vector3Int atk = tilemap.WorldToCell(attack.transform.position);
        Vector3Int def = tilemap.WorldToCell(defense.transform.position);
        return inArea(atk, def, (int) range);
    }
    public bool inArea(Vector3Int start,Vector3Int target, float range){
        /*foreach(Node n in GetTilesInArea(start,range)){
            if(n.gridX == target.x  && n.gridY == target.y){
                return true;
            }
        }*/
        return GetTilesInArea(start,range).Contains(GetNodeFromWorld(target));;
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

     public KeyValuePair<GameObject,Vector3Int> getClosestReachablePlayer(string tag, Vector3Int currentpos, float attackrange, float movrange){
        GameObject close = getClosestPlayer(tag, tilemap.GetCellCenterWorld(currentpos));            
        Debug.Log(close.name);
        Vector3Int targetNode = getClosestTiletoObject(close, currentpos, attackrange, movrange);
        return new KeyValuePair<GameObject, Vector3Int>(close,targetNode);
    }

    public Vector3Int getClosestTiletoObject(GameObject go, Vector3Int originNode, float attackrange, float movrange){
        GameObject player = go;
        Node ans = null;
        int mindis = int.MaxValue;   
        Vector3Int cellpos = tilemap.WorldToCell(player.transform.position);
        foreach(Node n in GetTilesInArea(originNode,movrange)){
            Vector3Int target = new Vector3Int((int)n.gridX,(int)n.gridY,0);
            int distance = (int)GetDistance(cellpos,target);
            int distance2 = (int)GetDistance(originNode,target);
            if(n.walkable && distance  < mindis){
                mindis = distance ;
                ans = n;
            }
        }
        if(ans == null){
            return originNode;
        }
        return tilemap.WorldToCell(new Vector3Int((int)ans.worldPosition.x, (int) ans.worldPosition.y, 0));
    }

    public bool EnemyInRange(string tag, float attackrange, GameObject go){
        Vector3Int currentPos = tilemap.WorldToCell(go.transform.position);
        foreach(Node node in GetTilesInArea(currentPos,(attackrange))){
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
    public bool IsAdjacent(Vector3Int node1, Vector3Int node2)
    {
        int xDiff = Mathf.Abs(node1.x - node2.x);
        int yDiff = Mathf.Abs(node1.y - node2.y);
        if (xDiff + yDiff == 1)
        {
            return true;
        }
        return false;
    }
    void highlightMap(){
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if(grid[x,y].walkable){
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    highlightTile.Invoke(pos);
                }
            }
        }
    }

    public Tilemap getTileMap(){
        return tilemap;
    }

    public Vector3Int WorldToCell(Vector3 pos){
        return tilemap.WorldToCell(pos);
    }
    public Vector3 GetCellCenterWorld(Vector3Int pos){
        return tilemap.GetCellCenterWorld(pos);
    }
    public bool HasTile(Vector3Int pos){
        return tilemap.HasTile(pos);
    }
    public TileBase GetTile(Vector3Int pos){
        return tilemap.GetTile(pos);
    }
    public void SetTileFlags(Vector3Int pos, TileFlags flag){
        tilemap.SetTileFlags(pos,flag);
    }
    public void SetTile(Vector3Int pos, TileBase tile){
        tilemap.SetTile(pos,tile);
    }
    public void SetColor(Vector3Int pos, Color c){
        tilemap.SetColor(pos,c);
    }
}