//using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Setting = GraphicalSettings.Setting;

[CreateAssetMenu]
public class DynamicGroundTile : RuleTile {
    public Setting setting;
    [SerializeField] private RuleTile highGround;
    [SerializeField] private RuleTile midGround;
    [SerializeField] private RuleTile lowGround;
    private int lastSetting = -1;
    private RuleTile CurrentTile => GetCurrentTile();
    private RuleTile currentTile;
    private RuleTile GetCurrentTile()
    {
        if (lastSetting == (int)setting)
            return currentTile;

        
        if (setting == Setting.High)
            currentTile = highGround;
        else if (setting == Setting.Medium)
            currentTile = midGround;
        else
            currentTile = lowGround;

        m_TilingRules = currentTile.m_TilingRules;
        lastSetting = (int)setting;
        return currentTile;
    }


    public class Neighbor : TilingRuleOutput.Neighbor
    {
        public const int isThis = 3;
        public const int isOther = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.isThis: return tile == CurrentTile;
            case Neighbor.isOther: return tile != CurrentTile;
        }
        return CurrentTile.RuleMatch(neighbor, tile);
    }
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        CurrentTile.GetTileData(position, tilemap, ref tileData);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        GetCurrentTile();
        base.RefreshTile(position, tilemap);
    }

    //[MenuItem("Assets/Create/2D/Tiles/DynamicGroundTile")]
    //public static void CreateRoadTile()
    //{
    //    string path = EditorUtility.SaveFilePanelInProject("Save Dynamic Ground Tile", "New Dynamic Ground Tile", "Asset", "Save Dynamic Ground Tile", "Assets");
    //    if (path == "")
    //        return;
    //    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<DynamicGroundTile>(), path);
    //}
}