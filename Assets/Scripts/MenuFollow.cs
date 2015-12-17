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

        Vector3 newPosition = new Vector3(Mathf.Lerp(transform.position.x, target.position.x + offset.x, 0.05f),
                                  Mathf.Lerp(transform.position.y, target.position.y + offset.y, 0.25f),   
                                  transform.position.z);

        float x = Mathf.Clamp(newPosition.x,
                      leftBorder.position.x + halfCameraWidth,
                      rightBorder.position.x - halfCameraWidth);

        transform.position = new Vector3(x, newPosition.y, newPosition.z);
    }
}
