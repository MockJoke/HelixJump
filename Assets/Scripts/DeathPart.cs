using UnityEngine;

public class DeathPart : MonoBehaviour 
{
    void OnEnable()     //as the object gets enabled
    {
        GetComponent<Renderer>().material.color = Color.red;        //colour of death part changed by renderer
    }

    public void HittedDeathPart()
    {
        GameManager.singletonGM.RestartLevel();     //as the ball hits death part, level restarted
    }
}
