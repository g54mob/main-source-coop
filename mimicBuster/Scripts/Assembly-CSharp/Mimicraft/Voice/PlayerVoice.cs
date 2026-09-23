using System;
using System.Collections.Generic;
using System.Text;
using Mimicraft.Gameplay;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.UI;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Voice
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerVoice : NetworkBehaviour
	{
		private struct Budget
		{
			public float WindowStart;

			public int Frames;
		}

		private const float AudibleMargin = 6f;

		private const float MicrophoneLingerSeconds = 3f;

		private const int FramesPerListenerPerSecond = 400;

		private VoiceMicrophone microphone;

		private IVoiceCodec encoder;

		private float[] frame;

		private byte[] encoded;

		private ushort sequence;

		private VoiceChannel held;

		private float nextMicrophoneAttempt;

		private float microphoneIdleAt;

		private VoiceJitterBuffer buffer;

		private AudioSource source;

		private AudioClip stream;

		private VoiceReach appliedReach;

		private bool reachApplied;

		private bool playing;

		private int appliedGainVersion = -1;

		private float appliedVoiceVolume = -1f;

		private static readonly Dictionary<ulong, Budget> budgets = new Dictionary<ulong, Budget>();

		private readonly List<ulong> listeners = new List<ulong>();

		private int sent;

		private int received;

		private static int refused;

		public override void OnNetworkSpawn()
		{
			source = base.gameObject.AddComponent<AudioSource>();
			source.playOnAwake = false;
			source.loop = true;
			if (base.IsOwner)
			{
				StartCapturing();
			}
		}

		public override void OnNetworkDespawn()
		{
			StopCapturing();
			StopListening();
			VoiceSpeakers.Clear(base.OwnerClientId);
			VoiceMuteStore.Forget(base.OwnerClientId);
			if (base.IsServer)
			{
				budgets.Remove(base.OwnerClientId);
			}
		}

		private void StartCapturing()
		{
			microphone = new VoiceMicrophone();
			encoder = new OpusVoiceCodec(encode: true, decode: false);
			frame = new float[960];
			encoded = new byte[512];
		}

		private void StopCapturing()
		{
			microphone?.Dispose();
			microphone = null;
			encoder?.Dispose();
			encoder = null;
			held = VoiceChannel.None;
		}

		private void Update()
		{
			if (base.IsSpawned)
			{
				if (base.IsOwner)
				{
					Talk();
				}
				Listen();
			}
		}

		private void Talk()
		{
			VoiceChannel voiceChannel = WantedChannel();
			if (voiceChannel == VoiceChannel.None)
			{
				if (held != VoiceChannel.None)
				{
					held = VoiceChannel.None;
					microphoneIdleAt = Time.unscaledTime + 3f;
					VoiceSpeakers.Clear(base.OwnerClientId);
				}
				Linger();
				return;
			}
			if (!microphone.Active)
			{
				if (Time.unscaledTime < nextMicrophoneAttempt)
				{
					return;
				}
				nextMicrophoneAttempt = Time.unscaledTime + 1f;
				if (!microphone.Start(GameSettings.MicDevice))
				{
					held = VoiceChannel.None;
					return;
				}
			}
			else if (microphone.Device != ResolvedDevice())
			{
				microphone.Start(GameSettings.MicDevice);
			}
			if (held != voiceChannel)
			{
				held = voiceChannel;
				VoiceSpeakers.Set(base.OwnerClientId, voiceChannel, VoiceReach.Lobby);
			}
			VoiceSpeakers.SetLevel(base.OwnerClientId, microphone.Level);
			microphone.Gain = GameSettings.MicVolume;
			bool audible;
			while (microphone.TryReadFrame(frame, out audible))
			{
				if (audible)
				{
					int num = encoder.Encode(frame, 0, encoded);
					if (num > 0)
					{
						byte[] array = new byte[num];
						Buffer.BlockCopy(encoded, 0, array, 0, num);
						SubmitVoiceRpc((byte)voiceChannel, sequence++, array);
						sent++;
					}
				}
			}
		}

		private void Linger()
		{
			if (microphone == null || !microphone.Active)
			{
				return;
			}
			if (Time.unscaledTime >= microphoneIdleAt)
			{
				microphone.Stop();
			}
			else
			{
				bool audible;
				while (microphone.TryReadFrame(frame, out audible))
				{
				}
			}
		}

		private static string ResolvedDevice()
		{
			string[] devices = Microphone.devices;
			if (devices == null || devices.Length == 0)
			{
				return null;
			}
			string micDevice = GameSettings.MicDevice;
			if (!string.IsNullOrEmpty(micDevice) && Array.IndexOf(devices, micDevice) >= 0)
			{
				return micDevice;
			}
			return devices[0];
		}

		private static VoiceChannel WantedChannel()
		{
			if (!GameSettings.VoiceEnabled)
			{
				return VoiceChannel.None;
			}
			if (ChatView.Instance != null && ChatView.Instance.IsTyping)
			{
				return VoiceChannel.None;
			}
			if (GameSettings.MuteWhenUnfocused && !Application.isFocused)
			{
				return VoiceChannel.None;
			}
			if (GameInput.VoiceTeam.IsPressed())
			{
				return VoiceChannel.Team;
			}
			if (!GameInput.VoiceAll.IsPressed())
			{
				return VoiceChannel.None;
			}
			return VoiceChannel.All;
		}

		[Rpc(SendTo.Server, Delivery = RpcDelivery.Unreliable, InvokePermission = RpcInvokePermission.Owner)]
		private void SubmitVoiceRpc(byte channel, ushort packet, byte[] payload, RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcParams rpcParams2 = rpcParams;
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					Delivery = RpcDelivery.Unreliable,
					InvokePermission = RpcInvokePermission.Owner
				};
				FastBufferWriter bufferWriter = __beginSendRpc(3319944203u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Unreliable);
				bufferWriter.WriteValueSafe(in channel, default(FastBufferWriter.ForPrimitives));
				BytePacker.WriteValueBitPacked(bufferWriter, packet);
				bool value = payload != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(payload, default(FastBufferWriter.ForPrimitives));
				}
				__endSendRpc(ref bufferWriter, 3319944203u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Unreliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (rpcParams.Receive.SenderClientId == base.OwnerClientId && payload != null && payload.Length != 0 && payload.Length <= 512 && (channel == 1 || channel == 2))
				{
					Route((VoiceChannel)channel, packet, payload);
				}
			}
		}

		private void Route(VoiceChannel channel, ushort packet, byte[] payload)
		{
			GameModeController current = GameModeController.Current;
			if (!(current == null))
			{
				Vector3 position = base.transform.position;
				Forward(current, channel, packet, payload, position, VoiceReach.Lobby);
				Forward(current, channel, packet, payload, position, VoiceReach.Proximity);
			}
		}

		private void Forward(GameModeController mode, VoiceChannel channel, ushort packet, byte[] payload, Vector3 from, VoiceReach reach)
		{
			listeners.Clear();
			float num = 51f * 51f;
			foreach (NetworkClient connectedClients in base.NetworkManager.ConnectedClientsList)
			{
				ulong clientId = connectedClients.ClientId;
				if (clientId != base.OwnerClientId)
				{
					VoicePolicy voicePolicy = mode.VoicePolicyFor(base.OwnerClientId, clientId, channel);
					if (voicePolicy.Allowed && voicePolicy.Reach == reach && (reach != VoiceReach.Proximity || (!(connectedClients.PlayerObject == null) && !((connectedClients.PlayerObject.transform.position - from).sqrMagnitude > num))) && Afford(clientId))
					{
						listeners.Add(clientId);
					}
				}
			}
			if (listeners.Count != 0)
			{
				PlayVoiceRpc((byte)channel, (byte)reach, packet, payload, base.RpcTarget.Group(listeners, RpcTargetUse.Temp));
			}
		}

		private static bool Afford(ulong listener)
		{
			budgets.TryGetValue(listener, out var value);
			float unscaledTime = Time.unscaledTime;
			if (unscaledTime - value.WindowStart >= 1f)
			{
				value.WindowStart = unscaledTime;
				value.Frames = 0;
			}
			if (value.Frames >= 400)
			{
				budgets[listener] = value;
				refused++;
				return false;
			}
			value.Frames++;
			budgets[listener] = value;
			return true;
		}

		[Rpc(SendTo.SpecifiedInParams, Delivery = RpcDelivery.Unreliable)]
		private void PlayVoiceRpc(byte channel, byte reach, ushort packet, byte[] payload, RpcParams rpcParams = default(RpcParams))
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
					Delivery = RpcDelivery.Unreliable
				};
				FastBufferWriter bufferWriter = __beginSendRpc(2720325912u, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Unreliable);
				bufferWriter.WriteValueSafe(in channel, default(FastBufferWriter.ForPrimitives));
				bufferWriter.WriteValueSafe(in reach, default(FastBufferWriter.ForPrimitives));
				BytePacker.WriteValueBitPacked(bufferWriter, packet);
				bool value = payload != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(payload, default(FastBufferWriter.ForPrimitives));
				}
				__endSendRpc(ref bufferWriter, 2720325912u, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Unreliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (payload != null && payload.Length != 0 && payload.Length <= 512)
			{
				VoiceReach voiceReach = (VoiceReach)reach;
				if (buffer == null)
				{
					buffer = new VoiceJitterBuffer(new OpusVoiceCodec(encode: false, decode: true));
				}
				if (!reachApplied || appliedReach != voiceReach)
				{
					appliedReach = voiceReach;
					reachApplied = true;
					ApplyReach(voiceReach);
				}
				received++;
				ApplyGain();
				buffer.Push(packet, payload, 0, payload.Length);
				VoiceSpeakers.Set(base.OwnerClientId, (VoiceChannel)channel, voiceReach);
				if (!playing)
				{
					StartPlaying();
				}
			}
		}

		private void ApplyReach(VoiceReach reach)
		{
			if (reach == VoiceReach.Proximity)
			{
				SpatialAudio.Apply(source);
				return;
			}
			source.spatialBlend = 0f;
			source.rolloffMode = AudioRolloffMode.Linear;
			source.minDistance = 1f;
			source.maxDistance = 500f;
		}

		private void StartPlaying()
		{
			if ((object)stream == null)
			{
				stream = AudioClip.Create($"Voice{base.OwnerClientId}", 48000, 1, 48000, stream: true, ReadSamples);
			}
			source.clip = stream;
			source.Play();
			playing = true;
		}

		private void PauseListening()
		{
			if (source != null && playing)
			{
				source.Stop();
			}
			playing = false;
			buffer?.Reset();
		}

		private void StopListening()
		{
			PauseListening();
			buffer?.Dispose();
			buffer = null;
			if (stream != null)
			{
				UnityEngine.Object.Destroy(stream);
				stream = null;
			}
		}

		private void ApplyGain()
		{
			if (!(source == null) && (appliedGainVersion != VoiceMuteStore.Version || !Mathf.Approximately(appliedVoiceVolume, GameSettings.VoiceVolume)))
			{
				appliedGainVersion = VoiceMuteStore.Version;
				appliedVoiceVolume = GameSettings.VoiceVolume;
				source.volume = VoiceMuteStore.GainFor(base.OwnerClientId) * appliedVoiceVolume;
			}
		}

		private void ReadSamples(float[] data)
		{
			int num = (buffer?.Samples)?.Read(data, 0, data.Length) ?? 0;
			if (num < data.Length)
			{
				Array.Clear(data, num, data.Length - num);
			}
		}

		public static string Report()
		{
			PlayerVoice[] array = UnityEngine.Object.FindObjectsByType<PlayerVoice>(FindObjectsSortMode.None);
			if (array.Length == 0)
			{
				return "Sahnede PlayerVoice yok - oyuncu spawn olmamis.";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"kodek=Opus {48000}Hz/{20}ms  " + $"butce-reddi={refused}");
			string[] devices = Microphone.devices;
			stringBuilder.AppendLine((devices == null || devices.Length == 0) ? "mikrofon cihazi: YOK" : $"mikrofon cihazi: {devices.Length} adet, ilki={devices[0]}");
			AudioListener audioListener = UnityEngine.Object.FindFirstObjectByType<AudioListener>();
			stringBuilder.AppendLine((audioListener == null) ? "AudioListener: YOK - sahnede hicbir ses duyulmaz" : ("AudioListener: " + audioListener.name + "  (aktif=" + (audioListener.isActiveAndEnabled ? "evet" : "hayir") + ")  " + $"genel ses={AudioListener.volume:0.00}  duraklatildi=" + (AudioListener.pause ? "evet" : "hayir")));
			PlayerVoice[] array2 = array;
			foreach (PlayerVoice playerVoice in array2)
			{
				stringBuilder.Append(string.Format("#{0}{1}  ", playerVoice.OwnerClientId, playerVoice.IsOwner ? " (ben)" : ""));
				if (playerVoice.IsOwner)
				{
					VoiceMicrophone voiceMicrophone = playerVoice.microphone;
					stringBuilder.Append((voiceMicrophone == null || !voiceMicrophone.Active) ? "mikrofon=kapali  " : (string.Format("mikrofon={0}@{1}Hz  ", voiceMicrophone.Device ?? "varsayilan", voiceMicrophone.DeviceRate) + $"cekilen={voiceMicrophone.Pumped}  bekleyen={voiceMicrophone.Buffered}  " + string.Format("seviye={0:0.00}  kapi={1}  ", voiceMicrophone.Level, voiceMicrophone.Open ? "acik" : "kapali")));
					stringBuilder.Append($"tus={playerVoice.held}  gonderilen={playerVoice.sent}  ");
				}
				VoiceJitterBuffer voiceJitterBuffer = playerVoice.buffer;
				stringBuilder.Append((voiceJitterBuffer == null) ? "gelen=yok" : (string.Format("gelen={0}  canli={1}  ", playerVoice.received, voiceJitterBuffer.Live ? "evet" : "hayir") + $"bekleyen={voiceJitterBuffer.Pending}  gec={voiceJitterBuffer.Late}  " + $"gizlenen={voiceJitterBuffer.Concealed}  tampon={voiceJitterBuffer.Samples.Count}  " + $"cekildi={voiceJitterBuffer.Drained}  " + string.Format("caliyor={0}  erisim={1}", playerVoice.playing ? "evet" : "hayir", playerVoice.appliedReach)));
				if (playerVoice.source != null)
				{
					stringBuilder.Append("  [unity: oynuyor=" + (playerVoice.source.isPlaying ? "evet" : "hayir") + "  " + $"ses={playerVoice.source.volume:0.00}  " + $"3d={playerVoice.source.spatialBlend:0.00}  " + "dongu=" + (playerVoice.source.loop ? "evet" : "hayir") + "  mikser=" + ((playerVoice.source.outputAudioMixerGroup != null) ? playerVoice.source.outputAudioMixerGroup.name : "yok") + "  klip=" + ((playerVoice.source.clip != null) ? playerVoice.source.clip.name : "yok") + "  sessiz=" + (playerVoice.source.mute ? "evet" : "hayir") + "]");
				}
				stringBuilder.AppendLine();
			}
			stringBuilder.Append(RouteReport());
			return stringBuilder.ToString();
		}

		private static string RouteReport()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null || !singleton.IsListening)
			{
				return "yonlendirme: ag calismiyor\n";
			}
			if (!singleton.IsServer)
			{
				return "yonlendirme: yalnizca sunucu bilir - host'ta calistir\n";
			}
			GameModeController current = GameModeController.Current;
			if (current == null)
			{
				return "yonlendirme: GameModeController.Current YOK - mod sahnesi yuklenmemis\n";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"yonlendirme  mod={current.GetType().Name}  faz={current.CurrentPhase.Value}");
			foreach (ulong connectedClientsId in singleton.ConnectedClientsIds)
			{
				foreach (ulong connectedClientsId2 in singleton.ConnectedClientsIds)
				{
					if (connectedClientsId != connectedClientsId2)
					{
						stringBuilder.AppendLine($"  #{connectedClientsId} -> #{connectedClientsId2}   " + "All=" + Describe(current, connectedClientsId, connectedClientsId2, VoiceChannel.All) + "   Team=" + Describe(current, connectedClientsId, connectedClientsId2, VoiceChannel.Team));
					}
				}
			}
			return stringBuilder.ToString();
		}

		private static string Describe(GameModeController mode, ulong speaker, ulong listener, VoiceChannel channel)
		{
			VoicePolicy voicePolicy = mode.VoicePolicyFor(speaker, listener, channel);
			if (!voicePolicy.Allowed)
			{
				return "KAPALI";
			}
			if (voicePolicy.Reach != VoiceReach.Proximity)
			{
				return "lobi";
			}
			return "yakinlik";
		}

		private void Listen()
		{
			if (buffer != null)
			{
				buffer.Pump();
				if (buffer.Live)
				{
					VoiceSpeakers.SetLevel(base.OwnerClientId, buffer.Level);
					ApplyGain();
				}
				else if (playing)
				{
					PauseListening();
					VoiceSpeakers.Clear(base.OwnerClientId);
				}
			}
		}

		protected override void __initializeVariables()
		{
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(3319944203u, __rpc_handler_3319944203, "SubmitVoiceRpc", RpcInvokePermission.Owner);
			__registerRpc(2720325912u, __rpc_handler_2720325912, "PlayVoiceRpc", RpcInvokePermission.Everyone);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_3319944203(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out byte value, default(FastBufferWriter.ForPrimitives));
				ByteUnpacker.ReadValueBitPacked(reader, out ushort value2);
				reader.ReadValueSafe(out bool value3, default(FastBufferWriter.ForPrimitives));
				byte[] value4 = null;
				if (value3)
				{
					reader.ReadValueSafe(out value4, default(FastBufferWriter.ForPrimitives));
				}
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerVoice)target).SubmitVoiceRpc(value, value2, value4, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_2720325912(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out byte value, default(FastBufferWriter.ForPrimitives));
				reader.ReadValueSafe(out byte value2, default(FastBufferWriter.ForPrimitives));
				ByteUnpacker.ReadValueBitPacked(reader, out ushort value3);
				reader.ReadValueSafe(out bool value4, default(FastBufferWriter.ForPrimitives));
				byte[] value5 = null;
				if (value4)
				{
					reader.ReadValueSafe(out value5, default(FastBufferWriter.ForPrimitives));
				}
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerVoice)target).PlayVoiceRpc(value, value2, value3, value5, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerVoice";
		}
	}
}
