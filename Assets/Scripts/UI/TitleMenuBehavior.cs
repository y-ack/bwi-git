using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuBehavior : MonoBehaviour
{
    public TitleBehavior titleController;

    public void newButton()
    {
        titleController.newClicked();
    }

    public void loadButton()
    {
        titleController.loadClicked();
    }

    public void creditButton()
    {

    }

    public void statisticButton()
    {
        //SceneManager.LoadScene("Statistic");
    }

    public void optionButton()
    {
        
    }

    public void exitButton()
    {
        Application.Quit();
    }

    public void backButton()
    {
        titleController.menuClicked();
    }
}
