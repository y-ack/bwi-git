using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//using PlayFab;
//using PlayFab.ClientModels;

public class GameManager : MonoBehaviour
{
    public static GameManager theManager = null;
    public PlayerBehavior mPlayer = null;
    public Spawner gameSpawner;
    public MapGenerator mapGenerator;
    public GameUIControl uiControl;
    public GameObject playerRoll;
    private PlayerData playerData;
    private gameState currentState;

    public GameObject[] currentBubbleUnit;
    public GameObject[] currentBubbleSpirit;
    public GameObject[] currentBubbleProjectile;
    public GameObject[] currentProjectile;
    public int unitCounter = 0;
    public int bubbleCounter = 0;
    public bool canMove;
    private bool isHelp;
    public bool isInvincible;
    public bool chainBonus;
    private float chainTime;
    private int chainCount;
    private Vector3 originalPos;

    //leaderboard shit
    public List<string> playFabIDList = new List<string>();
    public List<string> playFabScoreList = new List<string>();



    private enum gameState
    {
        LOAD,
        QUICK,
        PAUSE,
        HELP,
        RUN,
        LOSE,
        CLEARED,
        UPGRADE,
        NEXT
    }
    public void hello()
    {
        Debug.Log("hello");
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
        //PlayerBehavior.SetGameManager(this);
        isInvincible = false;
        uiControl.hideMenu();
        uiControl.hideLost();
        uiControl.hideResult();
        uiControl.hideUpgrade();

        if(RunStatistics.Instance.isQuick == true)
        {
            currentState = gameState.QUICK;
        }
        else
        {
            currentState = gameState.LOAD;
        }
    }

    // Update is called once per frame
    void Update()
    {
        sequenceControl();
    }

    // Method used to contain all the game's control.
    private void runButtonControl()
    {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                setPause();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                //saveGame();
            }

        if (Input.GetKeyDown(KeyCode.K))
                //playerHit();

            if (Input.GetKeyDown(KeyCode.P))
            {
                //currentState = gameState.LOSE;
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                //setCleared();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (isInvincible == false)
                {
                    //isInvincible = true;
                }
                else
                {
                    //isInvincible = false;
                }
            }
        

