using UnityEngine;

public class LoadAdditional : MonoBehaviour
{
	public int Level;

	void Awake()
	{
		Application.LoadLevelAdditive(Level);
		DestroyImmediate(gameObject);
	}
}