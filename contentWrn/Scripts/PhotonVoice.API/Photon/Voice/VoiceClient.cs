using System;
using System.Collections.Generic;

namespace Photon.Voice
{
	public class VoiceClient : IDisposable
	{
		public delegate void RemoteVoiceInfoDelegate(int channelId, int playerId, byte voiceId, VoiceInfo voiceInfo, ref RemoteVoiceOptions options);

		public struct CreateOptions
		{
			public byte VoiceIDMin;

			public byte VoiceIDMax;

			public static CreateOptions Default = new CreateOptions
			{
				VoiceIDMin = 1,
				VoiceIDMax = 15
			};
		}

		internal IVoiceTransport transport;

		internal ILogger logger;

		private int prevRtt;

		private Dictionary<Codec, int> remoteVoiceDelayFrames = new Dictionary<Codec, int>();

		private byte voiceIDMin;

		private byte voiceIDMax;

		private byte voiceIdLast;

		private Dictionary<byte, LocalVoice> localVoices = new Dictionary<byte, LocalVoice>();

		private Dictionary<int, List<LocalVoice>> localVoicesPerChannel = new Dictionary<int, List<LocalVoice>>();

		private Dictionary<int, Dictionary<byte, RemoteVoice>> remoteVoices = new Dictionary<int, Dictionary<byte, RemoteVoice>>();

		private Random rnd = new Random();

		public bool ThreadingEnabled { get; set; } = true;

		public int EventsLost { get; internal set; }

		public int FramesLost { get; internal set; }

		public int FramesFragPart { get; internal set; }

		public int FramesRecovered { get; internal set; }

		public int FramesMiss { get; internal set; }

		public int FramesLate { get; internal set; }

		public int FramesLateUsed => FramesMiss - FramesLost;

		public int FramesReceived { get; private set; }

		public int FramesReceivedFEC { get; internal set; }

		public int FramesTryFEC { get; internal set; }

		public int FramesReceivedFragments { get; internal set; }

		public int FramesReceivedFragmented { get; internal set; }

		public int FramesSent
		{
			get
			{
				int num = 0;
				foreach (KeyValuePair<byte, LocalVoice> localVoice in localVoices)
				{
					num += localVoice.Value.FramesSent;
				}
				return num;
			}
		}

		public int FramesSentBytes
		{
			get
			{
				int num = 0;
				foreach (KeyValuePair<byte, LocalVoice> localVoice in localVoices)
				{
					num += localVoice.Value.FramesSentBytes;
				}
				return num;
			}
		}

		public int RoundTripTime { get; private set; }

		public int RoundTripTimeVariance { get; private set; }

		public bool SuppressInfoDuplicateWarning { get; set; }

		public RemoteVoiceInfoDelegate OnRemoteVoiceInfoAction { get; set; }

		public int DebugLostPercent { get; set; }

		public IEnumerable<LocalVoice> LocalVoices
		{
			get
			{
				LocalVoice[] array = new LocalVoice[localVoices.Count];
				localVoices.Values.CopyTo(array, 0);
				return array;
			}
		}

		public IEnumerable<RemoteVoiceInfo> RemoteVoiceInfos
		{
			get
			{
				foreach (KeyValuePair<int, Dictionary<byte, RemoteVoice>> playerVoices in remoteVoices)
				{
					foreach (KeyValuePair<byte, RemoteVoice> item in playerVoices.Value)
					{
						yield return new RemoteVoiceInfo(item.Value.channelId, playerVoices.Key, item.Key, item.Value.Info);
					}
				}
			}
		}

		public IEnumerable<LocalVoice> LocalVoicesInChannel(int channelId)
		{
			if (localVoicesPerChannel.TryGetValue(channelId, out var value))
			{
				LocalVoice[] array = new LocalVoice[value.Count];
				value.CopyTo(array, 0);
				return array;
			}
			return new LocalVoice[0];
		}

		public void LogSpacingProfiles()
		{
			foreach (KeyValuePair<byte, LocalVoice> localVoice in localVoices)
			{
				localVoice.Value.SendSpacingProfileStart();
				logger.LogInfo(localVoice.Value.LogPrefix + " ev. prof.: " + localVoice.Value.SendSpacingProfileDump);
			}
			foreach (KeyValuePair<int, Dictionary<byte, RemoteVoice>> remoteVoice in remoteVoices)
			{
				foreach (KeyValuePair<byte, RemoteVoice> item in remoteVoice.Value)
				{
					item.Value.ReceiveSpacingProfileStart();
					logger.LogInfo(item.Value.LogPrefix + " ev. prof.: " + item.Value.ReceiveSpacingProfileDump);
				}
			}
		}

