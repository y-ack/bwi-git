using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QuickSaveData 
{
    // Runstatistic Section
    public string playerName;
    public int currentLife;
    public float time;
    public int currentStage;
    public int stagesCleared;
    public int totalScore;
    public int bubblesCleared;
    public int bossCleared;
    public int trapCount;
    public int[] bubblesChainCleared;

    // Game Manager Section
    public int totalBubbles;
    public SerializableBubbleUnit[] currentBubbleUnit;
    public SerializableBubbleSpirit[] currentBubbleSpirit;
    public SerializableBubbleProjectile[] currentBubbleProjectile;
    public SerializablePlayerProjectile[] currentPlayerProjectile;
    public SerializablePlayerProjectile[] capturePlayerProjectile;

    // Map Generator Section
    public float seedValue;
    public float thresholdValue;

    // Player section
    public float moveSpeed;
    public float normalSpeed;
    public float focusSpeed;

    public SerializableVector playerCurrentPos;
    public SerializableVector mousePos;
    public SerializableVector movementVector;
    public SerializableVector moveDir;
    public SerializableVector slideDir;
    public float slideSpeed;

    public bool isDashButtonDown;
    public float DashAmount;
    public float dashCoolDown;
    public float dashAfterSec;

    public float captureCoolDown;
    public float captureAfterSec;

    public float shootCoolDown;
    public float shootAfterSec;

    public bool isCapturing;
    public BubbleSpirit capturedBubble;

    public float extraTrap;
    public int trapCountCap;

    //Increase by 0.25f or 0.5f when upgrading 
    public float trapUpgrade;

    public float spriteBlinkingTimer;
    public float spriteBlinkingMiniDuration;
    public float spriteBlinkingTotalTimer;
    public float spriteBlinkingTotalDuration;
    public bool startBlinking;


    public QuickSaveData()
    {

    }



}
