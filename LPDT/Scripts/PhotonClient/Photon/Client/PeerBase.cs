using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Photon.Client.Encryption;

namespace Photon.Client
{
	public abstract class PeerBase
	{
		internal delegate void MyAction();

		private static class GpBinaryV3Parameters
		{
			public const byte CustomObject = 0;

			public const byte ExtraPlatformParams = 1;
		}

		internal PhotonPeer photonPeer;

		public Protocol SerializationProtocol;

		internal ConnectionProtocol usedTransportProtocol;

		internal PhotonSocket PhotonSocket;

		private int peerConnectionStateField;

		internal int ByteCountLastOperation;

		internal int ByteCountCurrentDispatch;

		internal NCommand CommandInCurrentDispatch;

		internal int packetLossByCrc;

		internal int packetLossByChallenge;

		internal int throttledBySendWindow;

		internal readonly Queue<MyAction> ActionQueue = new Queue<MyAction>();

		internal short peerID = -1;

		internal static short peerCount;

		internal int serverTimeOffset;

		internal bool serverTimeOffsetIsAvailable;

		internal float roundTripTime;

		internal float roundTripTimeVariance;

		internal int lastRoundTripTime;

		internal int lowestRoundTripTime;

		internal int highestRoundTripTimeVariance;

		internal int varianceJitterMin = 4;

		internal int varianceJitterMax = 100;

		internal int varianceJitterFactor = 4;

		internal object PhotonToken;

		internal object CustomInitData;

		public string AppId;

		internal EventData reusableEventData;

		internal Stopwatch watch = Stopwatch.StartNew();

		internal int timeoutInt;

		internal int timeLastAckReceive;

		internal int longestSendCall;

		internal int timeIntCurrentSend;

		internal bool ApplicationIsInitialized;

		internal bool isEncryptionAvailable;

		private ushort serverFeatureFlags;

		protected internal static Pool<StreamBuffer> MessageBufferPool = new Pool<StreamBuffer>(() => new StreamBuffer(PhotonPeer.OutgoingStreamBufferSize), delegate(StreamBuffer buffer)
		{
			buffer.Reset();
		}, 16);

		internal byte[] messageHeader;

		private volatile int prepareWebSocketUrlCount = -1;

		private StringBuilder prepareWebSocketUrlSB;

		internal ICryptoProvider CryptoProvider;

		private readonly Random lagRandomizer = new Random();

		internal readonly LinkedList<SimulationItem> NetSimListOutgoing = new LinkedList<SimulationItem>();

		internal readonly LinkedList<SimulationItem> NetSimListIncoming = new LinkedList<SimulationItem>();

		private readonly NetworkSimulationSet networkSimulationSettings = new NetworkSimulationSet();

		internal TrafficStats Stats => photonPeer.Stats;

		internal IPhotonPeerListener Listener => photonPeer.Listener;

		internal LogLevel LogLevel => photonPeer.LogLevel;

		public string ServerAddress { get; internal set; }

		public string ProxyServerAddress { get; internal set; }

		internal ConnectionStateValue peerConnectionState
		{
			get
			{
				return (ConnectionStateValue)Interlocked.CompareExchange(ref peerConnectionStateField, 0, 0);
			}
			set
			{
				Interlocked.Exchange(ref peerConnectionStateField, (int)value);
			}
		}

		internal string rttVarString => $"{roundTripTime,5:N}({roundTripTimeVariance,4:N})";

		internal int TimeoutVarianceCompensation
		{
			get
			{
				if (roundTripTimeVariance < (float)varianceJitterMin)
				{
					return varianceJitterMin * varianceJitterFactor;
				}
				if (roundTripTimeVariance > (float)varianceJitterMax)
				{
					return varianceJitterMax * varianceJitterFactor;
				}
				return (int)roundTripTimeVariance * varianceJitterFactor;
			}
		}

		internal int DisconnectTimeout => photonPeer.DisconnectTimeout;

		internal int PingInterval => photonPeer.PingInterval;

		internal byte ChannelCount => photonPeer.ChannelCount;

		internal abstract int QueuedIncomingCommandsCount { get; }

