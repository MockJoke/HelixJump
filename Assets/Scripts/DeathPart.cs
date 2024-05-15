using UnityEngine;

public class DeathPart : MonoBehaviour 
{
    void OnEnable()
    {
        GetComponent<Renderer>().material.color = Color.red;        //colour of death part changed by renderer
    }

    public void HitDeathPart()
    {
        GameManager.Instance.RestartLevel();     //as the ball hits death part, level restarted
    }
}