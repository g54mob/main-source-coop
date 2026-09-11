using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using pworld.Scripts.Extensions;

public class HatShop : MonoBehaviour
{
	public SFX_Instance buyHat;

	public SFX_Instance noHat;

	public static HatShop instance;

	public List<HatBuyInteractable> hatBuyInteractables = new List<HatBuyInteractable>();

	private int savedSeed;

	private PhotonView view;

	private void Awake()
	{
		instance = this;
		hatBuyInteractables = GetComponentsInChildren<HatBuyInteractable>().ToList();
		view = GetComponent<PhotonView>();
	}

	private void Start()
	{
		StockShop();
		SurfaceNetworkHandler surfaceNetworkHandler = SurfaceNetworkHandler.Instance;
		surfaceNetworkHandler.StartGameAction = (Action)Delegate.Combine(surfaceNetworkHandler.StartGameAction, new Action(StockShop));
		SurfaceNetworkHandler surfaceNetworkHandler2 = SurfaceNetworkHandler.Instance;
		surfaceNetworkHandler2.ReturnToSurfaceAction = (Action)Delegate.Combine(surfaceNetworkHandler2.ReturnToSurfaceAction, new Action(StockShop));
	}

	private void StockShop()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			view.RPC("RPCA_StockShop", RpcTarget.All, DateTime.Today.Date.GetHashCode());
		}
	}

	[PunRPC]
	public void RPCA_StockShop(int seed)
	{
		savedSeed = seed;
		Restock();
	}

	public void Restock()
	{
		UnityEngine.Random.State state = UnityEngine.Random.state;
		UnityEngine.Random.InitState(savedSeed);
		List<Hat> randomNoDuplicates = HatDatabase.instance.hats.GetRandomNoDuplicates(hatBuyInteractables.Count);
		for (int i = 0; i < hatBuyInteractables.Count; i++)
		{
			HatBuyInteractable hatBuyInteractable = hatBuyInteractables[i];
			Hat hat = randomNoDuplicates[i];
			hatBuyInteractable.LoadHat(price: Mathf.RoundToInt((float)hat.GetBasePrice() * UnityEngine.Random.Range(0.5f, 2f) / 10f) * 10, hatPrefab: hat.gameObject);
		}
		UnityEngine.Random.state = state;
	}

	public void HatBuyClicked(HatBuyInteractable hatBuyInteractable)
	{
		if (hatBuyInteractable == null)
		{
			Debug.LogError("HatBuyInteractable is null");
		}
		int num = hatBuyInteractables.IndexOf(hatBuyInteractable);
		Debug.Log($"indexOf {num}");
		if (PhotonNetwork.LocalPlayer == null)
		{
			Debug.LogError("PhotonNetwork.LocalPlayer is null");
		}
		Debug.Log($"PhotonNetwork.LocalPlayer.ActorNumber {PhotonNetwork.LocalPlayer.ActorNumber}");
		view.RPC("RPCM_TryBuyHat", RpcTarget.MasterClient, num, PhotonNetwork.LocalPlayer.ActorNumber);
	}

	[PunRPC]
	public void RPCM_TryBuyHat(int hatBuyIndex, int buyerActorNumber)
	{
		Debug.Log("RPCM_TryBuyHat");
		PhotonNetwork.PlayerList.First((Photon.Realtime.Player o) => o.ActorNumber == buyerActorNumber);
		if (hatBuyInteractables[hatBuyIndex].IsEmpty)
		{
			noHat.Play(base.transform.position);
			Debug.LogError("HatBuyInteractable is empty");
			return;
		}
		Debug.Log("Callling RPCA_BuyHat");
		view.RPC("RPCA_BuyHat", RpcTarget.All, hatBuyIndex, buyerActorNumber);
	}

	[PunRPC]
	public void RPCA_BuyHat(int hatBuyIndex, int buyerActorNumber)
	{
		Debug.Log("RPCA_BuyHat");
		HatBuyInteractable hatBuyInteractable = hatBuyInteractables[hatBuyIndex];
		if (!PlayerHandler.instance.TryGetPlayerFromOwnerID(buyerActorNumber, out var o))
		{
			noHat.Play(base.transform.position);
			Debug.LogError("Player not found");
			return;
		}
		if (Player.localPlayer == o)
		{
			Debug.LogError("Buying");
			HatDatabase.instance.GetIndexOfHat(hatBuyInteractable.hatPrefab.GetComponent<Hat>());
			MetaProgressionHandler.UnlockHat(hatBuyInteractable.ihat.runtimeHatIndex);
			MetaProgressionHandler.RemoveMetaCoins(hatBuyInteractable.ihat.priceToday);
			o.Call_EquipHat(hatBuyInteractable.ihat.runtimeHatIndex);
		}
		buyHat.Play(base.transform.position);
		hatBuyInteractable.ClearHat();
	}
}
