using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class StatisticUI : MonoBehaviour
{
    public Image localStatistic;
    public Image globalStatistic;

    private CanvasGroup localCanvas;
    private CanvasGroup globalCanvas;
    private GetLeaderboardResult board;

    void Start()
    {
        FindObjectOfType<AudioManager>().Stop("Title_Theme"); 
        localCanvas = localStatistic.GetComponent<CanvasGroup>();
        globalCanvas = globalStatistic.GetComponent<CanvasGroup>();
        updateLocal();

        StartCoroutine(updateGlobalWithSleeps());
    }

    IEnumerator updateGlobalWithSleeps()
    {
        PlayFabManager.thePlayFabManager.Login("ccb");
        yield return new WaitForSecondsRealtime(3);
        GameObject statisticBG = globalStatistic.transform.Find("Global Statistic View").gameObject;
        GameObject playerBG = statisticBG.transform.Find("PlayerRanking").gameObject;
        GameObject pPlayerPositionText = playerBG.transform.Find("PlayerPosition").gameObject;
        GameObject pPlayerNameText = playerBG.transform.Find("PlayerName").gameObject;
        GameObject pPlayerScoreText = playerBG.transform.Find("PlayerScore").gameObject;
        PlayFabManager.thePlayFabManager.GetLeaderboard();
        yield return new WaitForSecondsRealtime(3);
        board = PlayFabManager.thePlayFabManager.returnLeaderboard();
        Debug.Log("Makes it this far");
        foreach(var item in board.Leaderboard){
            Debug.Log("Makes it into here");
            pPlayerNameText.GetComponent<Text>().text = item.DisplayName;
            pPlayerScoreText.GetComponent<Text>().text =  item.StatValue.ToString();
        }
    }


    /*
     * Method used to update the local statistic Screen
     * I'm not going to lie, I hate the fact that I wrote this.
     * */
    private void updateLocal()
    {
        
        if(SaveSystem.loadPlayer() != null)
        {
            GameObject LocalEmptyUI = localStatistic.transform.Find("Local Empty View").gameObject;
            CanvasGroup emptyView = LocalEmptyUI.GetComponent<CanvasGroup>();
            emptyView.alpha = 0;
            emptyView.interactable = false;
            emptyView.blocksRaycasts = false;

            GameObject statisticBG = localStatistic.transform.Find("Local Statistic View").gameObject;
            GameObject playerText = statisticBG.transform.Find("Player Name").gameObject;
            GameObject totalPlayText = statisticBG.transform.Find("Total Play Time").gameObject;
            GameObject averagePlayText = statisticBG.transform.Find("Average Play Time").gameObject;
            GameObject stageClearText = statisticBG.transform.Find("Stage Cleared").gameObject;
            GameObject maxScoreText = statisticBG.transform.Find("Max Score").gameObject;
            GameObject maxBubbleText = statisticBG.transform.Find("Max Bubble Cleared").gameObject;
            GameObject maxMatchText = statisticBG.transform.Find("Max Bubble Matched").gameObject;
            GameObject maxBossText = statisticBG.transform.Find("Max Boss Cleared").gameObject;
            GameObject lastTimeText = statisticBG.transform.Find("Last Play Time").gameObject;
            GameObject pStageText = statisticBG.transform.Find("Previous Stage Cleared").gameObject;
            GameObject pScoreText = statisticBG.transform.Find("Previous Score").gameObject;
            GameObject pBubbleText = statisticBG.transform.Find("Previous Bubble Cleared").gameObject;
            GameObject pMatchText = statisticBG.transform.Find("Previous Bubble Matched").gameObject;
            GameObject pBossText = statisticBG.transform.Find("Previous Boss Cleared").gameObject;

            PlayerData saveData = SaveSystem.loadPlayer();

            playerText.GetComponent<Text>().text = "Player Name: " + saveData.playerName;
            totalPlayText.GetComponent<Text>().text = "Total Play Time: " + System.Math.Round(saveData.totalPlayTime,2);
            averagePlayText.GetComponent<Text>().text = "Average Stage Time: " + System.Math.Round(saveData.averageStageTime, 2);
            stageClearText.GetComponent<Text>().text = "Stage Cleared: " + saveData.stageCleared;
            maxScoreText.GetComponent<Text>().text = "Max Score: " + saveData.maxScore;
            maxBubbleText.GetComponent<Text>().text = "Max Bubble Cleared: " + saveData.maxBubbleCleared;
            maxMatchText.GetComponent<Text>().text = "Max Bubble Matched: " + saveData.maxBubbleMatched;
            maxBossText.GetComponent<Text>().text = "Max Boss Cleared: " + saveData.maxBossCleared;
            lastTimeText.GetComponent<Text>().text = "Last Session Time: " + System.Math.Round(saveData.lastSessionTime, 2);
            pStageText.GetComponent<Text>().text = "Previous Stage Cleared: " + saveData.lastStageCleared;
            pScoreText.GetComponent<Text>().text = "Previous Score: " + saveData.lastScore;
            pBubbleText.GetComponent<Text>().text = "Previous Bubble Cleared: " + saveData.lastBubbleCleared;
            pMatchText.GetComponent<Text>().text = "Previous Bubble Matched: " + saveData.lastBubbleMatched;
            pBossText.GetComponent<Text>().text = "Previous Boss Cleared: " + saveData.lastBossCleared;
        }
    }

    /*
    public GetLeaderboardResult getBoard()
    {
        PlayFabManager.thePlayFabManager.getLeaderBoard();
    }
    */

    /*
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "GameScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful Leaderboard sent");
    }
    public void GetLeaderboard(){
        var request = new GetLeaderboardRequest{
            StatisticName = "GameScore",
            StartPosition = 0,
            MaxResultsCount = 1
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result){

        foreach(var item in result.Leaderboard){
            playFabIDList.Add(item.PlayFabId);
            playFabScoreList.Add(item.StatValue.ToString());
            Debug.Log(item.DisplayName + " " + item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }

    }

    void OnGetError(PlayFabError error)
    {
        Debug.Log("Error Getting Data");
        Debug.Log(error.GenerateErrorReport());
    }
    */
}
