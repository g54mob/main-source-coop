using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Mirror.Examples.PickupsDropsChilds
{
	[RequireComponent(typeof(Rigidbody))]
	public class SceneObject : NetworkBehaviour
	{
		[Header("Prefabs")]
		public GameObject ballPrefab;

		public GameObject batPrefab;

		public GameObject boxPrefab;

		[Header("Settings")]
		[Range(0f, 5f)]
		public float force = 1f;

		[Header("SyncVars in Specific Order")]
		[SyncVar(hook = "OnChangeEquippedItemConfig")]
		public EquippedItemConfig equippedItemConfig;

		[SyncVar(hook = "OnChangeEquipment")]
		public EquippedItem equippedItem;

		[Header("Diagnostics")]
		[ReadOnly]
		public GameObject equippedObject;

		[ReadOnly]
		public Vector3 direction;

		[ReadOnly]
		[SerializeField]
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

		protected override void OnValidate()
		{
			if (!Application.isPlaying)
			{
				base.OnValidate();
				if (TryGetComponent<Rigidbody>(out var component))
				{
					component.isKinematic = true;
				}
				if (TryGetComponent<NetworkTransformBase>(out var component2))
				{
					component2.syncDirection = SyncDirection.ServerToClient;
				}
			}
		}

		public override void OnStartServer()
		{
			if (TryGetComponent<Rigidbody>(out var component))
			{
				component.isKinematic = false;
				component.AddForce(direction * force, ForceMode.Impulse);
			}
		}

		private void OnMouseDown()
		{
			NetworkClient.localPlayer.GetComponent<PickupsDropsChilds>().CmdPickupItem(base.gameObject);
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
			while (base.transform.childCount > 0)
			{
				UnityEngine.Object.Destroy(base.transform.GetChild(0).gameObject);
				yield return null;
			}
			equippedObject = null;
			switch (equippedItem)
			{
			case EquippedItem.ball:
				equippedObject = UnityEngine.Object.Instantiate(ballPrefab, base.transform);
				break;
			case EquippedItem.bat:
				equippedObject = UnityEngine.Object.Instantiate(batPrefab, base.transform);
				break;
			case EquippedItem.box:
				equippedObject = UnityEngine.Object.Instantiate(boxPrefab, base.transform);
				break;
			}
			if (equippedObject != null && equippedObject.TryGetComponent<IEquipped>(out iEquipped) && !iEquipped.equippedItemConfig.Equals(equippedItemConfig))
			{
				iEquipped.equippedItemConfig = equippedItemConfig;
			}
		}

		public SceneObject()
		{
			_Mirror_SyncVarHookDelegate_equippedItemConfig = OnChangeEquippedItemConfig;
			_Mirror_SyncVarHookDelegate_equippedItem = OnChangeEquipment;
		}

		public override bool Weaved()
		{
			return true;
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
