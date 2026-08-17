using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileSiblingRule : RuleTile
{
	public enum SibingGroup
	{
		Ice = 0
	}

	public SibingGroup sibingGroup;

	public override bool RuleMatch(int neighbor, TileBase other)
	{
		if (other is RuleOverrideTile)
		{
			other = (other as RuleOverrideTile).m_InstanceTile;
		}
		switch (neighbor)
		{
		case 1:
			if (other is TileSiblingRule)
			{
				return (other as TileSiblingRule).sibingGroup == sibingGroup;
			}
			return false;
		case 2:
			if (other is TileSiblingRule)
			{
				return (other as TileSiblingRule).sibingGroup != sibingGroup;
			}
			return true;
		default:
			return base.RuleMatch(neighbor, other);
		}
	}
}
