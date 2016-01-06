using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour
{
    public float DampTime = 0.15f;

    public Transform target;
    public Vector3 offset;

    public Transform leftBorder;
    public Transform rightBorder;
    public Transform topBorder;
    public Transform bottomBorder;

    Vector3 velocity = Vector3.zero;
    Camera followCamera;

    void Start()
    {
        followCamera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position;
        targetPosition += offset;
        if (target)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, DampTime);

            float halfCameraWidth = followCamera.orthographicSize * followCamera.aspect;
            float halfCameraHeight = followCamera.orthographicSize;

            float x = Mathf.Clamp(transform.position.x,
                leftBorder != null ? leftBorder.position.x + halfCameraWidth : float.NegativeInfinity,
                rightBorder != null ?  rightBorder.position.x - halfCameraWidth : float.PositiveInfinity);

            float y = Mathf.Clamp(transform.position.y,
                bottomBorder != null ? bottomBorder.position.y + halfCameraHeight : float.NegativeInfinity,
                topBorder != null ?  topBorder.position.y - halfCameraHeight : float.PositiveInfinity);

            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
