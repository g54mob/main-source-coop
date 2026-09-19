using System;
using System.Collections;
using System.Runtime.InteropServices;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror.Examples.PickupsDropsChilds
{
	public class PickupsDropsChilds : NetworkBehaviour
	{
		[Header("Player Components")]
		public GameObject rightHand;

		[Header("Prefabs")]
		public GameObject ballPrefab;

		public GameObject batPrefab;

		public GameObject boxPrefab;

		public GameObject sceneObjectPrefab;

		[Header("SyncVars in Specific Order")]
		[SyncVar(hook = "OnChangeEquippedItemConfig")]
		public EquippedItemConfig equippedItemConfig;

		[SyncVar(hook = "OnChangeEquipment")]
		public EquippedItem equippedItem;

		[Header("Diagnostics")]
		[ReadOnly]
		public GameObject equippedObject;

		private IEquipped iEquipped;

		public Action<EquippedItemConfig, EquippedItemConfig> _Mirror_SyncVarHookDelegate_equippedItemConfig;

		public Action<EquippedItem, EquippedItem> _Mirror_SyncVarHookDelegate_equippedItem;

		public EquippedItemConfig NetworkequippedItemConfig
		{
			get
			{
				return equippedItemConfig;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref equippedItemConfig, 1uL, _Mirror_SyncVarHookDelegate_equippedItemConfig);
			}
		}

		public EquippedItem NetworkequippedItem
		{
			get
			{
				return equippedItem;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref equippedItem, 2uL, _Mirror_SyncVarHookDelegate_equippedItem);
			}
		}

		private void Update()
		{
			if (base.isLocalPlayer)
			{
				if (Input.GetKeyDown(KeyCode.Alpha0) && equippedItem != EquippedItem.nothing)
				{
					CmdChangeEquippedItem(EquippedItem.nothing);
				}
				if (Input.GetKeyDown(KeyCode.Alpha1) && equippedItem != EquippedItem.ball)
				{
					CmdChangeEquippedItem(EquippedItem.ball);
				}
				if (Input.GetKeyDown(KeyCode.Alpha2) && equippedItem != EquippedItem.bat)
				{
					CmdChangeEquippedItem(EquippedItem.bat);
				}
				if (Input.GetKeyDown(KeyCode.Alpha3) && equippedItem != EquippedItem.box)
				{
					CmdChangeEquippedItem(EquippedItem.box);
				}
				if (Input.GetKeyDown(KeyCode.U) && iEquipped != null)
				{
					CmdUseItem();
				}
				if (Input.GetKeyDown(KeyCode.I) && iEquipped != null)
				{
					CmdAddUsages(1);
				}
				if (Input.GetKeyDown(KeyCode.O) && iEquipped != null)
				{
					CmdResetUsages();
				}
				if (Input.GetKeyDown(KeyCode.P) && iEquipped != null)
				{
					CmdResetUsages(3);
				}
				if (Input.GetKeyDown(KeyCode.X) && equippedItem != EquippedItem.nothing)
				{
					CmdDropItem();
				}
			}
		}

		private void OnChangeEquippedItemConfig(EquippedItemConfig _, EquippedItemConfig newEquippedItemConfig)
		{
			if (equippedObject != null && equippedObject.TryGetComponent<IEquipped>(out iEquipped) && !iEquipped.equippedItemConfig.Equals(equippedItemConfig))
			{
				iEquipped.equippedItemConfig = equippedItemConfig;
			}
		}

		private void OnChangeEquipment(EquippedItem _, EquippedItem newEquippedItem)
		{
			StartCoroutine(ChangeEquipment());
		}

		private IEnumerator ChangeEquipment()
		{
			while (rightHand.transform.childCount > 0)
			{
				UnityEngine.Object.Destroy(rightHand.transform.GetChild(0).gameObject);
				yield return null;
			}
			equippedObject = null;
			switch (equippedItem)
			{
			case EquippedItem.ball:
				equippedObject = UnityEngine.Object.Instantiate(ballPrefab, rightHand.transform);
				break;
			case EquippedItem.bat:
				equippedObject = UnityEngine.Object.Instantiate(batPrefab, rightHand.transform);
				break;
			case EquippedItem.box:
				equippedObject = UnityEngine.Object.Instantiate(boxPrefab, rightHand.transform);
				break;
			}
			if (equippedObject != null && equippedObject.TryGetComponent<IEquipped>(out iEquipped) && !iEquipped.equippedItemConfig.Equals(equippedItemConfig))
			{
				iEquipped.equippedItemConfig = equippedItemConfig;
			}
		}

		[Command]
		private void CmdChangeEquippedItem(EquippedItem selectedItem)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItem(writer, selectedItem);
			SendCommandInternal("System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdChangeEquippedItem(Mirror.Examples.PickupsDropsChilds.EquippedItem)", 1687597733, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		public void CmdUseItem()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdUseItem()", -1636732761, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		public void CmdAddUsages(byte usages)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, usages);
			SendCommandInternal("System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdAddUsages(System.Byte)", -1962251701, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		public void CmdResetUsages()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdResetUsages()", 65225260, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		public void CmdResetUsages(byte usages)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, usages);
			SendCommandInternal("System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdResetUsages(System.Byte)", -1154054425, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		private void CmdDropItem()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdDropItem()", -339312271, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		public void CmdPickupItem(GameObject obj)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteGameObject(obj);
			SendCommandInternal("System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdPickupItem(UnityEngine.GameObject)", -1746849058, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		public void RpcUseItem()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::RpcUseItem()", -258282, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		public void RpcAddUsages(byte usages)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, usages);
			SendRPCInternal("System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::RpcAddUsages(System.Byte)", -2049832148, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		public void RpcResetUsages()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::RpcResetUsages()", -871734747, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		public void RpcResetUsages(byte usages)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, usages);
			SendRPCInternal("System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::RpcResetUsages(System.Byte)", -828557208, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		public PickupsDropsChilds()
		{
			_Mirror_SyncVarHookDelegate_equippedItemConfig = OnChangeEquippedItemConfig;
			_Mirror_SyncVarHookDelegate_equippedItem = OnChangeEquipment;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdChangeEquippedItem__EquippedItem(EquippedItem selectedItem)
		{
			switch (selectedItem)
			{
			case EquippedItem.ball:
			{
				if (ballPrefab.TryGetComponent<IEquipped>(out var component2))
				{
					NetworkequippedItemConfig = component2.equippedItemConfig;
				}
				break;
			}
			case EquippedItem.bat:
			{
				if (batPrefab.TryGetComponent<IEquipped>(out var component3))
				{
					NetworkequippedItemConfig = component3.equippedItemConfig;
				}
				break;
			}
			case EquippedItem.box:
			{
				if (boxPrefab.TryGetComponent<IEquipped>(out var component))
				{
					NetworkequippedItemConfig = component.equippedItemConfig;
				}
				break;
			}
			case EquippedItem.nothing:
				NetworkequippedItemConfig = default(EquippedItemConfig);
				break;
			}
			NetworkequippedItem = selectedItem;
		}

		protected static void InvokeUserCode_CmdChangeEquippedItem__EquippedItem(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdChangeEquippedItem called on client.");
			}
			else
			{
				((PickupsDropsChilds)obj).UserCode_CmdChangeEquippedItem__EquippedItem(GeneratedNetworkCode._Read_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItem(reader));
			}
		}

		protected void UserCode_CmdUseItem()
		{
			EquippedItemConfig networkequippedItemConfig = equippedItemConfig;
			networkequippedItemConfig.Use();
			NetworkequippedItemConfig = networkequippedItemConfig;
			RpcUseItem();
		}

		protected static void InvokeUserCode_CmdUseItem(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdUseItem called on client.");
			}
			else
			{
				((PickupsDropsChilds)obj).UserCode_CmdUseItem();
			}
		}

		protected void UserCode_CmdAddUsages__Byte(byte usages)
		{
			EquippedItemConfig networkequippedItemConfig = equippedItemConfig;
			networkequippedItemConfig.AddUsages(usages);
			NetworkequippedItemConfig = networkequippedItemConfig;
			RpcAddUsages(usages);
		}

		protected static void InvokeUserCode_CmdAddUsages__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdAddUsages called on client.");
			}
			else
			{
				((PickupsDropsChilds)obj).UserCode_CmdAddUsages__Byte(NetworkReaderExtensions.ReadByte(reader));
			}
		}

		protected void UserCode_CmdResetUsages()
		{
			EquippedItemConfig networkequippedItemConfig = equippedItemConfig;
			networkequippedItemConfig.ResetUsages();
			NetworkequippedItemConfig = networkequippedItemConfig;
			RpcResetUsages();
		}

		protected static void InvokeUserCode_CmdResetUsages(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdResetUsages called on client.");
			}
			else
			{
				((PickupsDropsChilds)obj).UserCode_CmdResetUsages();
			}
		}

		protected void UserCode_CmdResetUsages__Byte(byte usages)
		{
			EquippedItemConfig networkequippedItemConfig = equippedItemConfig;
			networkequippedItemConfig.ResetUsages(usages);
			NetworkequippedItemConfig = networkequippedItemConfig;
			RpcResetUsages(usages);
		}

		protected static void InvokeUserCode_CmdResetUsages__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdResetUsages called on client.");
			}
			else
			{
				((PickupsDropsChilds)obj).UserCode_CmdResetUsages__Byte(NetworkReaderExtensions.ReadByte(reader));
			}
		}

		protected void UserCode_CmdDropItem()
		{
			Vector3 position = rightHand.transform.position;
			Quaternion rotation = rightHand.transform.rotation;
			equippedObject = UnityEngine.Object.Instantiate(sceneObjectPrefab, position, rotation);
			equippedObject.GetComponent<Rigidbody>().isKinematic = false;
			SceneObject component = equippedObject.GetComponent<SceneObject>();
			component.NetworkequippedItem = equippedItem;
			component.NetworkequippedItemConfig = equippedItemConfig;
			component.direction = rightHand.transform.forward;
			NetworkServer.Spawn(equippedObject);
			NetworkequippedItem = EquippedItem.nothing;
			NetworkequippedItemConfig = default(EquippedItemConfig);
		}

		protected static void InvokeUserCode_CmdDropItem(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdDropItem called on client.");
			}
			else
			{
				((PickupsDropsChilds)obj).UserCode_CmdDropItem();
			}
		}

		protected void UserCode_CmdPickupItem__GameObject(GameObject obj)
		{
			if (obj.TryGetComponent<SceneObject>(out var component))
			{
				NetworkequippedItem = component.equippedItem;
				NetworkequippedItemConfig = component.equippedItemConfig;
			}
			NetworkServer.Destroy(obj);
		}

		protected static void InvokeUserCode_CmdPickupItem__GameObject(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPickupItem called on client.");
			}
			else
			{
				((PickupsDropsChilds)obj).UserCode_CmdPickupItem__GameObject(reader.ReadGameObject());
			}
		}

		protected void UserCode_RpcUseItem()
		{
			iEquipped?.Use();
		}

		protected static void InvokeUserCode_RpcUseItem(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcUseItem called on server.");
			}
			else
			{
				((PickupsDropsChilds)obj).UserCode_RpcUseItem();
			}
		}

		protected void UserCode_RpcAddUsages__Byte(byte usages)
		{
			iEquipped?.AddUsages(usages);
		}

		protected static void InvokeUserCode_RpcAddUsages__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcAddUsages called on server.");
			}
			else
			{
				((PickupsDropsChilds)obj).UserCode_RpcAddUsages__Byte(NetworkReaderExtensions.ReadByte(reader));
			}
		}

		protected void UserCode_RpcResetUsages()
		{
			iEquipped?.ResetUsages();
		}

		protected static void InvokeUserCode_RpcResetUsages(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcResetUsages called on server.");
			}
			else
			{
				((PickupsDropsChilds)obj).UserCode_RpcResetUsages();
			}
		}

		protected void UserCode_RpcResetUsages__Byte(byte usages)
		{
			iEquipped?.ResetUsages(usages);
		}

		protected static void InvokeUserCode_RpcResetUsages__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcResetUsages called on server.");
			}
			else
			{
				((PickupsDropsChilds)obj).UserCode_RpcResetUsages__Byte(NetworkReaderExtensions.ReadByte(reader));
			}
		}

		static PickupsDropsChilds()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PickupsDropsChilds), "System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdChangeEquippedItem(Mirror.Examples.PickupsDropsChilds.EquippedItem)", InvokeUserCode_CmdChangeEquippedItem__EquippedItem, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PickupsDropsChilds), "System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdUseItem()", InvokeUserCode_CmdUseItem, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PickupsDropsChilds), "System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdAddUsages(System.Byte)", InvokeUserCode_CmdAddUsages__Byte, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PickupsDropsChilds), "System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdResetUsages()", InvokeUserCode_CmdResetUsages, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PickupsDropsChilds), "System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdResetUsages(System.Byte)", InvokeUserCode_CmdResetUsages__Byte, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PickupsDropsChilds), "System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdDropItem()", InvokeUserCode_CmdDropItem, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PickupsDropsChilds), "System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::CmdPickupItem(UnityEngine.GameObject)", InvokeUserCode_CmdPickupItem__GameObject, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(PickupsDropsChilds), "System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::RpcUseItem()", InvokeUserCode_RpcUseItem);
			RemoteProcedureCalls.RegisterRpc(typeof(PickupsDropsChilds), "System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::RpcAddUsages(System.Byte)", InvokeUserCode_RpcAddUsages__Byte);
			RemoteProcedureCalls.RegisterRpc(typeof(PickupsDropsChilds), "System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::RpcResetUsages()", InvokeUserCode_RpcResetUsages);
			RemoteProcedureCalls.RegisterRpc(typeof(PickupsDropsChilds), "System.Void Mirror.Examples.PickupsDropsChilds.PickupsDropsChilds::RpcResetUsages(System.Byte)", InvokeUserCode_RpcResetUsages__Byte);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				GeneratedNetworkCode._Write_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItemConfig(writer, equippedItemConfig);
				GeneratedNetworkCode._Write_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItem(writer, equippedItem);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				GeneratedNetworkCode._Write_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItemConfig(writer, equippedItemConfig);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				GeneratedNetworkCode._Write_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItem(writer, equippedItem);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref equippedItemConfig, _Mirror_SyncVarHookDelegate_equippedItemConfig, GeneratedNetworkCode._Read_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItemConfig(reader));
				GeneratedSyncVarDeserialize(ref equippedItem, _Mirror_SyncVarHookDelegate_equippedItem, GeneratedNetworkCode._Read_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItem(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref equippedItemConfig, _Mirror_SyncVarHookDelegate_equippedItemConfig, GeneratedNetworkCode._Read_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItemConfig(reader));
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref equippedItem, _Mirror_SyncVarHookDelegate_equippedItem, GeneratedNetworkCode._Read_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItem(reader));
			}
		}
	}
}
