using System;
using UnityEngine;

[Serializable]
public class SerializedPickableNetworkDeal
{
	public DIFFICULTY difficulty;

	public string rewardType;

	public string dealType;

	public SerializedPickableNetworkDeal(NetworkDealBase deal)
	{
		difficulty = deal.difficulty;
		rewardType = deal.reward.GetType().AssemblyQualifiedName;
		dealType = deal.GetType().AssemblyQualifiedName;
	}

	public NetworkDealBase Deserialize()
	{
		Type type = Type.GetType(dealType);
		Type type2 = Type.GetType(rewardType);
		if (type == null || type2 == null)
		{
			Debug.LogError("Failed to deserialize deal or reward, falling back to default deal");
			DIFFICULTY dif = DIFFICULTY.medium;
			NetworkBingoBongo_0 networkBingoBongo_ = new NetworkBingoBongo_0();
			DealRewardMeta_2 dealRewardMeta_ = new DealRewardMeta_2();
			dealRewardMeta_.Init(dif);
			networkBingoBongo_.Init(dealRewardMeta_, dif);
			return networkBingoBongo_;
		}
		NetworkDealBase obj = (NetworkDealBase)Activator.CreateInstance(type);
		DealRewardBase dealRewardBase = (DealRewardBase)Activator.CreateInstance(type2);
		dealRewardBase.Init(difficulty);
		obj.Init(dealRewardBase, difficulty);
		return obj;
	}
}
