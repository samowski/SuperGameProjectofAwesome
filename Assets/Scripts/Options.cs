using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class Options : MonoBehaviour
{
    public static Options Instance;

    public float musicVolume;
    public float sfxVolume;
    public AudioMixer Mixer;

    public float MusicVolume
    {
        get
        {
            return musicVolume;
        }
        set
        {
            musicVolume = value;
            Mixer.SetFloat("musicVolume", toDb(value));
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
            Mixer.SetFloat("sfxVolume", toDb(value));
        }
    }
		
    static float toDb(float value)
    {
        if (value <= 0.0f)
        {
            return -80.0f;
        }
        else
        {
            return  20f * Mathf.Log10(value * value);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}