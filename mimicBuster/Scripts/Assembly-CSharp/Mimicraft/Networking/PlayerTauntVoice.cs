using System;
using Mimicraft.Customization;
using Mimicraft.Localization;
using Mimicraft.UI;
using Mimicraft.Voice;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerTauntVoice : NetworkBehaviour
	{
		private const double ServerMinSubmitIntervalSeconds = 5.0;

		private readonly NetworkVariable<VoxelModelPayload> voiceData = new NetworkVariable<VoxelModelPayload>();

		private readonly NetworkVariable<VoxelModelPayload> forcedVoiceData = new NetworkVariable<VoxelModelPayload>();

		private AudioClip clip;

		private AudioClip forcedClip;

		private double nextAcceptedSubmitServerTime;

		public AudioClip Clip => clip;

		public AudioClip ForcedClip => forcedClip;

		public override void OnNetworkSpawn()
		{
			NetworkVariable<VoxelModelPayload> networkVariable = voiceData;
			networkVariable.OnValueChanged = (NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate)Delegate.Combine(networkVariable.OnValueChanged, new NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate(OnVoiceChanged));
			NetworkVariable<VoxelModelPayload> networkVariable2 = forcedVoiceData;
			networkVariable2.OnValueChanged = (NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate)Delegate.Combine(networkVariable2.OnValueChanged, new NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate(OnForcedVoiceChanged));
			clip = Rebuild(voiceData.Value.Data, clip, "Taunt");
			forcedClip = Rebuild(forcedVoiceData.Value.Data, forcedClip, "ForcedTaunt");
			if (base.IsOwner)
			{
				SubmitSaved();
			}
		}

		public override void OnNetworkDespawn()
		{
			NetworkVariable<VoxelModelPayload> networkVariable = voiceData;
			networkVariable.OnValueChanged = (NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate)Delegate.Remove(networkVariable.OnValueChanged, new NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate(OnVoiceChanged));
			NetworkVariable<VoxelModelPayload> networkVariable2 = forcedVoiceData;
			networkVariable2.OnValueChanged = (NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate)Delegate.Remove(networkVariable2.OnValueChanged, new NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate(OnForcedVoiceChanged));
			Discard(ref clip);
			Discard(ref forcedClip);
		}

		private void OnVoiceChanged(VoxelModelPayload previous, VoxelModelPayload current)
		{
			clip = Rebuild(current.Data, clip, "Taunt");
		}

		private void OnForcedVoiceChanged(VoxelModelPayload previous, VoxelModelPayload current)
		{
			forcedClip = Rebuild(current.Data, forcedClip, "ForcedTaunt");
		}

		private AudioClip Rebuild(byte[] data, AudioClip previous, string label)
		{
			Discard(ref previous);
			if (data == null || data.Length == 0)
			{
				return null;
			}
			if (base.NetworkManager == null || !base.NetworkManager.IsClient)
			{
				return null;
			}
			if (!TauntVoiceClip.TryDecode(data, out var samples, out var _))
			{
				return null;
			}
			return TauntVoiceClip.ToClip(samples, $"{label}{base.OwnerClientId}");
		}

		private static void Discard(ref AudioClip target)
		{
			if (!(target == null))
			{
				UnityEngine.Object.Destroy(target);
				target = null;
			}
		}

		public void SubmitSaved()
		{
			if (base.IsOwner)
			{
				SubmitServerRpc(new VoxelModelPayload
				{
					Data = TauntVoiceLibrary.SelectedBytes()
				}, new VoxelModelPayload
				{
					Data = TauntVoiceLibrary.ForcedSelectedBytes()
				});
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Owner)]
		private void SubmitServerRpc(VoxelModelPayload voluntary, VoxelModelPayload forced)
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
				FastBufferWriter bufferWriter = __beginSendRpc(3354858039u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in voluntary, default(FastBufferWriter.ForNetworkSerializable));
				bufferWriter.WriteValueSafe(in forced, default(FastBufferWriter.ForNetworkSerializable));
				__endSendRpc(ref bufferWriter, 3354858039u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			bool flag = !IsEmpty(voluntary) || !IsEmpty(forced);
			if (flag && base.NetworkManager.ServerTime.Time < nextAcceptedSubmitServerTime)
			{
				Reject("Reject.TauntTooOften");
				return;
			}
			if (!IsEmpty(voluntary) && !TauntVoiceClip.Accepts(voluntary.Data, out var rejection))
			{
				Reject(rejection);
				return;
			}
			if (!IsEmpty(forced) && !TauntVoiceClip.Accepts(forced.Data, out var rejection2))
			{
				Reject(rejection2);
				return;
			}
			if (flag)
			{
				nextAcceptedSubmitServerTime = base.NetworkManager.ServerTime.Time + 5.0;
			}
			voiceData.Value = Normalised(voluntary);
			forcedVoiceData.Value = Normalised(forced);
		}

		private static bool IsEmpty(VoxelModelPayload payload)
		{
			if (payload.Data != null)
			{
				return payload.Data.Length == 0;
			}
			return true;
		}

		private static VoxelModelPayload Normalised(VoxelModelPayload payload)
		{
			if (!IsEmpty(payload))
			{
				return payload;
			}
			return new VoxelModelPayload
			{
				Data = Array.Empty<byte>()
			};
		}

		private void Reject(string reasonKey)
		{
			Debug.LogWarning($"[PlayerTauntVoice] Taunt kaydi reddedildi (owner {base.OwnerClientId}): {reasonKey}");
			RejectClientRpc(reasonKey, base.RpcTarget.Single(base.OwnerClientId, RpcTargetUse.Temp));
		}

		[Rpc(SendTo.SpecifiedInParams)]
		private void RejectClientRpc(string reasonKey, RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(2403820235u, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
				bool value = reasonKey != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(reasonKey);
				}
				__endSendRpc(ref bufferWriter, 2403820235u, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(Loc.Get(reasonKey));
				}
			}
		}

		protected override void __initializeVariables()
		{
			if (voiceData == null)
			{
				throw new Exception("PlayerTauntVoice.voiceData cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			voiceData.Initialize(this);
			__nameNetworkVariable(voiceData, "voiceData");
			NetworkVariableFields.Add(voiceData);
			if (forcedVoiceData == null)
			{
				throw new Exception("PlayerTauntVoice.forcedVoiceData cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			forcedVoiceData.Initialize(this);
			__nameNetworkVariable(forcedVoiceData, "forcedVoiceData");
			NetworkVariableFields.Add(forcedVoiceData);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(3354858039u, __rpc_handler_3354858039, "SubmitServerRpc", RpcInvokePermission.Owner);
			__registerRpc(2403820235u, __rpc_handler_2403820235, "RejectClientRpc", RpcInvokePermission.Everyone);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_3354858039(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out VoxelModelPayload value, default(FastBufferWriter.ForNetworkSerializable));
				reader.ReadValueSafe(out VoxelModelPayload value2, default(FastBufferWriter.ForNetworkSerializable));
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerTauntVoice)target).SubmitServerRpc(value, value2);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_2403820235(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
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
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerTauntVoice)target).RejectClientRpc(s, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerTauntVoice";
		}
	}
}
