using System;
using System.IO;
using Mimicraft.Customization;
using Mimicraft.Localization;
using Mimicraft.UI;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerCharacterAppearance : NetworkBehaviour
	{
		[Tooltip("Bu oyuncunun parçalarını kemiklere takan bileşen. Boş bırakılırsa altında aranır.")]
		[SerializeField]
		private CharacterAssembler assembler;

		private const double ServerMinSubmitIntervalSeconds = 2.0;

		private readonly NetworkVariable<VoxelModelPayload> characterData = new NetworkVariable<VoxelModelPayload>();

		private readonly NetworkVariable<byte> voiceType = new NetworkVariable<byte>(0);

		private double nextAcceptedSubmitServerTime;

		private byte[] appliedBytes;

		public byte[] CharacterBytes => characterData.Value.Data ?? Array.Empty<byte>();

		public CharacterVoice VoiceType => CharacterVoices.FromByte(voiceType.Value);

		private void Awake()
		{
			if (assembler == null)
			{
				assembler = GetComponentInChildren<CharacterAssembler>(includeInactive: true);
			}
		}

		public override void OnNetworkSpawn()
		{
			NetworkVariable<VoxelModelPayload> networkVariable = characterData;
			networkVariable.OnValueChanged = (NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate)Delegate.Combine(networkVariable.OnValueChanged, new NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate(OnCharacterChanged));
			Apply(characterData.Value);
			if (base.IsOwner)
			{
				SubmitSelectedCharacter();
			}
		}

		public override void OnNetworkDespawn()
		{
			NetworkVariable<VoxelModelPayload> networkVariable = characterData;
			networkVariable.OnValueChanged = (NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate)Delegate.Remove(networkVariable.OnValueChanged, new NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate(OnCharacterChanged));
		}

		public void SubmitSelectedCharacter()
		{
			if (base.IsOwner)
			{
				string text = CharacterSelection.Resolve();
				if (string.IsNullOrEmpty(text))
				{
					SubmitServerRpc(new VoxelModelPayload
					{
						Data = Array.Empty<byte>()
					}, 0);
					return;
				}
				CharacterRigDefinition rig = ((assembler != null) ? assembler.Rig : null);
				CharacterData characterData = CharacterStorage.Load(text, rig);
				byte[] array = CharacterCodec.Encode(characterData);
				Debug.Log("[PlayerCharacterAppearance] Gonderilen karakter: '" + Path.GetFileName(text) + "', " + ((characterData == null) ? "YUKLENEMEDI" : (characterData.Parts.Count + " parca")) + ", " + $"{array.Length} bayt, ses: {characterData?.VoiceType ?? CharacterVoice.Male}.", this);
				SubmitServerRpc(new VoxelModelPayload
				{
					Data = array
				}, (byte)(characterData?.VoiceType ?? CharacterVoice.Male));
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Owner)]
		private void SubmitServerRpc(VoxelModelPayload payload, byte voice)
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
				FastBufferWriter bufferWriter = __beginSendRpc(2280984928u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in payload, default(FastBufferWriter.ForNetworkSerializable));
				bufferWriter.WriteValueSafe(in voice, default(FastBufferWriter.ForPrimitives));
				__endSendRpc(ref bufferWriter, 2280984928u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			bool flag = payload.Data == null || payload.Data.Length == 0;
			if (!flag && base.NetworkManager.ServerTime.Time < nextAcceptedSubmitServerTime)
			{
				Reject("Reject.CharacterTooOften");
				return;
			}
			CharacterRigDefinition rig = ((assembler != null) ? assembler.Rig : null);
			if (!CharacterValidator.Accepts(payload.Data, rig, out var character, out var rejection, out var notice))
			{
				Reject(rejection);
				return;
			}
			if (notice.HasValue)
			{
				Debug.LogWarning("[PlayerCharacterAppearance] " + notice.Key + " " + $"({notice.First}, {notice.Second}) (owner {base.OwnerClientId})");
				RejectClientRpc(notice.Key, notice.First, notice.Second, base.RpcTarget.Single(base.OwnerClientId, RpcTargetUse.Temp));
			}
			if (!flag)
			{
				nextAcceptedSubmitServerTime = base.NetworkManager.ServerTime.Time + 2.0;
			}
			voiceType.Value = (byte)CharacterVoices.FromByte(voice);
			characterData.Value = new VoxelModelPayload
			{
				Data = ((character == null) ? Array.Empty<byte>() : CharacterCodec.Encode(character))
			};
		}

		private void OnCharacterChanged(VoxelModelPayload previous, VoxelModelPayload current)
		{
			Apply(current);
		}

		private void Apply(VoxelModelPayload payload)
		{
			if (assembler == null)
			{
				return;
			}
			byte[] b = payload.Data ?? Array.Empty<byte>();
			if (appliedBytes != null && SameBytes(appliedBytes, b) && assembler.HasLiveParts)
			{
				return;
			}
			appliedBytes = b;
			if (payload.Data == null || payload.Data.Length == 0)
			{
				ApplyDefault();
				return;
			}
			string rigId = ((assembler.Rig != null) ? assembler.Rig.RigId : "");
			if (!CharacterCodec.TryDecode(payload.Data, rigId, out var character))
			{
				Debug.LogWarning($"[PlayerCharacterAppearance] Oyuncu {base.OwnerClientId} icin karakter " + "verisi cozulemedi - varsayilan gorunum kullanilacak.", this);
				ApplyDefault();
			}
			else
			{
				assembler.Apply(character);
			}
		}

		private static bool SameBytes(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		private void ApplyDefault()
		{
			CharacterData defaultCharacterData = assembler.DefaultCharacterData;
			if (defaultCharacterData != null)
			{
				assembler.Apply(defaultCharacterData);
			}
			else
			{
				assembler.Clear();
			}
		}

		private void Reject(string reasonKey)
		{
			Debug.LogWarning("[PlayerCharacterAppearance] Karakter reddedildi " + $"(owner {base.OwnerClientId}): {reasonKey}");
			RejectClientRpc(reasonKey, 0, 0, base.RpcTarget.Single(base.OwnerClientId, RpcTargetUse.Temp));
		}

		[Rpc(SendTo.SpecifiedInParams)]
		private void RejectClientRpc(string reasonKey, int first, int second, RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcAttribute.RpcAttributeParams attributeParams = default(RpcAttribute.RpcAttributeParams);
				FastBufferWriter bufferWriter = __beginSendRpc(3516759110u, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
				bool value = reasonKey != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(reasonKey);
				}
				BytePacker.WriteValueBitPacked(bufferWriter, first);
				BytePacker.WriteValueBitPacked(bufferWriter, second);
				__endSendRpc(ref bufferWriter, 3516759110u, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(Loc.Format(reasonKey, first, second));
				}
			}
		}

		protected override void __initializeVariables()
		{
			if (characterData == null)
			{
				throw new Exception("PlayerCharacterAppearance.characterData cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			characterData.Initialize(this);
			__nameNetworkVariable(characterData, "characterData");
			NetworkVariableFields.Add(characterData);
			if (voiceType == null)
			{
				throw new Exception("PlayerCharacterAppearance.voiceType cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			voiceType.Initialize(this);
			__nameNetworkVariable(voiceType, "voiceType");
			NetworkVariableFields.Add(voiceType);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(2280984928u, __rpc_handler_2280984928, "SubmitServerRpc", RpcInvokePermission.Owner);
			__registerRpc(3516759110u, __rpc_handler_3516759110, "RejectClientRpc", RpcInvokePermission.Everyone);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_2280984928(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out VoxelModelPayload value, default(FastBufferWriter.ForNetworkSerializable));
				reader.ReadValueSafe(out byte value2, default(FastBufferWriter.ForPrimitives));
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerCharacterAppearance)target).SubmitServerRpc(value, value2);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3516759110(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out bool value, default(FastBufferWriter.ForPrimitives));
				string s = null;
				if (value)
				{
					reader.ReadValueSafe(out s, false);
				}
				ByteUnpacker.ReadValueBitPacked(reader, out int value2);
				ByteUnpacker.ReadValueBitPacked(reader, out int value3);
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerCharacterAppearance)target).RejectClientRpc(s, value2, value3, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerCharacterAppearance";
		}
	}
}
