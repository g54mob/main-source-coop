using Photon.Pun;
using UnityEngine;

public class RealmGateTrigger : MonoBehaviour
{
	internal Realm realmData;

	private void Update()
	{
		if (realmData == null)
		{
			return;
		}
		for (int num = realmData.playersInRealm.Count - 1; num >= 0; num--)
		{
			if (realmData.playersInRealm[num].data.dead)
			{
				ShadowRealmHandler.instance.PlayerLeaveRealm(realmData.playersInRealm[num], base.transform.root.gameObject);
			}
		}
	}

	private void OnTriggerEnter(Collider col)
	{
		if (PhotonNetwork.IsMasterClient && !col.isTrigger)
		{
			Player componentInParent = col.GetComponentInParent<Player>();
			if ((bool)componentInParent && !componentInParent.ai && realmData.playersInRealm.Contains(componentInParent))
			{
				ShadowRealmHandler.instance.PlayerLeaveRealm(componentInParent, base.transform.root.gameObject);
			}
		}
	}
}
