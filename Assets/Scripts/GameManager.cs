using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager theManager = null;
    public PlayerBehavior mPlayer = null;
    public Spawner gameSpawner;
    public MapGenerator mapGenerator;
    public Image mainMenuUI;
    public Image lostScreenUI; 
    public Image resultScreenUI;
    private PlayerData playerData;
    private gameState currentState;

    /*
     * Currently, Lost screen and result screen share the same format. 
     * Depends on the finalized design, will merge them if necessary.
     * 
     * */

    private CanvasGroup mainMenuGroup;
    private CanvasGroup lostScreenGroup;
    private CanvasGroup resultScreenGroup;
    private int bubbleCounter = 0;
    public bool canMove;

    private enum gameState
    {
        LOAD,
        PAUSE,
        RUN,
        LOSE,
        CLEARED,
        NEXT
    }

    void Start()
    {
        if (!theManager)
        {
            theManager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PlayerBulletBehavior.setParent(mPlayer);
        CaptureBulletBehavior.setParent(mPlayer);

        // Ideally, I want to clean this up so void start isn't as crowded
        mainMenuGroup = mainMenuUI.GetComponent<CanvasGroup>();
        lostScreenGroup = lostScreenUI.GetComponent<CanvasGroup>();
        resultScreenGroup = resultScreenUI.GetComponent<CanvasGroup>();
        // we can probably just start the game with these already disabled in the editor
        // I'm keeping this here for testing purposes
        hideMenu();
        hideLost();
        hideResult();
        currentState = gameState.LOAD;
    }

    // Update is called once per frame
    void Update()
    {
        sequenceControl();
    }

    // Method used to contain all the game's control.
    private void buttonControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentState = gameState.PAUSE;
            showMenu();
        }

        if (Input.GetKeyDown(KeyCode.L))
            saveGame();

        if (Input.GetKeyDown(KeyCode.P))
        {
            currentState = gameState.LOSE;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            currentState = gameState.CLEARED;
        }
    }

    /*
     * Will revisit
     * 
     * */
    private void gamepadControl()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            currentState = gameState.PAUSE;
            showMenu();
        }
    }

    // Method used to control all the game's gamestates. 
    private void sequenceControl()
    {
        switch (currentState)
        {
            case gameState.LOAD:
                loadSequence();
                break;
            case gameState.RUN:
                runSequence();
                break;
            case gameState.PAUSE:
                pauseSequence();
                break;
            case gameState.LOSE:
                loseSequence();
                break;
            case gameState.CLEARED:
                clearedSequence();
                break;
            case gameState.NEXT:
                nextSequence();
                break;
            default:
                break;
        }
    }

    /*
     * loadSequence method, run at the beginning of the game. Load all the player's data.
     * Create a new map, spawn bubble behavior and then move to the RUN gamestate.
     * */
    private void loadSequence()
    {
        RunStatistics.Instance.totalScore = 0;
        RunStatistics.Instance.stagesCleared = 0;
        RunStatistics.Instance.time = 0f;
        RunStatistics.Instance.bubblesCleared = 0;
        mapGenerator.generateNewGrid();
        gameSpawner.spawnWorld();
        currentState = gameState.RUN;
        canMove = true;
        
    }

    /*
     * pauseSequence method, run when ESC is pressed. All objects cannot move.
     * Used to show the main menu of the game
     * */
    private void pauseSequence()
    {
        showMenu();
        canMove = false;
    }

    /*
     * runSequence method, run when the session is play. 
     * Player, Bubble spirit and bullet can move.
     * */
    private void runSequence()
    {
        buttonControl();
        RunStatistics.Instance.time += Time.smoothDeltaTime;
        canMove = true;

        if(bubbleCounter == 0)
        {
            RunStatistics.Instance.stagesCleared++;
            currentState = gameState.CLEARED;
        }
    }

    /*
     * loseSequence method, used to show the player's final result of current run.
     * */
    private void loseSequence()
    {
        updateLost();
        showLost();
        canMove = false;
    }

    /*
     * clearsedSequence method, used to show the player's result in current run.
     * Runs after all bubble spirit on the map are cleared. 
     * */
    private void clearedSequence()
    {
        updateResult();
        showResult();
        canMove = false;
    }

    // nextSequence method, used to create a new stage for player
    private void nextSequence()
    {
        hideResult();
        mapGenerator.generateNewGrid();
        gameSpawner.spawnWorld();
        currentState = gameState.RUN;
    }

    #region Data Control

    // Method use the save the player's progress
    public void saveGame()
    {
        SaveSystem.savePlayer();
    }

    public void addBubble()
    {
        bubbleCounter++;
    }

    public void bubbleCleared()
    {
        RunStatistics.Instance.bubblesCleared++;
        RunStatistics.Instance.totalScore += 5;
        bubbleCounter--;
    }

    #endregion;

    #region UI Control
    // Method used to restart the run from the beginning
    public void restartLevel()
    {
        hideLost();
        currentState = gameState.LOAD;
    }

    // Method Used to show the menu screen
    private void showMenu()
    {
        mainMenuGroup.alpha = 1f;
        mainMenuGroup.blocksRaycasts = true;
        mainMenuGroup.interactable = true;
    }

    // Method used to hide the menu screen
    private void hideMenu()
    {
        mainMenuGroup.alpha = 0f;
        mainMenuGroup.blocksRaycasts = false;
        mainMenuGroup.interactable = false;
    }

    // Method used to show the lose screen
    private void showLost()
    {
        lostScreenGroup.alpha = 1f;
        lostScreenGroup.blocksRaycasts = true;
        lostScreenGroup.interactable = true;
    }

    // Method used to hide the Lose screen
    private void hideLost()
    {
        lostScreenGroup.alpha = 0f;
        lostScreenGroup.blocksRaycasts = false;
        lostScreenGroup.interactable = false;
    }

    // Method used to show the result screen
    private void showResult()
    {
        resultScreenGroup.alpha = 1f;
        resultScreenGroup.blocksRaycasts = true;
        resultScreenGroup.interactable = true;
    }

    // Method used to hide the result screen
    private void hideResult()
    {
        resultScreenGroup.alpha = 0f;
        resultScreenGroup.blocksRaycasts = false;
        resultScreenGroup.interactable = false;
    }

    // Method for button to change game state from Pause to Run
    public void setRun()
    {
        hideMenu();
        currentState = gameState.RUN;
    }

    // Method for not sure when to use this yet
    public void setLoad()
    {
        currentState = gameState.LOAD;
    }

    // Method for Playerbehavior to change game state to Lose
    public void setLose()
    {
        currentState = gameState.LOSE;
    }

    // Method for button to change game state to NEXT
    public void setNextSequence()
    {
        hideResult();
        currentState = gameState.NEXT;
    }

    // Method used to update the result screen. 
    private void updateResult()
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
    private void updateLost()
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
    #endregion


}
