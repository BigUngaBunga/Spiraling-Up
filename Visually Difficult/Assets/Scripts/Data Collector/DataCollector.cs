using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class DataCollector
{
    static string SavePath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "VD.data"); }}

    static int deathCount;
    static int currentLevel;
    static float sceneStartTime;
    static float attemptStartTime;
    static bool finishedGame;

    static float attemptFinishTime;

    static readonly StringBuilder dataString = new ();

    public static bool FinishedGame => finishedGame;

    public static int DeathCount => deathCount;
    public static float AttemptTime => RoundToDecimal((attemptFinishTime != 0 ? attemptFinishTime : Time.time) - attemptStartTime, 2);
    public static void IncreasePlayerDeaths() => deathCount++;
    public static void RestartAttemptTimer()
    {
        attemptStartTime = Time.time;
        attemptFinishTime = 0;
    }

    public static void StartLevel(int level)
    {
        if (level == currentLevel)
            return;

        currentLevel = level;
        deathCount = 0;
        attemptFinishTime = 0;
        attemptStartTime = sceneStartTime = Time.time;
    }

    public static void StartSession(string presetInformation) => dataString.AppendLine(presetInformation);

    public static void EndLevel()
    {
        dataString.Append($" {currentLevel}: Deaths={deathCount};Time={RoundToDecimal(Time.time - sceneStartTime)}");
        attemptFinishTime = Time.time;
    }

    public static void SaveData()
    {
        //EmailHandler.Send(dataString.ToString(), "Data collection");

        finishedGame = true;
    }

    public static string GetData() => dataString.ToString();

    public static void DeleteSaveFile()
    {
        if (Directory.Exists(SavePath))
            Directory.Delete(SavePath);
    }

    public static void Clear()
    {
        finishedGame = false;
        dataString.Clear();
        deathCount= 0;
        attemptFinishTime = 0;
        attemptStartTime = sceneStartTime = Time.time;
    }

    private static float RoundToDecimal(float time, int decimals = 1)
    {
        return Mathf.Round(time * Mathf.Pow(10, decimals)) / Mathf.Pow(10, decimals);
    }
}
