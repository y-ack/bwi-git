using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{

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
}
