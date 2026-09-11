using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ShadowRealmHandler : MonoBehaviour
{
	public static ShadowRealmHandler instance;

	private PhotonView view;

	private Realm[] currentRealms = new Realm[10];

	public List<GameObject> realms = new List<GameObject>();

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		view = GetComponent<PhotonView>();
	}

	public int TeleportPlayerToRandomRealm(Player target)
	{
		if (target.data.playerIsInRealm)
		{
			view.RPC("RPCA_RealmEntryDenied", RpcTarget.All, target.refs.view.ViewID);
			return -1;
		}
		int spotID = GetSpotID();
		view.RPC("RPCA_AddRealm", RpcTarget.All, Random.Range(0, realms.Count), spotID, target.refs.view.ViewID);
		return spotID;
	}

	private int GetSpotID()
	{
		for (int i = 0; i < currentRealms.Length; i++)
		{
			if (currentRealms[i] == null)
			{
				return i;
			}
		}
		return -1;
	}

	[PunRPC]
	public void RPCA_RealmEntryDenied(int targetID)
	{
		PlayerHandler.instance.TryGetPlayerFromViewID(targetID).ReToggleCollision();
	}

	[PunRPC]
	public void RPCA_AddRealm(int realmID, int spotID, int targetID)
	{
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(targetID);
		GameObject gameObject = Object.Instantiate(realms[realmID], GetRealmPosition(spotID), Quaternion.identity);
		Realm realm = new Realm();
		realm.realmObject = gameObject;
		realm.playersInRealm = new List<Player>();
		realm.playersInRealm.Add(player);
		currentRealms[spotID] = realm;
		gameObject.GetComponentInChildren<RealmGateTrigger>().realmData = realm;
		player.data.playerIsInRealm = true;
		Transform transform = gameObject.GetComponentInChildren<SpawnPoint>().transform;
		player.Teleport(transform.position + Vector3.up * 2.5f, transform.forward);
		player.ReToggleCollision();
		if (PhotonNetwork.IsMasterClient)
		{
			MonsterSpawner[] componentsInChildren = gameObject.GetComponentsInChildren<MonsterSpawner>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SpawnWithConditionalObject(gameObject);
			}
		}
	}

	private Vector3 GetRealmPosition(int spotID)
	{
		return new Vector3(0f, 0f, 500 + spotID * 500);
	}

	internal void PlayerLeaveRealm(Player p, GameObject realm)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			Transform transform = Level.currentLevel.GetRandomPoint(new List<PatrolPoint.PatrolGroup>
			{
				PatrolPoint.PatrolGroup.Bear,
				PatrolPoint.PatrolGroup.Dog
			}).transform;
			view.RPC("RPCA_RemovePlayerFromRealm", RpcTarget.All, GetRealmID(realm), p.refs.view.ViewID, transform.position);
		}
	}

	[PunRPC]
	public void RPCA_RemovePlayerFromRealm(int spotID, int targetID, Vector3 returnPos)
	{
		Player target = PlayerHandler.instance.TryGetPlayerFromViewID(targetID);
		target.data.playerIsInRealm = false;
		if (target.IsLocal)
		{
			BlackScreen.instance.SetBlackScreen(1f);
		}
		Level.currentLevel.ToggleLightsForSeconds(setLightsOn: false, returnPos, 100f, 0.25f);
		StartCoroutine(IReturnPlayer());
		currentRealms[spotID].playersInRealm.Remove(target);
		IEnumerator IReturnPlayer()
		{
			yield return new WaitForSeconds(0.3f);
			target.Teleport(returnPos, Vector3.forward);
			if (currentRealms[spotID].playersInRealm.Count == 0)
			{
				Object.Destroy(currentRealms[spotID].realmObject);
				currentRealms[spotID] = null;
			}
		}
	}

	private int GetRealmID(GameObject realm)
	{
		for (int i = 0; i < currentRealms.Length; i++)
		{
			if (currentRealms[i] != null && currentRealms[i].realmObject == realm)
			{
				return i;
			}
		}
		return -1;
	}

	[PunRPC]
	private void RPCA_AddPlayerToExistingRealm(int targetPlayer, int targetRealm)
	{
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(targetPlayer);
		Realm obj = currentRealms[targetRealm];
		obj.playersInRealm.Add(player);
		player.data.playerIsInRealm = true;
		Transform transform = obj.realmObject.GetComponentInChildren<SpawnPoint>().transform;
		player.Teleport(transform.position + Vector3.up * 2.5f, transform.forward);
		player.ReToggleCollision();
	}

	internal void AddPlayerToExistingRealm(Player target, int targetRealm)
	{
		view.RPC("RPCA_AddPlayerToExistingRealm", RpcTarget.All, target.refs.view.ViewID, targetRealm);
	}
}
