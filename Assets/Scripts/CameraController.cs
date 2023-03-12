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

    public void trackPlayer(){
        track = true;
        target = GameObject.Find("TurnManager").GetComponent<TurnManager>().getCurrenPlay();
    }
    
    public void initLoc(GameObject go){
        Vector3Int loc = tilemap.WorldToCell(go.transform.position);
        Vector3 newpos = tilemap.GetCellCenterWorld(loc);
        transform.position = new Vector3((int)newpos.x, (int)newpos.y, -1);
    }
    private bool IsCameraWithinTilemapBounds()
    {
        Camera camera = this.gameObject.GetComponent<Camera>();

        if (camera == null || tilemap == null)
        {
            return false;
        }

        Vector3 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0f, 0f, camera.nearClipPlane));
        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1f, 1f, camera.nearClipPlane));

        return tilemap.cellBounds.Contains(tilemap.WorldToCell(bottomLeft)) && tilemap.cellBounds.Contains(tilemap.WorldToCell(topRight));
    }


    void arrowControl(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 newPosition = transform.position + new Vector3(horizontal, vertical, 0) * speed*2 * Time.deltaTime;

        Vector3 cameraExtents = new Vector3(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize, 0f);
        Vector3 minTile = tilemap.CellToWorld(tilemap.cellBounds.min) + cameraExtents;
        Vector3 maxTile = tilemap.CellToWorld(tilemap.cellBounds.max) - cameraExtents;

        newPosition = new Vector3(
            Mathf.Clamp(newPosition.x, minTile.x, maxTile.x),
            Mathf.Clamp(newPosition.y, minTile.y, maxTile.y),
            transform.position.z
        );

        transform.position = newPosition;
    }


    void tracking(){
        if(track){
            Vector3Int loc = tilemap.WorldToCell(target.transform.position);
            Vector3 newpos = tilemap.GetCellCenterWorld(loc);
            if(tilemap.WorldToCell(transform.position) == tilemap.WorldToCell(new Vector3(newpos.x, newpos.y, -1))){
                track = false;
                return;
            }
            if (!IsCameraWithinTilemapBounds())
            {
                // Move camera in by 1 pixel
                Vector3 moveDir = tilemap.transform.position - transform.position;
                transform.position += moveDir.normalized * 0.5f;
                track = false;
                return;
            }
            Vector2 direction = newpos - transform.position;
            Vector2 movement = Vector2.ClampMagnitude(direction, speed*1.9f*Time.deltaTime);

            transform.position += new Vector3(movement.x, movement.y, 0);
        }
    }

    void Update()
    {
        arrowControl();
        tracking();
    }
}
