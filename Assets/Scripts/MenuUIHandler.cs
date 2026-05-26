using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    public InputField NameInput;
    public Text HighScoreText;

    private void Start()
    {
        if (DataManager.Instance != null && HighScoreText != null)
        {
            int score = DataManager.Instance.HighScore;
            string name = DataManager.Instance.HighScoreName;
            HighScoreText.text = string.IsNullOrEmpty(name)
                ? "Best Score: -"
                : $"Best Score: {name} : {score}";
        }
    }

    public void StartGame()
    {
        if (DataManager.Instance != null && NameInput != null)
            DataManager.Instance.PlayerName = NameInput.text;

        SceneManager.LoadScene("main");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
