using UnityEngine;

public class LevelWin : MonoBehaviour
{
    [SerializeField] private Canvas levelWinCanvas;
    [SerializeField] private Canvas gameCanvas;

    public void OnLevelWin()
    {
        Time.timeScale = 0;
        
        gameCanvas.enabled = false;
        levelWinCanvas.enabled = true;
    }

    public void OnNextLevel()
    {
        levelWinCanvas.enabled = false;
        gameCanvas.enabled = true;
        
        GameManager.Instance.GoToNextLevel();
        
        Time.timeScale = 1;
    }
}
