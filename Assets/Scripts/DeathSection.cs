using UnityEngine;

public class DeathSection : MonoBehaviour 
{
    public void HitDeathPart()
    {
        GameManager.Instance.RestartLevel();     //as the ball hits death part, level restarted
    }
}