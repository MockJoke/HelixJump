using UnityEngine;

public class Goal : MonoBehaviour 
{
    void OnCollisionEnter(Collision collision)
    {
        GameManager.Instance.NextLevel();        //as the ball touches the goal, next level should be called 
    }
}