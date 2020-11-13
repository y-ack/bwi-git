using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string profileName { get; set; }
    public float totalPlayTime { get; set; }
    public float averageStageTime { get; set; }
    public int stageCleared { get; set; }
    public int maxLevel { get; set; }
    public int maxScore { get; set; }
    public int maxBubbleCleared { get; set; }
    public int maxBubbleMatched { get; set; }
    public int maxBossCleared { get; set; }
    public float lastSessionTime { get; set; }
    public int lastStageCleared { get; set; }
    public int lastLevel { get; set; }
    public int lastScore { get; set; }
    public int lastBubbleCleared { get; set; }
    public int lastBubbleMatched { get; set; }
    public int lastBossMatched { get; set; }

    public PlayerData()
    {
    }
}
