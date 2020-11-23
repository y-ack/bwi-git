using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    
    /*
     * savePlayer Method
     * */
    public static void savePlayer()
    {
        

        if(loadPlayer() != null)
        {
            PlayerData playerData = loadPlayer();
            toSave(playerData);
        }
        else
        {
            PlayerData playerData = new PlayerData();
            toSave(playerData);
        }

        
    }

    private static void toSave(PlayerData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string playerName = RunStatistics.Instance.playerName;

        string savePath = Application.persistentDataPath + "/SaveData.score";
        FileStream stream = new FileStream(savePath, FileMode.Create);

        data.playerName = playerName;
        data.setSessionTime(RunStatistics.Instance.time);
        data.setStageCleared(RunStatistics.Instance.stagesCleared);
        data.setScore(RunStatistics.Instance.totalScore);
        data.setBubbleCleared(RunStatistics.Instance.bubblesCleared);
        data.setbubbleMatched(RunStatistics.Instance.bubblesChainCleared.Length);
        data.setBossCleared(RunStatistics.Instance.bossCleared);
        data.calculateStageAverage();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData loadPlayer()
    {
        string path = Application.persistentDataPath + "/SaveData.score";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }

}
