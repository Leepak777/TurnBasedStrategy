using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 10f;
    public Canvas canvas;
    bool track = false;
    GameObject target = null;

    void Start()
    {
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
            Vector3 newpos = target.transform.position;
            if (transform.position == new Vector3(newpos.x, newpos.y, -1))
            {
                track = false;
                return;
            }
            if (!IsCameraWithinCanvasBounds())
            {
                // Move camera in by 1 pixel
                Vector3 moveDir = canvas.transform.position - transform.position;
                transform.position += moveDir.normalized * 0.5f;
                track = false;
                return;
            }
            Vector2 direction = newpos - transform.position;
            Vector2 movement = Vector2.ClampMagnitude(direction, speed * 1.9f * Time.deltaTime);

            transform.position += new Vector3(movement.x, movement.y, 0);
        }
    }

    void Update()
    {
        arrowControl();
        tracking();
    }
}
