using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public float health = 3;
    private Animator animator; 
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
        }
        else
        {
            animator.SetTrigger("Block");
        }
    }
}
