using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WebParent : MonoBehaviour
{
	public int webs = 5;

	public GameObject webObj;

	private PhotonView view;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		if (view.IsMine)
		{
			Spawns();
		}
	}

	private void Spawns()
	{
		if (!view)
		{
			view = GetComponent<PhotonView>();
		}
		List<Vector3> list = new List<Vector3>();
		List<Quaternion> list2 = new List<Quaternion>();
		List<Vector3> list3 = new List<Vector3>();
		List<Vector3> list4 = new List<Vector3>();
		for (int i = 0; i < webs; i++)
		{
			Web component = Object.Instantiate(webObj, base.transform.position + base.transform.forward * 1f + Random.onUnitSphere, base.transform.rotation, base.transform).GetComponent<Web>();
			if (!component.TryInit())
			{
				break;
			}
			list.Add(component.transform.position);
			list2.Add(component.transform.rotation);
			list3.Add(component.p1);
			list4.Add(component.p2);
		}
		view.RPC("RPCA_SynkNets", RpcTarget.All, list.ToArray(), list3.ToArray(), list4.ToArray(), list2.ToArray());
	}

	[PunRPC]
	private void RPCA_SynkNets(Vector3[] positions, Vector3[] p1, Vector3[] p2, Quaternion[] rots)
	{
		if (!view)
		{
			view = GetComponent<PhotonView>();
		}
		for (int i = 0; i < positions.Length; i++)
		{
			GameObject gameObject = null;
			gameObject = ((!view.IsMine) ? Object.Instantiate(webObj, positions[i], rots[i], base.transform) : base.transform.GetChild(i).gameObject);
			gameObject.GetComponent<Web>().SetCustom(p1[i], p2[i]);
		}
	}

	internal void RequestStick(Player player, Web web, int bodypartID)
	{
		if (!view)
		{
			view = GetComponent<PhotonView>();
		}
		view.RPC("RPCM_RequestStick", RpcTarget.MasterClient, player.refs.view.ViewID, web.transform.GetSiblingIndex(), bodypartID);
	}

	[PunRPC]
	private void RPCM_RequestStick(int playerID, int webID, int bodypartID)
	{
		if (!view)
		{
			view = GetComponent<PhotonView>();
		}
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(playerID);
		Web component = base.transform.GetChild(webID).GetComponent<Web>();
		if (!component.heldPart)
		{
			view.RPC("RPCA_Stick", RpcTarget.All, player.refs.view.ViewID, component.transform.GetSiblingIndex(), bodypartID);
		}
	}

	[PunRPC]
	private void RPCA_Stick(int playerID, int webID, int bodypartID)
	{
		if (!view)
		{
			view = GetComponent<PhotonView>();
		}
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(playerID);
		Web component = base.transform.GetChild(webID).GetComponent<Web>();
		Debug.Log("Bodypart id" + bodypartID);
		Rigidbody rig = player.refs.ragdoll.GetBodypartFromID(bodypartID).rig;
		component.StickToPlayer(rig, player);
	}

	internal void LetPlayerGo(Web web)
	{
		if (!view)
		{
			view = GetComponent<PhotonView>();
		}
		view.RPC("RPCA_LetPlayerGo", RpcTarget.All, web.transform.GetSiblingIndex());
	}

	[PunRPC]
	private void RPCA_LetPlayerGo(int webID)
	{
		if (!view)
		{
			view = GetComponent<PhotonView>();
		}
		base.transform.GetChild(webID).GetComponent<Web>().LetGo();
	}
}
