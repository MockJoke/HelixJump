using UnityEngine;

public class Goal : MonoBehaviour 
{
    void OnCollisionEnter(Collision collision)
    {
        GameManager.singletonGM.NextLevel();        //as the ball touches the goal, next level should be called 
    }
}