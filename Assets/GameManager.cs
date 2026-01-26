using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int win_score = 20;
    
    public TMP_Text scoreText;
    public TMP_Text livesText;

    private int killCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        UpdateScoreText();
    }

    public void EnemyKilled()
    {
        killCount++;
        UpdateScoreText();
    }

    public void UpdateLives(int currentLives)
    {
        if (livesText != null)
        {
            livesText.text = $"Lives: {currentLives}";
        }

        if (currentLives <= 0)
        {
            livesText.text = "Skill issue";
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {killCount}";
        }

        if (killCount == win_score)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayWinMusic();
                scoreText.text = "Congrats, continue if you'd like";
            }
        }
        
    }
}

