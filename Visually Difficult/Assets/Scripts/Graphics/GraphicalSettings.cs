using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GraphicalSettings : MonoBehaviour
{
    public enum Setting { High, Medium , Low }
    [SerializeField] private List<Setting> settings = new();
    [SerializeField] private int numberOfLevels = 4;
    public int CurrentPreset { get; private set; } = -1;

    public void SetGraphicsPreset(int preset)
    {
        if (preset == CurrentPreset)
            return;

        CurrentPreset = preset;
        settings.Clear();

        switch (preset)
        {
            case 1:
                settings.Add(Setting.Low);
                settings.Add(Setting.Low);
                settings.Add(Setting.Medium);
                settings.Add(Setting.High);
                break;
            case 2:
                settings.Add(Setting.High);
                settings.Add(Setting.High);
                settings.Add(Setting.Medium);
                settings.Add(Setting.Low);
                break;
        }

        for (int i = settings.Count; i < numberOfLevels; i++)
        {
            settings.Add(Setting.Medium);
        }

        Debug.Log(ToString());
    }

    public Setting GetLevelSetting(int level) => settings[level];

    public override string ToString()
    {
        StringBuilder stringBuilder= new StringBuilder();
        stringBuilder.Append($"Current preset: {CurrentPreset}. Settings: ");
        foreach (var setting in settings)
        {
            stringBuilder.Append(setting.ToString());
            stringBuilder.Append(", ");
        }
        return stringBuilder.ToString();
    }
}
