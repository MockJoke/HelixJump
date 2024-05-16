using UnityEngine;

public class GoalSection : MonoBehaviour 
{
    void OnCollisionEnter(Collision collision)
    {
        GameManager.Instance.NextLevel();        //as the ball touches the goal, next level should be called 
    }
}