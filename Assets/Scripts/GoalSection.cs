using UnityEngine;

public class GoalSection : MonoBehaviour 
{
    void OnCollisionEnter(Collision collision)
    {
        GameManager.Instance.LevelWin();        //as the ball touches the goal, next level should be called 
    }
}