using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{

    public PlayerBehavior thePlayer;

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
        SceneManager.LoadScene("TitleScreen");
    }

    // Function used to move the player to the title screen from statistic scene
    public void statisticToMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void upgradeSpeed()
    {
        if(RunStatistics.Instance.totalScore >= 1000)
        {
            if(thePlayer.normalSpeed < 18f)
            {
                if((thePlayer.normalSpeed += 0.2f) == 18f)
                {
                    thePlayer.normalSpeed = 18f;
                    RunStatistics.Instance.totalScore -= 1000;
                }
                else
                {
                    thePlayer.normalSpeed += 0.2f;
                    RunStatistics.Instance.totalScore -= 1000;
                }
            }
            else
            {

            }
        }
        else
        {

        }
    }

    public void upgradeTrap()
    {
        if (RunStatistics.Instance.totalScore >= 1000)
        {
            if (thePlayer.shootCoolDown > 0.3f)
            {
                if ((thePlayer.shootCoolDown -= 0.01f) <= 0.3f)
                {
                    thePlayer.shootCoolDown = 0.3f;
                    RunStatistics.Instance.totalScore -= 1000;
                }
                else
                {
                    thePlayer.shootCoolDown -= 0.01f;
                    RunStatistics.Instance.totalScore -= 1000;
                }
            }
            else
            {

            }
        }
        else
        {

        }
    }

    public void upgradeCapture()
    {
        if (RunStatistics.Instance.totalScore >= 1000)
        {
            if (thePlayer.captureCoolDown > 2f)
            {
                if((thePlayer.captureCoolDown -= 0.1f) <= 2f)
                {
                    thePlayer.captureCoolDown = 2f;
                    RunStatistics.Instance.totalScore -= 1000;
                }
                else
                {
                    thePlayer.captureCoolDown -= 0.1f;
                    RunStatistics.Instance.totalScore -= 1000;
                }
            }
            else
            {

            }
        }
        else
        {

        }
    }

    public void upgradeRoll()
    {
        if (RunStatistics.Instance.totalScore >= 1000)
        {
            if (thePlayer.dashCoolDown > 4f)
            {
                if((thePlayer.dashCoolDown -= 0.1f) <= 4f)
                {
                    thePlayer.dashCoolDown = 4f;
                    RunStatistics.Instance.totalScore -= 1000;
                }
                else
                {
                    thePlayer.dashCoolDown -= 0.1f;
                    RunStatistics.Instance.totalScore -= 1000;
                }
            }
            else
            {

            }
        }
        else
        {

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
        else
        {

        }
    }
}