		internal abstract int QueuedOutgoingCommandsCount { get; }

		public virtual string PeerID => ((ushort)peerID).ToString();

		internal int timeInt => (int)watch.ElapsedMilliseconds;

		public ushort ServerFeatureFlags
		{
			get
			{
				return serverFeatureFlags;
			}
			internal set
			{
				serverFeatureFlags = value;
				serverFeatureFlagsAvailable = serverFeatureFlags > 0;
				serverFeatureAck2Available = (serverFeatureFlags & 1) > 0;
				serverFeatureSyncReliableQueue = (serverFeatureFlags & 2) > 0;
				if (!serverFeatureFlagsAvailable)
				{
					ServerMaxQueueableReliableCommands = 0;
				}
			}
		}

		internal bool serverFeatureFlagsAvailable { get; private set; }

		internal bool serverFeatureAck2Available { get; private set; }

		internal bool serverFeatureSyncReliableQueue { get; private set; }

		public ushort ServerMaxQueueableReliableCommands { get; internal set; }

		internal int mtu => photonPeer.MaximumTransferUnit;

		protected internal bool IsIpv6 => PhotonSocket != null && PhotonSocket.AddressResolvedAsIpv6;

		public NetworkSimulationSet NetworkSimulationSettings => networkSimulationSettings;

		internal bool TryUpdateConnectionState(ConnectionStateValue expected, ConnectionStateValue desired)
		{
			int num = Interlocked.CompareExchange(ref peerConnectionStateField, (int)desired, (int)expected);
			return num == (int)expected;
		}

		protected PeerBase()
		{
			networkSimulationSettings.peerBase = this;
			peerCount++;
		}

		internal virtual void Reset()
		{
			SerializationProtocol = SerializationProtocolFactory.Create(photonPeer.SerializationProtocolType);
			ByteCountLastOperation = 0;
			ByteCountCurrentDispatch = 0;
			Stats.BytesIn = 0L;
			Stats.BytesOut = 0L;
			packetLossByCrc = 0;
			packetLossByChallenge = 0;
			networkSimulationSettings.LostPackagesIn = 0;
			networkSimulationSettings.LostPackagesOut = 0;
			throttledBySendWindow = 0;
			lock (NetSimListOutgoing)
			{
				NetSimListOutgoing.Clear();
			}
			lock (NetSimListIncoming)
			{
				NetSimListIncoming.Clear();
			}
			lock (ActionQueue)
			{
				ActionQueue.Clear();
			}
			peerConnectionState = ConnectionStateValue.Disconnected;
			watch.Reset();
			watch.Start();
			isEncryptionAvailable = false;
			ServerFeatureFlags = 0;
			ApplicationIsInitialized = false;
			CryptoProvider = null;
			roundTripTime = 200f;
			roundTripTimeVariance = 5f;
			serverTimeOffsetIsAvailable = false;
			serverTimeOffset = 0;
		}

		internal abstract bool Connect(string serverAddress, string proxyServerAddress, string appID, object photonToken);

		private string GetHttpKeyValueString(Dictionary<string, string> dic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> item in dic)
			{
				stringBuilder.Append(item.Key).Append("=").Append(item.Value)
					.Append("&");
			}
			return stringBuilder.ToString();
		}

