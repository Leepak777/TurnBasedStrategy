using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aoiti.Pathfinding; //import the pathfinding library
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    Pathfinder<Vector3Int> pathfinder;
    HighlightReachableTiles hightlightReachableTile;
    List<Vector3Int> path = new List<Vector3Int>();
    Tilemap tilemap;
    [SerializeField] float movementSpeed = 100f;  // Add this to control the movement speed
    int maxTiles; // Add this to limit the number of tiles the object can travel
    public int tilescheck = 0;
    int attackrange; 
    int attackArea;
    public int tilesTraveled = 0; // Add this to keep track of the number of tiles the object has traveled
    public int tilesTraveled_damage = 0;
    public int tilesfat = 0;
    public bool isMoving = false;
    public bool turn = false;
    public bool moved = false;
    public bool origin = false;
    public bool attacking= false;
    public bool dead = false;
    public bool setPath = false;
    TileManager tileM;
    //Vector3Int targetTileN;
    Vector3Int originTileN;
    GameObject targetEnemy;

    private void Start()
    {
    // Get the Tilemap component from the scene
    maxTiles = this.gameObject.GetComponent<StatUpdate>().getMaxTiles();
    attackrange = this.gameObject.GetComponent<StatUpdate>().getAttackRange();
    tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
    tileM = GameObject.Find("Tilemanager").GetComponent<TileManager>();
    hightlightReachableTile = this.gameObject.GetComponent<HighlightReachableTiles>();
    pathfinder = new Pathfinder<Vector3Int>(GetDistance, GetNeighbourTileNs);
    transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));
    tilescheck = maxTiles;
    TileN locn = tileM.GetTileNFromWorld(tilemap.WorldToCell(transform.position));
    Vector3Int loc = new Vector3Int((int)locn.worldPosition.x,(int)locn.worldPosition.y,0);
    this.gameObject.GetComponentInChildren<Ghost>().setLocation(loc);

    }

    private void Update()
    {
        targetEnemy = null;
        if(turn && !moved)
         {
            moving();
        }
        else{
           notmoving();
        }
    }

    public void notmoving(){
            origin = false;
            tileM.setWalkable(this.gameObject,tilemap.WorldToCell(transform.position), false);
            hightlightReachableTile.UnhighlightReachable();
            hightlightReachableTile.UnhighlightEnemy();
    }

    public void setPathAI(){
        if(!setPath){
            
            GameObject targetPlayer = getClosestPlayer();
            Vector3Int targetTileN = tilemap.WorldToCell(targetPlayer.transform.position);
            Vector3Int startTileN = tilemap.WorldToCell(transform.position);  
                     
            
            if(!tileM.GetTileNFromWorld(targetTileN).walkable && tileM.GetTileNFromWorld(targetTileN).occupant == null){
                Debug.Log("Target occupied.");
                return;
            }
            
            if(tileM.GetTileNFromWorld(targetTileN).occupant != null){
                GameObject go = tileM.GetTileNFromWorld(targetTileN).occupant;
                if(go.tag == "Player"){
                    if(inArea(tilemap.WorldToCell(transform.position),tilemap.WorldToCell(go.transform.position),attackrange)){
                        targetEnemy = go;
                        flagEnemyArea(targetEnemy,"Player",attackArea);
                        AttackCheck("Player");
                        return;
                    }
                    targetTileN = tilemap.WorldToCell(getClosestTiletoObject(go));

                }
                
            }
             if(!inArea(originTileN,targetTileN, tilescheck)){
                return;
            }
            tileM.setWalkable(this.gameObject,startTileN,true);
            path = new List<Vector3Int>();
            /*Debug.Log("Start TileN: " + startTileN);
            Debug.Log("Closest Reachable Tile: " + targetTileN);
            Debug.Log("GetDistance: " + GetDistance(startTileN, targetTileN));
            Debug.Log("GetNeighbourTileNs: " + GetNeighbourTileNs(startTileN));*/
            
            if (pathfinder.GenerateAstarPath(startTileN, targetTileN, out path))
            {
                if(path.Count > tilescheck && !inArea(originTileN,targetTileN,tilescheck)){
                    Debug.Log("Target is too far away.");
                    isMoving = false;
                    path = null;
                }
                else{
                    tilesTraveled = 0;
                    isMoving = true;
                }
                 setPath = true;
            }
            }
    }

    public bool GetMouseButtonDown(int button)
    {
        if(GameObject.Find("AttackPrompt").GetComponent<AttackPrompt>().checkOnButton()){
            return false;
        }
        return Input.GetMouseButtonDown(button);
    }


    public void setPathPlayer(){
        //this.gameObject.GetComponentInChildren<Ghost>().enabled = true;
        //this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
        this.gameObject.GetComponentInChildren<Ghost>().setOnOff(true);
        Vector3 shadowtarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int shadowtargetTileN = tilemap.WorldToCell(shadowtarget);
        if(inArea(originTileN,shadowtargetTileN, tilescheck)){
            TileN locn = tileM.GetTileNFromWorld(shadowtargetTileN);
            Vector3Int loc = new Vector3Int((int)locn.worldPosition.x,(int)locn.worldPosition.y,0);
            this.gameObject.GetComponentInChildren<Ghost>().setLocation(loc);
        }
        else{
            this.gameObject.GetComponentInChildren<Ghost>().setOnOff(false);
        }
        if (GetMouseButtonDown(0)) //check for a new target
            {
            if(tilemap.WorldToCell(transform.position) != originTileN && !attacking){
                transform.position = tilemap.GetCellCenterWorld(originTileN);    
            }
            this.gameObject.GetComponentInChildren<Ghost>().setOnOff(false);
            //this.gameObject.GetComponentInChildren<Ghost>().enabled = false;
            //this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int targetTileN = tilemap.WorldToCell(target);
            Vector3Int startTileN = tilemap.WorldToCell(transform.position);           

            if(!tileM.GetTileNFromWorld(targetTileN).walkable && tileM.GetTileNFromWorld(targetTileN).occupant == null){
                Debug.Log("Target occupied.");
                //Debug.Log(tileM.GetTileNFromWorld(targetTileN).occupant.name);
                return;
            }
            
            if(tileM.GetTileNFromWorld(targetTileN).occupant != null){
                GameObject go = tileM.GetTileNFromWorld(targetTileN).occupant;
                if(go.tag == "Enemy"){
                    if(attacking && inArea(tilemap.WorldToCell(transform.position),tilemap.WorldToCell(go.transform.position),attackrange)){
                        targetEnemy = go;
                        flagEnemyArea(targetEnemy,"Enemy",attackArea);
                        AttackCheck("Enemy");
                        return;
                    }
                    //Debug.Log(tileM.GetTileNFromWorld(targetTileN).occupant.name);
                    targetTileN = tilemap.WorldToCell(getClosestTiletoObject(go));
                    /*if(this.gameObject.GetComponent<StatUpdate>().getbuff(10)){
                        flagAdjacent();
                    }*/
                }
                
            }
             if(!inArea(originTileN,targetTileN, tilescheck)){
                return;
            }
            tileM.setWalkable(this.gameObject,startTileN,true);
            path = new List<Vector3Int>();
            //Debug.Log("Start TileN: " + startTileN);
            //Debug.Log("Closest Reachable Tile: " + targetTileN);
            //Debug.Log("GetDistance: " + GetDistance(startTileN, targetTileN));
            //Debug.Log("GetNeighbourTileNs: " + GetNeighbourTileNs(startTileN));
            
            if (pathfinder.GenerateAstarPath(startTileN, targetTileN, out path))
            {
                if(path.Count > tilescheck && !inArea(originTileN,targetTileN,tilescheck)){
                    Debug.Log("Target is too far away.");
                    isMoving = false;
                    path = null;
                }
                else{
                    tilesTraveled = 0;
                    isMoving = true;
                }
            }
        }
    }

    public void moveTo(){
         if(isMoving && tilesTraveled_damage>0){
                    this.gameObject.GetComponent<StatUpdate>().Damage -= tilesTraveled_damage;
                    tilesTraveled_damage = 0;
            }

            if (path != null && path.Count > 0)
            {
                Vector3 targetPosition = tilemap.GetCellCenterWorld(path[0]);
                Vector3 dir = (targetPosition - transform.position).normalized;

                if (path.Count > 1)
                {
                    if (!IsAdjacent(path[0], path[1]))
                    {
                        path.RemoveAt(0);
                        return;
                    }
                }

                if ((transform.position - targetPosition).sqrMagnitude < movementSpeed * movementSpeed * Time.deltaTime * Time.deltaTime)
                {
                    TileN TileN =tileM.GetTileNFromWorld(tilemap.WorldToCell(targetPosition));
                    if(this.gameObject.tag == "Player"){
                        Color c = Color.red;
                        c.a = 0.5f;
                        Vector3Int tilePos = new Vector3Int((int)TileN.gridX , (int)TileN.gridY , 0);
                        tilemap.SetColor(tilePos, c);
                    }
                    path.RemoveAt(0);
                    tilesTraveled++; // Increment the tiles traveled
                    
                }
                transform.position += dir * movementSpeed * Time.deltaTime;  // Move the character based on the movement speed
            }
    }

    public void moving(){
            setRange();
            //Debug.Log(targetEnemy);
            setOrigin();
            if(gameObject.tag == "Player"){
                setPathPlayer();
            }
            else{
                setPathAI();
            }
            moveTo();
           
            if ((path == null || path.Count == 0))
            {
                if(isMoving || tilesTraveled >= tilescheck){
                    tilesfat = tilesTraveled;
                    isMoving = false;
                    if(this.gameObject.tag == "Enemy"){
                        turn = false;
                        moved = true;   
                    }
                    tilesTraveled = 0;
                }
            }
            highlight();

    }
    void setRange(){
        tilescheck = this.gameObject.GetComponent<StatUpdate>().getMaxTiles();
    }

    void setOrigin(){
        if(!origin){
            hightlightReachableTile.highlightorigin = tilemap.WorldToCell(transform.position);
            originTileN = tilemap.WorldToCell(transform.position);
            origin = true;
        }
    }
    void highlight(){
         if (isMoving)
            {
                hightlightReachableTile.UnhighlightReachable();
            }
            else
            {        
                hightlightReachableTile.UnhighlightReachable();
                hightlightReachableTile.HighlightReachable();
            }
    }
   public bool IsAdjacent(Vector3Int TileN1, Vector3Int TileN2)
    {
        int xDiff = Mathf.Abs(TileN1.x - TileN2.x);
        int yDiff = Mathf.Abs(TileN1.y - TileN2.y);
        if (xDiff + yDiff == 1)
        {
            return true;
        }
        return false;
    }
    public float GetDistance(Vector3Int A, Vector3Int B)
    {
        // Use Manhattan distance for tilemap
        return Mathf.Abs(A.x - B.x) + Mathf.Abs(A.y - B.y);
    }

    Dictionary<Vector3Int, float> GetNeighbourTileNs(Vector3Int pos) 
    {
        Dictionary<Vector3Int, float> neighbours = new Dictionary<Vector3Int, float>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) continue;{
                    if (Mathf.Abs(i) == Mathf.Abs(j)) continue;{
                        Vector3Int neighbourPos = new Vector3Int(pos.x + i, pos.y + j, pos.z);
                        if (inArea(originTileN,neighbourPos,tilescheck) && tileM.GetTileNFromWorld(neighbourPos)!=null && tileM.GetTileNFromWorld(neighbourPos).walkable)
                        {
                            neighbours.Add(neighbourPos, 1);
                        }
                    }
                }
            }
        }
        return neighbours;
    }

    public List<GameObject> getTaginArea(Vector3Int start, float range, string tag){
        List<GameObject> tags = new List<GameObject>(); 
        foreach(TileN n in tileM.GetTilesInArea(start,range)){
            if(n.occupant != null && n.occupant.tag == tag){
                tags.Add(n.occupant);
            }
        }
        return tags;
    }

    public bool inArea(Vector3Int start,Vector3Int target, float range){
        foreach(TileN n in tileM.GetTilesInArea(start,range)){
            if(n.gridX == target.x  && n.gridY == target.y){
                return true;
            }
        }
        return false;
    }

    public Vector3Int inAreaTile(Vector3Int start,Vector3Int target, float range){
        foreach(TileN n in tileM.GetTilesInArea(start,range)){
            if(n.gridX == target.x  && n.gridY == target.y){
                return new Vector3Int((int)n.worldPosition.x, (int) n.worldPosition.y, 0);
            }
        }
        return originTileN;
    }

    GameObject getClosestPlayer(){
        int mindis = int.MaxValue;   
        GameObject close = null; 
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject n in objectsWithTag){
            //Debug.Log(n.occupant.name);
            //Debug.Log(Mathf.Abs((int)(n.worldPosition.x - transform.position.x)) + Mathf.Abs((int)(n.worldPosition.y - transform.position.y)));
            Vector3Int cellpos = tilemap.WorldToCell(n.transform.position);
            Vector3Int curpos = tilemap.WorldToCell(transform.position);
            int distance = Mathf.Abs((int)(((curpos.x - cellpos.x) ))) + Mathf.Abs((int)((curpos.y - cellpos.y)));
            if( distance< mindis){
                mindis = distance;
                close = n;
            }  
        }
        return close;
    }

    Vector3Int getClosestTiletoObject(GameObject go){
        GameObject player = go;
        TileN ans = null;
        int mindis = int.MaxValue;    
        Vector3Int cellpos = tilemap.WorldToCell(player.transform.position);
        foreach(TileN n in tileM.GetTilesInArea(originTileN,tilescheck)){
            Vector3Int target = tilemap.WorldToCell(new Vector3Int((int)n.worldPosition.x,(int)n.worldPosition.y,0));
            int distance = Mathf.Abs((int)(n.gridX - (cellpos.x))) + Mathf.Abs((int)(n.gridY - (cellpos.y)));
                if(n.walkable && distance < mindis && distance >= (int)attackrange){
                    mindis = distance;
                    ans = n;
                }
        }
        
        return new Vector3Int((int)ans.worldPosition.x, (int) ans.worldPosition.y, 0);
    }

    public void flagEnemyArea(GameObject enemy, string tag,int range){
        enemy.GetComponent<StatUpdate>().Flagging();
        foreach(TileN n in tileM.GetTilesInArea(tilemap.WorldToCell(enemy.transform.position),range)){
                if(n.occupant != null&& n.occupant.tag == tag){
                    n.occupant.GetComponent<StatUpdate>().Flagging();
                }
        }
    }

    public void Attack(){
        if(!attacking && hightlightReachableTile.EnemyInRange()){
            hightlightReachableTile.HighlightEnemy();
            attacking = true;
        }
        else{
            attacking = false;
            hightlightReachableTile.UnhighlightEnemy();
        }
    }
    public void AttackCheck(string tag){
        foreach(GameObject en in GameObject.FindGameObjectsWithTag(tag)){
            if(en.GetComponent<StatUpdate>().flag){
                this.gameObject.GetComponent<StatUpdate>().attackEn(en);
            }
        }
        attacking = false;
            
        turn = false;
        moved = true;
        
    }

    public bool getisMoving(){
        return isMoving;
    }

    public GameObject getTargetEnemy(){
        return targetEnemy;
    }

    public void switchCheckTiles(bool change, int x){
        if(change){
            tilescheck /= x;
        }
        else{
            tilescheck = maxTiles;
        }
    }

    public bool getTurn(){
        return turn;
    }

    public void startTurn(){
        if(!turn){
            tilesfat = 0;
            turn = true;
            moved = false;
        }
    }
    public void setAttackArea(int i){
        attackArea = i;
    }
    public void resetTurn(){
        tilesfat = 0;
        turn = false;
        moved = false;
    }

    public void endTurn(){
        tilesfat = 0;
        turn = false;
        moved = true;
    }

    public int getTilesFat(){
        return tilesfat;
    }

    public Vector3Int getOrigin(){
        return originTileN;
    }

    
}