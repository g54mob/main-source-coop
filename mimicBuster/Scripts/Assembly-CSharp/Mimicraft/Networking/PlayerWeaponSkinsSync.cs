using System;
using System.Collections.Generic;
using Mimicraft.Customization;
using Mimicraft.Gameplay;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerWeaponSkinsSync : NetworkBehaviour
	{
		[Tooltip("Silah modellerini uygulayan bileşen. Boş bırakılırsa bu objenin altında aranır.")]
		[SerializeField]
		private PlayerWeaponSkins skins;

		private const int MaxPayloadBytes = 262144;

		private const double ServerMinSubmitIntervalSeconds = 2.0;

		private readonly NetworkVariable<VoxelModelPayload> loadout = new NetworkVariable<VoxelModelPayload>();

		private double nextAcceptedSubmitServerTime;

		public byte[] LoadoutBytes => loadout.Value.Data ?? Array.Empty<byte>();

		private void Awake()
		{
			if (skins == null)
			{
				skins = GetComponentInChildren<PlayerWeaponSkins>(includeInactive: true);
			}
		}

		public override void OnNetworkSpawn()
		{
			NetworkVariable<VoxelModelPayload> networkVariable = loadout;
			networkVariable.OnValueChanged = (NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate)Delegate.Combine(networkVariable.OnValueChanged, new NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate(OnLoadoutChanged));
			Apply(loadout.Value);
			if (base.IsOwner)
			{
				SubmitLocalLoadout();
			}
		}

		public override void OnNetworkDespawn()
		{
			NetworkVariable<VoxelModelPayload> networkVariable = loadout;
			networkVariable.OnValueChanged = (NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate)Delegate.Remove(networkVariable.OnValueChanged, new NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate(OnLoadoutChanged));
		}

		public void SubmitLocalLoadout()
		{
			if (base.IsOwner)
			{
				Dictionary<string, WeaponSkinData> dictionary = WeaponSkinStorage.LoadAll();
				byte[] array = ((dictionary == null || dictionary.Count == 0) ? Array.Empty<byte>() : WeaponSkinFile.Encode(dictionary));
				if (array.Length > 262144)
				{
					Debug.LogWarning($"[PlayerWeaponSkinsSync] Loadout {array.Length} bayt - " + $"{262144} sinirini asiyor, gonderilmedi. Silah modellerini kucult.", this);
					return;
				}
				Debug.Log($"[PlayerWeaponSkinsSync] Gonderilen loadout: {dictionary?.Count ?? 0} silah, " + $"{array.Length} bayt. (owner {base.OwnerClientId})", this);
				SubmitServerRpc(new VoxelModelPayload
				{
					Data = array
				});
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Owner)]
		private void SubmitServerRpc(VoxelModelPayload payload)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Owner
				};
				RpcParams rpcParams = default(RpcParams);
				FastBufferWriter bufferWriter = __beginSendRpc(995970231u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in payload, default(FastBufferWriter.ForNetworkSerializable));
				__endSendRpc(ref bufferWriter, 995970231u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			byte[] array = payload.Data ?? Array.Empty<byte>();
			bool flag = array.Length == 0;
			if ((flag || array.Length <= 262144) && (flag || !(base.NetworkManager.ServerTime.Time < nextAcceptedSubmitServerTime)))
			{
				if (flag)
				{
					loadout.Value = new VoxelModelPayload
					{
						Data = Array.Empty<byte>()
					};
					return;
				}
				if (!WeaponSkinFile.TryDecode(array, out var dictionary))
				{
					Debug.LogWarning($"[PlayerWeaponSkinsSync] Client {base.OwnerClientId} okunamayan bir loadout " + "gonderdi - yok sayildi.");
					return;
				}
				nextAcceptedSubmitServerTime = base.NetworkManager.ServerTime.Time + 2.0;
				byte[] array2 = WeaponSkinFile.Encode(dictionary);
				Debug.Log($"[PlayerWeaponSkinsSync] Kabul edildi: {dictionary.Count} silah, " + $"{array2.Length} bayt yayinlaniyor. (owner {base.OwnerClientId})", this);
				loadout.Value = new VoxelModelPayload
				{
					Data = array2
				};
			}
		}

		private void OnLoadoutChanged(VoxelModelPayload previous, VoxelModelPayload current)
		{
			Apply(current);
		}

		private void Apply(VoxelModelPayload payload)
		{
			if (!(skins == null))
			{
				byte[] array = payload.Data ?? Array.Empty<byte>();
				if (array.Length == 0)
				{
					skins.SetSkins(new Dictionary<string, WeaponSkinData>());
					return;
				}
				if (!WeaponSkinFile.TryDecode(array, out var dictionary))
				{
					Debug.LogWarning($"[PlayerWeaponSkinsSync] {array.Length} baytlik loadout cozulemedi - " + $"bu oyuncu fabrika silahlariyla gorunecek. (owner {base.OwnerClientId})", this);
					return;
				}
				Debug.Log($"[PlayerWeaponSkinsSync] Uygulandi: {dictionary.Count} silah, {array.Length} bayt. " + $"(owner {base.OwnerClientId}, yerel mi: {base.IsOwner})", this);
				skins.SetSkins(dictionary);
			}
		}

		protected override void __initializeVariables()
		{
			if (loadout == null)
			{
				throw new Exception("PlayerWeaponSkinsSync.loadout cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			loadout.Initialize(this);
			__nameNetworkVariable(loadout, "loadout");
			NetworkVariableFields.Add(loadout);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(995970231u, __rpc_handler_995970231, "SubmitServerRpc", RpcInvokePermission.Owner);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_995970231(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out VoxelModelPayload value, default(FastBufferWriter.ForNetworkSerializable));
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerWeaponSkinsSync)target).SubmitServerRpc(value);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerWeaponSkinsSync";
		}
	}
}
