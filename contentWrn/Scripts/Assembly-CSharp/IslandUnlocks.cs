using System;
using Photon.Pun;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class IslandUnlocks : MonoBehaviour
{
	public bool active;

	private PhotonView view;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		SurfaceNetworkHandler instance = SurfaceNetworkHandler.Instance;
		instance.StartGameAction = (Action)Delegate.Combine(instance.StartGameAction, new Action(LoadUnlocks));
		SurfaceNetworkHandler instance2 = SurfaceNetworkHandler.Instance;
		instance2.ReturnToSurfaceAction = (Action)Delegate.Combine(instance2.ReturnToSurfaceAction, new Action(LoadUnlocks));
	}

	private void LoadUnlocks()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			view.RPC("RPCA_RequestUnlocks", RpcTarget.All);
		}
	}

	[PunRPC]
	private void RPCA_RequestUnlocks()
	{
		view.RPC("RPCA_Activate", RpcTarget.All, MetaProgressionHandler.GetUnlockedIslandUpgrades());
	}

	[PunRPC]
	private void RPCA_Activate(int[] unlocks)
	{
		for (int i = 0; i < unlocks.Length; i++)
		{
			IslandUnlock component = base.transform.GetChild(unlocks[i]).GetComponent<IslandUnlock>();
			if (component.locked)
			{
				component.Activate();
			}
		}
	}

	public void Interact(int unlockID, IslandUnlock unlock)
	{
		if (unlock.locked && MetaProgressionHandler.CanAffordPurchase(unlock.price))
		{
			MetaProgressionHandler.UnlockIslandUpgrade(unlock);
			view.RPC("RPCA_Unlock", RpcTarget.All, unlockID);
			MetaProgressionHandler.RemoveMetaCoins(unlock.price);
		}
	}

	[PunRPC]
	private void RPCA_Unlock(int unlock)
	{
		IslandUnlock component = base.transform.GetChild(unlock).GetComponent<IslandUnlock>();
		if (component.locked)
		{
			component.Unlock();
		}
	}
}
