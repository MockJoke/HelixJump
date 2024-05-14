using UnityEngine;

public class PassCheck : MonoBehaviour
{
    [SerializeField] private BallController ballController;

    void Awake()
    {
        if (ballController == null)
            ballController = FindObjectOfType<BallController>();
    }

    void OnTriggerEnter(Collider other)     
    {
        GameManager.Instance.AddScore(2);        //add 2 points whenever ball passes through one disk level

        ballController.perfectPass++;           //increasing perfect pass count as ball passes through 
    }
}
