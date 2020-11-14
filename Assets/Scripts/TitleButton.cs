using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    string savePath;
    public Text userInput;
    private bool isShake;
    private float shakeTime;
    private Vector3 originalPos;
    private float shakeMagnitude;


    void Start()
    {
        shakeTime = 0f;
        originalPos = transform.localPosition;
    }

    void Update()
    {
        checkTime();
        if (isShake == true)
            shake();
    }


    public void newGame()
    {
        string pName = userInput.text.Trim();

        if (pName != "" && pName.Length >= 3 && pName.Length <= 6)
        {
         RunStatistics.Instance.playerName = pName;
         SceneManager.LoadScene("Main");
        }
        else
        {
            isShake = true;
            shakeTime = Time.timeSinceLevelLoad + 1f;
            shakeMagnitude = 1f;
        }
    }

    // Method used to load the player's progress
    public void loadGame()
    {
        PlayerData playerData = SaveSystem.loadPlayer(savePath);
        RunStatistics.Instance.playerName = playerData.playerName;
        RunStatistics.Instance.saveNum = playerData.saveNum;
        RunStatistics.Instance.savePath = savePath;
        SceneManager.LoadScene("Main");
    }

    public void setPath(string iPath)
    {
        savePath = iPath;
    }

    private void checkTime()
    {
        if (Time.timeSinceLevelLoad >= shakeTime)
        {
            isShake = false;
        }
    }

    private void shake()
    {
        Vector3 Pos = originalPos;
        Pos.x += shakeMagnitude * Mathf.Sin(Time.time * 62.832f); // Sin for more even shake
        Pos.y += shakeMagnitude * Mathf.Sin(Time.time * 62.832f); // 62.832f ensure 10 frequency per sec
        transform.localPosition = Pos;
        shakeMagnitude -= shakeMagnitude * Time.smoothDeltaTime;
    }


}
