using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{

    public PlayerBehavior thePlayer;
    public GameUIControl theUI;
    public StatisticUI theStatistic;

    // Function used to set the game back to run
    public void resumeGame()
    {
        GameManager.theManager.setRun();
    }

    // Function used to restarting the game in the current game scene
    public void gameToNewGame()
    {
        GameManager.theManager.saveGame();
        GameManager.theManager.restartLevel();
    }

    // Function used to continue the current game save
    public void continueGame()
    {
        GameManager.theManager.setUpgrade();
    }

    // Function used to move from upgrade sequence to next sequence
    public void continueFromUpgrade()
    {
        GameManager.theManager.saveGame();
        GameManager.theManager.setNextSequence();
    }

    // Function used to move the player to the title screen from game scene
    public void gameToMenu()
    {
        GameManager.theManager.saveGame();
        FindObjectOfType<AudioManager>().Stop("Stage_BG"); 
        FindObjectOfType<AudioManager>().Play("Title_Theme");
        RunStatistics.Instance.bubblesChainCleared = new int[BubbleColor.count];
        SceneManager.LoadScene("TitleScreen");
    }

    public void menuToTitle()
    {      
        FindObjectOfType<AudioManager>().Stop("Stage_BG"); 
        FindObjectOfType<AudioManager>().Play("Title_Theme");
        SaveSystem.deleteQuick();
        RunStatistics.Instance.bubblesChainCleared = new int[BubbleColor.count];
        SceneManager.LoadScene("TitleScreen");
    }

    // Function used to move the player to the title screen from statistic scene
    public void statisticToMenu()
    {
        FindObjectOfType<AudioManager>().Play("Title_Theme"); 
        SceneManager.LoadScene("TitleScreen");
    }

    public void quickSaveAndQuit()
    {
        GameManager.theManager.quickSave();
        SceneManager.LoadScene("TitleScreen");
        Debug.Log(SaveSystem.quickLoad().playerCurrentPos.x);
    }

    public void upgradeTrap()
    {
        if (RunStatistics.Instance.totalScore >= 1000)
        {
            if (thePlayer.trapCountCap < 10f)
            {
                if ((thePlayer.trapCountCap += 3) >= 10f)
                {
                    thePlayer.shootCoolDown = 10f;
                    RunStatistics.Instance.totalScore -= 1000;
                    theUI.trapMax = true;
                }
                else
                {
                    RunStatistics.Instance.totalScore -= 1000;
                }
            }
            else
            {

            }
        }
    }

    public void upgradeCapture()
    {
        if (RunStatistics.Instance.totalScore >= 1000)
        {
            if (thePlayer.captureCoolDown > 1f)
            {
                if((thePlayer.captureCoolDown -= 0.2f) <= 1f)
                {
                    thePlayer.captureCoolDown = 1f;
                    RunStatistics.Instance.totalScore -= 1000;
                    theUI.captureMax = true;
                }
                else
                {
                    RunStatistics.Instance.totalScore -= 1000;
                }
            }
            else
            {

            }
        }
    }

    public void upgradeRoll()
    {
        if (RunStatistics.Instance.totalScore >= 500)
        {
            if (thePlayer.dashCoolDown > 4f)
            {
                if((thePlayer.dashCoolDown -= 0.5f) <= 4f)
                {
                    thePlayer.dashCoolDown = 4f;
                    RunStatistics.Instance.totalScore -= 500;
                    theUI.rollMax = true;
                }
                else
                {
                    RunStatistics.Instance.totalScore -= 500;
                }
            }
            else
            {
                // hide button
            }
        }
    }

    public void upgradeLife()
    {
        if (RunStatistics.Instance.totalScore >= 1000)
        {
            if (RunStatistics.Instance.currentLife < 3f)
            {
                RunStatistics.Instance.currentLife += 1;
                RunStatistics.Instance.totalScore -= 1000;
            }
            else
            {

            }
        }
    }

    public void toShowlocalStatistic()
    {
        theStatistic.showLocal();
        theStatistic.hideGlobal();
    }

    public void toShowGlobalStatistic()
    {
        theStatistic.showGlobal();
        theStatistic.hideLocal();
    }

    public void hoverOne()
    {
        theUI.setCost(500);
    }

    public void hoverTwo()
    {
        theUI.setCost(1000);
    }

    public void nextHelp()
    {
        theUI.helpPageTwo();
    }

    public void previousHelp()
    {
        theUI.helpPageOne();
    }
}
