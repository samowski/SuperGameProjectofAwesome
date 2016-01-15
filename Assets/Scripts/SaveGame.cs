using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveGame
{
	static string path;

	[System.Serializable]
	class PersistentData
	{
		uint level;
		float musicVolume;
		float sfxVolume;

		public PersistentData()
		{
			level = GameProgress.Instance.Level;
			musicVolume = Options.Instance.MusicVolume;
			sfxVolume = Options.Instance.SfxVolume;
		}

		public void Set()
		{
			GameProgress.Instance.Level = level;
			Options.Instance.MusicVolume = musicVolume;
			Options.Instance.SfxVolume = sfxVolume;
		}
	}

	static SaveGame()
	{
		path = Path.Combine(Application.persistentDataPath, "saveGame.dat");
	}

	public static void Save()
	{
		var data = new PersistentData();
		var binaryFormatter = new BinaryFormatter();
		var file = File.Create(path);
		binaryFormatter.Serialize(file, data);
		file.Close();
	}

	public static void Load()
	{
		if (File.Exists(path))
		{
			var binaryFormatter = new BinaryFormatter();
			var file = File.Open(path, FileMode.Open);
			var data = (PersistentData)binaryFormatter.Deserialize(file);
			file.Close();
			data.Set();
		}
	}
}