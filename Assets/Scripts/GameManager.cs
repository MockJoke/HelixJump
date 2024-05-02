using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static GameManager singletonGM;        //this object is only available once in whole project, so only one instance of game manager 
    public int best;
    public int score;
    public int currentStage = 0;
    
    void Awake()
    {
        if(singletonGM == null)
            singletonGM = this;
        else if(singletonGM != this)
            Destroy(gameObject);

        best = PlayerPrefs.GetInt("Highscore");
    }

    public void NextLevel()
    {
        currentStage++;
        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
    }

    public void RestartLevel()      //restart the current level/state
    {
        Debug.Log("Restarting Level");

        singletonGM.score = 0;
        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;

        if(score > best)
        {
            PlayerPrefs.SetInt("Highscore", score);
            best = score;
        }
    }
}