		public void LogStats()
		{
			int statDisposerCreated = FrameBuffer.statDisposerCreated;
			int statDisposerDisposed = FrameBuffer.statDisposerDisposed;
			int statPinned = FrameBuffer.statPinned;
			int statUnpinned = FrameBuffer.statUnpinned;
			logger.LogInfo("[PV] FrameBuffer stats Disposer: " + statDisposerCreated + " - " + statDisposerDisposed + " = " + (statDisposerCreated - statDisposerDisposed));
			logger.LogInfo("[PV] FrameBuffer stats Pinned: " + statPinned + " - " + statUnpinned + " = " + (statPinned - statUnpinned));
		}

		public void SetRemoteVoiceDelayFrames(Codec codec, int delayFrames)
		{
			remoteVoiceDelayFrames[codec] = delayFrames;
			foreach (KeyValuePair<int, Dictionary<byte, RemoteVoice>> remoteVoice in remoteVoices)
			{
				foreach (KeyValuePair<byte, RemoteVoice> item in remoteVoice.Value)
				{
					if (codec == item.Value.Info.Codec)
					{
						item.Value.DelayFrames = delayFrames;
					}
				}
			}
		}

		public VoiceClient(IVoiceTransport transport, ILogger logger, CreateOptions opt = default(CreateOptions))
		{
			this.transport = transport;
			this.logger = logger;
			if (opt.Equals(default(CreateOptions)))
			{
				opt = CreateOptions.Default;
			}
			voiceIDMin = opt.VoiceIDMin;
			voiceIDMax = opt.VoiceIDMax;
			voiceIdLast = voiceIDMax;
		}

		public void Service()
		{
			foreach (KeyValuePair<byte, LocalVoice> localVoice in localVoices)
			{
				localVoice.Value.service();
			}
		}

		private LocalVoice createLocalVoice(int channelId, Func<byte, int, LocalVoice> voiceFactory)
		{
			byte newVoiceId = getNewVoiceId();
			if (newVoiceId != 0)
			{
				LocalVoice localVoice = voiceFactory(newVoiceId, channelId);
				if (localVoice != null)
				{
					addVoice(newVoiceId, channelId, localVoice);
					logger.LogInfo(localVoice.LogPrefix + " added enc: " + localVoice.Info.ToString());
					return localVoice;
				}
			}
			return null;
		}

		public LocalVoice CreateLocalVoice(VoiceInfo voiceInfo, int channelId, VoiceCreateOptions options = default(VoiceCreateOptions))
		{
			return createLocalVoice(channelId, (byte vId, int chId) => new LocalVoice(this, vId, voiceInfo, channelId, options));
		}

		public LocalVoiceAudio<T> CreateLocalVoiceAudio<T>(VoiceInfo voiceInfo, IAudioDesc audioSourceDesc, int channelId, VoiceCreateOptions options = default(VoiceCreateOptions))
		{
			return (LocalVoiceAudio<T>)createLocalVoice(channelId, (byte vId, int chId) => LocalVoiceAudio<T>.Create(this, vId, voiceInfo, audioSourceDesc, channelId, options));
		}

