using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using pworld.Scripts.Extensions;

public class BedBoss : MonoBehaviour
{
	[Serializable]
	public class Bed
	{
		public BedBoss bedBoss;

		public MeshRenderer duveMeRe;

		public GameObject go;

		public int currentMaterialIndex = -1;

		private Player sleepingPlayer;

		public bool IsClaimed => sleepingPlayer != null;

		public Bed(BedBoss bedboss, GameObject go)
		{
			bedBoss = bedboss;
			this.go = go;
			duveMeRe = go.transform.Find("Duvet").GetComponent<MeshRenderer>();
			PlayerHandler instance = PlayerHandler.instance;
			instance.OnPlayerLeft = (Action<Player>)Delegate.Combine(instance.OnPlayerLeft, new Action<Player>(OnPlayerLeft));
		}

		private void OnPlayerLeft(Player player)
		{
			if (sleepingPlayer == player)
			{
				player = null;
			}
		}

		public void SetPlayer(Player player)
		{
			Debug.Log($"{player.name} is sleeping in bed {go.transform.GetSiblingIndex()}");
			sleepingPlayer = player;
		}

		public void Update()
		{
			if (IsClaimed)
			{
				currentMaterialIndex = sleepingPlayer.refs.visor.visorColorIndex;
				currentMaterialIndex = currentMaterialIndex.PClampToSizeOf(bedBoss.materials);
				Material sharedMaterial = bedBoss.materials[currentMaterialIndex];
				if (!bedBoss.dontApply)
				{
					duveMeRe.sharedMaterial = sharedMaterial;
				}
			}
		}
	}

	public Material[] materials;

	public List<Bed> beds = new List<Bed>();

	public Material[] unusedBedMaterials;

	public bool dontApply;

	private PhotonView view_g;

	private void Awake()
	{
		view_g = GetComponent<PhotonView>();
		foreach (Transform item in base.transform)
		{
			beds.Add(new Bed(this, item.gameObject));
		}
		if (PhotonNetwork.IsMasterClient)
		{
			PlayerHandler instance = PlayerHandler.instance;
			instance.OnPlayerJoined = (Action<Player>)Delegate.Combine(instance.OnPlayerJoined, new Action<Player>(OnPlayerJoined));
		}
	}

	private void Update()
	{
		if (Time.frameCount % 10 != 0)
		{
			return;
		}
		foreach (Bed bed in beds)
		{
			bed.Update();
		}
	}

	private void OnDestroy()
	{
		PlayerHandler instance = PlayerHandler.instance;
		instance.OnPlayerJoined = (Action<Player>)Delegate.Remove(instance.OnPlayerJoined, new Action<Player>(OnPlayerJoined));
	}

	private void OnPlayerJoined(Player player)
	{
		Debug.Log("OnPlayerJoined");
		if (PhotonNetwork.IsMasterClient)
		{
			for (int i = 0; i < PlayerHandler.instance.players.Count; i++)
			{
				Player player2 = PlayerHandler.instance.players[i];
				view_g.RPC("AssignBed", RpcTarget.All, player2.refs.view.ViewID, i);
			}
		}
	}

	[PunRPC]
	private void AssignBed(int viewId, int siblingId)
	{
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(viewId);
		if (player == null)
		{
			Debug.LogError("Coulnd't find player with viewID " + viewId);
		}
		else
		{
			beds[siblingId].SetPlayer(player);
		}
	}
}
