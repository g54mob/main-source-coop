using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.CLI;

public class NetworkDealBoss : MonoBehaviourPunCallbacks
{
	public static NetworkDealBoss me;

	public static NetworkDealBase activeDeal;

	private PhotonView view;

	private void Awake()
	{
		me = this;
		view = GetComponent<PhotonView>();
	}

	private void Update()
	{
		if (PhotonNetwork.IsMasterClient && activeDeal != null)
		{
			if (activeDeal.IsDirty)
			{
				SyncDealProgress();
			}
			activeDeal.Update();
		}
	}

	public void PrintActiveDeal()
	{
		if (activeDeal != null)
		{
			Debug.Log($"Active Deal {activeDeal.DealName} Progress: {activeDeal.ProgressInt} state: {activeDeal.State} reward: {activeDeal.reward.GetRewardDescription()} dealId: {activeDeal.GetIndex()} rewardId: {activeDeal.reward.GetIndex()} diff: {activeDeal.difficulty} ");
			activeDeal.reward.DebugReward();
		}
	}

	public void AddDeal(NetworkDealBase deal)
	{
		view.RPC("RPCA_AddDeal", RpcTarget.All, deal.GetIndex(), deal.reward.GetIndex(), deal.difficulty);
	}

	public void HardSyncDeal()
	{
		if (activeDeal != null)
		{
			view.RPC("RPCA_HardSyncDeal", RpcTarget.All, activeDeal.ProgressInt, activeDeal.State, activeDeal.GetIndex(), activeDeal.reward.GetIndex(), activeDeal.difficulty);
		}
	}

	[PunRPC]
	public void RPCA_HardSyncDeal(int progress, NetworkDealBase.DEAL_STATE dealState, byte dealIndex, byte rewardIndex, DIFFICULTY difficulty)
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			NetworkDealBase dealFromIndex = SingletonAsset<NetworkDealDataBase>.Instance.GetDealFromIndex(dealIndex);
			DealRewardBase rewardFromIndex = SingletonAsset<DealRewardDatabase>.Instance.GetRewardFromIndex(rewardIndex);
			dealFromIndex.Init(rewardFromIndex, difficulty);
			dealFromIndex.ProgressInt = progress;
			dealFromIndex.State = dealState;
			activeDeal = dealFromIndex;
		}
	}

	private void SyncDealProgress()
	{
		activeDeal.ClearDirty();
		view.RPC("RPCA_SyncDealProgress", RpcTarget.All, activeDeal.ProgressInt, activeDeal.State);
	}

	[PunRPC]
	public void RPCA_SyncDealProgress(int progress, NetworkDealBase.DEAL_STATE dealState)
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			if (activeDeal == null)
			{
				Debug.LogError("active deal is null");
				return;
			}
			activeDeal.ProgressInt = progress;
			activeDeal.State = dealState;
		}
	}

	[PunRPC]
	public void RPCA_AddDeal(byte dealIndex, byte rewardIndex, DIFFICULTY difficulty)
	{
		Debug.Log("RPCA_AddDeal");
		NetworkDealBase dealFromIndex = SingletonAsset<NetworkDealDataBase>.Instance.GetDealFromIndex(dealIndex);
		DealRewardBase rewardFromIndex = SingletonAsset<DealRewardDatabase>.Instance.GetRewardFromIndex(rewardIndex);
		dealFromIndex.Init(rewardFromIndex, difficulty);
		activeDeal = dealFromIndex;
		if (PhotonNetwork.IsMasterClient)
		{
			SurfaceNetworkHandler.RoomStats.SetNetworkDealsToSelect(Array.Empty<NetworkDealBase>());
			dealFromIndex.SpawnSponsorItem();
		}
	}

	[PunRPC]
	public void RPCA_ClaimReward()
	{
		if (activeDeal != null)
		{
			activeDeal.ClaimReward();
			activeDeal = null;
		}
	}

	[PunRPC]
	public void RPCA_RemoveDeal()
	{
		Debug.Log("RPCA_RemoveDeal");
		activeDeal = null;
	}

	public void RemoveDeal(NetworkDealBase deal)
	{
		Debug.Log("remove deal");
		view.RPC("RPCA_RemoveDeal", RpcTarget.All);
	}

	public void ReviewUploadContent(ContentBuffer contentBuffer)
	{
		if (activeDeal != null)
		{
			Debug.LogError("ReviewUploadContent Dealboss");
			activeDeal.ReviewUploadContent(contentBuffer);
		}
	}

	public void OnStatusUpdated()
	{
		if (activeDeal != null)
		{
			activeDeal.OnStatusUpdated();
		}
	}

	public void OnAddQuota(int quotaToAdd)
	{
		if (activeDeal != null)
		{
			activeDeal.OnAddQuota(quotaToAdd);
		}
	}

	public void OnMoneyRemoved(int money)
	{
		if (activeDeal != null)
		{
			activeDeal.OnRemovedMoney(money);
		}
	}

	public void ClaimReward()
	{
		if (activeDeal == null)
		{
			Debug.LogError("Tried to claim reward but no active deal or not master client");
		}
		else
		{
			view.RPC("RPCA_ClaimReward", RpcTarget.All);
		}
	}

	[ConsoleCommand]
	public static void PickDeal(NetworkDealBase deal)
	{
		deal.Init(SingletonAsset<DealRewardDatabase>.Instance.GetRandom(), deal.AllowedDifficulties[0]);
		me.AddDeal(deal);
	}

	[ConsoleCommand]
	public static void CompleteDeal()
	{
		if (activeDeal == null)
		{
			Debug.Log("No active deal");
			return;
		}
		Debug.Log("Completing Deal");
		activeDeal.ProgressInt = 100;
	}

	[ConsoleCommand]
	public static void CompleteDealAndClaimReward()
	{
		CompleteDeal();
		ClaimTheReward();
	}

	[ConsoleCommand]
	public static void ClaimTheReward()
	{
		me.ClaimReward();
	}

	[ConsoleCommand]
	public static void FailDeal()
	{
		if (activeDeal == null)
		{
			Debug.Log("No active deal");
			return;
		}
		Debug.Log("failing Deal");
		activeDeal.State = NetworkDealBase.DEAL_STATE.failed;
	}

	public static void Cleanup()
	{
		activeDeal = null;
	}

	public void RequestClaimReward()
	{
		ClaimReward();
	}

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		base.OnPlayerEnteredRoom(newPlayer);
		if (PhotonNetwork.IsMasterClient && activeDeal != null)
		{
			HardSyncDeal();
			view.RPC("RPCA_HardSyncDeal", RpcTarget.All, activeDeal.ProgressInt, activeDeal.State, activeDeal.GetIndex(), activeDeal.reward.GetIndex(), activeDeal.difficulty);
		}
	}
}
