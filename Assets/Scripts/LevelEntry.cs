using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEntry : MonoBehaviour
{
    public string Level;
	public uint neededLevel = 0;

    MainMenu menu;

    SpriteRenderer spriteRenderer;
    Material enabledMaterial;
    public Material disabledMaterial;

    void Start()
    {
        menu = GameObject.Find("Menu").GetComponent<MainMenu>();
       
        transform.parent.FindChild("Canvas/Text").GetComponent<Text>().text = neededLevel.ToString();

        spriteRenderer = GetComponent<SpriteRenderer>();
        enabledMaterial = spriteRenderer.material;

        UpdateEnabled();
    }

    void ApplyDamage(float damage)
    {
        if (GameProgress.instance.level >= neededLevel)
        {
			StartCoroutine(waitAndLoad());
        }
    }

	IEnumerator waitAndLoad()
	{
		yield return new WaitForSeconds(0.5f);
		menu.LoadGame(Level);
	}

    public void UpdateEnabled()
    {
        spriteRenderer.material = GameProgress.instance.level >= neededLevel ? enabledMaterial : disabledMaterial;
    }
}
