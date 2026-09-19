using System;
using System.Collections.Generic;
using System.Threading;
using Photon.Client.Encryption;

namespace Photon.Client
{
	public class PhotonPeer
	{
		public const bool NoSocket = false;

		public const bool DebugBuild = true;

		public const int NativeEncryptorApiVersion = 2;

		public TargetFrameworks TargetFramework = TargetFrameworks.NetStandard20;

		public byte ClientSdkId = 15;

		private static string clientVersion;

		public static bool NoNativeCallbacks;

		public bool RemoveAppIdFromWebSocketPath;

		internal bool UseInitV3;

		public bool EnableEncryptedFlag = false;

		public Dictionary<ConnectionProtocol, Type> SocketImplementationConfig;

		public LogLevel LogLevel = LogLevel.Error;

		private bool reuseEventInstance = true;

		private bool useByteArraySlicePoolForEvents = false;

		private bool wrapIncomingStructs = false;

		public bool SendInCreationOrder = true;

		public int SendWindowSize = 50;

		private byte quickResendAttempts = 2;

		public int MaxResends = 15;

		public int InitialResendTimeMax = 400;

		private int disconnectTimeout = 10000;

		private bool crcEnabled;

		public int PingInterval = 1000;

		public byte ChannelCount = 2;

		public static int OutgoingStreamBufferSize = 1200;

		private int mtu = 1200;

		public static bool AsyncKeyExchange = false;

		internal bool RandomizeSequenceNumbers;

		internal byte[] RandomizedSequenceNumbers;

		private Type payloadEncryptorType;

		protected internal byte[] PayloadEncryptionSecret;

		private Type encryptorType;

		protected internal IPhotonEncryptor Encryptor;

		public ITrafficRecorder TrafficRecorder;

		public bool PingUsedAsInit = false;

		[Obsolete("Not used anymore.")]
		public bool TrafficStatsEnabled;

		internal PeerBase peerBase;

		private readonly object sendOutgoingLockObject = new object();

		private readonly object dispatchLockObject = new object();

		private readonly object enqueueLock = new object();

		protected internal byte ClientSdkIdShifted => (byte)((ClientSdkId << 1) | 0);

		public static string Version
		{
			get
			{
				if (string.IsNullOrEmpty(clientVersion))
				{
					clientVersion = $"{Photon.Client.Version.clientVersion[0]}.{Photon.Client.Version.clientVersion[1]}.{Photon.Client.Version.clientVersion[2]}.{Photon.Client.Version.clientVersion[3]}";
				}
				return clientVersion;
			}
		}

		public bool UseAck2
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool IsAck2Available => false;

		public SerializationProtocol SerializationProtocolType { get; set; }

		public Type SocketImplementation { get; internal set; }

		public int SocketErrorCode => (peerBase != null && peerBase.PhotonSocket != null) ? peerBase.PhotonSocket.SocketErrorCode : 0;

		[Obsolete("Use LogLevel instead.")]
		public LogLevel DebugOut
		{
			get
			{
				return LogLevel;
			}
			set
			{
				LogLevel = value;
			}
		}

		public IPhotonPeerListener Listener { get; protected set; }

		public PeerStateValue PeerState
		{
			get
			{
				if (peerBase.peerConnectionState == ConnectionStateValue.Connected && !peerBase.ApplicationIsInitialized)
				{
					return PeerStateValue.InitializingApplication;
				}
				return (PeerStateValue)peerBase.peerConnectionState;
			}
		}

		public string PeerID => peerBase.PeerID;

		public bool ReuseEventInstance
		{
			get
			{
				return reuseEventInstance;
			}
			set
			{
				lock (dispatchLockObject)
				{
					reuseEventInstance = value;
					if (!value)
					{
						peerBase.reusableEventData = null;
					}
				}
			}
		}

		public bool UseByteArraySlicePoolForEvents
		{
			get
			{
				return useByteArraySlicePoolForEvents;
			}
			set
			{
				useByteArraySlicePoolForEvents = value;
			}
		}

