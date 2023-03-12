using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class DataCollector
{
    static int deathCount;
    static int currentLevel;

    static float sceneStartTime;

    static readonly StringBuilder dataString = new ();

    public static void IncreasePlayerDeaths() => deathCount++;
    
    public static void StartLevel(int level)
    {
        if (level == currentLevel)
            return;

        currentLevel = level;
        deathCount = 0;
        sceneStartTime = Time.time;
    }

    public static void EndLevel() => dataString.Append($"{currentLevel}: Deaths={deathCount};Time={Time.time - sceneStartTime}{Environment.NewLine}");

    public static void SaveData()
    {
        //TODO: Change so as to send information instead of saving to file
        File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}{Path.DirectorySeparatorChar}VD.data", dataString.ToString());
    }
}
