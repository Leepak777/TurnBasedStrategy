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
                
                // Set the TileFlags.Transparent flag for each tile
                TileBase tile = tilemap.GetTile(tilePos);
                if (tile != null && tile.name == "EarthHexXY")
                {
                    tilemap.SetTileFlags(tilePos, TileFlags.None);
                    tilemap.SetColor(tilePos, new Color(1f, 1f, 1f, 0f)); // Set alpha to 0 to make the tile transparent
                }
            }
        }
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(tilePos);
                if (tile != null)
                {
                    setTileSize(grid[x,y].tile,x,y);
                }
            }
        } 
    }

    public void printGrid(){
            for(int i = 0; i < gridSize.x; i++){
                for(int j = 0; j < gridSize.y; j++){
                    Debug.Log(grid[i,j].worldPosition);
        
               }
            }
    }

    void setTileSize(Tile tile, int x, int y) {
        Vector3 pos = GetCellCenterWorld(new Vector3Int(x, y, 0));
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacles")) {
            SpriteRenderer spriteRenderer = obstacle.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null) {
                Bounds spriteBounds = spriteRenderer.bounds;
                if (spriteBounds.Contains(pos) || 
                    (pos.x > spriteBounds.min.x - 5f && pos.x < spriteBounds.max.x + 5f &&
                    pos.y > spriteBounds.min.y - 5f && pos.y < spriteBounds.max.y + 5f)) {
                    grid[x, y].walkable = false;
                    break;
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


    public List<Node> GetTilesInArea(Vector3Int center, float range) {
        List<Node> area = new List<Node>();
        Vector3Int centerCube = OffsetToCube(center);
        for (int x = -Mathf.FloorToInt(range); x <= Mathf.FloorToInt(range); x++) {
            for (int y = -Mathf.FloorToInt(range); y <= Mathf.FloorToInt(range); y++) {
                Vector3Int target = CubeToOffset(centerCube + new Vector3Int(x, -x-y, y));
                if (GetDistanceCube(centerCube, OffsetToCube(target)) <= Mathf.FloorToInt(range)) {
                    Node node = GetNodeFromWorld(target);
                    if (node != null) {
                        area.Add(node);
                    }
                }
            }
        }
        return area;
    }


    private Vector3Int OffsetToCube(Vector3Int offset) {
        int x = offset.x - (offset.y - (offset.y&1)) / 2;
        int z = offset.y;
        int y = -x-z;
        return new Vector3Int(x, y, z);
    }

    private Vector3Int CubeToOffset(Vector3Int cube) {
        int x = cube.x + (cube.z - (cube.z&1)) / 2;
        int y = cube.z;
        return new Vector3Int(x, y, -x-y);
    }

    private int GetDistanceCube(Vector3Int a, Vector3Int b) {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
    }

    private bool IsAdjacentCube(Vector3Int node1, Vector3Int node2) {
        return GetDistanceCube(node1, node2) == 1;
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
        return GetTilesInArea(start,range).Contains(GetNodeFromWorld(target));
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
        //Debug.Log(close.name);         
        Vector3Int targetNode = getCloestTile(WorldToCell(close.transform.position), currentpos, attackrange, movrange);
        return new KeyValuePair<GameObject, Vector3Int>(close,targetNode);
    }
    public Vector3Int getCloestTile(Vector3Int targetNode, Vector3Int originNode, float attackrange, float movrange){
        if(GetNodeFromWorld(targetNode).occupant != null){
            Vector3Int pos = WorldToCell(GetNodeFromWorld(targetNode).occupant.transform.position);
            return (getClosestTiletoObject(pos, originNode, attackrange, movrange));
        }
        if(GetNodeFromWorld(targetNode).walkable && inArea(originNode,targetNode,movrange)){
            return targetNode;
        }
        else{
            return (getClosestTiletoObject(targetNode, originNode, attackrange, movrange));
        }
    }

   public Vector3Int getClosestTiletoObject(Vector3Int go, Vector3Int originNode, float attackRange, float movRange)
    {
        Vector3Int closestTileToGo = originNode;
        Vector3Int closestTileToOrigin = go;
        List<Node> areaInMovRange = GetTilesInArea(originNode, movRange);
        List<Node> areaInAttackRange = GetTilesInArea(go, attackRange);
        
        // Get the closest tile to the object within the movement range
        foreach (Node tile in areaInMovRange)
        {
            if(tile.walkable){
                Vector3Int tilePos = new Vector3Int((int)tile.gridX, (int)tile.gridY, originNode.z);
                if (GetDistance(go, tilePos) < GetDistance(go, closestTileToGo))
                {
                    closestTileToGo = tilePos;
                }
            }
        }
        
        // Get the farthest tile from the object within the attack range
        foreach (Node tile in areaInAttackRange)
        {
            if(tile.walkable){
                Vector3Int tilePos = new Vector3Int((int)tile.gridX, (int)tile.gridY, originNode.z);
                if (GetDistance(go, tilePos) > GetDistance(go, closestTileToOrigin))
                {
                    closestTileToOrigin = tilePos;
                }
            }
        }
        
        // Check if there is a tile in both areas
        foreach (Node tile in areaInMovRange)
        {
            if(tile.walkable){
                Vector3Int tilePos = new Vector3Int((int)tile.gridX, (int)tile.gridY, originNode.z);
                if (areaInAttackRange.Contains(tile))
                {
                    return tilePos;
                }   
            }
        }
        
        // If there is no tile in both areas, return the closest tile to the object within the movement range
        return closestTileToGo;
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
    public float GetDistance(Vector3Int a, Vector3Int b) {
        int deltaX = Mathf.Abs(a.x - b.x);
        int deltaY = Mathf.Abs(a.y - b.y);
        int deltaZ = Mathf.Abs(a.z - b.z);

        return Mathf.Max(deltaX, deltaY, deltaZ);
    }


    public bool IsAdjacent(Vector3Int node1, Vector3Int node2)
    {
        int deltaX = Mathf.Abs(node1.x - node2.x);
        int deltaY = Mathf.Abs(node1.y - node2.y);

        // Determine the offset of node1 based on its x position.
        int offset = node1.x % 2 == 0 ? 0 : 1;

        // Hexagons with a flat top have two possible adjacent hexagons
        // in the horizontal direction depending on the column of the node.
        bool isAdjacentHorizontal = (deltaX == 1 && deltaY == 0) || (deltaX == 1 && deltaY == 1 && node1.y % 2 == offset);

        // Hexagons with a flat top have two possible adjacent hexagons
        // in the vertical direction depending on the row of the node.
        bool isAdjacentVertical = (deltaX == 0 && deltaY == 1) || (deltaX == 1 && deltaY == 1 && node1.y % 2 != offset);

        return isAdjacentHorizontal || isAdjacentVertical;
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