		internal byte[] WriteInitRequest()
		{
			if (photonPeer.UseInitV3)
			{
				return WriteInitV3();
			}
			if (PhotonToken == null)
			{
				byte[] array = new byte[41];
				byte[] clientVersion = Version.clientVersion;
				array[0] = 243;
				array[1] = 0;
				array[2] = SerializationProtocol.VersionBytes[0];
				array[3] = SerializationProtocol.VersionBytes[1];
				array[4] = photonPeer.ClientSdkIdShifted;
				array[5] = (byte)((byte)(clientVersion[0] << 4) | clientVersion[1]);
				array[6] = clientVersion[2];
				array[7] = clientVersion[3];
				array[8] = 0;
				if (string.IsNullOrEmpty(AppId))
				{
					AppId = "Realtime";
				}
				for (int i = 0; i < 32; i++)
				{
					array[i + 9] = (byte)((i < AppId.Length) ? ((byte)AppId[i]) : 0);
				}
				if (IsIpv6)
				{
					array[5] |= 128;
				}
				else
				{
					array[5] &= 127;
				}
				return array;
			}
			if (PhotonToken != null)
			{
				byte[] array2 = null;
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary["init"] = null;
				dictionary["app"] = AppId;
				dictionary["clientversion"] = PhotonPeer.Version;
				dictionary["protocol"] = SerializationProtocol.ProtocolType;
				dictionary["sid"] = photonPeer.ClientSdkIdShifted.ToString();
				byte[] array3 = null;
				int num = 0;
				if (PhotonToken != null)
				{
					array3 = SerializationProtocol.Serialize(PhotonToken);
					num += array3.Length;
				}
				string text = GetHttpKeyValueString(dictionary);
				if (IsIpv6)
				{
					text += "&IPv6";
				}
				string text2 = $"POST /?{text} HTTP/1.1\r\nHost: {ServerAddress}\r\nContent-Length: {num}\r\n\r\n";
				array2 = new byte[text2.Length + num];
				if (array3 != null)
				{
					Buffer.BlockCopy(array3, 0, array2, text2.Length, array3.Length);
				}
				Buffer.BlockCopy(Encoding.UTF8.GetBytes(text2), 0, array2, 0, text2.Length);
				return array2;
			}
			return null;
		}

		private byte[] WriteInitV3()
		{
			StreamBuffer streamBuffer = new StreamBuffer();
			streamBuffer.WriteByte(245);
			InitV3Flags initV3Flags = InitV3Flags.NoFlags;
			if (IsIpv6)
			{
				initV3Flags |= InitV3Flags.IPv6Flag;
			}
			IPhotonEncryptor encryptor = photonPeer.Encryptor;
			if (encryptor != null)
			{
				initV3Flags |= InitV3Flags.EncryptionFlag;
			}
			streamBuffer.WriteBytes((byte)((int)initV3Flags >> 8), (byte)initV3Flags);
			switch (SerializationProtocol.VersionBytes[1])
			{
			case 6:
				streamBuffer.WriteByte(16);
				break;
			case 8:
				streamBuffer.WriteByte(18);
				break;
			default:
				throw new Exception("Unknown protocol version: " + SerializationProtocol.VersionBytes[1]);
			}
			streamBuffer.Write(Version.clientVersion, 0, 4);
			streamBuffer.WriteByte(photonPeer.ClientSdkIdShifted);
			streamBuffer.WriteByte(0);
			if (string.IsNullOrEmpty(AppId))
			{
				AppId = "Master";
			}
			byte[] bytes = Encoding.UTF8.GetBytes(AppId);
			int num = bytes.Length;
			if (num > 255)
			{
				throw new Exception("AppId is too long. Limited by 255 symbols.");
			}
			streamBuffer.WriteByte((byte)num);
			streamBuffer.Write(bytes, 0, bytes.Length);
			if (PhotonToken is byte[] array)
			{
				num = array.Length;
				streamBuffer.WriteBytes((byte)(num >> 8), (byte)num);
				streamBuffer.Write(array, 0, num);
			}
			else
			{
				streamBuffer.WriteBytes(0, 0);
			}
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
			if (CustomInitData != null)
			{
				dictionary.Add(0, CustomInitData);
			}
			if (encryptor != null)
			{
				throw new NotImplementedException("InitV3 with encryption is not implemented yet.");
			}
			SerializationProtocol.Serialize(streamBuffer, dictionary, setType: true);
			return streamBuffer.ToArray();
		}

