﻿using System;
using System.Collections.Generic;

//
// the statistics data class.
// access it with e.g.
// RunStatistics.Instance.bubblesCleared++
//
public class RunStatistics : Singleton<RunStatistics>
{
    // (Optional) Prevent non-singleton constructor use.
    protected RunStatistics() { }

    // these are stats for a single run, not sure if totals should
    // be separate or also accessed here

    public string playerName;
    //don't set this, save manager will calculate at run finish
    public string savePath = "";
    public int totalSaveNum; // How many save files are in total. Change when Delete save is implemented.
    public int saveNum; // This player's statistic saveNumfile. Use to create savePath.
    public int currentLife; // Player start with 3
    public float time;
    public int controlMode; // 0 = Mouse & Keyboard. 1 = Controller
    public int currentStage;
    public int stagesCleared;
    public int totalScore;
    public int bubblesCleared;
    public int bossCleared;
    public bool isNew; // If true, show the control.

    // index with BubbleColor.red etc. int value;
    public int[] bubblesChainCleared = new int[BubbleColor.count];
    // Total Chain Clears: calculate when displaying
    // Clears Per Minute: calculate when displaying

    public int trapsUsed;
    public int trapsMissed; //set when player bullet hits wall/expires
    public int capturesUsed;
    

    public float grazeTime;
    public float focusTime;
    public int rollsPerformed;
}
