using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance;        //this object is only available once in whole project, so only one instance of game manager 

    [Header("UI")] 
    [SerializeField] private Start startMenu;
    [SerializeField] private GameOver gameOverMenu;
    [Header("Controllers")]
    [SerializeField] private HelixController helixController;
    [SerializeField] private BallController ballController;
    
    private int currLevel = 0;
    
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

        if (startMenu == null)
            startMenu = FindObjectOfType<Start>();
        
        if (gameOverMenu == null)
            gameOverMenu = FindObjectOfType<GameOver>();
        if (helixController == null)
            helixController = FindObjectOfType<HelixController>();

        if (ballController == null)
            ballController = FindObjectOfType<BallController>();

        startMenu.OnStart += OnGameStart;
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void OnGameStart()
    {
        LoadLevel();
    }
    

    public void RestartLevel()      //restart the current level/state
    {
        Score = 0;
    private void LoadLevel()
    {
        PassedRingCnt = 0;
        ballController.ResetBall();
        helixController.LoadLevel(currLevel);
    }

    public void GameOver()
    {
        gameOverMenu.OnGameOver();
    }

    public void RestartLevel()
    {
        LoadLevel();
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
