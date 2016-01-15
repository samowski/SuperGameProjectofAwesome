using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			other.SendMessage("ApplyGrannyDamage", SendMessageOptions.DontRequireReceiver);
		}
	}
}