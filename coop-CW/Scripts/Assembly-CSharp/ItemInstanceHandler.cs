using System;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;
using Zorro.PhotonUtility;

public class ItemInstanceHandler : Singleton<ItemInstanceHandler>
{
	public Dictionary<Guid, ItemInstance> m_itemInstances = new Dictionary<Guid, ItemInstance>();

	private Dictionary<Guid, Dictionary<ItemRPC, Action<BinaryDeserializer>>> m_rpcActions = new Dictionary<Guid, Dictionary<ItemRPC, Action<BinaryDeserializer>>>();

	private ListenerHandle m_listenerHandle;

	protected override void OnCreated()
	{
		base.OnCreated();
		m_listenerHandle = CustomCommandListener<CustomCommandType>.RegisterListener<ItemInstancePackage>(HandleRPC);
	}

	private void OnDestroy()
	{
		CustomCommandListener<CustomCommandType>.UnregisterListener(m_listenerHandle);
	}

	private static void HandleRPC(ItemInstancePackage package)
	{
		if (Singleton<ItemInstanceHandler>.Instance.m_rpcActions.ContainsKey(package.ItemInstanceGuid))
		{
			if (Singleton<ItemInstanceHandler>.Instance.m_rpcActions[package.ItemInstanceGuid].ContainsKey(package.ItemRPC))
			{
				Singleton<ItemInstanceHandler>.Instance.m_rpcActions[package.ItemInstanceGuid][package.ItemRPC](new BinaryDeserializer(package.Data));
			}
			else
			{
				Debug.LogError($"No action registered for RPC {package.ItemRPC} on item {package.ItemInstanceGuid}");
			}
		}
		else
		{
			Debug.LogError($"No actions registered for item {package.ItemInstanceGuid}");
		}
	}

	public static void RegisterItemInstance(Guid guid, ItemInstance itemInstance)
	{
		if (Singleton<ItemInstanceHandler>.Instance.m_itemInstances.ContainsKey(guid))
		{
			Singleton<ItemInstanceHandler>.Instance.m_itemInstances[guid] = itemInstance;
		}
		else
		{
			Singleton<ItemInstanceHandler>.Instance.m_itemInstances.Add(guid, itemInstance);
		}
	}

	public static void UnregisterItemInstance(Guid guid)
	{
		if (!(Singleton<ItemInstanceHandler>.Instance == null))
		{
			if (Singleton<ItemInstanceHandler>.Instance.m_itemInstances.ContainsKey(guid))
			{
				Singleton<ItemInstanceHandler>.Instance.m_itemInstances.Remove(guid);
			}
			if (Singleton<ItemInstanceHandler>.Instance.m_rpcActions.ContainsKey(guid))
			{
				Singleton<ItemInstanceHandler>.Instance.m_rpcActions.Remove(guid);
			}
		}
	}

	public static void RegisterRPC(Guid itemInstance, ItemRPC rpc, Action<BinaryDeserializer> action)
	{
		if (!Singleton<ItemInstanceHandler>.Instance.m_rpcActions.ContainsKey(itemInstance))
		{
			Singleton<ItemInstanceHandler>.Instance.m_rpcActions.Add(itemInstance, new Dictionary<ItemRPC, Action<BinaryDeserializer>>());
		}
		if (Singleton<ItemInstanceHandler>.Instance.m_rpcActions[itemInstance].ContainsKey(rpc))
		{
			Debug.LogError($"RPC {rpc} already registered for item {itemInstance}");
			return;
		}
		string[] obj = new string[6]
		{
			"Registering RPC ",
			rpc.ToString(),
			" for item ",
			null,
			null,
			null
		};
		Guid guid = itemInstance;
		obj[3] = guid.ToString();
		obj[4] = " with action ";
		obj[5] = action.Method.Name;
		Debug.Log(string.Concat(obj));
		Singleton<ItemInstanceHandler>.Instance.m_rpcActions[itemInstance].Add(rpc, action);
	}

	public static void CallRPC(Guid guid, ItemRPC rpc, BinarySerializer binarySerializer)
	{
		ItemInstancePackage obj = new ItemInstancePackage
		{
			ItemInstanceGuid = guid,
			ItemRPC = rpc,
			Data = binarySerializer.buffer
		};
		CustomCommands<CustomCommandType>.SendPackage(obj, ReceiverGroup.Others);
		HandleRPC(obj);
		binarySerializer.Dispose();
	}
}
