using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class Node
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;
    public GameObject occupant;
    public Node(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }
}

public class GridGraph : MonoBehaviour
{
    public Tilemap tilemap;
    public LayerMask unwalkableMask;

    public Node[,] grid;
    public Node[,] Grid { get { return grid; } }
    private Vector3Int gridSize;
    public Vector3Int GridSize { get { return gridSize; } }
    private List<Node> reachableTiles;
    void Awake()
    {
        gridSize = tilemap.size;
        grid = new Node[gridSize.x, gridSize.y];
        CreateGrid();
        //printGrid();
    }
    
    void CreateGrid()
    {
        grid = new Node[gridSize.x, gridSize.y];
        //Debug.Log(gridSize);
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3Int tilePos = new Vector3Int(x - gridSize.x/2, y- gridSize.y/2, 0);
                //Debug.Log(tilemap.GetCellCenterWorld(tilePos));
                RaycastHit2D hit = Physics2D.BoxCast(tilemap.GetCellCenterWorld(tilePos), new Vector2(8f, 8f), 0f, Vector2.zero, 0f, unwalkableMask);
                bool walkable = (hit.collider == null);
                grid[x, y] = new Node(walkable, tilemap.GetCellCenterWorld(tilePos), x, y);
            }
        }
    }
    public void printGrid(){
            for(int i = 0; i < gridSize.x; i++){
                for(int j = 0; j < gridSize.y; j++){
                    //Debug.Log(grid[i,j].gridX+", "+grid[i,j].gridY +": "+ grid[i,j].walkable);
                    Debug.Log(grid[i,j].worldPosition);

                }
            }
    }



    public void setWalkable(Vector3Int world,bool walkable){
        GetNodeFromWorld(world).walkable = walkable;
    }
    public Node GetNodeFromIndex(int x, int y)
    {
     return grid[x+gridSize.x/2 , y + gridSize.y/2];   
    }
    public Node GetNodeFromWorld(Vector3Int world){
        //Vector3Int tilePos = new Vector3Int(world.x+gridSize.x/2, world.y + gridSize.y/2, 0);
        Vector3 center = tilemap.GetCellCenterWorld(world);
        //Debug.Log(center);
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
        reachableTiles = new List<Node>();

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (grid[x, y].walkable)
                {
                    reachableTiles.Add(grid[x, y]);
                }
            }
        }

        return reachableTiles;
    }

  

}