		internal string PrepareWebSocketUrl(string serverAddress, string appId, object photonToken)
		{
			if (prepareWebSocketUrlSB == null)
			{
				prepareWebSocketUrlSB = new StringBuilder(256);
			}
			prepareWebSocketUrlSB.Clear();
			prepareWebSocketUrlCount++;
			prepareWebSocketUrlSB.Append(serverAddress);
			prepareWebSocketUrlSB.AppendFormat("/?libversion={0}", PhotonPeer.Version);
			prepareWebSocketUrlSB.AppendFormat("&sid={0}", photonPeer.ClientSdkIdShifted);
			prepareWebSocketUrlSB.AppendFormat("&peerId={0}_{1}", peerID, prepareWebSocketUrlCount);
			if (!photonPeer.RemoveAppIdFromWebSocketPath && appId != null && appId.Length >= 8)
			{
				prepareWebSocketUrlSB.AppendFormat("&app={0}", appId.Substring(0, 8));
			}
			if (IsIpv6)
			{
				prepareWebSocketUrlSB.Append("&IPv6");
			}
			if (photonToken != null)
			{
				prepareWebSocketUrlSB.Append("&xInit=");
			}
			return prepareWebSocketUrlSB.ToString();
		}

		[Obsolete("This callback is no longer required by PhotonSocket implementations.")]
		public void OnConnect()
		{
		}

		internal void OnInitResponse()
		{
			if (peerConnectionState == ConnectionStateValue.Connecting)
			{
				peerConnectionState = ConnectionStateValue.Connected;
			}
			ApplicationIsInitialized = true;
			FetchServerTimestamp();
			Listener.OnStatusChanged(StatusCode.Connect);
		}

		internal abstract void Disconnect(bool queueStatusChangeCallback = true);

		internal abstract void SimulateTimeoutDisconnect(bool queueStatusChangeCallback = true);

		internal abstract void FetchServerTimestamp();

		internal abstract bool IsTransportEncrypted();

		internal abstract bool EnqueuePhotonMessage(StreamBuffer opBytes, SendOptions sendParams);

		internal StreamBuffer SerializeOperationToMessage(byte opCode, ParameterDictionary parameters, EgMessageType messageType, bool encrypt)
		{
			bool flag = encrypt && !IsTransportEncrypted();
			StreamBuffer streamBuffer = MessageBufferPool.Acquire();
			streamBuffer.SetLength(0L);
			if (!flag)
			{
				streamBuffer.Write(messageHeader, 0, messageHeader.Length);
			}
			SerializationProtocol.SerializeOperationRequest(streamBuffer, opCode, parameters, setType: false);
			if (flag)
			{
				byte[] array = CryptoProvider.Encrypt(streamBuffer.GetBuffer(), 0, streamBuffer.Length);
				streamBuffer.SetLength(0L);
				streamBuffer.Write(messageHeader, 0, messageHeader.Length);
				streamBuffer.Write(array, 0, array.Length);
			}
			byte[] buffer = streamBuffer.GetBuffer();
			if (messageType != EgMessageType.Operation)
			{
				buffer[messageHeader.Length - 1] = (byte)messageType;
			}
			if (flag || (encrypt && photonPeer.EnableEncryptedFlag))
			{
				buffer[messageHeader.Length - 1] = (byte)(buffer[messageHeader.Length - 1] | 0x80);
			}
			return streamBuffer;
		}

		internal StreamBuffer SerializeMessageToMessage(object message, bool encrypt)
		{
			bool flag = encrypt && !IsTransportEncrypted();
			StreamBuffer streamBuffer = MessageBufferPool.Acquire();
			streamBuffer.SetLength(0L);
			if (!flag)
			{
				streamBuffer.Write(messageHeader, 0, messageHeader.Length);
			}
			bool flag2 = message is byte[];
			if (flag2)
			{
				byte[] array = message as byte[];
				streamBuffer.Write(array, 0, array.Length);
			}
			else
			{
				SerializationProtocol.SerializeMessage(streamBuffer, message);
			}
			if (flag)
			{
				byte[] array2 = CryptoProvider.Encrypt(streamBuffer.GetBuffer(), 0, streamBuffer.Length);
				streamBuffer.SetLength(0L);
				streamBuffer.Write(messageHeader, 0, messageHeader.Length);
				streamBuffer.Write(array2, 0, array2.Length);
			}
			byte[] buffer = streamBuffer.GetBuffer();
			buffer[messageHeader.Length - 1] = (byte)(flag2 ? 9 : 8);
			if (flag || (encrypt && photonPeer.EnableEncryptedFlag))
			{
				buffer[messageHeader.Length - 1] = (byte)(buffer[messageHeader.Length - 1] | 0x80);
			}
			return streamBuffer;
		}

