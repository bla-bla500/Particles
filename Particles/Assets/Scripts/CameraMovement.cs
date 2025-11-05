using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector2 mouseStartingPosition;
    private Vector2 cameraStartingPositionWorldSpace;
    private float zoomAmount;
    private bool hasStartingPosition = false;
    private float distancePerPixel;
    [SerializeField] private float zoomSensitivity;

    private void Start()
    {
        distancePerPixel = Camera.main.ScreenToWorldPoint(new Vector2(0,0)).x - Camera.main.ScreenToWorldPoint(new Vector2(1,0)).x;
        zoomAmount = Camera.main.orthographicSize;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (hasStartingPosition == false)
            {
                mouseStartingPosition = Input.mousePosition;
                cameraStartingPositionWorldSpace = transform.position;
                hasStartingPosition = true;
            }
            else
            {
                transform.position = (Vector3)(cameraStartingPositionWorldSpace + (((Vector2)Input.mousePosition - mouseStartingPosition) * distancePerPixel))+ new Vector3 (0,0,-10);
            }

        }
        if (hasStartingPosition && Input.GetKeyUp(KeyCode.Mouse1))
        {
            hasStartingPosition = false;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            zoomAmount += Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
            Camera.main.orthographicSize = zoomAmount;

            distancePerPixel = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - Camera.main.ScreenToWorldPoint(new Vector2(1, 0)).x;
        }
    }

}
