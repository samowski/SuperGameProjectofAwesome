using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour
{

    // Use this for initialization
    public float DampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform FollowedObject;
    private Vector3 GrannyPos;

    // Update is called once per frame
    void FixedUpdate()
    {
        GrannyPos = FollowedObject.position;
        GrannyPos.y += 17;
        if (FollowedObject)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(GrannyPos);
            Vector3 delta = GrannyPos - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);
        }
    }
}
