using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public float speed = 10f;
    public Tilemap tilemap;
    bool track = false;
    GameObject target = null;

    void Start(){
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();


    }

    public void trackPlayer(GameObject go){
        track = true;
        target = go;
    }
    public void initLoc(GameObject go){
        Vector3Int loc = tilemap.WorldToCell(go.transform.position);
        Vector3 newpos = tilemap.GetCellCenterWorld(loc);
        transform.position = new Vector3((int)newpos.x, (int)newpos.y, -1);
    }
    void arrowControl(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.position += new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
    }

    void tracking(){
        if(track){
            Vector3Int loc = tilemap.WorldToCell(target.transform.position);
            Vector3 newpos = tilemap.GetCellCenterWorld(loc);
            if(tilemap.WorldToCell(transform.position) == tilemap.WorldToCell(new Vector3(newpos.x, newpos.y, -1))){
                track = false;
                return;
            }
            int h = -1;
            if(newpos.x > transform.position.x){
                h = 1;
            }
            if(newpos.x == transform.position.x){
                h = 0;
            }
            int v = -1;
            if(newpos.y > transform.position.y){
                v = 1;
            }
            if(newpos.y == transform.position.y){
                v = 0;
            }
            transform.position += new Vector3(h, v, 0) * (int)(speed*1.9f * Time.deltaTime);
        }
    }

    void Update()
    {
        arrowControl();
        tracking();
        
    }
}
