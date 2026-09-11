using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Latch : MonoBehaviour
{
	private PhotonView view;

	public List<Player> ignoredPlayers = new List<Player>();

	private bool closed;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		if (view.IsMine)
		{
			RandomizeClose(0.85f);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.isTrigger && !closed && view.IsMine)
		{
			Player componentInParent = other.GetComponentInParent<Player>();
			if (!(componentInParent == null) && !ignoredPlayers.Contains(componentInParent))
			{
				ignoredPlayers.Add(componentInParent);
				RandomizeClose(0.1f);
			}
		}
	}

	private void RandomizeClose(float chance)
	{
		if (!closed && Random.value < chance)
		{
			view.RPC("RPCA_Close", RpcTarget.All);
		}
	}

	[PunRPC]
	private void RPCA_Close()
	{
		if (!closed)
		{
			closed = true;
			GetComponent<BoxCollider>().enabled = false;
			GetComponentInChildren<Animator>().Play("Latch_Close");
		}
	}
}
