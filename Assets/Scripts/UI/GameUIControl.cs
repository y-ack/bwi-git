﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIControl : MonoBehaviour
{
    public PlayerBehavior thePlayer;
    public Image mainMenuUI;
    public Image lostScreenUI;
    public Image resultScreenUI;
    public Image upgradeScreenUI;
    public Image helpScreenUI;
    public Image captureUI;
    public Image trapUI;
    public Image rollUI;
    public Text stageUI;
    public Text playerLifeUI;
    public Text scoreUI;

    private CanvasGroup mainMenuGroup;
    private CanvasGroup lostScreenGroup;
    private CanvasGroup resultScreenGroup;
    private CanvasGroup upgradeScreenGroup;
    private CanvasGroup helpScreenGroup;

    private float rollCooldown;
    private float trapCooldown;
    private float captureCooldown;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuGroup = mainMenuUI.GetComponent<CanvasGroup>();
        lostScreenGroup = lostScreenUI.GetComponent<CanvasGroup>();
        resultScreenGroup = resultScreenUI.GetComponent<CanvasGroup>();
        upgradeScreenGroup = upgradeScreenUI.GetComponent<CanvasGroup>();
        helpScreenGroup = helpScreenUI.GetComponent<CanvasGroup>();

        rollCooldown = thePlayer.dashCoolDown;
        trapCooldown = thePlayer.shootCoolDown - 0.0825f;
        captureCooldown = thePlayer.captureCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.theManager.canMove == true)
        {
            buttonControl();
        }

        updateLives();
        updateScore();

        if(trapUI.fillAmount <= 1)
        {
            updateTrap();
        }

        if(captureUI.fillAmount <= 1)
        {
            updateCapture();
        }

        if(rollUI.fillAmount <= 1)
        {
            updateRoll();
        }
    }

    private void buttonControl()
    {
        if (Input.GetMouseButton(0) && trapUI.fillAmount == 1)
            trapUI.fillAmount = 0;
        if (Input.GetMouseButtonDown(1) && captureUI.fillAmount == 1)
            captureUI.fillAmount = 0;
        if (Input.GetKeyDown(KeyCode.Space) && rollUI.fillAmount == 1)
            rollUI.fillAmount = 0;
    }

    // Method Used to show the menu screen
    public void showMenu()
    {
        mainMenuGroup.alpha = 1f;
        mainMenuGroup.blocksRaycasts = true;
        mainMenuGroup.interactable = true;
    }

    // Method used to hide the menu screen
    public void hideMenu()
    {
        mainMenuGroup.alpha = 0f;
        mainMenuGroup.blocksRaycasts = false;
        mainMenuGroup.interactable = false;
    }

    // Method used to show the lose screen
    public void showLost()
    {
        lostScreenGroup.alpha = 1f;
        lostScreenGroup.blocksRaycasts = true;
        lostScreenGroup.interactable = true;
    }

    // Method used to hide the Lose screen
    public void hideLost()
    {
        lostScreenGroup.alpha = 0f;
        lostScreenGroup.blocksRaycasts = false;
        lostScreenGroup.interactable = false;
    }

    // Method used to show the result screen
    public void showResult()
    {
        resultScreenGroup.alpha = 1f;
        resultScreenGroup.blocksRaycasts = true;
        resultScreenGroup.interactable = true;
    }

    // Method used to hide the result screen
    public void hideResult()
    {
        resultScreenGroup.alpha = 0f;
        resultScreenGroup.blocksRaycasts = false;
        resultScreenGroup.interactable = false;
    }

    // Method used to show the upgrade screen
    public void showUpgrade()
    {
        upgradeScreenGroup.alpha = 1f;
        upgradeScreenGroup.blocksRaycasts = true;
        upgradeScreenGroup.interactable = true;
    }

    // Method used to hide the upgrade screen
    public void hideUpgrade()
    {
        upgradeScreenGroup.alpha = 0f;
        upgradeScreenGroup.blocksRaycasts = false;
        upgradeScreenGroup.interactable = false;
    }

    public void showHelp()
    {
        helpScreenGroup.alpha = 1f;
    }

    public void hideHelp()
    {
        helpScreenGroup.alpha = 0f;
    }

    // Method used to update the result screen. 
    public void updateResult()
    {
        GameObject statisticBG = resultScreenUI.transform.Find("Statistic Background").gameObject;
        GameObject scoreText = statisticBG.transform.Find("ScoreText").gameObject;
        GameObject stageText = statisticBG.transform.Find("StageText").gameObject;
        GameObject timeText = statisticBG.transform.Find("TimeText").gameObject;
        GameObject clearText = statisticBG.transform.Find("ClearText").gameObject;
        GameObject chainText = statisticBG.transform.Find("ChainText").gameObject;

        scoreText.GetComponent<Text>().text = "Current Score: " + RunStatistics.Instance.totalScore;
        stageText.GetComponent<Text>().text = "Stage Cleared: " + RunStatistics.Instance.stagesCleared;
        timeText.GetComponent<Text>().text = "Session Time: " + RunStatistics.Instance.time;
        clearText.GetComponent<Text>().text = "Bubble Cleared: " + RunStatistics.Instance.bubblesCleared;
        chainText.GetComponent<Text>().text = "Bubble Chained: " + RunStatistics.Instance.bubblesChainCleared.Length;
    }

    // Method used to update the lost screen.
    public void updateLost()
    {
        GameObject statisticBG = lostScreenUI.transform.Find("Statistic Background").gameObject;
        GameObject scoreText = statisticBG.transform.Find("ScoreText").gameObject;
        GameObject stageText = statisticBG.transform.Find("StageText").gameObject;
        GameObject timeText = statisticBG.transform.Find("TimeText").gameObject;
        GameObject clearText = statisticBG.transform.Find("ClearText").gameObject;
        GameObject chainText = statisticBG.transform.Find("ChainText").gameObject;

        scoreText.GetComponent<Text>().text = "Current Score: " + RunStatistics.Instance.totalScore;
        stageText.GetComponent<Text>().text = "Stage Cleared: " + RunStatistics.Instance.stagesCleared;
        timeText.GetComponent<Text>().text = "Session Time: " + RunStatistics.Instance.time;
        clearText.GetComponent<Text>().text = "Bubble Cleared: " + RunStatistics.Instance.bubblesCleared;
        chainText.GetComponent<Text>().text = "Bubble Chained: " + RunStatistics.Instance.bubblesChainCleared.Length;
    }

    // Method used to update the upgrade screen.
    public void updateUpgrade()
    {
        GameObject upgradeBG = upgradeScreenUI.transform.Find("Upgrade Background").gameObject;
        GameObject speedText = upgradeBG.transform.Find("speedText").gameObject;
        GameObject trapText = upgradeBG.transform.Find("trapText").gameObject;
        GameObject captureText = upgradeBG.transform.Find("captureText").gameObject;
        GameObject rollText = upgradeBG.transform.Find("rollText").gameObject;
        GameObject lifeText = upgradeBG.transform.Find("lifeText").gameObject;

        speedText.GetComponent<Text>().text = "Iris Movement Speed: " + thePlayer.normalSpeed + " (Max 18)";
        trapText.GetComponent<Text>().text = "Bubble Trap Cooldown: " + thePlayer.shootCoolDown + " (Max 0.3)";
        captureText.GetComponent<Text>().text = "Bubble Capture Cooldown: " + thePlayer.captureCoolDown + " (Max 2)";
        rollText.GetComponent<Text>().text = "Roll Cooldown: " + thePlayer.dashCoolDown + " (Max 4)";
        lifeText.GetComponent<Text>().text = "Player Life: " + RunStatistics.Instance.currentLife + " (Max 3)";
    }


    public void updateRoll()
    {
        rollUI.fillAmount += 1 / rollCooldown * Time.deltaTime;
    }

    public void updateTrap() 
    {
        trapUI.fillAmount += 1 / trapCooldown * Time.deltaTime;
    }

    public void updateCapture()
    {
        captureUI.fillAmount += 1 / captureCooldown * Time.deltaTime;
    }

    public void updateLives()
    {
        playerLifeUI.text = "x " + RunStatistics.Instance.currentLife;
    }

    public void updateScore()
    {
        scoreUI.text = "SCORE: " + RunStatistics.Instance.totalScore;
    }

    public void updateStage()
    {
        stageUI.text = "STAGE " + RunStatistics.Instance.currentStage;
    }
}
