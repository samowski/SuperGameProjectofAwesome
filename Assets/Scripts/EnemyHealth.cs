using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public float health = 3;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void ApplyDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            //Destroy(gameObject);
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