		internal abstract bool SendOutgoingCommands();

		internal virtual bool SendAcksOnly()
		{
			return false;
		}

		internal abstract void ReceiveIncomingCommands(byte[] inBuff, int dataLength);

		internal abstract bool DispatchIncomingCommands();

		internal virtual bool DeserializeMessageAndCallback(StreamBuffer stream)
		{
			if (stream.Length < 2)
			{
				if ((int)LogLevel >= 4)
				{
					Listener.DebugReturn(LogLevel.Debug, $"Discarding message: Less than 2 bytes. Length: {stream.Length}");
				}
				return false;
			}
			byte b = stream.ReadByte();
			if (b != 243 && b != 253)
			{
				if ((int)LogLevel >= 4)
				{
					Listener.DebugReturn(LogLevel.Debug, $"Discarding message: Unknown magic byte: {b}");
				}
				return false;
			}
			byte b2 = stream.ReadByte();
			byte b3 = (byte)(b2 & 0x7F);
			bool flag = (b2 & 0x80) > 0;
			if (b3 != 1)
			{
				try
				{
					if (flag)
					{
						byte[] buf = CryptoProvider.Decrypt(stream.GetBuffer(), 2, stream.Length - 2);
						stream = new StreamBuffer(buf);
					}
					else
					{
						stream.Seek(2L, SeekOrigin.Begin);
					}
				}
				catch (Exception ex)
				{
					if ((int)LogLevel >= 1)
					{
						Listener.DebugReturn(LogLevel.Error, $"Decryption caught exception handling msgType: {b3} exception: {ex}");
					}
					SupportClass.WriteStackTrace(ex);
					return false;
				}
			}
			Protocol.DeserializationFlags flags = (Protocol.DeserializationFlags)((photonPeer.UseByteArraySlicePoolForEvents ? 1 : 0) | (photonPeer.WrapIncomingStructs ? 2 : 0));
			int num = 0;
			switch (b3)
			{
			case 3:
			{
				OperationResponse operationResponse = null;
				try
				{
					operationResponse = SerializationProtocol.DeserializeOperationResponse(stream, flags);
				}
				catch (Exception arg4)
				{
					if ((int)LogLevel >= 1)
					{
						EnqueueDebugReturn(LogLevel.Error, $"Deserialization caught exception for Operation Response: {arg4}");
					}
					return false;
				}
				num = timeInt;
				Listener.OnOperationResponse(operationResponse);
				Stats.LastDispatchDuration = timeInt - num;
				break;
			}
			case 4:
			{
				EventData eventData = null;
				try
				{
					eventData = SerializationProtocol.DeserializeEventData(stream, reusableEventData, flags);
				}
				catch (Exception arg)
				{
					if ((int)LogLevel >= 1)
					{
						EnqueueDebugReturn(LogLevel.Error, $"Deserialization caught exception for Event: {arg}");
					}
					return false;
				}
				num = timeInt;
				Listener.OnEvent(eventData);
				Stats.LastDispatchDuration = timeInt - num;
				if (photonPeer.ReuseEventInstance)
				{
					reusableEventData = eventData;
				}
				break;
			}
			case 5:
				try
				{
					DisconnectMessage dm = SerializationProtocol.DeserializeDisconnectMessage(stream);
					Listener.OnDisconnectMessage(dm);
				}
				catch (Exception arg3)
				{
					if ((int)LogLevel >= 1)
					{
						EnqueueDebugReturn(LogLevel.Error, $"Deserialization caught exception for Disconnect Message: {arg3}");
					}
					return false;
				}
				break;
			case 1:
				OnInitResponse();
				break;
			case 7:
			{
				OperationResponse operationResponse;
				try
				{
					operationResponse = SerializationProtocol.DeserializeOperationResponse(stream);
				}
				catch (Exception arg2)
				{
					if ((int)LogLevel >= 1)
					{
						EnqueueDebugReturn(LogLevel.Error, $"Deserialization caught exception for Internal Operation Response: {arg2}");
					}
					return false;
				}
				num = timeInt;
				if (operationResponse.OperationCode == PhotonCodes.InitEncryption)
				{
					DeriveSharedKey(operationResponse);
				}
				else if (operationResponse.OperationCode == PhotonCodes.Ping)
				{
					if (peerConnectionState == ConnectionStateValue.Connecting && (usedTransportProtocol == ConnectionProtocol.WebSocket || usedTransportProtocol == ConnectionProtocol.WebSocketSecure))
					{
						photonPeer.PingUsedAsInit = true;
						EnqueueActionForDispatch(delegate
						{
							OnInitResponse();
						});
					}
					if (this is TPeer tPeer)
					{
						tPeer.ReadPingResult(operationResponse);
					}
				}
				else if ((int)LogLevel >= 1)
				{
					EnqueueDebugReturn(LogLevel.Error, "Deserialization failed for unknown Internal Operation Response Code: " + operationResponse.ToStringFull());
				}
				Stats.LastDispatchDuration = timeInt - num;
				break;
			}
			case 8:
			{
				object message = SerializationProtocol.DeserializeMessage(stream);
				num = timeInt;
				Listener.OnMessage(isRawMessage: false, message);
				Stats.LastDispatchDuration = timeInt - num;
				break;
			}
			case 9:
				num = timeInt;
				Listener.OnMessage(isRawMessage: true, stream);
				Stats.LastDispatchDuration = timeInt - num;
				break;
			default:
				if ((int)LogLevel >= 1)
				{
					EnqueueDebugReturn(LogLevel.Error, $"Deserialization failed for unexpected msgType: {b3}");
				}
				break;
			}
			return true;
		}

