using UnityEngine;

public class UnlockLevel : MonoBehaviour
{
	public uint Level;

	public void Unlock()
	{
		if (Level > GameProgress.Instance.Level)
		{
			GameProgress.Instance.Level = Level;
			SaveGame.Save();
		}
	}
}