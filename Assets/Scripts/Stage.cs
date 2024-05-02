using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Level
{
    [Range(1,11)]
    public int partCount = 11;      

    [Range(0, 11)]
    public int deathPartCount = 1;      //initial value of death part per stage is 1
}

[CreateAssetMenu(fileName = "New Stage")]
public class Stage : ScriptableObject 
{
    public Color stageBackgroundColor = Color.white;
    public Color stageLevelPartColor = Color.white;
    public Color HelixColor = Color.white; 
    public Color stageBallColor = Color.white;
    public List<Level> levels = new List<Level>();
}
