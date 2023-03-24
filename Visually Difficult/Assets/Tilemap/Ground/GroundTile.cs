using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class GroundTile : RuleTile
{
    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is RuleOverrideTile)
            other = (other as RuleOverrideTile).m_InstanceTile;

        switch (neighbor)
        {
            case TilingRule.Neighbor.This:
                {
                    return IsCompatibleType(other);
                }
            case TilingRule.Neighbor.NotThis:
                {
                    return !IsCompatibleType(other);
                }
        }

        return base.RuleMatch(neighbor, other);
    }

    private bool IsCompatibleType(TileBase other) => other is DynamicGroundTile || other is GroundTile;
}