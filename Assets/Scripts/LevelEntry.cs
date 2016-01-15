using UnityEngine;
using UnityEngine.UI;

public class LevelEntry : MonoBehaviour
{
	bool isLoading = false;

	public string Level;
	public uint NeededLevel = 0;

	MainMenu menu;

	SpriteRenderer spriteRenderer;
	Material enabledMaterial;
	public Material DisabledMaterial;

	Collider2D rollatorCollider;

	void Start()
	{
		rollatorCollider = GameObject.Find("Rollator").GetComponent<Collider2D>();

		menu = GameObject.Find("Menu").GetComponent<MainMenu>();

		transform.parent.FindChild("Canvas/Text").GetComponent<Text>().text = NeededLevel.ToString();

		spriteRenderer = GetComponent<SpriteRenderer>();
		enabledMaterial = spriteRenderer.material;

		UpdateEnabled();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other == rollatorCollider && GameProgress.Instance.Level >= NeededLevel && PlayerController.IsAttacking && !isLoading)
		{
			isLoading = true;
			Invoke("loadLevel", 0.5f);
		}
	}

	void loadLevel()
	{
		menu.LoadGame(Level);
	}

	public void UpdateEnabled()
	{
		spriteRenderer.material = GameProgress.Instance.Level >= NeededLevel ? enabledMaterial : DisabledMaterial;
	}
}