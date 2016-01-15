using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
	public float DampTime = 0.15f;

	public Transform Target;
	public Vector3 Offset;

	public Transform LeftBorder;
	public Transform RightBorder;
	public Transform TopBorder;
	public Transform BottomBorder;

	Vector3 velocity = Vector3.zero;

	Camera followCamera;

	void Start()
	{
		followCamera = GetComponent<Camera>();
	}

	void FixedUpdate()
	{
		Vector3 targetPosition = Target.position + Offset;

		if (Target)
		{
			transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, DampTime);

			float halfCameraWidth = followCamera.orthographicSize * followCamera.aspect;
			float halfCameraHeight = followCamera.orthographicSize;

			float clampedX = Mathf.Clamp(transform.position.x,
				LeftBorder != null ? LeftBorder.position.x + halfCameraWidth : float.NegativeInfinity,
				RightBorder != null ? RightBorder.position.x - halfCameraWidth : float.PositiveInfinity);

			float clampedY = Mathf.Clamp(transform.position.y,
				BottomBorder != null ? BottomBorder.position.y + halfCameraHeight : float.NegativeInfinity,
				TopBorder != null ? TopBorder.position.y - halfCameraHeight : float.PositiveInfinity);

			transform.position = new Vector3(clampedX, clampedY, transform.position.z);
		}
	}
}