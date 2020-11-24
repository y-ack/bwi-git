using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TitleBehavior : MonoBehaviour
{
    public Image titleScreenUI;
    public Image savedGamesUI;
    public Image mainMenuUI;
    public Image savedListUI;
    public Image newGameUI;
    public Image optionUI;
    public Image creditUI;
    public Text userInput;
    public Text exampleUI;

    private TitleState currentState;
    private CanvasGroup savedGamesCanvas;
    private CanvasGroup mainMenuCanvas;
    private CanvasGroup emptySavedCanvas;
    private CanvasGroup newGameCanvas;
    private CanvasGroup optionCanvas;
    private CanvasGroup creditCanvas;
    private CanvasGroup exampleCanvas;

    private string[] saveFile;

    public enum TitleState
    {
        INTRO,
        MAIN,
        NEW,
        LOAD,
        CREDITS
    };

    private void Awake() {
        GameObject audioManager = Instantiate(Resources.Load("Prefabs/AudioManager") as GameObject);
    }
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Title_Theme");
        currentState = TitleState.INTRO;
        savedGamesCanvas = savedGamesUI.GetComponent<CanvasGroup>();
        mainMenuCanvas = mainMenuUI.GetComponent<CanvasGroup>();
        newGameCanvas = newGameUI.GetComponent<CanvasGroup>();
        exampleCanvas = exampleUI.GetComponent<CanvasGroup>();
        hideNew();
        hideSaves();
        findSaves();
        createSaves();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case TitleState.INTRO:
                introSequence();
                break;
            case TitleState.MAIN:
                mainSequence();
                break;
            case TitleState.NEW:
                newSequence();
                break;
            case TitleState.LOAD:
                loadSequence();
                break;
            case TitleState.CREDITS:
                creditSequence();
                break;
            default:
                break;
        }
    }

    public void introSequence()
    {

    }

    public void mainSequence()
    {
        hideNew();
        hideSaves();
        showMenu();
    }

    public void newSequence()
    {
        hideSaves();
        hideMenu();
        showNew();

        if(userInput.text.Trim() == "")
        {
            showExample();
        }
        else
        {
            hideExample();
        }
    }

    public void loadSequence()
    {
        hideMenu();
        hideNew();
        showSaves();
    }

    public void creditSequence()
    {

    }

    public void newClicked()
    {
        currentState = TitleState.NEW;
    }

    public void loadClicked()
    {
        currentState = TitleState.LOAD;
    }

    public void menuClicked()
    {
        currentState = TitleState.MAIN;
    }

    private void findSaves()
    {
        try
        {
            saveFile = Directory.GetFiles(@Application.persistentDataPath, "*.score*");
            //RunStatistics.Instance.totalSaveNum = saveFile.Length; keeping the multiple save code incase we switch back for some reason
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError("No Save Files Found");
            throw e;  
        }
    }

    public void createSaves()
    {
        if (saveFile.Length > 0)
        {
            // Get the data for Empty Save Data game object
            GameObject emptySaveData = savedListUI.transform.Find("Empty Save Data").gameObject;
            emptySavedCanvas = emptySaveData.GetComponent<CanvasGroup>(); // Set the empty canvas group
            hideEmpty(); // Now we can hide the empty canvas group since there are save data

            // Get the data for the SaveList
            GameObject saveData = savedListUI.transform.Find("saveDataContent").gameObject;

            // Loop used to go through the saveFile array
            for (int i = 0; i < saveFile.Length; i++)
            {
                // Create a PlayerData based on the path in safeFile array at i
                PlayerData sData = SaveSystem.loadPlayer();

                // instantiate new SaveData object
                GameObject e = Instantiate(Resources.Load("Prefabs/dataButton") as
                                   GameObject);
                TitleButton eControl = e.GetComponent<TitleButton>(); // create a loadDataControl Behavior
                eControl.setPath(saveFile[i].ToString());// set save path string
                GameObject saveText = e.transform.Find("Text").gameObject; // Create a Text UI Game Object
                Text userScore = saveText.GetComponent<Text>(); // Get the new Text UI text component
                userScore.text = sData.playerName; // Change the save file button's text
                e.transform.SetParent(saveData.transform); // Set the Saved List Data As New Parent
            }
        }
    }

    private void showMenu()
    {
        mainMenuCanvas.alpha = 1f;
        mainMenuCanvas.interactable = true;
        mainMenuCanvas.blocksRaycasts = true;
    }

    private void hideMenu()
    {
        mainMenuCanvas.alpha = 0f;
        mainMenuCanvas.interactable = false;
        mainMenuCanvas.blocksRaycasts = false;
    }

    private void showNew()
    {
        newGameCanvas.alpha = 1f;
        newGameCanvas.interactable = true;
        newGameCanvas.blocksRaycasts = true;
    }

    private void hideNew()
    {
        newGameCanvas.alpha = 0f;
        newGameCanvas.interactable = false;
        newGameCanvas.blocksRaycasts = false;
    }

    private void showSaves()
    {
        savedGamesCanvas.alpha = 1f;
        savedGamesCanvas.interactable = true;
        savedGamesCanvas.blocksRaycasts = true;
    }

    private void hideSaves()
    {
        savedGamesCanvas.alpha = 0f;
        savedGamesCanvas.interactable = false;
        savedGamesCanvas.blocksRaycasts = false;
    }

    private void hideEmpty()
    {
        emptySavedCanvas.alpha = 0f;
        emptySavedCanvas.interactable = false;
        emptySavedCanvas.blocksRaycasts = false;
    }

    private void showExample()
    {
        exampleCanvas.alpha = 1f;
        exampleCanvas.interactable = true;
        exampleCanvas.blocksRaycasts = true;
    }

    private void hideExample()
    {
        exampleCanvas.alpha = 0f;
        exampleCanvas.interactable = false;
        exampleCanvas.blocksRaycasts = false;
    }
}
