using UnityEngine;
using System.Collections;

public class LevelEntry : MonoBehaviour
{
    public string Level;

    Collider2D grannyCollider;
    MainMenu menu;

    void Start()
    {
        grannyCollider = GameObject.Find("Granny").GetComponent<Collider2D>();
        menu = GameObject.Find("Menu").GetComponent<MainMenu>();
    }
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == grannyCollider)
        {
            //Debug.Log("enter");
        }
    }

    void ApplyDamage(float damage)
    {
        menu.LoadGame(Level);
    }

    void Update()
    {
	
    }
}
