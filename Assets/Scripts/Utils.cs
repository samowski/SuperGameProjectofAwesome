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

    public static float fract(float i)
    {
        return i - Mathf.Floor(i);
    }

    public static Vector3 fract(Vector3 i)
    {
        return new Vector3(fract(i.x), fract(i.y), fract(i.z));
    }

    public static Vector3 abs(Vector3 i)
    {
        return new Vector3(Mathf.Abs(i.x), Mathf.Abs(i.y), Mathf.Abs(i.z));
    }
        
    public static float clamp(float i)
    {
        return Mathf.Min(Mathf.Max(i, 0.0f), 1.0f);
    }

    public static Vector3 clamp(Vector3 i)
    {
        return new Vector3(clamp(i.x), clamp(i.y), clamp(i.z));
    }

    public static Color hsvToRgb(float h, float s, float v)
    {
        Vector4 K = new Vector4(1.0f, 2.0f / 3.0f, 1.0f / 3.0f, 3.0f);
        Vector3 p = abs(fract(
            new Vector3(h, h, h) +
            new Vector3(K.x, K.y, K.z)) * 6.0f -
            new Vector3(K.w, K.w, K.w));
        Vector3 col = v * Vector3.Lerp(new Vector3(K.x, K.x, K.x), clamp(p - new Vector3(K.x, K.x, K.x)), s);
        return new Color(col.x, col.y, col.z);
    }

    public static Color randomHueColor()
    {
        return hsvToRgb(Random.value, 1.0f, 1.0f);
    }
}

