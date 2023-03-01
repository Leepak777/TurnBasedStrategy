using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public float speed = 10f;
    public Tilemap tilemap;

    void Start(){
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        /*GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Player");
        Vector3Int loc = tilemap.WorldToCell(objectsWithTag[0].transform.position);
        Vector3 newpos = tilemap.GetCellCenterWorld(loc);
        this.gameObject.transform.position = new Vector3(newpos.x, newpos.y, -1);*/


    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.position += new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
    }
}
