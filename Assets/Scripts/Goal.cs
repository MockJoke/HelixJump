using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        GameManager.singletonGM.NextLevel();        //as the ball touches the goal, next level should be called 
    }
}
