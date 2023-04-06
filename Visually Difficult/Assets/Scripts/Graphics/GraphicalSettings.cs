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
        for (int i = 0; i < numberOfLevels; i++)
            settings.Add(Setting.Medium);

        switch (preset)
        {
            case 1:
                settings[1] = Setting.Low;
                settings[3] = Setting.High;
                break;
            case 2:
                settings[1] = Setting.High;
                settings[3] = Setting.Low;
                break;
        }
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
