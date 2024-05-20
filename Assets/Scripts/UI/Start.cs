using System;
using UnityEngine;

public class Start : MonoBehaviour
{
    [SerializeField] private Canvas startCanvas;
    [SerializeField] private Canvas gameCanvas;

    public Action OnStart;
    
    void Awake()
    {
        Time.timeScale = 0;
    }

    public void OnStartClick()
    {
        startCanvas.enabled = false;
        gameCanvas.enabled = true;

        Time.timeScale = 1;
        
        OnStart?.Invoke();
    }
}
