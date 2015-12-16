using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public float speed = 3.0f;

	void Update ()
	{
        float delta = Time.deltaTime;
		float hAxis = Input.GetAxis("Horizontal");
		Vector3 offset = new Vector3((Mathf.Abs(hAxis) > 0.1f ? hAxis : 0.0f) * speed * delta, 0.0f, 0.0f);
        transform.position += offset;
	}
}
