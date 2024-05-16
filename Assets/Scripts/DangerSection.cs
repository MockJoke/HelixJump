using UnityEngine;

public class DangerSection : MonoBehaviour 
{
    public void OnHitDangerSection()
    {
        GameManager.Instance.RestartLevel();     //as the ball hits death part, level restarted
    }
}