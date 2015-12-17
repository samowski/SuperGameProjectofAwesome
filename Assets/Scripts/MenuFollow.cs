using UnityEngine;
using System.Collections;

public class MenuFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Transform leftBorder;
    public Transform rightBorder;

    Camera followcamera;

    void Start()
    {
        followcamera = GetComponent<Camera>();
    }
        
    void FixedUpdate()
    {
        float halfCameraWidth = followcamera.orthographicSize * followcamera.aspect;

        Vector3 newPosition = Vector3.Lerp(transform.position, target.position + offset, 0.05f);

        float x = Mathf.Clamp(newPosition.x,
                      leftBorder.position.x + halfCameraWidth,
                      rightBorder.position.x - halfCameraWidth);

        transform.position = new Vector3(x, newPosition.y, newPosition.z);
    }
}
