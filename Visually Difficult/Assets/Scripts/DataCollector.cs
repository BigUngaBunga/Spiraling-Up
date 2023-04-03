using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class DataCollector
{
    private static int deathCount;
    private static int currentLevel;
    private static float sceneStartTime;
    private static float attemptStartTime;

    private static readonly StringBuilder dataString = new ();

    public static int DeathCount => deathCount;
    public static float AttemptTime => RoundToDecimal(Time.time - attemptStartTime, 2);
    public static void IncreasePlayerDeaths() => deathCount++;
    public static void RestartAttemptTimer() => attemptStartTime = Time.time;
    
    public static void StartLevel(int level)
    {
        if (level == currentLevel)
            return;

        currentLevel = level;
        deathCount = 0;
        attemptStartTime = sceneStartTime = Time.time;
    }


    public static void EndLevel() => dataString.Append($"{currentLevel}: Deaths={deathCount};Time={RoundToDecimal(Time.time - sceneStartTime)}{Environment.NewLine}");

    public static void SaveData()
    {
        //TODO: Change so as to send information instead of saving to file
        File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{Path.DirectorySeparatorChar}VD.data", dataString.ToString());
    }

    public static void Clear()
    {
        dataString.Clear();
        deathCount= 0;
        attemptStartTime = sceneStartTime = Time.time;
    }

    private static float RoundToDecimal(float time, int decimals = 1)
    {
        return Mathf.Round(time * Mathf.Pow(10, decimals)) * Mathf.Pow(10, -decimals);
    }
}
