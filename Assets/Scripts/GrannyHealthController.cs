using UnityEngine;
using System.Collections;

public class GrannyHealthController : MonoBehaviour
{

    public float health = 1;
    private Animator animator;
    private PlayerController playerController;

	LevelMenu levelMenu;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
		levelMenu = GameObject.Find("LevelMenu").GetComponent<LevelMenu>();
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
        levelMenu.Exit(false);
        //Application.LoadLevel(1);
        //health = 1;
        //animator.SetBool("Dying", false);
    }
}

