using System.Collections;
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

        if (trapUI.fillAmount < 1)
        {
            updateTrap();
        }
        else
        {
            colorTrap();
        }

        if (captureUI.fillAmount < 1 && thePlayer.isCapturing == true)
        {
            captureUI.fillAmount = 1;
            colorCapture();
        }
        else if (captureUI.fillAmount < 1 && thePlayer.isCapturing == false)
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
        GameObject speedText = upgradeBG.transform.Find("speedText").gameObject;
        GameObject trapText = upgradeBG.transform.Find("trapText").gameObject;
        GameObject captureText = upgradeBG.transform.Find("captureText").gameObject;
        GameObject rollText = upgradeBG.transform.Find("rollText").gameObject;
        GameObject lifeText = upgradeBG.transform.Find("lifeText").gameObject;

        speedText.GetComponent<Text>().text = "Iris Movement Speed: " + thePlayer.moveSpeed + " (Max 18)";
        trapText.GetComponent<Text>().text = "Bubble Trap Cooldown: " + thePlayer.shootCoolDown + " (Max 0.3)";
        captureText.GetComponent<Text>().text = "Bubble Capture Cooldown: " + thePlayer.captureCoolDown + " (Max 2)";
        rollText.GetComponent<Text>().text = "Roll Cooldown: " + thePlayer.dashCoolDown + " (Max 4)";
        lifeText.GetComponent<Text>().text = "Player Life: " + RunStatistics.Instance.currentLife + " (Max 3)";
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
