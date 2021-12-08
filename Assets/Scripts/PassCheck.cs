using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCheck : MonoBehaviour {

    private void OnTriggerEnter(Collider other)     
    {
        GameManager.singletonGM.AddScore(2);        //add 2 points whenever ball passes through one disk level

        FindObjectOfType<BallController>().perfectPass++;           //increasing perfect pass count as ball passes through 
    }
}
