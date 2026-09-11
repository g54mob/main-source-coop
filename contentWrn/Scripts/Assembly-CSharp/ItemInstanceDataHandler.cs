using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstanceDataHandler : RetrievableSingleton<ItemInstanceDataHandler>
{
	private Dictionary<Guid, ItemInstanceData> m_instanceData = new Dictionary<Guid, ItemInstanceData>();

	protected override void OnCreated()
	{
		base.OnCreated();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public static void AddInstanceData(ItemInstanceData instanceData)
	{
		RetrievableSingleton<ItemInstanceDataHandler>.Instance.m_instanceData.TryAdd(instanceData.m_guid, instanceData);
	}

	public static void RemoveInstanceData(Guid guid)
	{
		if (!RetrievableSingleton<ItemInstanceDataHandler>.Instance.m_instanceData.Remove(guid))
		{
			Debug.LogError($"ItemInstanceData with guid {guid} does not exist");
		}
		else
		{
			VerboseDebug.Log($"Removed ItemInstanceData with guid {guid}");
		}
	}

	public static bool TryGetInstanceData(Guid guid, out ItemInstanceData o)
	{
		if (RetrievableSingleton<ItemInstanceDataHandler>.Instance.m_instanceData.TryGetValue(guid, out var value))
		{
			o = value;
			return true;
		}
		o = null;
		return false;
	}
}
