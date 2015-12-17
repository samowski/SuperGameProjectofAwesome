using UnityEngine;
using System.Collections;

public class PunchDamage : MonoBehaviour
{
    public float damage = 1;


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Caregiver") && PlayerController.IsAttacking)
        {
			//If a collider is hit, search in the hit object for a script with a function "ApplyDamage" and execute this function.
			//DontRequireReceiver catches exceptions, if the function wasn't found.
			other.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver); 

            damage = 1;

            PlayerController.IsAttacking = false;
        }
    }
}
