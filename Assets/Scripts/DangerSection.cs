using UnityEngine;

public class DangerSection : MonoBehaviour 
{
    public void OnHitDangerSection()
    {
        GameManager.Instance.GameOver();     //as the ball hits death part, level restarted
    }
}