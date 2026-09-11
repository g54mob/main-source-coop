using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zorro.Core;

[CreateAssetMenu(fileName = "DealRewardDatabase", menuName = "DealRewardDatabase")]
public class DealRewardDatabase : SingletonAsset<DealRewardDatabase>
{
	public Dictionary<int, Type> rewards;

	public DealRewardBase GetRewardFromIndex(int index)
	{
		if (rewards.TryGetValue(index, out var value))
		{
			return (DealRewardBase)Activator.CreateInstance(value);
		}
		Debug.LogError("coulndt find reward with index " + index);
		return null;
	}

	public DealRewardBase GetRandom()
	{
		return (DealRewardBase)Activator.CreateInstance(rewards.Values.ToList().GetRandom());
	}
}
