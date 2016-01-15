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
		Options.Instance.MusicVolume = value;
	}

	public static void ChangeSfxVolume(float value)
	{
		Options.Instance.SfxVolume = value;
	}

	public static float Fract(float i)
	{
		return i - Mathf.Floor(i);
	}

	public static Vector3 Fract(Vector3 i)
	{
		return new Vector3(Fract(i.x), Fract(i.y), Fract(i.z));
	}

	public static Vector3 Abs(Vector3 i)
	{
		return new Vector3(Mathf.Abs(i.x), Mathf.Abs(i.y), Mathf.Abs(i.z));
	}

	public static Vector3 Clamp01(Vector3 i)
	{
		return new Vector3(Mathf.Clamp01(i.x), Mathf.Clamp01(i.y), Mathf.Clamp01(i.z));
	}

	public static Color HsvToRgb(float h, float s, float v)
	{
		Vector4 K = new Vector4(1.0f, 2.0f / 3.0f, 1.0f / 3.0f, 3.0f);
		Vector3 p = Abs(Fract(
			new Vector3(h, h, h) +
			new Vector3(K.x, K.y, K.z)) * 6.0f -
			new Vector3(K.w, K.w, K.w));
		Vector3 col = v * Vector3.Lerp(new Vector3(K.x, K.x, K.x), Clamp01(p - new Vector3(K.x, K.x, K.x)), s);
		return new Color(col.x, col.y, col.z);
	}

	public static Color RandomHueColor()
	{
		return HsvToRgb(Random.value, 1.0f, 1.0f);
	}
}