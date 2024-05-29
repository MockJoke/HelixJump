using TMPro;
using UnityEngine;

public class Score : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    void Start()
    {
        GameManager.Instance.OnScoreChange += UpdateScore;
    }

    void OnDestroy()
    {
        GameManager.Instance.OnScoreChange -= UpdateScore;
    }

    void UpdateScore()
    {
        bestScoreText.text = "Best: " + GameManager.Instance.HighScore;
        scoreText.text = "Score: " + GameManager.Instance.Score;
    }
}