using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour
{

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform FollowedObject;

    void Update()
    {
        if (FollowedObject)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(FollowedObject.position);
            Vector3 delta = FollowedObject.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }
}