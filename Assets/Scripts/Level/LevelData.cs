using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public List<Level> levels = new List<Level>();
    public List<ColorPalette> ColorPalettes = new List<ColorPalette>();
}

[Serializable]
public class Level
{
    public List<RingData> rings = new List<RingData>();
    
    // public Color BgColor = Color.white;
    // public Color BgVignetteColor = Color.white;
    // public Color BallColor = Color.white;
    // public Color PillarColor = Color.white;
    // public Color NormalSectionColor = Color.white;
    // public Color DangerSectionColor = Color.white;
    // public Color GoalSectionColor = Color.white;
}

[Serializable]
public class ColorPalette
{
    public Color BgColor = Color.white;
    public Color BgVignetteColor = Color.white;
    public Color BallColor = Color.white;
    public Color PillarColor = Color.white;
    public Color NormalSectionColor = Color.white;
    public Color DangerSectionColor = Color.white;
    public Color GoalSectionColor = Color.white;
}

[Serializable]
public struct RingData
{
    // max range : 11 because each ring contains 12 sections but always need 1 section empty for pass through
    [Range(1,11)]
    public int totalSections;      

    [Range(0, 11)]
    public int dangerSections;
    
    [Range(0, 11)]
    public int droppingSections;
    
    [Range(0, 11)]
    public int blinkingSections;
}