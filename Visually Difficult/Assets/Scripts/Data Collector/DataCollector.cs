using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class DataCollector
{
    static int deathCount;
    static float attemptStartTime;
    static float attemptFinishTime;


    public static int DeathCount => deathCount;
    public static float AttemptTime => RoundToDecimal((attemptFinishTime != 0 ? attemptFinishTime : Time.time) - attemptStartTime, 2);
    public static void IncreasePlayerDeaths() => deathCount++;
    public static void RestartAttemptTimer()
    {
        attemptStartTime = Time.time;
        attemptFinishTime = 0;
    }

    public static void EndLevel()
    {
        attemptFinishTime = Time.time;
    }

    public static void Clear()
    {
        deathCount= 0;
        RestartTime();
    }

    public static void RestartTime()
    {
        attemptFinishTime = 0;
        attemptStartTime = Time.time;
    }

    private static float RoundToDecimal(float time, int decimals = 1)
    {
        return Mathf.Round(time * Mathf.Pow(10, decimals)) / Mathf.Pow(10, decimals);
    }
}
