using System;
using Rewired;
using RewiredEnums;

[Serializable]
public struct IconChangerInfo
{
	public PenguinRewiredActions ActionID;

	public AxisRange AxisRange;

	public int PlayerID;
}
