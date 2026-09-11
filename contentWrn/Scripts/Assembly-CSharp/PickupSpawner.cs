using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PickupSpawner : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private Item m_ItemToSpawn;

	private Transform m_Transform;

	private bool m_Spawned;

	public Item ItemToSpawn => m_ItemToSpawn;

	private void Awake()
	{
		m_Transform = base.transform;
		if (m_ItemToSpawn == null)
		{
			Debug.LogError("Pickup Spawner Has NULL item to spawn");
		}
	}

	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();
		if (!PhotonNetwork.IsMasterClient)
		{
			DestroyMe();
		}
	}

	public void SpawnMe(bool force = false)
	{
		if (!m_Spawned || force)
		{
			m_Spawned = true;
			Pickup pickup = PickupHandler.CreatePickup(m_ItemToSpawn.id, new ItemInstanceData(Guid.NewGuid()), m_Transform.position, m_Transform.rotation);
			StartCoroutine(IChill(pickup));
		}
		static IEnumerator IChill(Pickup pickup2)
		{
			Rigidbody rig = pickup2.GetComponentInChildren<Rigidbody>();
			rig.linearVelocity *= 0f;
			rig.angularVelocity *= 0f;
			rig.useGravity = false;
			float c = 0f;
			while (c < 3f)
			{
				if (rig != null)
				{
					rig.AddForce(Vector3.down * 100f * Time.deltaTime, ForceMode.Acceleration);
					rig.linearVelocity *= 0.5f;
					rig.angularVelocity *= 0.5f;
				}
				c += Time.deltaTime;
				yield return null;
			}
			if (rig != null)
			{
				rig.useGravity = true;
			}
		}
	}

	private void DestroyMe()
	{
		Debug.Log("Already in room, destroying pickupSpawner: " + base.gameObject.name + " For: " + m_ItemToSpawn.name);
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
