﻿using UnityEngine;

public class DangerSection : MonoBehaviour 
{
    public void OnHitDangerSection()
    {
        GameManager.Instance.GameOver();
    }
}