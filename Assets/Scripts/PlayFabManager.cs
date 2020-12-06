using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager thePlayFabManager = null;

    private string playerLeaderboardName;
    string myGUID = System.Guid.NewGuid().ToString();
    public List<string> playFabIDList = new List<string>();
    public List<string> playFabScoreList = new List<string>();

    public GetLeaderboardResult publicLeaderboard;

    void Awake () 
    {
        DontDestroyOnLoad (transform.gameObject);
    }
    void Start()
    {
        if (!thePlayFabManager)
        {
            thePlayFabManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //Login();
    }
    public void Login(string loginName)
    {
        playerLeaderboardName = loginName;
        var request = new LoginWithCustomIDRequest 
        {
            CustomId = myGUID,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID( request, OnSuccess, OnError);
    }
    
    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create!");
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest {DisplayName = playerLeaderboardName}, OnDisplayName, OnSendError);
    }

    void OnDisplayName(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log(result.DisplayName + "This is your new display name");
    }

    void OnSendError(PlayFabError error)
    {
        Debug.Log("Error Sending Data");
        Debug.Log(error.GenerateErrorReport());
    }
    
    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account");
        Debug.Log(error.GenerateErrorReport());
    }
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

        publicLeaderboard = result;
        /*
        foreach(var item in result.Leaderboard){
            playFabIDList.Add(item.PlayFabId);
            playFabScoreList.Add(item.StatValue.ToString());
            Debug.Log(item.DisplayName + " " + item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
        */

    }
    public GetLeaderboardResult returnLeaderboard()
    {
        return publicLeaderboard;
    }
}
