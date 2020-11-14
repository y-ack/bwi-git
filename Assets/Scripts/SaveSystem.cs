using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    

    public static void savePlayer()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string playerName = RunStatistics.Instance.playerName;
        int saveNum;

        if(RunStatistics.Instance.saveNum < 1) // does not exist
        {
            RunStatistics.Instance.totalSaveNum++;
            saveNum = RunStatistics.Instance.totalSaveNum;
        }
        else // does exist
        {
            saveNum = RunStatistics.Instance.saveNum;
        }

        string savePath = Application.persistentDataPath + "/" + playerName + saveNum + ".score";
        FileStream stream = new FileStream(savePath, FileMode.Create);

        PlayerData data = new PlayerData();

        data.playerName = playerName;
        data.setSessionTime(RunStatistics.Instance.time);
        data.setStageCleared(RunStatistics.Instance.stagesCleared);
        data.setBubbleCleared(RunStatistics.Instance.bubblesCleared);
        data.setbubbleMatched(RunStatistics.Instance.bubblesChainCleared.Length);
        data.setBossCleared(RunStatistics.Instance.bossCleared);
        data.calculateStageAverage();
        data.saveNum = saveNum;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData loadPlayer(string savePath)
    {
        string path = savePath;

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
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}
