using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public GameManager theManager;

    public void resumeGame()
    {
        theManager.setRun();
    }
}
