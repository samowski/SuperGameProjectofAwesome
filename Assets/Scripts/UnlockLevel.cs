using UnityEngine;
using System.Collections;

public class UnlockLevel : MonoBehaviour
{
    public uint level;

    public void Unlock()
    {
        if (level > GameProgress.instance.level)
        {
            GameProgress.instance.level = level;
            SaveGame.Save();
        }
    }
}
