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
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
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

        AddPoint(0);
        DisplayBestScore();
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
                LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        if (GameManager.Instance != null)
            ScoreText.text = $"{GameManager.Instance.PlayerName}'s Score : {m_Points}";
        else
            ScoreText.text = $"Anon's Score : {m_Points}";
    }

    void DisplayBestScore()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.BestScore <= 0)
            {
                BestScoreText.text = "No Best Scorer!";
            }
            else
            {
                BestScoreText.text = $"Best Scorer: {GameManager.Instance.BestScorer}: {GameManager.Instance.BestScore}";
            }
        }
        else
        {
            BestScoreText.text = "No Best Scorer!";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameManager.Instance.SaveGame(m_Points);
        GameManager.Instance.LoadGame();
        GameOverText.SetActive(true);
    }

    public void GoBackToMenu()
    {
        LoadScene("menu");
    }

    private void LoadScene(string name)
    {
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadScene(name);
        else
            SceneManager.LoadScene(name);
    }
}
