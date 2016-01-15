using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float Health = 3;

	Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void ApplyDamage(float damage)
	{
		Health -= damage;

		if (Health <= 0)
		{
			gameObject.SendMessage("SetUnconscious", SendMessageOptions.DontRequireReceiver);
			animator.SetTrigger("Unconscious");
			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
		}
		else
		{
			animator.SetTrigger("Block");
		}
	}
}