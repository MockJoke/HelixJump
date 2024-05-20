using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Canvas gameCanvas;

    public void OnGameOver()
    {
        Time.timeScale = 0;
        
        gameCanvas.enabled = false;
        gameOverCanvas.enabled = true;
    }

    public void PlayAgain()
    {
        gameOverCanvas.enabled = false;
        gameCanvas.enabled = true;
        
        GameManager.Instance.RestartLevel();
        
        Time.timeScale = 1;
    }
}
