using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public float speed = 10f;
    public Canvas canvas;
    public Tilemap tilemap;
    bool track = false;
    GameObject target = null;

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
                track = false;
            }
        }
    }


    void Update()
    {
        arrowControl();
        tracking();
    }
}
