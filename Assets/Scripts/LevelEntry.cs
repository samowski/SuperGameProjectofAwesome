using UnityEngine;
using System.Collections;

public class LevelEntry : MonoBehaviour
{
    public string Level;
	public uint neededLevel = 0;

	Collider2D rollatorCollider;
    MainMenu menu;

    SpriteRenderer spriteRenderer;
    Material enabledMaterial;
    public Material disabledMaterial;

    void Start()
    {
        rollatorCollider = GameObject.Find("Granny/Textures/Rollator").GetComponent<Collider2D>();
        menu = GameObject.Find("Menu").GetComponent<MainMenu>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        enabledMaterial = spriteRenderer.material;

        UpdateEnabled();
    }
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == rollatorCollider && GameProgress.instance.level >= neededLevel)
        {
            //Debug.Log("enter");
        }
    }

    void ApplyDamage(float damage)
    {
        if (GameProgress.instance.level >= neededLevel)
        {
            menu.LoadGame(Level);
        }
    }

    public void UpdateEnabled()
    {
        spriteRenderer.material = GameProgress.instance.level >= neededLevel ? enabledMaterial : disabledMaterial;
    }

    void Update()
    {
    }
}
