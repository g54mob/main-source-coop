using System;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.UI;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerTaunt : NetworkBehaviour
	{
		private const int SampleRate = 44100;

		private const float ToneSeconds = 0.18f;

		private const float FirstToneHz = 660f;

		private const float SecondToneHz = 440f;

		[SerializeField]
		private AudioSource source;

		private const double ManualTauntCooldownSeconds = 3.0;

		[Tooltip("Modelcinin boyutunun sesinin menziline etkisi. 0 = boyut menzili hiç etkilemez. 1 = menzil boyutla birebir ölçeklenir (iki katı büyük bir model iki katı uzaktan duyulur). Aradaki değerler etkiyi yumuşatır.")]
		[SerializeField]
		[Min(0f)]
		private float sizeLoudnessInfluence = 1f;

		private PlayerVoxelBody voxelBody;

		private static AudioClip chirp;

		private RoundManager roundManager;

		private double nextAllowedTauntServerTime;

		private double nextAllowedTauntLocalTime;

		private float scheduledAt;

		private bool scheduledForced;

		private PlayerTauntVoice voice;

		public void SetSource(AudioSource source)
		{
			this.source = source;
		}

		private RoundManager ResolveRoundManager()
		{
			if (!(roundManager != null))
			{
				return roundManager = GameModeController.Current as RoundManager;
			}
			return roundManager;
		}

		private void Update()
		{
			TickScheduledTaunt();
			if (base.IsOwner && !GameMenuState.IsMenuOpen && GameInput.Taunt.WasPressedThisFrame())
			{
				RoundManager roundManager = ResolveRoundManager();
				if (!(roundManager == null) && roundManager.LocalRole == PlayerRole.Hider && roundManager.CurrentPhase.Value == RoundPhase.Hunt && !(base.NetworkManager.ServerTime.Time < nextAllowedTauntLocalTime))
				{
					nextAllowedTauntLocalTime = base.NetworkManager.ServerTime.Time + 3.0;
					RequestTauntServerRpc();
				}
			}
		}

		[ServerRpc]
		private void RequestTauntServerRpc()
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				if (base.OwnerClientId != networkManager.LocalClientId)
				{
					if (networkManager.LogLevel <= LogLevel.Normal)
					{
						Debug.LogError("Only the owner can invoke a ServerRpc that requires ownership!");
					}
					return;
				}
				ServerRpcParams serverRpcParams = default(ServerRpcParams);
				FastBufferWriter bufferWriter = __beginSendServerRpc(3132491512u, serverRpcParams, RpcDelivery.Reliable);
				__endSendServerRpc(ref bufferWriter, 3132491512u, serverRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				RoundManager roundManager = ResolveRoundManager();
				if (!(roundManager == null) && roundManager.CurrentPhase.Value == RoundPhase.Hunt && roundManager.GetServerRole(base.OwnerClientId) == PlayerRole.Hider && !(base.NetworkManager.ServerTime.Time < nextAllowedTauntServerTime))
				{
					nextAllowedTauntServerTime = base.NetworkManager.ServerTime.Time + 3.0;
					PlayTauntClientRpc();
				}
			}
		}

		[ClientRpc]
		public void PlayTauntClientRpc(bool forced = false, float delaySeconds = 0f)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(756455407u, clientRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in forced, default(FastBufferWriter.ForPrimitives));
				bufferWriter.WriteValueSafe(in delaySeconds, default(FastBufferWriter.ForPrimitives));
				__endSendClientRpc(ref bufferWriter, 756455407u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute || (!networkManager.IsClient && !networkManager.IsHost))
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (!(source == null))
			{
				if (delaySeconds > 0f)
				{
					scheduledAt = Time.unscaledTime + delaySeconds;
					scheduledForced = forced;
				}
				else
				{
					Play(forced);
				}
			}
		}

		private void TickScheduledTaunt()
		{
			if (!(scheduledAt <= 0f) && !(Time.unscaledTime < scheduledAt))
			{
				scheduledAt = 0f;
				Play(scheduledForced);
			}
		}

		private void Play(bool forced)
		{
			if (!(source == null))
			{
				SpatialAudio.Apply(source, SizeRangeMultiplier());
				source.PlayOneShot(Clip(forced));
			}
		}

		private AudioClip Clip(bool forced)
		{
			if (voice == null)
			{
				voice = GetComponent<PlayerTauntVoice>();
			}
			AudioClip audioClip = ((voice == null) ? null : (forced ? voice.ForcedClip : voice.Clip));
			if (audioClip != null)
			{
				return audioClip;
			}
			AudioLibrary instance = AudioLibrary.Instance;
			AudioClip audioClip2 = ((instance == null) ? null : (forced ? instance.forcedTauntClip : instance.tauntClip));
			if (audioClip2 != null)
			{
				return audioClip2;
			}
			return chirp ?? (chirp = BuildChirp());
		}

		private float SizeRangeMultiplier()
		{
			if (Mathf.Approximately(sizeLoudnessInfluence, 0f))
			{
				return 1f;
			}
			if (voxelBody == null)
			{
				voxelBody = GetComponent<PlayerVoxelBody>();
			}
			if (voxelBody == null)
			{
				return 1f;
			}
			return Mathf.LerpUnclamped(1f, voxelBody.BodySizeRatio, sizeLoudnessInfluence);
		}

		private static AudioClip BuildChirp()
		{
			int num = Mathf.RoundToInt(7938.0005f);
			float[] array = new float[num * 2];
			WriteTone(array, 0, num, 660f);
			WriteTone(array, num, num, 440f);
			AudioClip audioClip = AudioClip.Create("Taunt", array.Length, 1, 44100, stream: false);
			audioClip.SetData(array, 0);
			return audioClip;
		}

		private static void WriteTone(float[] samples, int offset, int count, float frequency)
		{
			int num = Mathf.Max(1, count / 8);
			for (int i = 0; i < count; i++)
			{
				float num2 = Mathf.Min(1f, (float)Mathf.Min(i, count - 1 - i) / (float)num);
				samples[offset + i] = Mathf.Sin(MathF.PI * 2f * frequency * (float)i / 44100f) * num2 * 0.6f;
			}
		}

		protected override void __initializeVariables()
		{
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(3132491512u, __rpc_handler_3132491512, "RequestTauntServerRpc", RpcInvokePermission.Owner);
			__registerRpc(756455407u, __rpc_handler_756455407, "PlayTauntClientRpc", RpcInvokePermission.Server);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_3132491512(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				return;
			}
			if (rpcParams.Server.Receive.SenderClientId != target.OwnerClientId)
			{
				if (networkManager.LogLevel <= LogLevel.Normal)
				{
					Debug.LogError("Only the owner can invoke a ServerRpc that requires ownership!");
				}
			}
			else
			{
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerTaunt)target).RequestTauntServerRpc();
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_756455407(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out bool value, default(FastBufferWriter.ForPrimitives));
				reader.ReadValueSafe(out float value2, default(FastBufferWriter.ForPrimitives));
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerTaunt)target).PlayTauntClientRpc(value, value2);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerTaunt";
		}
	}
}