		public bool WrapIncomingStructs
		{
			get
			{
				return wrapIncomingStructs;
			}
			set
			{
				wrapIncomingStructs = value;
			}
		}

		public ByteArraySlicePool ByteArraySlicePool => peerBase.SerializationProtocol.ByteArraySlicePool;

		public static Pool<StreamBuffer> MessageBufferPool => PeerBase.MessageBufferPool;

		[Obsolete("Use SendWindowSize instead.")]
		public int SequenceDeltaLimitSends
		{
			get
			{
				return SendWindowSize;
			}
			set
			{
				SendWindowSize = value;
			}
		}

		public byte QuickResendAttempts
		{
			get
			{
				return quickResendAttempts;
			}
			set
			{
				quickResendAttempts = value;
				if (quickResendAttempts > 4)
				{
					quickResendAttempts = 4;
				}
				else if (quickResendAttempts < 1)
				{
					quickResendAttempts = 1;
				}
			}
		}

		[Obsolete("Use MaxResends instead.")]
		public int SentCountAllowance
		{
			get
			{
				return MaxResends;
			}
			set
			{
				MaxResends = value;
			}
		}

		public int DisconnectTimeout
		{
			get
			{
				return disconnectTimeout;
			}
			set
			{
				if (value < 0)
				{
					disconnectTimeout = 10000;
				}
				disconnectTimeout = value;
			}
		}

		public bool CrcEnabled
		{
			get
			{
				return crcEnabled;
			}
			set
			{
				if (crcEnabled != value)
				{
					if (peerBase.peerConnectionState != ConnectionStateValue.Disconnected)
					{
						throw new Exception("CrcEnabled can only be set while disconnected.");
					}
					crcEnabled = value;
				}
			}
		}

		[Obsolete("Use PingInterval instead.")]
		public int TimePingInterval
		{
			get
			{
				return PingInterval;
			}
			set
			{
				PingInterval = value;
			}
		}

		public string ServerAddress => peerBase.ServerAddress;

		public string ServerIpAddress
		{
			get
			{
				if (peerBase != null && peerBase.PhotonSocket != null)
				{
					return peerBase.PhotonSocket.ServerIpAddress;
				}
				return string.Empty;
			}
		}

		public ConnectionProtocol UsedProtocol => peerBase.usedTransportProtocol;

		public ConnectionProtocol TransportProtocol { get; set; }

		public virtual bool IsSimulationEnabled
		{
			get
			{
				return NetworkSimulationSettings.IsSimulationEnabled;
			}
			set
			{
				if (value == NetworkSimulationSettings.IsSimulationEnabled)
				{
					return;
				}
				lock (sendOutgoingLockObject)
				{
					NetworkSimulationSettings.IsSimulationEnabled = value;
				}
			}
		}

		public NetworkSimulationSet NetworkSimulationSettings => peerBase.NetworkSimulationSettings;

		public int MaximumTransferUnit
		{
			get
			{
				return mtu;
			}
			set
			{
				if (PeerState != PeerStateValue.Disconnected)
				{
					throw new Exception("MaximumTransferUnit is only settable while disconnected. State: " + PeerState);
				}
				if (value < 576)
				{
					value = 576;
				}
				mtu = value;
			}
		}

		public bool IsEncryptionAvailable => peerBase.isEncryptionAvailable;

		public Type PayloadEncryptorType
		{
			get
			{
				return payloadEncryptorType;
			}
			set
			{
				if (value == null || typeof(ICryptoProvider).IsAssignableFrom(value))
				{
					payloadEncryptorType = value;
				}
				else if ((int)LogLevel >= 1)
				{
					Listener.DebugReturn(LogLevel.Error, "Failed to set PayloadEncryptorType. Must implement ICryptoProvider.");
				}
			}
		}

