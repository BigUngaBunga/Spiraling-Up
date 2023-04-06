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

    static readonly StringBuilder dataString = new ();

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

    public static void StartSession(string presetInformation) => dataString.AppendLine(presetInformation);

    public static void EndLevel() => dataString.Append($"{currentLevel}: Deaths={deathCount};Time={RoundToDecimal(Time.time - sceneStartTime)}{Environment.NewLine}");

    public static void SaveData()
    {
        File.WriteAllText(SavePath, dataString.ToString());
        EmailHandler.SendEmail(SavePath, "Data collection");
    }

    public static void DeleteSaveFile()
    {
        if (Directory.Exists(SavePath))
            Directory.Delete(SavePath);
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
