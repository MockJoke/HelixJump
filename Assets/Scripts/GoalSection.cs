using UnityEngine;

public class GoalSection : MonoBehaviour 
{
    void OnCollisionEnter(Collision collision)
    {
        GameManager.Instance.LevelWin(); 
    }
}