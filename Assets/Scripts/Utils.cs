using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    // fixes selectable not getting higlighted after .Select()
    public static void Select(Selectable selectable)
    {
        selectable.Select();
        selectable.OnSelect(null);
    }

    public static void ChangeMusicVolume(float value)
    { 
        Options.instance.MusicVolume = value;
    }

    public static void ChangeSfxVolume(float value)
    {
        Options.instance.SfxVolume = value;
    }
}

