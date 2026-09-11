using System;
using Photon.Pun;
using UnityEngine;

public class DroneBox : MonoBehaviour
{
	public GameObject part;

	public bool ready;

	private bool spawned;

	private void Start()
	{
	}

	private void OnCollisionEnter(Collision col)
	{
		if (ready && !col.rigidbody && !spawned)
		{
			UnityEngine.Object.Instantiate(part, col.contacts[0].point, Quaternion.LookRotation(Vector3.up));
			GamefeelHandler.instance.perlin.AddShake(10f, 0.25f);
			if (PhotonNetwork.IsMasterClient)
			{
				Spawn();
			}
			spawned = true;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Spawn()
	{
		Debug.Log("Spawning items");
		Item[] items = GetComponentInParent<Drone>().items;
		for (int i = 0; i < items.Length; i++)
		{
			Debug.Log("Spawning item: " + items[i].name);
			PickupHandler.CreatePickup(items[i].id, new ItemInstanceData(Guid.NewGuid()), base.transform.position + Vector3.up * 0.75f + UnityEngine.Random.insideUnitSphere * 0.75f, UnityEngine.Random.rotation, (Vector3.up + UnityEngine.Random.onUnitSphere) * 2f, UnityEngine.Random.onUnitSphere * 5f);
		}
	}
}
