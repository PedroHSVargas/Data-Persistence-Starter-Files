using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private string m_PlayerName = "Player";


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"[MainManager] Start. DataManager.Instance is null? {DataManager.Instance == null}");
        Debug.Log($"[MainManager] HighScoreText assigned? {HighScoreText != null}");

        if (DataManager.Instance != null)
        {
            Debug.Log($"[MainManager] PlayerName='{DataManager.Instance.PlayerName}', HighScoreName='{DataManager.Instance.HighScoreName}', HighScore={DataManager.Instance.HighScore}");
            if (!string.IsNullOrEmpty(DataManager.Instance.PlayerName))
                m_PlayerName = DataManager.Instance.PlayerName;
        }

        UpdateScoreText();
        UpdateHighScoreText();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        UpdateScoreText();
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (DataManager.Instance != null && m_Points > DataManager.Instance.HighScore)
        {
            DataManager.Instance.HighScore = m_Points;
            DataManager.Instance.HighScoreName = m_PlayerName;
            DataManager.Instance.SaveHighScore();
            UpdateHighScoreText();
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    private void UpdateScoreText()
    {
        ScoreText.text = $"{m_PlayerName} - Score : {m_Points}";
    }

    private void UpdateHighScoreText()
    {
        Debug.Log($"[MainManager] UpdateHighScoreText called. HighScoreText null? {HighScoreText == null}");

        if (HighScoreText == null)
        {
            Debug.LogWarning("[MainManager] HighScoreText is NOT assigned in Inspector. Drag the Text component into the High Score Text field.");
            return;
        }

        if (DataManager.Instance == null || string.IsNullOrEmpty(DataManager.Instance.HighScoreName))
        {
            HighScoreText.text = "Best Score: -";
            Debug.Log("[MainManager] No high score yet — showing 'Best Score: -'");
            return;
        }

        HighScoreText.text = $"Best Score: {DataManager.Instance.HighScoreName} : {DataManager.Instance.HighScore}";
        Debug.Log($"[MainManager] HighScoreText set to: {HighScoreText.text}");
    }
}