		internal void UpdateRoundTripTimeAndVariance(int lastRoundtripTime)
		{
			if (lastRoundtripTime >= 0)
			{
				roundTripTimeVariance -= roundTripTimeVariance / 4f;
				if ((float)lastRoundtripTime >= roundTripTime)
				{
					roundTripTime += ((float)lastRoundtripTime - roundTripTime) / 8f;
					roundTripTimeVariance += ((float)lastRoundtripTime - roundTripTime) / 4f;
				}
				else
				{
					roundTripTime += ((float)lastRoundtripTime - roundTripTime) / 8f;
					roundTripTimeVariance -= ((float)lastRoundtripTime - roundTripTime) / 4f;
				}
				if (roundTripTime < (float)lowestRoundTripTime)
				{
					lowestRoundTripTime = (int)roundTripTime;
				}
				if (roundTripTimeVariance > (float)highestRoundTripTimeVariance)
				{
					highestRoundTripTimeVariance = (int)roundTripTimeVariance;
				}
				Stats.RoundtripTime = (int)roundTripTime;
				Stats.RoundtripTimeVariance = (int)roundTripTimeVariance;
				Stats.LastRoundtripTime = lastRoundTripTime;
			}
		}

		internal bool ExchangeKeysForEncryption(object lockObject)
		{
			if (lockObject == null)
			{
				throw new NotSupportedException("Parameter lockObject must be non-Null.");
			}
			isEncryptionAvailable = false;
			if (CryptoProvider != null)
			{
				CryptoProvider.Dispose();
				CryptoProvider = null;
			}
			if (photonPeer.PayloadEncryptorType != null)
			{
				try
				{
					CryptoProvider = (ICryptoProvider)Activator.CreateInstance(photonPeer.PayloadEncryptorType);
					if (CryptoProvider == null)
					{
						Listener.DebugReturn(LogLevel.Warning, $"Payload encryptor creation by type failed, Activator.CreateInstance() returned null for: {photonPeer.PayloadEncryptorType}");
					}
				}
				catch (Exception arg)
				{
					Listener.DebugReturn(LogLevel.Warning, $"Payload encryptor creation by type failed. Caught: {arg}");
				}
			}
			if (CryptoProvider == null)
			{
				CryptoProvider = new DiffieHellmanCryptoProvider();
			}
			ParameterDictionary parameterDictionary = new ParameterDictionary(1);
			parameterDictionary[PhotonCodes.ClientKey] = CryptoProvider.PublicKey;
			lock (lockObject)
			{
				SendOptions sendParams = new SendOptions
				{
					Channel = 0,
					Encrypt = false,
					Reliability = true
				};
				StreamBuffer opBytes = SerializeOperationToMessage(PhotonCodes.InitEncryption, parameterDictionary, EgMessageType.InternalOperationRequest, sendParams.Encrypt);
				return EnqueuePhotonMessage(opBytes, sendParams);
			}
		}

