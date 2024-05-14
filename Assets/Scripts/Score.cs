using TMPro;
using UnityEngine;

public class Score : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    void Update()
    {
        bestScoreText.text = "Best: " + GameManager.Instance.best;
        scoreText.text = "Score: " + GameManager.Instance.score;
    }
}
