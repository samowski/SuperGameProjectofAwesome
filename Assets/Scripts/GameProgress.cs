﻿using UnityEngine;

public class GameProgress : MonoBehaviour
{
	public static GameProgress Instance;

	public uint Level;

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