		public Type EncryptorType
		{
			get
			{
				return encryptorType;
			}
			set
			{
				if (value == null || typeof(IPhotonEncryptor).IsAssignableFrom(value))
				{
					encryptorType = value;
				}
				else if ((int)LogLevel >= 1)
				{
					Listener.DebugReturn(LogLevel.Error, "Failed to set PhotonPeer.EncryptorType. Type '" + value?.ToString() + "' does not implement IPhotonEncryptor.");
				}
			}
		}

		public int ServerTimeInMilliseconds => peerBase.serverTimeOffsetIsAvailable ? (peerBase.serverTimeOffset + ConnectionTime) : 0;

		public bool EnableServerTracing { get; set; }

		public int ConnectionTime => peerBase.timeInt;

		[Obsolete("Use Stats.RoundtripTime instead.")]
		public int RoundTripTime => (int)peerBase.roundTripTime;

		[Obsolete("Use Stats.RoundTripTimeVariance instead.")]
		public int RoundTripTimeVariance => (int)peerBase.roundTripTimeVariance;

		[Obsolete("Use Stats.LastRoundTripTime instead.")]
		public int LastRoundTripTime => peerBase.lastRoundTripTime;

		public long BytesIn => Stats.BytesIn;

		public long BytesOut => Stats.BytesOut;

		public int ByteCountCurrentDispatch => peerBase.ByteCountCurrentDispatch;

		public string CommandInfoCurrentDispatch => (peerBase.CommandInCurrentDispatch != null) ? peerBase.CommandInCurrentDispatch.ToString() : string.Empty;

		public int ByteCountLastOperation => peerBase.ByteCountLastOperation;

		public int PacketLossByCrc => peerBase.packetLossByCrc;

		public int PacketLossByChallenge => peerBase.packetLossByChallenge;

		[Obsolete("Use Stats.UdpReliableCommandsResent instead.")]
		public int ResentReliableCommands => Stats.UdpReliableCommandsResent;

		[Obsolete("Use Stats.LastSendAckTimestamp instead.")]
		public int LastSendAckTime => Stats.LastSendAckTimestamp;

		public int LastSendAckDeltaTime => peerBase.timeInt - Stats.LastSendAckTimestamp;

		[Obsolete("Use Stats.LastSendOutgoingTimestamp instead.")]
		public int LastSendOutgoingTime => Stats.LastSendOutgoingTimestamp;

		public int LastSendOutgoingDeltaTime => peerBase.timeInt - Stats.LastSendOutgoingTimestamp;

		[Obsolete("Use Stats.LastReceiveTimestamp instead.")]
		public int TimestampOfLastSocketReceive => Stats.LastReceiveTimestamp;

		public int LastReceiveDeltaTime => peerBase.timeInt - Stats.LastReceiveTimestamp;

		public int LongestSendCall
		{
			get
			{
				return peerBase.longestSendCall;
			}
			set
			{
				peerBase.longestSendCall = value;
			}
		}

		public int CountDiscarded { get; set; }

		public int DeltaUnreliableNumber { get; set; }

		public int QueuedIncomingCommands => peerBase.QueuedIncomingCommandsCount;

		public int QueuedOutgoingCommands => peerBase.QueuedOutgoingCommandsCount;

		[Obsolete("Use Stats.UdpReliableCommandsInFlight.")]
		public int ReliableCommandsInFlight => Stats.UdpReliableCommandsInFlight;

		[Obsolete("Use Stats.UdpReliableCommandsInFlight instead. Check reference doc to make sure this is what you want to check.")]
		public int SentReliableCommandsCount => Stats.UdpReliableCommandsInFlight;

		public TrafficStats Stats { get; internal set; }

		public string VitalStatsToString(bool all = true)
		{
			float num = (float)peerBase.timeInt / 1000f;
			long num2 = (BytesIn + BytesOut) / 1000;
			int num3 = ((!(num <= 0f)) ? ((int)((float)num2 / num)) : 0);
			string text = $"Stats duration: {num:F2} sec. rtt(var): {Stats.RoundtripTime}({Stats.RoundtripTimeVariance})ms.  {num2:N0} kB -> {num3:N0} kB/sec.";
			if (!all)
			{
				return text;
			}
			return $"{text}\n{Stats}";
		}