		public LocalVoice CreateLocalVoiceAudioFromSource(VoiceInfo voiceInfo, IAudioDesc source, AudioSampleType sampleType, int channelId, VoiceCreateOptions options = default(VoiceCreateOptions))
		{
			if (sampleType == AudioSampleType.Source)
			{
				if (source is IAudioPusher<float> || source is IAudioReader<float>)
				{
					sampleType = AudioSampleType.Float;
				}
				else if (source is IAudioPusher<short> || source is IAudioReader<short>)
				{
					sampleType = AudioSampleType.Short;
				}
			}
			if (options.Encoder == null)
			{
				switch (sampleType)
				{
				case AudioSampleType.Float:
					options.Encoder = Platform.CreateDefaultAudioEncoder<float>(logger, voiceInfo);
					break;
				case AudioSampleType.Short:
					options.Encoder = Platform.CreateDefaultAudioEncoder<short>(logger, voiceInfo);
					break;
				}
			}
			if (source is IAudioPusher<float>)
			{
				if (sampleType == AudioSampleType.Short)
				{
					logger.LogInfo("[PV] Creating local voice with source samples type conversion from IAudioPusher float to short.");
					LocalVoiceAudio<short> localVoice = CreateLocalVoiceAudio<short>(voiceInfo, source, channelId, options);
					FactoryReusableArray<float> bufferFactory = new FactoryReusableArray<float>(0);
					((IAudioPusher<float>)source).SetCallback(delegate(float[] buf)
					{
						short[] array = localVoice.BufferFactory.New(buf.Length);
						AudioUtil.Convert(buf, array, buf.Length);
						localVoice.PushDataAsync(array);
					}, bufferFactory);
					return localVoice;
				}
				LocalVoiceAudio<float> localVoice2 = CreateLocalVoiceAudio<float>(voiceInfo, source, channelId, options);
				((IAudioPusher<float>)source).SetCallback(delegate(float[] buf)
				{
					localVoice2.PushDataAsync(buf);
				}, localVoice2.BufferFactory);
				return localVoice2;
			}
			if (source is IAudioPusher<short>)
			{
				if (sampleType == AudioSampleType.Float)
				{
					logger.LogInfo("[PV] Creating local voice with source samples type conversion from IAudioPusher short to float.");
					LocalVoiceAudio<float> localVoice3 = CreateLocalVoiceAudio<float>(voiceInfo, source, channelId, options);
					FactoryReusableArray<short> bufferFactory2 = new FactoryReusableArray<short>(0);
					((IAudioPusher<short>)source).SetCallback(delegate(short[] buf)
					{
						float[] array = localVoice3.BufferFactory.New(buf.Length);
						AudioUtil.Convert(buf, array, buf.Length);
						localVoice3.PushDataAsync(array);
					}, bufferFactory2);
					return localVoice3;
				}
				LocalVoiceAudio<short> localVoice4 = CreateLocalVoiceAudio<short>(voiceInfo, source, channelId, options);
				((IAudioPusher<short>)source).SetCallback(delegate(short[] buf)
				{
					localVoice4.PushDataAsync(buf);
				}, localVoice4.BufferFactory);
				return localVoice4;
			}
			if (source is IAudioReader<float>)
			{
				if (sampleType == AudioSampleType.Short)
				{
					logger.LogInfo("[PV] Creating local voice with source samples type conversion from IAudioReader float to short.");
					LocalVoiceAudio<short> localVoiceAudio = CreateLocalVoiceAudio<short>(voiceInfo, source, channelId, options);
					localVoiceAudio.LocalUserServiceable = new BufferReaderPushAdapterAsyncPoolFloatToShort(source as IAudioReader<float>);
					return localVoiceAudio;
				}
				LocalVoiceAudio<float> localVoiceAudio2 = CreateLocalVoiceAudio<float>(voiceInfo, source, channelId, options);
				localVoiceAudio2.LocalUserServiceable = new BufferReaderPushAdapterAsyncPool<float>(source as IAudioReader<float>);
				return localVoiceAudio2;
			}
			if (source is IAudioReader<short>)
			{
				if (sampleType == AudioSampleType.Float)
				{
					logger.LogInfo("[PV] Creating local voice with source samples type conversion from IAudioReader short to float.");
					LocalVoiceAudio<float> localVoiceAudio3 = CreateLocalVoiceAudio<float>(voiceInfo, source, channelId, options);
					localVoiceAudio3.LocalUserServiceable = new BufferReaderPushAdapterAsyncPoolShortToFloat(source as IAudioReader<short>);
					return localVoiceAudio3;
				}
				LocalVoiceAudio<short> localVoiceAudio4 = CreateLocalVoiceAudio<short>(voiceInfo, source, channelId, options);
				localVoiceAudio4.LocalUserServiceable = new BufferReaderPushAdapterAsyncPool<short>(source as IAudioReader<short>);
				return localVoiceAudio4;
			}
			logger.LogError("[PV] CreateLocalVoiceAudioFromSource does not support Voice.IAudioDesc of type {0}", source.GetType());
			return LocalVoiceAudioDummy.Dummy;
		}

		private byte idInc(byte id)
		{
			if (id != voiceIDMax)
			{
				return (byte)(id + 1);
			}
			return voiceIDMin;
		}

		private byte getNewVoiceId()
		{
			bool[] array = new bool[256];
			foreach (KeyValuePair<byte, LocalVoice> localVoice in localVoices)
			{
				array[localVoice.Value.id] = true;
			}
			for (byte b = idInc(voiceIdLast); b != voiceIdLast; b = idInc(b))
			{
				if (!array[b])
				{
					voiceIdLast = b;
					return b;
				}
			}
			return 0;
		}

