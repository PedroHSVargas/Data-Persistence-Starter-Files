using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public string PlayerName;
    public string HighScoreName;
    public int HighScore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
    }

    [System.Serializable]
    private class SaveData
    {
        public string HighScoreName;
        public int HighScore;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData
        {
            HighScoreName = HighScoreName,
            HighScore = HighScore
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (!File.Exists(path))
            return;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        HighScoreName = data.HighScoreName;
        HighScore = data.HighScore;
    }
}