		public PhotonPeer(ConnectionProtocol protocolType)
		{
			TransportProtocol = protocolType;
			SocketImplementationConfig = new Dictionary<ConnectionProtocol, Type>(5);
			SocketImplementationConfig[ConnectionProtocol.Udp] = typeof(SocketUdp);
			SocketImplementationConfig[ConnectionProtocol.Tcp] = typeof(SocketTcp);
			SocketImplementationConfig[ConnectionProtocol.WebSocket] = typeof(PhotonClientWebSocket);
			SocketImplementationConfig[ConnectionProtocol.WebSocketSecure] = typeof(PhotonClientWebSocket);
			CreatePeerBase();
			Stats = new TrafficStats(peerBase.watch);
		}

		public PhotonPeer(IPhotonPeerListener listener, ConnectionProtocol protocolType)
			: this(protocolType)
		{
			Listener = listener;
		}

		[Obsolete("Use new overload with updated parameter order.")]
		public virtual bool Connect(string serverAddress, string proxyServerAddress, string appId, object photonToken, object customInitData = null)
		{
			return Connect(serverAddress, appId, photonToken, customInitData, proxyServerAddress);
		}

		public virtual bool Connect(string serverAddress, string appId, object photonToken, object customInitData = null, string proxyServerAddress = null)
		{
			lock (dispatchLockObject)
			{
				lock (sendOutgoingLockObject)
				{
					if (peerBase != null && peerBase.peerConnectionState != ConnectionStateValue.Disconnected)
					{
						if ((int)LogLevel >= 2)
						{
							Listener.DebugReturn(LogLevel.Warning, $"Connect() failed. Peer is not Disconnected. peerConnectionState: {peerBase.peerConnectionState}.");
						}
						return false;
					}
					if (photonToken == null)
					{
						Encryptor = null;
						RandomizedSequenceNumbers = null;
						RandomizeSequenceNumbers = false;
					}
					CreatePeerBase();
					peerBase.Reset();
					Stats = new TrafficStats(peerBase.watch);
					PingUsedAsInit = false;
					peerBase.ServerAddress = serverAddress;
					peerBase.ProxyServerAddress = proxyServerAddress;
					peerBase.AppId = appId;
					peerBase.PhotonToken = photonToken;
					peerBase.CustomInitData = customInitData;
					Type value = null;
					if (!SocketImplementationConfig.TryGetValue(TransportProtocol, out value))
					{
						peerBase.EnqueueDebugReturn(LogLevel.Error, $"Connect() failed. SocketImplementationConfig is not set for protocol {TransportProtocol}: {SupportClass.DictionaryToString(SocketImplementationConfig, includeTypes: false)}");
						return false;
					}
					SocketImplementation = value;
					try
					{
						peerBase.PhotonSocket = (PhotonSocket)Activator.CreateInstance(SocketImplementation, peerBase);
					}
					catch (Exception arg)
					{
						if ((int)LogLevel >= 1)
						{
							Listener.DebugReturn(LogLevel.Error, $"Connect() failed to create a PhotonSocket instance for {TransportProtocol}. SocketImplementationConfig: {SupportClass.DictionaryToString(SocketImplementationConfig, includeTypes: false)} Exception: {arg}");
						}
						return false;
					}
					return peerBase.Connect(serverAddress, proxyServerAddress, appId, photonToken);
				}
			}
		}

		private void CreatePeerBase()
		{
			ConnectionProtocol transportProtocol = TransportProtocol;
			ConnectionProtocol connectionProtocol = transportProtocol;
			if (connectionProtocol == ConnectionProtocol.Tcp || connectionProtocol - 4 <= ConnectionProtocol.Tcp)
			{
				TPeer tPeer = peerBase as TPeer;
				if (tPeer == null)
				{
					tPeer = (TPeer)(peerBase = new TPeer());
				}
				tPeer.DoFraming = TransportProtocol == ConnectionProtocol.Tcp;
			}
			else if (!(peerBase is EnetPeer))
			{
				peerBase = new EnetPeer();
			}
			peerBase.photonPeer = this;
			peerBase.usedTransportProtocol = TransportProtocol;
		}

