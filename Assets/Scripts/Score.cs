using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    void Update()
    {
        bestScoreText.text = "Best: " + GameManager.singletonGM.best;
        scoreText.text = "Score: " + GameManager.singletonGM.score;
    }
}
