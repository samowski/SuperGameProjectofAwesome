using UnityEngine;

public class PunchDamage : MonoBehaviour
{
	public float Damage = 1;

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Caregiver") && PlayerController.IsAttacking)
		{
			//If a collider is hit, search in the hit object for a script with a function "ApplyDamage" and execute this function.
			//DontRequireReceiver catches exceptions, if the function wasn't found.
			other.SendMessage("ApplyDamage", Damage, SendMessageOptions.DontRequireReceiver); 

			Damage = 1;

			PlayerController.IsAttacking = false;
		}
	}
}