		public virtual void Disconnect()
		{
			lock (dispatchLockObject)
			{
				lock (sendOutgoingLockObject)
				{
					peerBase.Disconnect();
				}
			}
		}

		public virtual void SimulateTimeoutDisconnect()
		{
			lock (dispatchLockObject)
			{
				lock (sendOutgoingLockObject)
				{
					peerBase.SimulateTimeoutDisconnect();
				}
			}
		}

		public virtual void FetchServerTimestamp()
		{
			peerBase.FetchServerTimestamp();
		}

		public bool EstablishEncryption()
		{
			if (AsyncKeyExchange)
			{
				ThreadPool.QueueUserWorkItem(delegate
				{
					peerBase.ExchangeKeysForEncryption(enqueueLock);
				});
				return true;
			}
			return peerBase.ExchangeKeysForEncryption(enqueueLock);
		}

		[Obsolete("Use InitDatagramEncryption(byte[] encryptionSecret, byte[] hmacSecret).")]
		public bool InitDatagramEncryption(byte[] encryptionSecret, byte[] hmacSecret, bool randomizedSequenceNumbers, bool chainingModeGCM)
		{
			if (!randomizedSequenceNumbers || !chainingModeGCM)
			{
				if ((int)LogLevel >= 1)
				{
					Listener.DebugReturn(LogLevel.Error, "InitDatagramEncryption now requires randomizedSequenceNumbers and chainingModeGCM being true.");
				}
				return false;
			}
			return InitDatagramEncryption(encryptionSecret, hmacSecret);
		}

		public bool InitDatagramEncryption(byte[] encryptionSecret, byte[] hmacSecret)
		{
			if (encryptionSecret == null)
			{
				Listener.DebugReturn(LogLevel.Error, "InitDatagramEncryption() failed. Parameter encryptionSecret can not be null.");
				peerBase.EnqueueStatusCallback(StatusCode.EncryptionFailedToEstablish);
				return false;
			}
			if (EncryptorType == null)
			{
				Listener.DebugReturn(LogLevel.Error, "InitDatagramEncryption() failed. PhotonPeer.EncryptorType must be set to non-null value to initialize Datagram Encryption.");
				peerBase.EnqueueStatusCallback(StatusCode.EncryptionFailedToEstablish);
				return false;
			}
			try
			{
				Encryptor = (IPhotonEncryptor)Activator.CreateInstance(EncryptorType);
			}
			catch (Exception arg)
			{
				if ((int)LogLevel >= 2)
				{
					Listener.DebugReturn(LogLevel.Warning, $"InitDatagramEncryption() failed in CreateInstance({EncryptorType}). Caught exception: {arg}");
				}
			}
			if (Encryptor == null)
			{
				Listener.DebugReturn(LogLevel.Error, "InitDatagramEncryption() failed. Could not create an encryptor instance.");
				peerBase.EnqueueStatusCallback(StatusCode.EncryptionFailedToEstablish);
				return false;
			}
			try
			{
				Encryptor.LogLevel = (int)LogLevel;
				Encryptor.Init(encryptionSecret, hmacSecret, null, chainingModeGCM: true, mtu);
				if ((int)LogLevel >= 3)
				{
					Listener.DebugReturn(LogLevel.Info, $"Datagram Encryptor ({Encryptor.GetType()}) successfully initialized.");
				}
			}
			catch (Exception arg2)
			{
				Listener.DebugReturn(LogLevel.Error, $"InitDatagramEncryption() failed in {Encryptor}.Init(). Caught exception: {arg2}");
				peerBase.EnqueueStatusCallback(StatusCode.EncryptionFailedToEstablish);
				return false;
			}
			RandomizedSequenceNumbers = encryptionSecret;
			RandomizeSequenceNumbers = true;
			return true;
		}