		private void addVoice(byte newId, int channelId, LocalVoice v)
		{
			localVoices[newId] = v;
			if (!localVoicesPerChannel.TryGetValue(channelId, out var value))
			{
				value = new List<LocalVoice>();
				localVoicesPerChannel[channelId] = value;
			}
			value.Add(v);
			if (transport.IsChannelJoined(channelId))
			{
				v.sendVoiceInfoAndConfigFrame();
			}
		}

		public void RemoveLocalVoice(LocalVoice voice)
		{
			localVoices.Remove(voice.id);
			localVoicesPerChannel[voice.channelId].Remove(voice);
			if (transport.IsChannelJoined(voice.channelId))
			{
				voice.sendVoiceRemove();
			}
			voice.Dispose();
			logger.LogInfo(voice.LogPrefix + " removed");
		}

		private void clearRemoteVoices()
		{
			foreach (KeyValuePair<int, Dictionary<byte, RemoteVoice>> remoteVoice in remoteVoices)
			{
				foreach (KeyValuePair<byte, RemoteVoice> item in remoteVoice.Value)
				{
					item.Value.removeAndDispose();
				}
			}
			remoteVoices.Clear();
			logger.LogInfo("[PV] Remote voices cleared");
		}

		private void clearRemoteVoicesInChannel(int channelId)
		{
			foreach (KeyValuePair<int, Dictionary<byte, RemoteVoice>> remoteVoice in remoteVoices)
			{
				List<byte> list = new List<byte>();
				foreach (KeyValuePair<byte, RemoteVoice> item in remoteVoice.Value)
				{
					if (item.Value.channelId == channelId)
					{
						item.Value.removeAndDispose();
						list.Add(item.Key);
					}
				}
				foreach (byte item2 in list)
				{
					remoteVoice.Value.Remove(item2);
				}
			}
			logger.LogInfo("[PV] Remote voices for channel " + channelStr(channelId) + " cleared");
		}

		private void clearRemoteVoicesInChannelForPlayer(int channelId, int playerId)
		{
			Dictionary<byte, RemoteVoice> value = null;
			if (!remoteVoices.TryGetValue(playerId, out value))
			{
				return;
			}
			List<byte> list = new List<byte>();
			foreach (KeyValuePair<byte, RemoteVoice> item in value)
			{
				if (item.Value.channelId == channelId)
				{
					item.Value.removeAndDispose();
					list.Add(item.Key);
				}
			}
			foreach (byte item2 in list)
			{
				value.Remove(item2);
			}
		}

		public void onJoinChannel(int channelId)
		{
			if (!localVoicesPerChannel.TryGetValue(channelId, out var value))
			{
				return;
			}
			foreach (LocalVoice item in value)
			{
				item.onJoinChannel();
			}
		}

		public void onJoinAllChannels()
		{
			foreach (KeyValuePair<byte, LocalVoice> localVoice in localVoices)
			{
				localVoice.Value.onJoinChannel();
			}
		}

		public void onLeaveChannel(int channel)
		{
			clearRemoteVoicesInChannel(channel);
		}

		public void onLeaveAllChannels()
		{
			clearRemoteVoices();
		}

		public void onPlayerJoin(int channelId, int playerId)
		{
			if (!localVoicesPerChannel.TryGetValue(channelId, out var value))
			{
				return;
			}
			foreach (LocalVoice item in value)
			{
				item.onPlayerJoin(playerId);
			}
		}

		public void onPlayerJoin(int playerId)
		{
			foreach (KeyValuePair<byte, LocalVoice> localVoice in localVoices)
			{
				localVoice.Value.onPlayerJoin(playerId);
			}
		}

		public void onPlayerLeave(int channelId, int playerId)
		{
			clearRemoteVoicesInChannelForPlayer(channelId, playerId);
		}

		public void onPlayerLeave(int playerId)
		{
			if (!remoteVoices.TryGetValue(playerId, out var value))
			{
				return;
			}
			List<byte> list = new List<byte>();
			foreach (KeyValuePair<byte, RemoteVoice> item in value)
			{
				item.Value.removeAndDispose();
				list.Add(item.Key);
			}
			foreach (byte item2 in list)
			{
				value.Remove(item2);
			}
		}

