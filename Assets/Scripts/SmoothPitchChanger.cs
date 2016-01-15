using UnityEngine;
using System.Collections;

public class SmoothPitchChanger : MonoBehaviour
{
	AudioSource audioSource;

	bool isRunning = false;
	Coroutine setter;

	float old;
	float target;
	float current;

	float interpolationTime;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void SetPitch(float pitch, float time)
	{
		target = pitch;
		this.interpolationTime = time;

		if (isRunning)
		{
			StopCoroutine(setter);
			old = current;
		}
		else
		{
			old = audioSource.pitch;
		}

		isRunning = true;
		setter = StartCoroutine(interpolate());
	}

	IEnumerator interpolate()
	{
		float endTime = Time.time + interpolationTime;

		while (Time.time < endTime)
		{
			current = Mathf.Lerp(target, old, (endTime - Time.time) / interpolationTime);
			audioSource.pitch = current;
			yield return null;
		}

		audioSource.pitch = target;
		isRunning = false;
	}
}