        if (Input.GetKeyDown(KeyCode.H))
        {
            setHelp();
        }
    }

    private void pauseButtonControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            setRun();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            uiControl.hideMenu();
            setHelp();
        }
    }

    private void helpButtonControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toHideHelp();
            setPause();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            toHideHelp();
            setRun();
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
            case gameState.QUICK:
                quickSequence();
                break;
            case gameState.RUN:               
                runSequence();
                break;
            case gameState.PAUSE:
                pauseSequence();
                break;
            case gameState.HELP:
                helpSequence();
                break;
            case gameState.LOSE:
                loseSequence();
                break;
            case gameState.CLEARED:
                clearedSequence();
                break;
            case gameState.UPGRADE:
                upgradeSequence();
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
        RunStatistics.Instance.currentLife = 3;
        RunStatistics.Instance.trapCount = 0;
        mPlayer.setDefaultState();
        //Debug.Log("Diff: " + difficulty);
        generateStage();
        originalPos = mPlayer.transform.position;
        FindObjectOfType<AudioManager>().Play("Stage_BG"); 
        uiControl.updateStage();
        if (RunStatistics.Instance.isNew == true)
        {
            setHelp();
            RunStatistics.Instance.isNew = false;
        }
        else
        {
            currentState = gameState.RUN;
            unpauseGame();
        }
        
    }

    //set
    private void generateStage()
    {
        mPlayer.setTrapCount(RunStatistics.Instance.trapCount);
        float difficulty = setStageDifficulty(RunStatistics.Instance.currentStage);      
        Debug.Log("Current difficulty: " + difficulty);
        //spawn boss bubble every 3 level for alpha playtest
        if (RunStatistics.Instance.currentStage % 3 == 0)
        {
            mapGenerator.bossGeneration(difficulty);
            mapGenerator.generateNewGrid();           
            gameSpawner.spawnBoss(difficulty);
        }
        // spawn normal bubbles
        else
        {
            mapGenerator.normalGeneration(difficulty);
            mapGenerator.generateNewGrid();           
            gameSpawner.spawnNormal(difficulty);
        }
    }

    private void quickSequence()
    {
        quickLoad();
        quickSpawnWorld();
        FindObjectOfType<AudioManager>().Play("Stage_BG");
        uiControl.updateStage();
        unpauseGame();
        setRun();
    }

    private void quickLoad()
    {
        QuickSaveData quickSave = SaveSystem.quickLoad();

        RunStatistics.Instance.playerName = quickSave.playerName;
        RunStatistics.Instance.currentLife = quickSave.currentLife;
        RunStatistics.Instance.time = quickSave.time;
        RunStatistics.Instance.stagesCleared = quickSave.stagesCleared;
        RunStatistics.Instance.totalScore = quickSave.totalScore;
        RunStatistics.Instance.bubblesCleared = quickSave.bubblesCleared;
        RunStatistics.Instance.bubblesChainCleared = quickSave.bubblesChainCleared;
        RunStatistics.Instance.bossCleared = quickSave.bossCleared;

        // Procedural Saves go here
        mPlayer = quickSave.thePlayer;
        currentBubbleUnit = quickSave.currentBubbleUnit;
        currentBubbleSpirit = quickSave.currentBubbleSpirit;
        currentBubbleProjectile = quickSave.currentBubbleProjectile;
        currentProjectile = quickSave.currentPlayerProjectile;
    }

    private void quickSpawnWorld()
    {
        // Procedural World Spawn Goes Here

        for(int i = 0; i < currentBubbleUnit.Length; i++)
        {
            GameObject e = Instantiate(currentBubbleUnit[i]);
        }

        for( int i = 0; i < currentBubbleUnit.Length; i++)
        {
            GameObject e = Instantiate(currentBubbleUnit[i]);
        }

        for(int i = 0; i < currentProjectile.Length; i++)
        {
            GameObject e = Instantiate(currentProjectile[i]);
        }
    }

    /*
     * pauseSequence method, run when ESC is pressed. All objects cannot move.
     * Used to show the main menu of the game
     * */
    private void pauseSequence()
    {
        pauseButtonControl();
    }

    // helpSequence method, run when a new game is loaded or H is pressed. All objects cannot move.
    private void helpSequence()
    {
        helpButtonControl();
    }

    /*
     * runSequence method, run when the session is play. 
     * Player, Bubble spirit and bullet can move.
     * */
    private void runSequence()
    {
        runButtonControl();
        RunStatistics.Instance.time += Time.smoothDeltaTime;
        updateChainTimer();

        if(isInvincible == true)
        {
            showRoll();
        }
        else
        {
            hideRoll();
        }

        if (bubbleCounter == 0)
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

    // method used to control the upgrade sequence gamestate
    private void upgradeSequence()
    {
        uiControl.updateUpgrade();
    }

    // nextSequence method, used to create a new stage for player
    private void nextSequence()
    {
        //Reset player, can be used to keep the trap count from the previus stage.
        RunStatistics.Instance.trapCount = 0;
        uiControl.hideResult();
        uiControl.hideUpgrade();
        clearEnemy(); // Not necessary if everything runs well.
        RunStatistics.Instance.currentStage++;
        generateStage();
        uiControl.updateStage();
        originalPos = mPlayer.transform.position;
        currentState = gameState.RUN;
        unpauseGame();
    }


    private float setStageDifficulty(int stage)
    {
        float difficulty;
        //curve is the stage scale, set to 2 for quick demo of the curve,
        //change to something like 12 for full game.
        int curve = 2;
        difficulty = 100/(1 + Mathf.Exp(-((stage/curve) - (1.7f * Mathf.Exp(1)))));
        return difficulty;
        Debug.Log("Current State difficulty: " + difficulty);
    }

    // Method used to clear all the bubblet spirits and their projectile for a new game
    private void clearEnemy()
    {
        currentBubbleSpirit = GameObject.FindGameObjectsWithTag("BubbleSpirit");
        for(int i = 0; i < currentBubbleSpirit.Length; i++)
        {
            if(currentBubbleSpirit[i] != null)
            {
                Destroy(currentBubbleSpirit[i]);
            }
        }

        currentBubbleUnit = GameObject.FindGameObjectsWithTag("BubbleUnit");
        for(int j = 0; j < currentBubbleUnit.Length; j++)
        {
            if(currentBubbleUnit[j] != null)
            {
                Destroy(currentBubbleUnit[j]);
            }
        }

        currentBubbleProjectile = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for(int i = 0; i < currentBubbleProjectile.Length; i++)
        {
            Destroy(currentBubbleProjectile[i]);
        }

        currentProjectile = GameObject.FindGameObjectsWithTag("Bullet");
        for (int i = 0; i < currentProjectile.Length; i++)
        {
            Destroy(currentProjectile[i]);
        }

        bubbleCounter = 0;
    }

    // Method used to pause the game
    private void pauseGame()
    {
        Time.timeScale = 0;
        canMove = false;
    }
    
    // Method used to unpause the game
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

    public void quickSave()
    {
        QuickSaveData quickSave = new QuickSaveData();

        quickSave.playerName = RunStatistics.Instance.playerName;
        quickSave.currentLife = RunStatistics.Instance.currentLife;
        quickSave.time = RunStatistics.Instance.time;
        quickSave.stagesCleared = (RunStatistics.Instance.stagesCleared);
        quickSave.totalScore = (RunStatistics.Instance.totalScore);
        quickSave.bubblesCleared = (RunStatistics.Instance.bubblesCleared);
        quickSave.bubblesChainCleared = (RunStatistics.Instance.bubblesChainCleared);
        quickSave.bossCleared = (RunStatistics.Instance.bossCleared);

        quickSave.thePlayer = mPlayer;
        quickSave.currentBubbleUnit = GameObject.FindGameObjectsWithTag("BubbleUnit");
        quickSave.currentBubbleSpirit = GameObject.FindGameObjectsWithTag("BubbleSpirit");
        quickSave.currentBubbleProjectile = GameObject.FindGameObjectsWithTag("EnemyBullet");
        quickSave.currentPlayerProjectile = GameObject.FindGameObjectsWithTag("Bullet");


        SaveSystem.quickSave(quickSave);
    }

    public void addBubble()
    {
        bubbleCounter++;
    }

    public void bubbleCleared()
    {
        bubbleCounter--;
    }

    public void updateChainTimer()
    {
        if(chainTime <= 0)
        {
            defaultChainTimer();
        }
        else
        {
            chainTime -= Time.deltaTime;
        }
    }

    private void defaultChainTimer()
    {
        chainBonus = false;
        chainCount = 0;
        chainTime = 0f;
    }

    public void addChain()
    {
        if(chainBonus == false)
        {
            RunStatistics.Instance.totalScore += 15;
            chainBonus = true;
            chainTime = 0.5f;
            chainCount = 0;
        }
        else
        {
            if(chainCount <= 5)
            {
                RunStatistics.Instance.totalScore += (15 + (int)(15 * 0.25));
                chainTime = 0.5f;
                chainCount++;
            }
            else
            {
                RunStatistics.Instance.totalScore += (15 + (int)(15 * 0.5));
                chainTime = 0.5f;
                chainCount++;
            }
        }
    }

    #endregion;

    // Method used to restart the run from the beginning
    public void restartLevel()
    {
        uiControl.hideLost();
        clearEnemy();
        RunStatistics.Instance.bubblesChainCleared = new int[BubbleColor.count];
        currentState = gameState.LOAD;
    }

    // Method for button to change game state from Run to Pause
    public void setPause()
    {
        uiControl.showMenu();
        currentState = gameState.PAUSE;
        pauseGame();
    }

    // Method for button to change the game state to HELP
    public void setHelp()
    {
        currentState = gameState.HELP;
        toShowHelp();
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
        clearEnemy();
        SaveSystem.deleteQuick();
        currentState = gameState.LOSE;
    }

    public void setCleared()
    {
        pauseGame();
        RunStatistics.Instance.stagesCleared++;
        SaveSystem.deleteQuick();
        currentState = gameState.CLEARED;
    }


    // Method used to set the game to upgrade sequence
    public void setUpgrade()
    {
        uiControl.hideResult();
        uiControl.showUpgrade();
        currentState = gameState.UPGRADE;
    }

    // Method for button to change game state to NEXT
    public void setNextSequence()
    {
        currentState = gameState.NEXT;
    }

    // Method for when the player is hit by an enemy bullet. Reduce 1 life and return to stage's original position
    public void playerHit()
    {
        RunStatistics.Instance.currentLife--;
        //mPlayer.transform.position = originalPos;
    }

    // Method to show the help screen
    private void toShowHelp()
    {
        isHelp = true;
        uiControl.helpController();
        pauseGame();
    }

    // Method to hide the help screen
    private void toHideHelp()
    {
        isHelp = false;
        uiControl.hideHelp();
        unpauseGame();
    }

    private void showRoll()
    {
        playerRoll.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/bubble_overlay");
    }

    private void hideRoll()
    {
        playerRoll.GetComponent<SpriteRenderer>().sprite = null;
    }

/*
    //leaderboard shit
    public void Login()
    {
        var request = new LoginWithCustomIDRequest 
        {
            CustomId = RunStatistics.Instance.playerName,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID( request, OnSuccess, OnError);
    }
    private void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create!");
    }
    private void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account");
        Debug.Log(error.GenerateErrorReport());
    }
    public void SendLeaderboard()
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "GameScore",
                    Value = RunStatistics.Instance.totalScore
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    void OnLeaderboardFixedUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful Leaderboard sent");
    }
    public void GetLeaderboard(){
        var request = new GetLeaderboardRequest{
            StatisticName = "GameScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result){
        foreach(var item in result.Leaderboard){
            playFabIDList.Add(item.PlayFabId);
            playFabScoreList.Add(item.StatValue.ToString());
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
    */
}