		public void InitPayloadEncryption(byte[] secret)
		{
			PayloadEncryptionSecret = secret;
		}

		public virtual void Service()
		{
			while (DispatchIncomingCommands())
			{
			}
			while (SendOutgoingCommands())
			{
			}
		}

		public virtual bool SendOutgoingCommands()
		{
			Stats.SendOutgoingCommandsCalled(peerBase.timeInt);
			lock (sendOutgoingLockObject)
			{
				return peerBase.SendOutgoingCommands();
			}
		}

		public virtual bool SendAcksOnly()
		{
			lock (sendOutgoingLockObject)
			{
				return peerBase.SendAcksOnly();
			}
		}

		public virtual bool DispatchIncomingCommands()
		{
			Stats.DispatchIncomingCommandsCalled(peerBase.timeInt);
			lock (dispatchLockObject)
			{
				peerBase.ByteCountCurrentDispatch = 0;
				return peerBase.DispatchIncomingCommands();
			}
		}

		public virtual bool SendOperation(byte operationCode, ParameterDictionary operationParameters, SendOptions sendOptions)
		{
			if (sendOptions.Encrypt && !IsEncryptionAvailable && peerBase.usedTransportProtocol != ConnectionProtocol.WebSocketSecure)
			{
				throw new ArgumentException("Can't use encryption yet. Exchange keys first.");
			}
			if (peerBase.peerConnectionState != ConnectionStateValue.Connected)
			{
				if ((int)LogLevel >= 1)
				{
					Listener.DebugReturn(LogLevel.Error, $"SendOperation failed. Not connected. Failed operation: {operationCode} PeerState: {peerBase.peerConnectionState}");
				}
				Listener.OnStatusChanged(StatusCode.SendError);
				return false;
			}
			if (sendOptions.Channel >= ChannelCount)
			{
				if ((int)LogLevel >= 1)
				{
					Listener.DebugReturn(LogLevel.Error, $"SendOperation failed. Channel unavailable: ({sendOptions.Channel} >= channelCount {ChannelCount}). Failed operation: {operationCode}");
				}
				Listener.OnStatusChanged(StatusCode.SendError);
				return false;
			}
			lock (enqueueLock)
			{
				StreamBuffer opBytes = peerBase.SerializeOperationToMessage(operationCode, operationParameters, EgMessageType.Operation, sendOptions.Encrypt);
				return peerBase.EnqueuePhotonMessage(opBytes, sendOptions);
			}
		}

		public virtual bool SendMessage(object message, SendOptions sendOptions)
		{
			if (sendOptions.Encrypt && !IsEncryptionAvailable && peerBase.usedTransportProtocol != ConnectionProtocol.WebSocketSecure)
			{
				throw new ArgumentException("Can't use encryption yet. Exchange keys first.");
			}
			if (peerBase.peerConnectionState != ConnectionStateValue.Connected)
			{
				if ((int)LogLevel >= 1)
				{
					Listener.DebugReturn(LogLevel.Error, $"SendMessage failed. Not connected. PeerState: {peerBase.peerConnectionState}");
				}
				Listener.OnStatusChanged(StatusCode.SendError);
				return false;
			}
			if (sendOptions.Channel >= ChannelCount)
			{
				if ((int)LogLevel >= 1)
				{
					Listener.DebugReturn(LogLevel.Error, $"SendMessage failed. Channel unavailable: ({sendOptions.Channel} >= channelCount {ChannelCount}). Failed message: {message}");
				}
				Listener.OnStatusChanged(StatusCode.SendError);
				return false;
			}
			lock (enqueueLock)
			{
				StreamBuffer opBytes = peerBase.SerializeMessageToMessage(message, sendOptions.Encrypt);
				return peerBase.EnqueuePhotonMessage(opBytes, sendOptions);
			}
		}

		public static bool RegisterType(Type customType, byte code, SerializeStreamMethod serializeMethod, DeserializeStreamMethod deserializeMethod)
		{
			return Protocol.TryRegisterType(customType, code, serializeMethod, deserializeMethod);
		}
	}
}
