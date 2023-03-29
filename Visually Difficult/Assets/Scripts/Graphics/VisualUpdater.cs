using UnityEngine;
using UnityEngine.Tilemaps;
using Setting = GraphicalSettings.Setting;

public class VisualUpdater : MonoBehaviour
{
    [SerializeField] private int level = 0;
    [SerializeField] private DynamicGroundTile tile;
    [SerializeField] private Setting setting;
    public Setting Settings { get; private set; }
    private void Awake()
    {
        try
        {
            var graphicalSettings = GameObject.Find("Settings").GetComponent<GraphicalSettings>();
            Settings = graphicalSettings.GetLevelSetting(level);
            setting = Settings;
        }
        catch (System.Exception)
        {
            Debug.LogWarning("No \"Settings\" object found");
            Settings = Setting.Medium;
        }
        tile.setting = Settings;
        GameObject.Find("Tilemap").GetComponent<Tilemap>().RefreshAllTiles();
    }
}
