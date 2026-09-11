using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Zorro.Core;

public class PickupHandler : Singleton<PickupHandler>
{
	private List<Pickup> m_pickup = new List<Pickup>();

	public static Pickup CreatePickup(byte itemID, ItemInstanceData data, Vector3 pos, Quaternion rot, Vector3 vel, Vector3 angVel)
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			Debug.LogError("Only master client can create pickups");
			return null;
		}
		Pickup component = PhotonNetwork.InstantiateRoomObject("PickupHolder", pos, rot, 0).GetComponent<Pickup>();
		component.ConfigurePickup(itemID, data, vel, angVel);
		return component;
	}

	public static Pickup CreatePickup(byte itemID, ItemInstanceData data, Vector3 pos, Quaternion rot)
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			Debug.LogError("Only master client can create pickups");
			return null;
		}
		Pickup component = PhotonNetwork.InstantiateRoomObject("PickupHolder", pos, rot, 0).GetComponent<Pickup>();
		component.ConfigurePickup(itemID, data);
		return component;
	}

	public static void RegisterPickup(Pickup pickup)
	{
		if (!Singleton<PickupHandler>.Instance.m_pickup.Contains(pickup))
		{
			Singleton<PickupHandler>.Instance.m_pickup.Add(pickup);
			VerboseDebug.Log("Registered pickup: " + pickup.name);
		}
	}

	public static void UnregisterPickup(Pickup pickup)
	{
		if (!(Singleton<PickupHandler>.Instance == null) && Singleton<PickupHandler>.Instance.m_pickup.Contains(pickup))
		{
			Singleton<PickupHandler>.Instance.m_pickup.Remove(pickup);
			VerboseDebug.Log("Unregistered pickup: " + pickup.name);
		}
	}

	public static bool GetRandomPickup(out Pickup pickup, List<Item> excludedItems = null)
	{
		if (Singleton<PickupHandler>.Instance.m_pickup.Count == 0)
		{
			pickup = null;
			return false;
		}
		List<Pickup> list = Singleton<PickupHandler>.Instance.m_pickup;
		if (excludedItems != null)
		{
			list = list.Where((Pickup pickup2) => excludedItems.All((Item excludeItem) => excludeItem.id != pickup2.itemInstance.item.id)).ToList();
		}
		if (list.Count == 0)
		{
			pickup = null;
			return false;
		}
		return pickup = list[Random.Range(0, list.Count)];
	}

	public static void SendPosToEveryone()
	{
		foreach (Pickup item in Singleton<PickupHandler>.Instance.m_pickup)
		{
			item.ForceSync();
		}
	}
}
