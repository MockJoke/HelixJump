using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance;        //this object is only available once in whole project, so only one instance of game manager 

    [SerializeField] private HelixController helixController;
    [SerializeField] private BallController ballController;
    
    private int currentStage = 0;
    
    public int HighScore { get; private set; }
    public int Score { get; private set; }
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log($"{Instance.name} {gameObject.name} An instance of this singleton already exists");
            // throw new System.Exception($"{Instance.name} {gameObject.name} An instance of this singleton already exists.");
        }
        else
        {
            Instance = this;
        }

        if (helixController == null)
            helixController = FindObjectOfType<HelixController>();

        if (ballController == null)
            ballController = FindObjectOfType<BallController>();
        
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void NextLevel()
    {
        currentStage++;
        ballController.ResetBall();
        helixController.LoadStage(currentStage);
    }

    public void RestartLevel()      //restart the current level/state
    {
        Debug.Log("Restarting Level");

        Instance.Score = 0;
        ballController.ResetBall();
        helixController.LoadStage(currentStage);
    }

    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;

        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt("HighScore", Score);
        }
    }
}
