using UnityEngine;
using System.Collections;

public class Options : MonoBehaviour
{
    public static Options instance;

    public float musicVolume;
    public float sfxVolume;

    public float MusicVolume
    {
        get
        {
            return musicVolume;
        }
        set
        {
            musicVolume = value;
            // TODO set volume
        }
    }

    public float SfxVolume
    {
        get
        {
            return sfxVolume;
        }
        set
        {
            sfxVolume = value;
            // TODO set volume
        }
    }

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