		internal void DeriveSharedKey(OperationResponse operationResponse)
		{
			if (operationResponse.ReturnCode != 0)
			{
				EnqueueDebugReturn(LogLevel.Error, "Establishing encryption keys failed. ReturnCode != OK: " + operationResponse.ToStringFull());
				EnqueueStatusCallback(StatusCode.EncryptionFailedToEstablish);
				return;
			}
			byte[] array = (byte[])operationResponse.Parameters[PhotonCodes.ServerKey];
			if (array == null || array.Length == 0)
			{
				EnqueueDebugReturn(LogLevel.Error, "Establishing encryption keys failed. Server public key is null or empty: " + operationResponse.ToStringFull());
				EnqueueStatusCallback(StatusCode.EncryptionFailedToEstablish);
			}
			else
			{
				CryptoProvider.DeriveSharedKey(array);
				isEncryptionAvailable = true;
				EnqueueStatusCallback(StatusCode.EncryptionEstablished);
			}
		}

		internal virtual void InitEncryption(byte[] secret)
		{
			if (photonPeer.PayloadEncryptorType != null)
			{
				try
				{
					CryptoProvider = (ICryptoProvider)Activator.CreateInstance(photonPeer.PayloadEncryptorType, secret);
					if (CryptoProvider == null)
					{
						if ((int)LogLevel >= 2)
						{
							Listener.DebugReturn(LogLevel.Warning, $"Payload encryptor creation by type failed, Activator.CreateInstance() returned null for: {photonPeer.PayloadEncryptorType}");
						}
					}
					else
					{
						isEncryptionAvailable = true;
					}
				}
				catch (Exception arg)
				{
					if ((int)LogLevel >= 2)
					{
						Listener.DebugReturn(LogLevel.Warning, $"Payload encryptor creation by type failed: {arg}");
					}
				}
			}
			if (CryptoProvider == null)
			{
				CryptoProvider = new DiffieHellmanCryptoProvider(secret);
				isEncryptionAvailable = true;
			}
		}

		internal void EnqueueActionForDispatch(MyAction action)
		{
			lock (ActionQueue)
			{
				ActionQueue.Enqueue(action);
			}
		}

		internal void EnqueueDebugReturn(LogLevel level, string debugReturn)
		{
			lock (ActionQueue)
			{
				ActionQueue.Enqueue(delegate
				{
					Listener.DebugReturn(level, debugReturn);
				});
			}
		}

		internal void EnqueueStatusCallback(StatusCode statusValue)
		{
			lock (ActionQueue)
			{
				ActionQueue.Enqueue(delegate
				{
					Listener.OnStatusChanged(statusValue);
				});
			}
		}

		internal void SendNetworkSimulated(byte[] dataToSend)
		{
			if (!NetworkSimulationSettings.IsSimulationEnabled)
			{
				throw new NotImplementedException("SendNetworkSimulated was called, despite NetworkSimulationSettings.IsSimulationEnabled == false.");
			}
			if (usedTransportProtocol == ConnectionProtocol.Udp && NetworkSimulationSettings.OutgoingLossPercentage > 0 && lagRandomizer.Next(101) < NetworkSimulationSettings.OutgoingLossPercentage)
			{
				networkSimulationSettings.LostPackagesOut++;
				return;
			}
			int num = ((networkSimulationSettings.OutgoingJitter > 0) ? (lagRandomizer.Next(networkSimulationSettings.OutgoingJitter * 2) - networkSimulationSettings.OutgoingJitter) : 0);
			int num2 = networkSimulationSettings.OutgoingLag + num;
			int num3 = timeInt + num2;
			SimulationItem value = new SimulationItem
			{
				DelayedData = dataToSend,
				TimeToExecute = num3,
				Delay = num2
			};
			lock (NetSimListOutgoing)
			{
				if (NetSimListOutgoing.Count == 0 || usedTransportProtocol == ConnectionProtocol.Tcp)
				{
					NetSimListOutgoing.AddLast(value);
					return;
				}
				LinkedListNode<SimulationItem> linkedListNode = NetSimListOutgoing.First;
				while (linkedListNode != null && linkedListNode.Value.TimeToExecute < num3)
				{
					linkedListNode = linkedListNode.Next;
				}
				if (linkedListNode == null)
				{
					NetSimListOutgoing.AddLast(value);
				}
				else
				{
					NetSimListOutgoing.AddBefore(linkedListNode, value);
				}
			}
		}

