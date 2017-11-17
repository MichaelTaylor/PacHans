using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static void SaveData(HighScoreManager _highscoreManager)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/HighScoreData.sav", FileMode.Create);

        HighScoreData data = new HighScoreData(_highscoreManager);

        bf.Serialize(stream, data);
        stream.Close();

    }

    public static float[] LoadScores()
    {
        if (!File.Exists(Application.persistentDataPath + "/HighScoreData.sav")) return null;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/HighScoreData.sav", FileMode.Open);

        HighScoreData data = bf.Deserialize(stream) as HighScoreData;

        stream.Close();
        return data.Scores;
    }

    public static string[] LoadNames()
    {
        if (!File.Exists(Application.persistentDataPath + "/HighScoreData.sav")) return null;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/HighScoreData.sav", FileMode.Open);

        HighScoreData data = bf.Deserialize(stream) as HighScoreData;

        stream.Close();
        return data.Names;
    }
}

[Serializable]
public class HighScoreData
{
    public float[] Scores;
    public string[] Names;

    public HighScoreData(HighScoreManager highScoreManager)
    {
        Scores = new float[5];
        Scores[0] = highScoreManager.TopScore1;
        Scores[1] = highScoreManager.TopScore2;
        Scores[2] = highScoreManager.TopScore3;
        Scores[3] = highScoreManager.TopScore4;
        Scores[4] = highScoreManager.TopScore5;

        Names = new string[5];
        Names[0] = highScoreManager.TopScoreName1;
        Names[1] = highScoreManager.TopScoreName2;
        Names[2] = highScoreManager.TopScoreName3;
        Names[3] = highScoreManager.TopScoreName4;
        Names[4] = highScoreManager.TopScoreName5;
        //Debug.Log(Names[4]);
    }
}
