using UnityEngine;
using System.Collections;

public class GrannyHealthController : MonoBehaviour
{

    private float health = 1;
    private Animator animator;
    private PlayerController playerController;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void ApplyDamage()
    {
        health--;
        health = Mathf.Max(0, health);

        if (health == 0)
        {
            Dying();
        }
    }


    void Dying()
    {
        animator.SetBool("Dying", true);
        //Restart Level
        Invoke("RestartGame", 3);
        playerController.enabled = false;
    }

    void RestartGame()
    {
        Application.LoadLevel(1);
        health = 1;
        animator.SetBool("Dying", false);

    }
}

