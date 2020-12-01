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
    public Image trapButtonUI;
    public Image captureButtonUI;
    public Image rollButtonUI;
    public Image lifeButtonUI;
    public Image tutorialOne;
    public Image tutorialTwo;
    public Text stageUI;
    public Text playerLifeUI;
    public Text scoreUI;
    public Text trapCountUI;

    private CanvasGroup mainMenuGroup;
    private CanvasGroup lostScreenGroup;
    private CanvasGroup resultScreenGroup;
    private CanvasGroup upgradeScreenGroup;
    private CanvasGroup helpScreenGroup;

    private float rollCooldown;
    private float trapCooldown;
    private float captureCooldown;
    private int trapCount;

    public bool trapMax = false;
    public bool captureMax = false;
    public bool rollMax = false;
    public bool lifeMax = false;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuGroup = mainMenuUI.GetComponent<CanvasGroup>();
        lostScreenGroup = lostScreenUI.GetComponent<CanvasGroup>();
        resultScreenGroup = resultScreenUI.GetComponent<CanvasGroup>();
        upgradeScreenGroup = upgradeScreenUI.GetComponent<CanvasGroup>();
        helpScreenGroup = helpScreenUI.GetComponent<CanvasGroup>();

        /*
        rollCooldown = thePlayer.dashCoolDown;
        trapCooldown = thePlayer.shootCoolDown - 0.0825f;
        captureCooldown = thePlayer.captureCoolDown;
        */
    }


     void Update()
    {
        if (thePlayer.isCapturing == true)
        {
            captureUI.fillAmount = 1;
        }
        rollCooldown = thePlayer.dashCoolDown;
        trapCooldown = thePlayer.shootCoolDown - 0.0825f;
        captureCooldown = thePlayer.captureAfterSec + .825f;
        //captureCooldown = thePlayer.captureCoolDown;
        trapCount = thePlayer.getTrapCount();
        if (GameManager.theManager.canMove == true)
        {
            buttonControl();
        }

        updateLives();
        updateScore();
        updateTrapCount();
        updateCaptureIcon();
        updateUpgradeButton();

        if (trapUI.fillAmount < 1)
        {
            updateTrap();
        }
        else
        {
            colorTrap();
        }

        if (captureUI.fillAmount < 1)
        {
            updateCapture();
        } 
        else
        {
            colorCapture();
        }

        if (rollUI.fillAmount < 1)
        {
            updateRoll();
        }
        else
        {
            colorRoll();
        }
    }

    private void buttonControl()
    {
        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.K)) && trapUI.fillAmount == 1 && trapCount > 0)
            activateTrap();
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.L)) && captureUI.fillAmount == 1)
            activateCapture();
        if (Input.GetKeyDown(KeyCode.Space) && rollUI.fillAmount == 1)
            activateRoll();
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

    public void helpController()
    {
        showHelp();
        helpPageOne();
    }

    public void showHelp()
    {
        helpScreenGroup.alpha = 1f;
        helpScreenGroup.interactable = true;
        helpScreenGroup.blocksRaycasts = true;
    }

    public void hideHelp()
    {
        helpScreenGroup.alpha = 0f;
        helpScreenGroup.interactable = false;
        helpScreenGroup.blocksRaycasts = false;
    }

    public void helpPageOne()
    {
        showFirstHelp();
        hideSecondHelp();
    }

    public void helpPageTwo()
    {
        showSecondHelp();
        hideFirstHelp();
    }

    public void showFirstHelp()
    {
        GameObject pageHelp = helpScreenUI.transform.Find("Page Help").gameObject;
        CanvasGroup firstTutorialGroup = tutorialOne.GetComponent<CanvasGroup>();
        pageHelp.GetComponent<Text>().text = "1/2";

        firstTutorialGroup.alpha = 1;
        firstTutorialGroup.interactable = true;
        firstTutorialGroup.blocksRaycasts = true;
    }

    public void hideFirstHelp()
    {
        CanvasGroup firstTutorialGroup = tutorialOne.GetComponent<CanvasGroup>();

        firstTutorialGroup.alpha = 0;
        firstTutorialGroup.interactable = false;
        firstTutorialGroup.blocksRaycasts = false;
    }

    public void showSecondHelp()
    {
        GameObject pageHelp = helpScreenUI.transform.Find("Page Help").gameObject;
        CanvasGroup secondTutorialGroup = tutorialTwo.GetComponent<CanvasGroup>();
        pageHelp.GetComponent<Text>().text = "2/2";

        secondTutorialGroup.alpha = 1;
        secondTutorialGroup.interactable = true;
        secondTutorialGroup.blocksRaycasts = true;
    }

    public void hideSecondHelp()
    {
        CanvasGroup secondTutorialGroup = tutorialTwo.GetComponent<CanvasGroup>();

        secondTutorialGroup.alpha = 0;
        secondTutorialGroup.interactable = false;
        secondTutorialGroup.blocksRaycasts = false;
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
        int chainCleard = 0;
        for (int i = 0; i < RunStatistics.Instance.bubblesChainCleared.Length; i++)
        {
            chainCleard += RunStatistics.Instance.bubblesChainCleared[i];
        }

        chainText.GetComponent<Text>().text = "Bubble Chained: " + chainCleard;
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
        int chainCleard = 0;
        for (int i = 0; i < RunStatistics.Instance.bubblesChainCleared.Length; i++)
        {
            chainCleard += RunStatistics.Instance.bubblesChainCleared[i];
        }

        chainText.GetComponent<Text>().text = "Bubble Chained: " + chainCleard;
    }

    // Method used to update the upgrade screen.
    public void updateUpgrade()
    {
        GameObject upgradeBG = upgradeScreenUI.transform.Find("Upgrade Background").gameObject;
        GameObject trapText = upgradeBG.transform.Find("trapText").gameObject;
        GameObject captureText = upgradeBG.transform.Find("captureText").gameObject;
        GameObject rollText = upgradeBG.transform.Find("rollText").gameObject;
        GameObject lifeText = upgradeBG.transform.Find("lifeText").gameObject;

        trapText.GetComponent<Text>().text = "Bubble Trap Cap: " + thePlayer.trapCountCap + " (Max 10)";
        captureText.GetComponent<Text>().text = "Bubble Capture Cooldown: " + thePlayer.captureCoolDown + " (Max 1)";
        rollText.GetComponent<Text>().text = "Roll Cooldown: " + thePlayer.dashCoolDown + " (Max 4)";
        lifeText.GetComponent<Text>().text = "Player Life: " + RunStatistics.Instance.currentLife + " (Max 3)";
    }

    public void updateUpgradeButton()
    {
       if(RunStatistics.Instance.totalScore < 500)
        {
            hideTrapButton();
            hideCaptureButton();
            hideRollButton();
            hideLifeButton();
        } else if (RunStatistics.Instance.totalScore > 500 && RunStatistics.Instance.totalScore < 1000)
        {
            hideTrapButton();
            hideCaptureButton();
            if(rollMax != true)
            {
                showRollButton();
            }
            else
            {
                hideRollButton();
            }
            hideLifeButton();
        } else if (RunStatistics.Instance.totalScore > 1000)
        {
            if(trapMax != true)
            {
                showTrapButton();
            }
            else
            {
                hideTrapButton();
            }

            if (captureMax != true)
            {
                showCaptureButton();
            }
            else
            {
                hideCaptureButton();
            }

            if (rollMax != true)
            {
                showRollButton();
            }
            else
            {
                hideRollButton();
            }

            if(RunStatistics.Instance.currentLife < 3)
            {
                lifeMax = false;
            }
            else
            {
                lifeMax = true;
            }

            if (lifeMax != true)
            {
                showLifeButton();
            }
            else
            {
                hideLifeButton();
            }
        }
        
    }

    public void showTrapButton()
    {
        CanvasGroup trapButtonCanvas = trapButtonUI.GetComponent<CanvasGroup>();

        trapButtonCanvas.alpha = 1f;
        trapButtonCanvas.interactable = true;
        trapButtonCanvas.blocksRaycasts = true;

    }

    public void hideTrapButton()
    {
        CanvasGroup trapButtonCanvas = trapButtonUI.GetComponent<CanvasGroup>();

        trapButtonCanvas.alpha = 0.25f;
        trapButtonCanvas.interactable = false;
        trapButtonCanvas.blocksRaycasts = false;
    }

    public void showCaptureButton()
    {
        CanvasGroup captureButtonCanvas = captureButtonUI.GetComponent<CanvasGroup>();

        captureButtonCanvas.alpha = 1f;
        captureButtonCanvas.interactable = true;
        captureButtonCanvas.blocksRaycasts = true;
    }

    public void hideCaptureButton()
    {
        CanvasGroup captureButtonCanvas = captureButtonUI.GetComponent<CanvasGroup>();

        captureButtonCanvas.alpha = 0.25f;
        captureButtonCanvas.interactable = false;
        captureButtonCanvas.blocksRaycasts = false;
    }

    public void showRollButton()
    {
        CanvasGroup rollButtonCanvas = rollButtonUI.GetComponent<CanvasGroup>();

        rollButtonCanvas.alpha = 1f;
        rollButtonCanvas.interactable = true;
        rollButtonCanvas.blocksRaycasts = true;
    }

    public void hideRollButton()
    {
        CanvasGroup rollButtonCanvas = rollButtonUI.GetComponent<CanvasGroup>();

        rollButtonCanvas.alpha = 0.25f;
        rollButtonCanvas.interactable = false;
        rollButtonCanvas.blocksRaycasts = false;
    }

    public void showLifeButton()
    {
        CanvasGroup lifeButtonCanvas = lifeButtonUI.GetComponent<CanvasGroup>();

        lifeButtonCanvas.alpha = 1f;
        lifeButtonCanvas.interactable = true;
        lifeButtonCanvas.blocksRaycasts = true;
    }

    public void hideLifeButton()
    {
        CanvasGroup lifeButtonCanvas = lifeButtonUI.GetComponent<CanvasGroup>();

        lifeButtonCanvas.alpha = 0.25f;
        lifeButtonCanvas.interactable = false;
        lifeButtonCanvas.blocksRaycasts = false;
    }

    public void setCost(int upgradeCost)
    {
        GameObject upgradeBG = upgradeScreenUI.transform.Find("Upgrade Background").gameObject;
        GameObject costText = upgradeBG.transform.Find("costText").gameObject;
        costText.GetComponent<Text>().text = "Cost: " + upgradeCost + " Points";
    }

    public void activateRoll()
    {
        rollUI.fillAmount = 0;
        GameObject iconHUD = rollUI.transform.Find("rollHUD").gameObject;
        Image iconImage = iconHUD.GetComponent<Image>();
        iconImage.GetComponent<Image>().color = new Color32(90, 89, 89, 255);
    }

    private void colorRoll()
    {
        GameObject iconHUD = rollUI.transform.Find("rollHUD").gameObject;
        Image iconImage = iconHUD.GetComponent<Image>();
        iconImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void updateRoll()
    {
        rollUI.fillAmount += 1 / rollCooldown * Time.smoothDeltaTime;
    }

    public void activateTrap()
    {
        trapUI.fillAmount = 0;
        GameObject iconHUD = trapUI.transform.Find("trapHUD").gameObject;
        Image iconImage = iconHUD.GetComponent<Image>();
        iconImage.GetComponent<Image>().color = new Color32(90, 89, 89, 255);
    }

    private void colorTrap()
    {
        GameObject iconHUD = trapUI.transform.Find("trapHUD").gameObject;
        Image iconImage = iconHUD.GetComponent<Image>();
        iconImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void updateTrap() 
    {
        trapUI.fillAmount += 1 / trapCooldown * Time.smoothDeltaTime;
    }

    public void activateCapture()
    {
        captureUI.fillAmount = 0;
        
        GameObject iconHUD = captureUI.transform.Find("captureHUD").gameObject;
        Image iconImage = iconHUD.GetComponent<Image>();
        iconImage.GetComponent<Image>().color = new Color32(90, 89, 89, 255);
    }

    private void colorCapture()
    {
        GameObject iconHUD = captureUI.transform.Find("captureHUD").gameObject;
        Image iconImage = iconHUD.GetComponent<Image>();
        iconImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void updateCaptureIcon()
    {
        GameObject iconHUD = captureUI.transform.Find("captureHUD").gameObject;

        if (thePlayer.getbubbleSprite() != null)
        {
            switch (thePlayer.getbubbleSprite().color)
            {
                case 0:
                    iconHUD.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/bubble_red_bright");
                    break;
                case 1:
                    iconHUD.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/bubble_blue_bright");
                    break;
                case 2:
                    iconHUD.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/bubble_yellow_bright");
                    break;
                case 3:
                    iconHUD.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/bubble_purple_bright");
                    break;
                case 4:
                    iconHUD.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/bubble_purple_bright");
                    break;
                default:
                    break;
            }
        }
        else
        {
            iconHUD.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/capture");
        }
    }

    public void updateCapture()
    {
        captureUI.fillAmount += 1 / captureCooldown * Time.smoothDeltaTime;
    }

    public void updateLives()
    {
        playerLifeUI.text = "x" + RunStatistics.Instance.currentLife;
    }

    public void updateScore()
    {
        scoreUI.text = "SCORE " + RunStatistics.Instance.totalScore;
    }
    public void updateTrapCount()
    {
        trapCountUI.text = "x" + RunStatistics.Instance.trapCount;
    }

    public void updateStage()
    {
        stageUI.text = "STAGE " + RunStatistics.Instance.currentStage;
    }

}