		internal void ReceiveNetworkSimulated(byte[] dataReceived)
		{
			if (!networkSimulationSettings.IsSimulationEnabled)
			{
				throw new NotImplementedException("ReceiveNetworkSimulated was called, despite NetworkSimulationSettings.IsSimulationEnabled == false.");
			}
			if (usedTransportProtocol == ConnectionProtocol.Udp && networkSimulationSettings.IncomingLossPercentage > 0 && lagRandomizer.Next(101) < networkSimulationSettings.IncomingLossPercentage)
			{
				networkSimulationSettings.LostPackagesIn++;
				return;
			}
			int num = ((networkSimulationSettings.IncomingJitter > 0) ? (lagRandomizer.Next(networkSimulationSettings.IncomingJitter * 2) - networkSimulationSettings.IncomingJitter) : 0);
			int num2 = networkSimulationSettings.IncomingLag + num;
			int num3 = timeInt + num2;
			SimulationItem value = new SimulationItem
			{
				DelayedData = dataReceived,
				TimeToExecute = num3,
				Delay = num2
			};
			lock (NetSimListIncoming)
			{
				if (NetSimListIncoming.Count == 0 || usedTransportProtocol == ConnectionProtocol.Tcp)
				{
					NetSimListIncoming.AddLast(value);
					return;
				}
				LinkedListNode<SimulationItem> linkedListNode = NetSimListIncoming.First;
				while (linkedListNode != null && linkedListNode.Value.TimeToExecute < num3)
				{
					linkedListNode = linkedListNode.Next;
				}
				if (linkedListNode == null)
				{
					NetSimListIncoming.AddLast(value);
				}
				else
				{
					NetSimListIncoming.AddBefore(linkedListNode, value);
				}
			}
		}

		protected internal void NetworkSimRun()
		{
			while (true)
			{
				bool flag = false;
				lock (networkSimulationSettings.NetSimManualResetEvent)
				{
					flag = networkSimulationSettings.IsSimulationEnabled;
				}
				if (!flag)
				{
					networkSimulationSettings.NetSimManualResetEvent.WaitOne();
					continue;
				}
				lock (NetSimListIncoming)
				{
					SimulationItem simulationItem = null;
					while (NetSimListIncoming.First != null)
					{
						simulationItem = NetSimListIncoming.First.Value;
						if (simulationItem.stopw.ElapsedMilliseconds < simulationItem.Delay)
						{
							break;
						}
						ReceiveIncomingCommands(simulationItem.DelayedData, simulationItem.DelayedData.Length);
						NetSimListIncoming.RemoveFirst();
					}
				}
				lock (NetSimListOutgoing)
				{
					SimulationItem simulationItem2 = null;
					while (NetSimListOutgoing.First != null)
					{
						simulationItem2 = NetSimListOutgoing.First.Value;
						if (simulationItem2.stopw.ElapsedMilliseconds < simulationItem2.Delay)
						{
							break;
						}
						if (PhotonSocket != null && PhotonSocket.Connected)
						{
							PhotonSocket.Send(simulationItem2.DelayedData, simulationItem2.DelayedData.Length);
						}
						NetSimListOutgoing.RemoveFirst();
					}
				}
				Thread.Sleep(0);
			}
		}
	}
}
