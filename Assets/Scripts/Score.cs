using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    [SerializeField] private Text txtScore;
    [SerializeField] private Text txtBest;

    void Update()
    {
        txtBest.text = "Best: " + GameManager.singletonGM.best;
        txtScore.text = "Score: " + GameManager.singletonGM.score;
    }
}
