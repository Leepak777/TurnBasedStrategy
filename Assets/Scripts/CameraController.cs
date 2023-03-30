using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;


public class CameraController : MonoBehaviour 
{
    public float speed = 10f;
    public Canvas canvas;
    public Tilemap tilemap;
    bool track = false;
    GameObject target = null;
    private bool dragging = false;
    private Vector3 mouseOrigin;
    private Vector3 cameraOrigin;
    private Vector3 difference;
    private Vector3 resetCamera;
    void Start()
    {
        tilemap = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        
    }

    public void trackPlayer()
    {
        track = true;
        target = GameObject.Find("TurnManager").GetComponent<TurnManager>().getCurrenPlay();
    }

    public void initLoc(GameObject go)
    {
        Vector3 newpos = go.transform.position;
        transform.position = new Vector3((int)newpos.x, (int)newpos.y, -1);
    }

    private bool IsCameraWithinCanvasBounds()
    {
        Camera camera = this.gameObject.GetComponent<Camera>();

        if (camera == null || canvas == null)
        {
            return false;
        }

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        canvasRect.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        return camera.transform.position.x > bottomLeft.x && camera.transform.position.x < topRight.x && camera.transform.position.y > bottomLeft.y && camera.transform.position.y < topRight.y;
    }

    void arrowControl()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 newPosition = transform.position + new Vector3(horizontal, vertical, 0) * speed * 2 * Time.deltaTime;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        canvasRect.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        newPosition = new Vector3(
            Mathf.Clamp(newPosition.x, bottomLeft.x, topRight.x),
            Mathf.Clamp(newPosition.y, bottomLeft.y, topRight.y),
            transform.position.z
        );

        transform.position = newPosition;
    }

   void tracking()
    {
        if (track)
        {
            Vector3 targetPos = target.transform.position;
            Vector3Int targetTilePos = tilemap.WorldToCell(targetPos);

            // Calculate the center of the hexagon at the target tile position
            Vector3Int targetHexCenter = new Vector3Int(
                Mathf.RoundToInt(targetTilePos.x),
                Mathf.RoundToInt(targetTilePos.y),
                0
            );
            Vector3 targetHexPos = tilemap.GetCellCenterWorld(targetHexCenter);

            // Check if the camera is already at the target position
            if (transform.position == targetHexPos)
            {
                resetCamera = transform.position;
                track = false;
                return;
            }

            // Calculate the direction and distance to move the camera
            Vector2 direction = targetHexPos - transform.position;
            float distance = direction.magnitude;

            // Check if the camera is within the canvas bounds
            if (!IsCameraWithinCanvasBounds())
            {
                // Move the camera towards the center of the canvas
                Vector3 canvasCenter = canvas.transform.position;
                direction = canvasCenter - transform.position;
                distance = direction.magnitude;
            }

            // Move the camera towards the target position
            if (distance > 0)
            {
                Vector2 movement = Vector2.ClampMagnitude(direction, speed * Time.deltaTime);
                transform.position += new Vector3(movement.x, movement.y, 0);
            }
            else
            {
                resetCamera = transform.position;
                track = false;
            }
        }
    }
    bool inObject(){
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")){
            if(g.GetComponent<PositionSetup>().checkisDragging()){
                return true;
            }
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy")){
            if(g.GetComponent<PositionSetup>().checkisDragging()){
                return true;
            }
        }
        return false;
    }
    private void LateUpdate()
    {
        
        if (Input.GetMouseButton(0)&& GameObject.Find("InfoPanel") == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && (hit.collider.CompareTag("Player")||hit.collider.CompareTag("Enemy")))
            {
                return;
            }
            if(SceneManager.GetActiveScene().name == "GameScene" ){
                if(GameObject.Find("Canvas").GetComponent<EventTrig>().isNavigate()){
                    dragMove();
                }
            }
            else if(SceneManager.GetActiveScene().name == "MapSelection"){
                RectTransform r = GameObject.Find("scroll").GetComponent<RectTransform>();
                if(!RectTransformUtility.RectangleContainsScreenPoint(r, Input.mousePosition, Camera.main) && !inObject()){
                    dragMove();
                }
            }
            
        }
        else
        {
            dragging = false;
        }

        if (Input.GetMouseButton(1))
        {
            transform.position = resetCamera;
        }
    }

    void dragMove(){
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position;

        if (!dragging)
        {
            dragging = true;
            mouseOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cameraOrigin = transform.position;            }

        Vector3 newPos = cameraOrigin - difference * 0.5f;
        float cameraHeight = Camera.main.orthographicSize * 2f;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        float minX = canvas.transform.position.x - canvas.GetComponent<RectTransform>().rect.width / 2f + cameraWidth / 2f;
        float maxX = canvas.transform.position.x + canvas.GetComponent<RectTransform>().rect.width / 2f - cameraWidth / 2f;
        float minY = canvas.transform.position.y - canvas.GetComponent<RectTransform>().rect.height / 2f + cameraHeight / 2f;
        float maxY = canvas.transform.position.y + canvas.GetComponent<RectTransform>().rect.height / 2f - cameraHeight / 2f;
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        transform.position = newPos;
    }



    void Update()
    {
        arrowControl();
        tracking();
    }
    

}