		public void onVoiceInfo(int channelId, int playerId, byte voiceId, byte eventNumber, VoiceInfo info)
		{
			Dictionary<byte, RemoteVoice> value = null;
			if (!remoteVoices.TryGetValue(playerId, out value))
			{
				value = new Dictionary<byte, RemoteVoice>();
				remoteVoices[playerId] = value;
			}
			if (!value.ContainsKey(voiceId))
			{
				string text = " p#" + playerStr(playerId) + " v#" + voiceId + " ch#" + channelStr(channelId);
				logger.LogInfo("[PV] " + text + " Info received: " + info.ToString() + " ev=" + eventNumber);
				string logPrefix = "[PV] Remote " + info.Codec.ToString() + text;
				RemoteVoiceOptions options = new RemoteVoiceOptions(logger, logPrefix, info);
				if (OnRemoteVoiceInfoAction != null)
				{
					OnRemoteVoiceInfoAction(channelId, playerId, voiceId, info, ref options);
				}
				RemoteVoice remoteVoice = (value[voiceId] = new RemoteVoice(this, options, channelId, playerId, voiceId, info, eventNumber));
				if (remoteVoiceDelayFrames.TryGetValue(info.Codec, out var value2))
				{
					remoteVoice.DelayFrames = value2;
				}
			}
			else if (!SuppressInfoDuplicateWarning)
			{
				logger.LogWarning("[PV] Info duplicate for voice #" + voiceId + " of player " + playerStr(playerId) + " at channel " + channelStr(channelId));
			}
		}

		public void onVoiceRemove(int playerId, byte[] voiceIds)
		{
			Dictionary<byte, RemoteVoice> value = null;
			if (remoteVoices.TryGetValue(playerId, out value))
			{
				for (int i = 0; i < voiceIds.Length; i++)
				{
					byte key = voiceIds[i];
					if (value.TryGetValue(key, out var value2))
					{
						value.Remove(key);
						logger.LogInfo("[PV] Remote voice #" + key + " of player " + playerStr(playerId) + " at channel " + channelStr(value2.channelId) + " removed");
						value2.removeAndDispose();
					}
					else
					{
						logger.LogWarning("[PV] Remote voice #" + key + " of player " + playerStr(playerId) + " at channel " + channelStr(value2.channelId) + " not found when trying to remove");
					}
				}
			}
			else
			{
				logger.LogWarning("[PV] Remote voice list of player " + playerStr(playerId) + " not found when trying to remove voice(s)");
			}
		}

		public void onFrame(int playerId, byte voiceId, byte evNumber, ref FrameBuffer receivedBytes, bool isLocalPlayer)
		{
			if (isLocalPlayer && localVoices.TryGetValue(voiceId, out var value) && value.eventTimestamps.TryGetValue(evNumber, out var value2))
			{
				int num = Environment.TickCount - value2;
				int num2 = num - prevRtt;
				prevRtt = num;
				if (num2 < 0)
				{
					num2 = -num2;
				}
				RoundTripTimeVariance = (num2 + RoundTripTimeVariance * 19) / 20;
				RoundTripTime = (num + RoundTripTime * 19) / 20;
			}
			if (DebugLostPercent > 0 && rnd.Next(100) < DebugLostPercent)
			{
				logger.LogWarning("[PV] Debug Lost Sim: 1 packet dropped");
				return;
			}
			FramesReceived++;
			if (remoteVoices.TryGetValue(playerId, out var value3))
			{
				if (value3.TryGetValue(voiceId, out var value4))
				{
					value4.receiveBytes(ref receivedBytes, evNumber);
				}
				else
				{
					logger.LogWarning("[PV] Frame event for not inited voice #" + voiceId + " of player " + playerStr(playerId));
				}
			}
			else
			{
				logger.LogWarning("[PV] Frame event for voice #" + voiceId + " of not inited player " + playerStr(playerId));
			}
		}

		internal string channelStr(int channelId)
		{
			string text = transport.ChannelIdStr(channelId);
			if (text != null)
			{
				return channelId + "(" + text + ")";
			}
			return channelId.ToString();
		}

		internal string playerStr(int playerId)
		{
			string text = transport.PlayerIdStr(playerId);
			if (text != null)
			{
				return playerId + "(" + text + ")";
			}
			return playerId.ToString();
		}

		public void Dispose()
		{
			foreach (KeyValuePair<byte, LocalVoice> localVoice in localVoices)
			{
				localVoice.Value.Dispose();
			}
			foreach (KeyValuePair<int, Dictionary<byte, RemoteVoice>> remoteVoice in remoteVoices)
			{
				foreach (KeyValuePair<byte, RemoteVoice> item in remoteVoice.Value)
				{
					item.Value.Dispose();
				}
			}
		}
	}
}
