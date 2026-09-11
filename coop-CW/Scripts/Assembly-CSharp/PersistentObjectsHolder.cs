using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentObjectsHolder : RetrievableSingleton<PersistentObjectsHolder>
{
	private List<PersistentObjectInfo> m_PersistentDiveBellObjects;

	private List<PersistentObjectInfo> m_PersistentObjects;

	private List<PersistentObjectInfo> m_PersistentSurfaceObjects;

	private Dictionary<Pickup, PersistentObjectInfo> m_PersistentObjectDic;

	private string m_LastScene = string.Empty;

	protected override void OnCreated()
	{
		base.OnCreated();
		Object.DontDestroyOnLoad(base.gameObject);
		m_PersistentObjects = new List<PersistentObjectInfo>();
		m_PersistentSurfaceObjects = new List<PersistentObjectInfo>();
		m_PersistentDiveBellObjects = new List<PersistentObjectInfo>();
		m_PersistentObjectDic = new Dictionary<Pickup, PersistentObjectInfo>();
		VerboseDebug.Log("Persistent Objects OnCreate!");
	}

	public void SpawnPersistentSurfaceObjects()
	{
		SpawnPersistentObjects(m_PersistentSurfaceObjects);
		ClearPersistentSurfaceObjects();
		SpawnPersistentDiveBellObjects();
	}

	private void SpawnPersistentDiveBellObjects(DivingBell bell = null)
	{
		if (m_PersistentDiveBellObjects.Count != 0)
		{
			if (bell == null)
			{
				bell = Object.FindObjectOfType<DivingBell>();
			}
			if (bell == null)
			{
				Debug.LogError("No Bell Found, Error");
				return;
			}
			Debug.Log($"Spawning Dive Bell Objects: {m_PersistentDiveBellObjects.Count} at {bell.itemSpawns.position}");
			StartCoroutine(SpawnObjectsDelayed(m_PersistentDiveBellObjects.ToArray(), bell.itemSpawns.position));
			ClearPersistentDivebellObjects();
		}
	}

	private IEnumerator SpawnPersistentDiveBellObjectsDelayed()
	{
		yield return 2;
		DiveBellParent diveBellParent = DiveBellParent.instance;
		if (diveBellParent == null)
		{
			Debug.LogError("No Dive Bell Parent Found");
			yield break;
		}
		while (diveBellParent.TargetDivingBell == null)
		{
			yield return null;
		}
		SpawnPersistentDiveBellObjects(diveBellParent.TargetDivingBell);
	}

	public void SpawnPersistentObjects()
	{
		string text = SceneManager.GetActiveScene().name;
		if (m_LastScene == text)
		{
			SpawnPersistentObjects(m_PersistentObjects);
			ClearPersistentObjects();
		}
		else
		{
			Debug.Log("Persistant Objects, Not Same Scene, Not Spawning");
		}
		StartCoroutine(SpawnPersistentDiveBellObjectsDelayed());
	}

	private void SpawnPersistentObjects(List<PersistentObjectInfo> objects)
	{
		StartCoroutine(SpawnObjectsDelayed(objects.ToArray()));
	}

	private IEnumerator SpawnObjectsDelayed(PersistentObjectInfo[] objectInfos, Vector3? overridePos = null)
	{
		yield return 2;
		for (int i = 0; i < objectInfos.Length; i++)
		{
			PersistentObjectInfo persistentObjectInfo = objectInfos[i];
			Vector3 vector = overridePos ?? persistentObjectInfo.Position;
			Debug.Log($"Spawning Persistant Object: {persistentObjectInfo.Item.name} at {vector}");
			Pickup pickup = PickupHandler.CreatePickup(persistentObjectInfo.Item.id, persistentObjectInfo.InstanceData, vector, persistentObjectInfo.Rotation);
			pickup.Rigidbody.position = vector + Vector3.up * 0.1f;
			pickup.Rigidbody.rotation = persistentObjectInfo.Rotation;
		}
	}

	public void FindPersistantObjects()
	{
		VerboseDebug.Log("Looking for persistant objects...");
		PersistantObject[] array = Object.FindObjectsOfType<PersistantObject>();
		foreach (PersistantObject persistantObject in array)
		{
			AddPersistentObject(persistantObject);
			VerboseDebug.Log("Found persistant Object..." + persistantObject.name);
		}
		m_LastScene = SceneManager.GetActiveScene().name;
		VerboseDebug.Log("Persistant Object LastScene: " + m_LastScene);
		FindDivebellItems();
	}

	public void FindPersistantSurfaceObjects()
	{
		Debug.Log("Looking for Surface persistant objects...");
		Pickup[] array = Object.FindObjectsOfType<Pickup>();
		foreach (Pickup pickup in array)
		{
			if (pickup.itemInstance.item.itemType != Item.ItemType.Camera)
			{
				AddPersistentObject(pickup, pickup.itemInstance.item);
				VerboseDebug.Log("Found persistant Object..." + pickup.name);
			}
		}
		FindDivebellItems();
	}

	public PersistentObjectInfo[] FindPersistantSurfaceObjectsLite()
	{
		Debug.Log("Looking for Surface persistant objects for save...");
		Pickup[] array = Object.FindObjectsOfType<Pickup>();
		List<PersistentObjectInfo> list = new List<PersistentObjectInfo>();
		Pickup[] array2 = array;
		foreach (Pickup pickup in array2)
		{
			if (!(pickup == null))
			{
				ItemInstance componentInChildren = pickup.GetComponentInChildren<ItemInstance>();
				if (componentInChildren.item.itemType != Item.ItemType.Camera && componentInChildren.m_guid.IsSome && ItemInstanceDataHandler.TryGetInstanceData(componentInChildren.m_guid.Value, out var o))
				{
					Transform transform = pickup.Rigidbody.transform;
					PersistentObjectInfo item = new PersistentObjectInfo(pickup.itemInstance.item, transform.position, transform.rotation, o, pickup);
					list.Add(item);
				}
			}
		}
		return list.ToArray();
	}

	private void FindDivebellItems()
	{
		DivingBell divingBell = Object.FindObjectOfType<DivingBell>();
		DiveBellPickupDetector pickupDetector = divingBell.pickupDetector;
		if (!pickupDetector)
		{
			Debug.LogError("No DiveBellPickupDetector Found!");
			return;
		}
		foreach (Pickup item2 in pickupDetector.CheckForPickups())
		{
			Debug.Log("Found DiveBell Pickup: " + item2.itemInstance?.name);
			if (m_PersistentObjectDic.ContainsKey(item2))
			{
				PersistentObjectInfo item = m_PersistentObjectDic[item2];
				if (m_PersistentSurfaceObjects.Contains(item))
				{
					m_PersistentSurfaceObjects.Remove(item);
				}
				m_PersistentObjectDic.Remove(item2);
				if (m_PersistentObjects.Contains(item))
				{
					m_PersistentObjects.Remove(item);
				}
				Debug.Log("Removing Persistent object: " + item2.name);
			}
			AddPersistentDiveBellObject(item2, divingBell);
		}
	}

	private void AddPersistentObject(PersistantObject go)
	{
		Pickup componentInParent = go.GetComponentInParent<Pickup>();
		if (m_PersistentObjectDic.ContainsKey(componentInParent))
		{
			Debug.Log("Persistent object Already in list: " + go.name);
			return;
		}
		Debug.Log("Adding Persistent object: " + go.name);
		if (!(componentInParent == null))
		{
			Item itemToUse = ((go.ItemSwitch == null) ? componentInParent.itemInstance.item : go.ItemSwitch);
			AddPersistentObject(componentInParent, itemToUse);
		}
	}

	private void AddPersistentObject(Pickup p, Item itemToUse)
	{
		if (p == null)
		{
			return;
		}
		string text = SceneManager.GetActiveScene().name;
		ItemInstance componentInChildren = p.GetComponentInChildren<ItemInstance>();
		if (componentInChildren.m_guid.IsSome && ItemInstanceDataHandler.TryGetInstanceData(componentInChildren.m_guid.Value, out var o))
		{
			Transform transform = p.Rigidbody.transform;
			PersistentObjectInfo persistentObjectInfo = new PersistentObjectInfo(itemToUse, transform.position, transform.rotation, o, p);
			m_PersistentObjectDic.Add(p, persistentObjectInfo);
			if (text == "SurfaceScene")
			{
				m_PersistentSurfaceObjects.Add(persistentObjectInfo);
			}
			else
			{
				m_PersistentObjects.Add(persistentObjectInfo);
			}
		}
	}

	private void AddPersistentDiveBellObject(Pickup p, DivingBell bell)
	{
		if (p == null || p.itemInstance.item.itemType == Item.ItemType.Artifact)
		{
			return;
		}
		Vector3 position = bell.transform.position;
		Item item = p.itemInstance.item;
		ItemInstance componentInChildren = p.GetComponentInChildren<ItemInstance>();
		if (componentInChildren.m_guid.IsSome)
		{
			if (ItemInstanceDataHandler.TryGetInstanceData(componentInChildren.m_guid.Value, out var o))
			{
				Transform transform = p.Rigidbody.transform;
				Vector3 vector = position - transform.position;
				Debug.Log($"Adding Persistent DiveBell Object: {p.itemInstance?.name} BellPos: {position} SpawnPos: {vector}");
				PersistentObjectInfo persistentObjectInfo = new PersistentObjectInfo(item, vector, transform.rotation, o, p);
				m_PersistentObjectDic.Add(p, persistentObjectInfo);
				m_PersistentDiveBellObjects.Add(persistentObjectInfo);
			}
			else
			{
				Debug.Log("Missing Item Instance Data: " + p.itemInstance?.name);
			}
		}
	}

	public void ClearPersistentObjects()
	{
		VerboseDebug.Log("Clear Persistent objects");
		foreach (PersistentObjectInfo persistentObject in m_PersistentObjects)
		{
			if (m_PersistentObjectDic.ContainsKey(persistentObject.Pickup))
			{
				m_PersistentObjectDic.Remove(persistentObject.Pickup);
			}
		}
		m_PersistentObjects = new List<PersistentObjectInfo>();
	}

	public void ClearPersistentSurfaceObjects()
	{
		VerboseDebug.Log("Clear Persistent Surface objects");
		foreach (PersistentObjectInfo persistentSurfaceObject in m_PersistentSurfaceObjects)
		{
			if (m_PersistentObjectDic.ContainsKey(persistentSurfaceObject.Pickup))
			{
				m_PersistentObjectDic.Remove(persistentSurfaceObject.Pickup);
			}
		}
		m_PersistentSurfaceObjects = new List<PersistentObjectInfo>();
	}

	private void ClearPersistentDivebellObjects()
	{
		VerboseDebug.Log("Clear Persistent DiveBell objects");
		foreach (PersistentObjectInfo persistentDiveBellObject in m_PersistentDiveBellObjects)
		{
			if (m_PersistentObjectDic.ContainsKey(persistentDiveBellObject.Pickup))
			{
				m_PersistentObjectDic.Remove(persistentDiveBellObject.Pickup);
			}
		}
		m_PersistentDiveBellObjects.Clear();
	}

	public void ResetPersistantObjects()
	{
		ClearPersistentObjects();
		ClearPersistentSurfaceObjects();
		ClearPersistentDivebellObjects();
		m_PersistentObjectDic = new Dictionary<Pickup, PersistentObjectInfo>();
	}

	private void OnDestroy()
	{
		Debug.Log("Persistent Objects OnDestroy!");
	}
}
