using UnityEngine;
using System.Collections;

public class GameProgress : MonoBehaviour
{
    public static GameProgress instance;

    public uint level;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
