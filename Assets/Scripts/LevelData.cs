using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public List<Level> levels = new List<Level>();
}

[Serializable]
public class Level
{
    public Color BgColor = Color.white;
    public Color RingColor = Color.white;
    public Color PillarColor = Color.white;
    public Color BallColor = Color.white;
    public List<Ring> rings = new List<Ring>();
}

[Serializable]
public struct Ring
{
    [Range(1,11)]
    public int totalSections;      

    [Range(0, 11)]
    public int deathSections;
}