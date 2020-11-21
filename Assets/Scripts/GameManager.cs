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
    public GameUIControl uiControl;
    private PlayerData playerData;
    private gameState currentState;

    public GameObject[] currentBubbleSpirit;
    public GameObject[] currentBubbleProjectile;
    public int bubbleCounter = 0;
    public bool canMove;
    public bool isInvincible = true;
    private Vector3 originalPos;



    private enum gameState
    {
        LOAD,
        PAUSE,
        RUN,
        LOSE,
        CLEARED,
        NEXT,
        FOCUS
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

        // we can probably just start the game with these already disabled in the editor
        // I'm keeping this here for testing purposes
        uiControl.hideMenu();
        uiControl.hideLost();
        uiControl.hideResult();
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
            setPause();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            saveGame();
        }

        if (Input.GetKeyDown(KeyCode.K))
            playerHit();

        if (Input.GetKeyDown(KeyCode.P))
        {
            currentState = gameState.LOSE;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            setCleared();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(isInvincible == false)
            {
                isInvincible = true;
            }
            else
            {
                isInvincible = false;
            }
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
            uiControl.showMenu();
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
        RunStatistics.Instance.currentStage = 1;
        RunStatistics.Instance.time = 0f;
        RunStatistics.Instance.bubblesCleared = 0;
        float difficulty = setStageDifficulty(RunStatistics.Instance.currentStage);
        //Debug.Log("Diff: " + difficulty);
        mapGenerator.normalGeneration(difficulty);
        mapGenerator.generateNewGrid();
        gameSpawner.spawnBubbles(difficulty);
        RunStatistics.Instance.currentLife = 3;
        originalPos = mPlayer.transform.position;
        currentState = gameState.RUN;
        unpauseGame();
    }

    /*
     * pauseSequence method, run when ESC is pressed. All objects cannot move.
     * Used to show the main menu of the game
     * */
    private void pauseSequence()
    {
        uiControl.showMenu();
    }

    /*
     * runSequence method, run when the session is play. 
     * Player, Bubble spirit and bullet can move.
     * */
    private void runSequence()
    {
        buttonControl();
        RunStatistics.Instance.time += Time.smoothDeltaTime;

        if(bubbleCounter == 0)
        {
            setCleared();
        }
    }

    /*
     * loseSequence method, used to show the player's final result of current run.
     * */
    private void loseSequence()
    {
        uiControl.updateLost();
        uiControl.showLost();
    }

    /*
     * clearsedSequence method, used to show the player's result in current run.
     * Runs after all bubble spirit on the map are cleared. 
     * */
    private void clearedSequence()
    {
        uiControl.updateResult();
        uiControl.showResult();
    }

    // nextSequence method, used to create a new stage for player
    private void nextSequence()
    {
        uiControl.hideResult();
        clearEnemy(); // Not necessary if everything runs well.
        RunStatistics.Instance.currentStage++;
        float difficulty = setStageDifficulty(RunStatistics.Instance.currentStage);
        //Debug.Log("Diff: " + difficulty);

         //TODO: spawn boss bubble every 3 level for alpha playtest
        if (RunStatistics.Instance.currentStage % 3 == 0)
        {
            mapGenerator.bossGeneration(difficulty);
            mapGenerator.generateNewGrid();           
            //gameSpawner.spawnBubbles();
        }
        //TODO: spawn normal bubbles
        else
        {
            mapGenerator.normalGeneration(difficulty);
            mapGenerator.generateNewGrid();           
            gameSpawner.spawnBubbles(difficulty);
        }

        uiControl.updateStage();
        originalPos = mPlayer.transform.position;
        currentState = gameState.RUN;
    }


    private float setStageDifficulty(int stage)
    {
        float difficulty;
        //curve is the stage scale, set to 2 for quick demo of the curve,
        //change to something like 12 for full game.
        int curve = 2;
        difficulty = 100/(1 + Mathf.Exp(-((stage/curve) - (1.7f * Mathf.Exp(1)))));
        return difficulty;
    }

    private void clearEnemy()
    {
        currentBubbleProjectile = GameObject.FindGameObjectsWithTag("EnemyBullet");

        for(int i = 0; i < currentBubbleSpirit.Length; i++)
        {
            if(currentBubbleSpirit[i] != null)
            {
                Destroy(currentBubbleSpirit[i]);
            }
        }

        for(int i = 0; i < currentBubbleProjectile.Length; i++)
        {
            if (currentBubbleProjectile[i] != null)
            {
                Destroy(currentBubbleProjectile[i]);
            }
        }

        bubbleCounter = 0;
    }

    private void pauseGame()
    {
        Time.timeScale = 0;
        canMove = false;
    }

    private void unpauseGame()
    {
        Time.timeScale = 1;
        canMove = true;
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
        bubbleCounter--;
    }

    #endregion;

    // Method used to restart the run from the beginning
    public void restartLevel()
    {
        uiControl.hideLost();
        clearEnemy();
        currentState = gameState.LOAD;
    }

    // Method for button to change game state from Run to Pause
    public void setPause()
    {
        currentState = gameState.PAUSE;
        pauseGame();
    }

    // Method for button to change game state from Pause to Run
    public void setRun()
    {
        uiControl.hideMenu();
        unpauseGame();
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
        pauseGame();
        currentState = gameState.LOSE;
    }

    public void setCleared()
    {
        pauseGame();
        RunStatistics.Instance.stagesCleared++;
        currentState = gameState.CLEARED;
    }

    // Method for button to change game state to NEXT
    public void setNextSequence()
    {
        currentState = gameState.NEXT;
    }

    public void playerHit()
    {
        RunStatistics.Instance.currentLife--;
        mPlayer.transform.position = originalPos;
    }
}
