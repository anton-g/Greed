using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour {
    public static DataManager Instance = null;
    [HideInInspector]
    public int reachedLevel;

    private string savePath = Application.persistentDataPath + "/gameData.dat"; 

    public float MusicVolume {
        get {
            return PlayerPrefs.GetFloat("music", 1.0f);
        }
        set {
            PlayerPrefs.SetFloat("music", value);
            PlayerPrefs.Save();
        }
    }
    
    public float SoundVolume {
        get {
            return PlayerPrefs.GetFloat("sound", 1.0f);
        }
        set {
            PlayerPrefs.SetFloat("sound", value);
            PlayerPrefs.Save();
        }
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }    
        DontDestroyOnLoad(gameObject);
        
        reachedLevel = 1;
        Load();
    }

    void Update() { }
    
    //Doesn't work on web
    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        
        PlayerData data = new PlayerData();
        data.reachedLevel = reachedLevel;
        
        bf.Serialize(file, data);
        file.Close();
    }
    
    public void Load() {
        if (File.Exists(savePath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            
            PlayerData data = (PlayerData)bf.Deserialize(file);
            reachedLevel = data.reachedLevel;
            
            file.Close();
        }
    }
}

[Serializable]
class PlayerData {
    public int reachedLevel;
}