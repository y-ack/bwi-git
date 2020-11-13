using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
    public GameManager theManager;

    // Function used to set the game back to run
    public void resumeGame()
    {
        theManager.setRun();
    }

    // Function used to restarting the game in the current game scene
    public void gameToNewGame()
    {
        theManager.saveGame();
        theManager.restartLevel();
    }

    // Function used to continue the current game save
    public void continueGame()
    {
        theManager.saveGame();
        theManager.setNextSequence();
    }

    // Function used to move the player to the title screen from game scene
    public void gameToMenu()
    {
        //theManager.saveGame();
        //SceneManager.LoadScene("TitleScreen");
    }
}
