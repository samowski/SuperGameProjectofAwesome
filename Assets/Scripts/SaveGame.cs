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
            level = GameProgress.instance.level;
            musicVolume = Options.instance.MusicVolume;
            sfxVolume = Options.instance.SfxVolume;
        }

        public void Set()
        {
            GameProgress.instance.level = level;
            Options.instance.MusicVolume = musicVolume;
            Options.instance.SfxVolume = sfxVolume;
        }
    }

    static SaveGame()
    {
        path = Path.Combine(Application.persistentDataPath, "saveGame.dat");
    }

    public static void Save()
    {
        var data = new PersistentData();
        var bf = new BinaryFormatter();
        var file = File.Create(path);
        bf.Serialize(file, data);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(path))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(path, FileMode.Open);
            var data = (PersistentData)bf.Deserialize(file);
            file.Close();
            data.Set();
        }
    }
}


