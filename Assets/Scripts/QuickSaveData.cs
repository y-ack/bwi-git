using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QuickSaveData 
{
    public string playerName;
    public int currentLife;
    public float time;
    public int currentStage;
    public int stagesCleared;
    public int totalScore;
    public int bubblesCleared;
    public int bossCleared;

    public int[] bubblesChainCleared;

    public GameObject[] currentBubbleUnit;
    public GameObject[] currentBubbleSpirit;
    public GameObject[] currentBubbleProjectile;
    public GameObject[] currentPlayerProjectile;

    public PlayerBehavior thePlayer;

    public QuickSaveData()
    {

    }



}
