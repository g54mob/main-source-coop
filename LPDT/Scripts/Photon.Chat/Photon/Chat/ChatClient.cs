#define SUPPORTED_UNITY
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Photon.Client;

namespace Photon.Chat
{
	public class ChatClient : IPhotonPeerListener
	{
		private const int FriendRequestListMax = 1024;

		public const int DefaultMaxSubscribers = 100;

		private const byte HttpForwardWebFlag = 1;

		private readonly string chatRegion = "eu";

		public int MessageLimit;

		public int PrivateChatHistoryLength = -1;

		public readonly Dictionary<string, ChatChannel> PublicChannels;

		public readonly Dictionary<string, ChatChannel> PrivateChannels;

		private readonly HashSet<string> PublicChannelsUnsubscribing;

		private readonly IChatClientListener listener;

		public readonly PhotonPeer Peer;

		private const string ChatAppName = "chat";

		private bool didAuthenticate;

		private int msDeltaForServiceCalls = 50;

		private Timer stateTimer;

		private int msTimestampOfLastServiceCall;

		public string NameServerHost = "ns.photonengine.io";

		private static readonly Dictionary<ConnectionProtocol, int> ProtocolToNameServerPort = new Dictionary<ConnectionProtocol, int>
		{
			{
				ConnectionProtocol.Udp,
				5058
			},
			{
				ConnectionProtocol.Tcp,
				4533
			},
			{
				ConnectionProtocol.WebSocket,
				9093
			},
			{
				ConnectionProtocol.WebSocketSecure,
				19093
			}
		};

		public ushort NameServerPortOverride;

		public ChatAppSettings AppSettings { get; private set; }

		[Obsolete("Replaced by this.AppSettings. Calling ConnectUsingSettings() will set/replace this.AppSettings.")]
		public bool EnableProtocolFallback
		{
			get
			{
				return AppSettings?.EnableProtocolFallback ?? false;
			}
			set
			{
				if (AppSettings != null)
				{
					AppSettings.EnableProtocolFallback = value;
				}
			}
		}

		[Obsolete("Replaced by this.FixedRegionOrDefault. Setting a region should be done via ConnectUsingSettings() parameter AppSettings.")]
		public string ChatRegion => FixedRegionOrDefault;

		public string FixedRegionOrDefault
		{
			get
			{
				if (AppSettings != null && !string.IsNullOrEmpty(AppSettings.FixedRegion))
				{
					return AppSettings.FixedRegion;
				}
				return chatRegion;
			}
		}

		[Obsolete("Replaced by this.AppSettings. Calling ConnectUsingSettings() will set/replace this.AppSettings.")]
		public string ProxyServerAddress => AppSettings?.ProxyServer ?? null;

		public string CurrentServerAddress => Peer.ServerAddress;

		public string FrontendAddress { get; private set; }

		public ChatState State { get; private set; }

		public ChatDisconnectCause DisconnectedCause { get; private set; }

		public LogLevel LogLevelPeer
		{
			get
			{
				return Peer.LogLevel;
			}
			set
			{
				Peer.LogLevel = value;
				AppSettings.NetworkLogging = value;
			}
		}

		public LogLevel LogLevelClient
		{
			get
			{
				return AppSettings.ClientLogging;
			}
			set
			{
				AppSettings.ClientLogging = value;
			}
		}

		public bool CanChat => State == ChatState.ConnectedToFrontEnd;

		[Obsolete("Replaced by this.AppSettings. Calling ConnectUsingSettings() will set/replace this.AppSettings.")]
		public string AppVersion => AppSettings?.AppVersion ?? null;

		[Obsolete("Replaced by this.AppSettings. Calling ConnectUsingSettings() will set/replace this.AppSettings.")]
		public string AppId => AppSettings?.AppIdChat ?? null;

		public AuthenticationValues AuthValues { get; set; }

		public string UserId
		{
			get
			{
				if (AuthValues == null)
				{
					return null;
				}
				return AuthValues.UserId;
			}
			private set
			{
				if (AuthValues == null)
				{
					AuthValues = new AuthenticationValues();
				}
				AuthValues.UserId = value;
			}
		}

		public bool UseBackgroundWorkerForSending { get; set; }

		public ConnectionProtocol TransportProtocol
		{
			get
			{
				return Peer.TransportProtocol;
			}
			private set
			{
				if (Peer == null || Peer.PeerState != PeerStateValue.Disconnected)
				{
					listener.DebugReturn(LogLevel.Warning, "Can't set TransportProtocol. Disconnect first! " + ((Peer != null) ? ("PeerState: " + Peer.PeerState) : "The Peer is null."));
				}
				else
				{
					Peer.TransportProtocol = value;
				}
			}
		}

		public Dictionary<ConnectionProtocol, Type> SocketImplementationConfig => Peer.SocketImplementationConfig;

		public string NameServerAddress => GetNameServerAddress();

		internal virtual bool IsProtocolSecure => TransportProtocol == ConnectionProtocol.WebSocketSecure;

		public bool CanChatInChannel(string channelName)
		{
			if (CanChat && PublicChannels.ContainsKey(channelName))
			{
				return !PublicChannelsUnsubscribing.Contains(channelName);
			}
			return false;
		}

		public ChatClient(IChatClientListener listener, ConnectionProtocol protocol = ConnectionProtocol.Udp)
		{
			this.listener = listener;
			State = ChatState.Uninitialized;
			AppSettings = new ChatAppSettings();
			Peer = new PhotonPeer(this, protocol);
			Peer.SerializationProtocolType = SerializationProtocol.GpBinaryV18;
			ConfigUnitySockets();
			PublicChannels = new Dictionary<string, ChatChannel>();
			PrivateChannels = new Dictionary<string, ChatChannel>();
			PublicChannelsUnsubscribing = new HashSet<string>();
		}

		public bool ConnectUsingSettings(ChatAppSettings appSettings, AuthenticationValues authValues)
		{
			AuthValues = authValues;
			return ConnectUsingSettings(appSettings);
		}

		public bool ConnectUsingSettings(ChatAppSettings appSettings)
		{
			if (appSettings == null)
			{
				listener.DebugReturn(LogLevel.Error, "ConnectUsingSettings() failed. The appSettings can't be null.'");
				return false;
			}
			AppSettings = new ChatAppSettings(appSettings);
			LogLevelPeer = appSettings.NetworkLogging;
			TransportProtocol = appSettings.Protocol;
			if (!appSettings.IsDefaultNameServer)
			{
				NameServerHost = appSettings.Server;
			}
			NameServerPortOverride = (ushort)((!appSettings.IsDefaultPort) ? appSettings.Port : 0);
			return ConnectIntern();
		}

		[Obsolete("Use ConnectUsingSettings, which is more feature complete.")]
		public bool Connect(string appId, string appVersion, AuthenticationValues authValues)
		{
			if (authValues != null)
			{
				AuthValues = authValues;
			}
			AppSettings.AppIdChat = appId;
			AppSettings.AppVersion = appVersion;
			return ConnectIntern();
		}

		private bool ConnectIntern()
		{
			Peer.PingInterval = 3000;
			Peer.QuickResendAttempts = 2;
			Peer.MaxResends = 7;
			PublicChannels.Clear();
			PrivateChannels.Clear();
			PublicChannelsUnsubscribing.Clear();
			DisconnectedCause = ChatDisconnectCause.None;
			didAuthenticate = false;
			bool num = Peer.Connect(NameServerAddress, AppSettings.AppIdChat, (object)null, (object)null, AppSettings.ProxyServer);
			if (num)
			{
				State = ChatState.ConnectingToNameServer;
			}
			if (UseBackgroundWorkerForSending)
			{
				stateTimer = new Timer(SendOutgoingInBackground, null, msDeltaForServiceCalls, msDeltaForServiceCalls);
			}
			return num;
		}

		public void Service()
		{
			while (Peer.DispatchIncomingCommands())
			{
			}
			if (!UseBackgroundWorkerForSending && (Environment.TickCount - msTimestampOfLastServiceCall > msDeltaForServiceCalls || msTimestampOfLastServiceCall == 0))
			{
				msTimestampOfLastServiceCall = Environment.TickCount;
				while (Peer.SendOutgoingCommands())
				{
				}
			}
		}

		private void SendOutgoingInBackground(object state = null)
		{
			bool flag = true;
			while (State != ChatState.Disconnected && flag)
			{
				flag = Peer.SendOutgoingCommands();
			}
		}

		public void Disconnect(ChatDisconnectCause cause = ChatDisconnectCause.DisconnectByClientLogic)
		{
			if (State == ChatState.Disconnecting || State == ChatState.Uninitialized)
			{
				listener.DebugReturn(LogLevel.Info, "Disconnect() call gets skipped due to State " + State.ToString() + ". DisconnectedCause: " + DisconnectedCause.ToString() + " Parameter cause: " + cause);
			}
			else
			{
				if (DisconnectedCause == ChatDisconnectCause.None)
				{
					DisconnectedCause = cause;
				}
				if (Peer.PeerState != PeerStateValue.Disconnected)
				{
					State = ChatState.Disconnecting;
					Peer.Disconnect();
				}
			}
		}

		public bool Subscribe(string[] channels, int[] lastMsgIds)
		{
			if (!CanChat)
			{
				if ((int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, "Subscribe called while not connected to front end server.");
				}
				return false;
			}
			if (channels == null || channels.Length == 0)
			{
				if ((int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, "Subscribe can't be called for empty or null channels-list.");
				}
				return false;
			}
			for (int i = 0; i < channels.Length; i++)
			{
				if (string.IsNullOrEmpty(channels[i]))
				{
					if ((int)LogLevelClient >= 1)
					{
						listener.DebugReturn(LogLevel.Error, $"Subscribe can't be called with a null or empty channel name at index {i}.");
					}
					return false;
				}
			}
			if (lastMsgIds == null || lastMsgIds.Length != channels.Length)
			{
				if ((int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, "Subscribe can't be called when \"lastMsgIds\" array is null or does not have the same length as \"channels\" array.");
				}
				return false;
			}
			ParameterDictionary operationParameters = new ParameterDictionary
			{
				{ 0, channels },
				{ 9, lastMsgIds },
				{ 14, -1 }
			};
			return Peer.SendOperation(0, operationParameters, SendOptions.SendReliable);
		}

		public bool Subscribe(string[] channels, int messagesFromHistory = 0)
		{
			if (!CanChat)
			{
				if ((int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, "Subscribe called while not connected to front end server.");
				}
				return false;
			}
			if (channels == null || channels.Length == 0)
			{
				if ((int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, "Subscribe can't be called for empty or null channels-list.");
				}
				return false;
			}
			return SendChannelOperation(channels, 0, messagesFromHistory);
		}

		public bool Subscribe(string channel, int lastMsgId = 0, int messagesFromHistory = -1, ChannelCreationOptions creationOptions = null)
		{
			if (creationOptions == null)
			{
				creationOptions = ChannelCreationOptions.Default;
			}
			int maxSubscribers = creationOptions.MaxSubscribers;
			bool publishSubscribers = creationOptions.PublishSubscribers;
			if (maxSubscribers < 0)
			{
				if ((int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, "Cannot set MaxSubscribers < 0.");
				}
				return false;
			}
			if (lastMsgId < 0)
			{
				if ((int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, "lastMsgId cannot be < 0.");
				}
				return false;
			}
			if (messagesFromHistory < -1)
			{
				if ((int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, "messagesFromHistory < -1, setting it to -1");
				}
				messagesFromHistory = -1;
			}
			if (lastMsgId > 0 && messagesFromHistory == 0)
			{
				if ((int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, "lastMsgId will be ignored because messagesFromHistory == 0");
				}
				lastMsgId = 0;
			}
			Dictionary<object, object> dictionary = null;
			if (publishSubscribers)
			{
				if (maxSubscribers > 100)
				{
					if ((int)LogLevelClient >= 1)
					{
						listener.DebugReturn(LogLevel.Error, $"Cannot set MaxSubscribers > {100} when PublishSubscribers == true.");
					}
					return false;
				}
				dictionary = new Dictionary<object, object>();
				dictionary[(byte)254] = true;
			}
			if (maxSubscribers > 0)
			{
				if (dictionary == null)
				{
					dictionary = new Dictionary<object, object>();
				}
				dictionary[byte.MaxValue] = maxSubscribers;
			}
			ParameterDictionary parameterDictionary = new ParameterDictionary();
			parameterDictionary.Add(0, new string[1] { channel });
			ParameterDictionary parameterDictionary2 = parameterDictionary;
			if (messagesFromHistory != 0)
			{
				parameterDictionary2.Add(14, messagesFromHistory);
			}
			if (lastMsgId > 0)
			{
				parameterDictionary2.Add(9, new int[1] { lastMsgId });
			}
			if (dictionary != null && dictionary.Count > 0)
			{
				parameterDictionary2.Add(22, dictionary);
			}
			return Peer.SendOperation(0, parameterDictionary2, SendOptions.SendReliable);
		}

		public bool Unsubscribe(string[] channels)
		{
			if (!CanChat)
			{
				if ((int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, "Unsubscribe called while not connected to front end server.");
				}
				return false;
			}
			if (channels == null || channels.Length == 0)
			{
				if ((int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, "Unsubscribe can't be called for empty or null channels-list.");
				}
				return false;
			}
			foreach (string item in channels)
			{
				PublicChannelsUnsubscribing.Add(item);
			}
			return SendChannelOperation(channels, 1, 0);
		}

		public bool PublishMessage(string channelName, object message, bool forwardAsWebhook = false)
		{
			return publishMessage(channelName, message, reliable: true, forwardAsWebhook);
		}

		internal bool PublishMessageUnreliable(string channelName, object message, bool forwardAsWebhook = false)
		{
			return publishMessage(channelName, message, reliable: false, forwardAsWebhook);
		}

		private bool publishMessage(string channelName, object message, bool reliable, bool forwardAsWebhook = false)
		{
			if (!CanChat)
			{
				if ((int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, "PublishMessage called while not connected to front end server.");
				}
				return false;
			}
			if (string.IsNullOrEmpty(channelName) || message == null)
			{
				if ((int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, "PublishMessage parameters must be non-null and not empty.");
				}
				return false;
			}
			ParameterDictionary parameterDictionary = new ParameterDictionary
			{
				{ 1, channelName },
				{ 3, message }
			};
			if (forwardAsWebhook)
			{
				parameterDictionary.Add((byte)21, (byte)1);
			}
			return Peer.SendOperation(2, parameterDictionary, new SendOptions
			{
				Reliability = reliable
			});
		}

		public bool SendPrivateMessage(string target, object message, bool forwardAsWebhook = false)
		{
			return SendPrivateMessage(target, message, encrypt: false, forwardAsWebhook);
		}

		public bool SendPrivateMessage(string target, object message, bool encrypt, bool forwardAsWebhook)
		{
			return sendPrivateMessage(target, message, encrypt, reliable: true, forwardAsWebhook);
		}

		internal bool SendPrivateMessageUnreliable(string target, object message, bool encrypt, bool forwardAsWebhook = false)
		{
			return sendPrivateMessage(target, message, encrypt, reliable: false, forwardAsWebhook);
		}

		private bool sendPrivateMessage(string target, object message, bool encrypt, bool reliable, bool forwardAsWebhook = false)
		{
			if (!CanChat)
			{
				if ((int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, "SendPrivateMessage called while not connected to front end server.");
				}
				return false;
			}
			if (string.IsNullOrEmpty(target) || message == null)
			{
				if ((int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, "SendPrivateMessage parameters must be non-null and not empty.");
				}
				return false;
			}
			ParameterDictionary parameterDictionary = new ParameterDictionary
			{
				{ 225, target },
				{ 3, message }
			};
			if (forwardAsWebhook)
			{
				parameterDictionary.Add((byte)21, (byte)1);
			}
			return Peer.SendOperation(3, parameterDictionary, new SendOptions
			{
				Reliability = reliable,
				Encrypt = encrypt
			});
		}

		public bool SetOnlineStatus(int status, object message = null, bool skipMessage = false)
		{
			if (!CanChat)
			{
				if ((int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, "SetOnlineStatus called while not connected to front end server.");
				}
				return false;
			}
			ParameterDictionary parameterDictionary = new ParameterDictionary { { 10, status } };
			if (skipMessage)
			{
				parameterDictionary[12] = true;
			}
			else
			{
				parameterDictionary[3] = message;
			}
			return Peer.SendOperation(5, parameterDictionary, SendOptions.SendReliable);
		}

		public bool AddFriends(string[] friends)
		{
			if (!CanChat)
			{
				if ((int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, "AddFriends called while not connected to front end server.");
				}
				return false;
			}
			if (friends == null || friends.Length == 0)
			{
				if ((int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, "AddFriends can't be called for empty or null list.");
				}
				return false;
			}
			if (friends.Length > 1024)
			{
				if ((int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, "AddFriends max list size exceeded: " + friends.Length + " > " + 1024);
				}
				return false;
			}
			ParameterDictionary operationParameters = new ParameterDictionary { { 11, friends } };
			return Peer.SendOperation(6, operationParameters, SendOptions.SendReliable);
		}

		public bool RemoveFriends(string[] friends)
		{
			if (!CanChat)
			{
				if ((int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, "RemoveFriends called while not connected to front end server.");
				}
				return false;
			}
			if (friends == null || friends.Length == 0)
			{
				if ((int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, "RemoveFriends can't be called for empty or null list.");
				}
				return false;
			}
			if (friends.Length > 1024)
			{
				if ((int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, "RemoveFriends max list size exceeded: " + friends.Length + " > " + 1024);
				}
				return false;
			}
			ParameterDictionary operationParameters = new ParameterDictionary { { 11, friends } };
			return Peer.SendOperation(7, operationParameters, SendOptions.SendReliable);
		}

		public string GetPrivateChannelNameByUser(string userName)
		{
			return $"{UserId}:{userName}";
		}

		public bool TryGetChannel(string channelName, bool isPrivate, out ChatChannel channel)
		{
			if (!isPrivate)
			{
				return PublicChannels.TryGetValue(channelName, out channel);
			}
			return PrivateChannels.TryGetValue(channelName, out channel);
		}

		public bool TryGetChannel(string channelName, out ChatChannel channel)
		{
			if (PublicChannels.TryGetValue(channelName, out channel))
			{
				return true;
			}
			return PrivateChannels.TryGetValue(channelName, out channel);
		}

		public bool TryGetPrivateChannelByUser(string userId, out ChatChannel channel)
		{
			channel = null;
			if (string.IsNullOrEmpty(userId))
			{
				return false;
			}
			string privateChannelNameByUser = GetPrivateChannelNameByUser(userId);
			return TryGetChannel(privateChannelNameByUser, isPrivate: true, out channel);
		}

		void IPhotonPeerListener.DebugReturn(LogLevel level, string message)
		{
			listener.DebugReturn(level, message);
		}

		void IPhotonPeerListener.OnEvent(EventData eventData)
		{
			switch (eventData.Code)
			{
			case 0:
				HandleChatMessagesEvent(eventData);
				break;
			case 2:
				HandlePrivateMessageEvent(eventData);
				break;
			case 4:
				HandleStatusUpdate(eventData);
				break;
			case 5:
				HandleSubscribeEvent(eventData);
				break;
			case 6:
				HandleUnsubscribeEvent(eventData);
				break;
			case 8:
				HandleUserSubscribedEvent(eventData);
				break;
			case 9:
				HandleUserUnsubscribedEvent(eventData);
				break;
			case 1:
			case 3:
			case 7:
				break;
			}
		}

		void IPhotonPeerListener.OnOperationResponse(OperationResponse operationResponse)
		{
			if (operationResponse.ReturnCode == 32743)
			{
				Disconnect(ChatDisconnectCause.DisconnectByOperationLimit);
			}
			byte operationCode = operationResponse.OperationCode;
			if ((uint)operationCode > 3u && (uint)(operationCode - 230) <= 1u)
			{
				HandleAuthResponse(operationResponse);
			}
			else if (operationResponse.ReturnCode != 0 && (int)LogLevelClient >= 1)
			{
				if (operationResponse.ReturnCode == -2)
				{
					listener.DebugReturn(LogLevel.Error, $"Chat Operation {operationResponse.OperationCode} failed on server. Message by server: {operationResponse.DebugMessage}");
				}
				else
				{
					listener.DebugReturn(LogLevel.Error, $"Chat Operation {operationResponse.OperationCode} failed (Code: {operationResponse.ReturnCode}). Debug Message: {operationResponse.DebugMessage}");
				}
			}
		}

		void IPhotonPeerListener.OnStatusChanged(StatusCode statusCode)
		{
			switch (statusCode)
			{
			case StatusCode.Connect:
				if (!IsProtocolSecure)
				{
					if (!Peer.EstablishEncryption() && (int)LogLevelClient >= 1)
					{
						listener.DebugReturn(LogLevel.Error, "Error establishing encryption");
					}
				}
				else
				{
					TryAuthenticateOnNameServer();
				}
				if (State == ChatState.ConnectingToNameServer)
				{
					State = ChatState.ConnectedToNameServer;
					listener.OnChatStateChange(State);
				}
				else if (State == ChatState.ConnectingToFrontEnd && !AuthenticateOnFrontEnd() && (int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, $"Error authenticating on frontend! Check log output, AuthValues and if you're connected. State: {State}");
				}
				break;
			case StatusCode.EncryptionEstablished:
				TryAuthenticateOnNameServer();
				break;
			case StatusCode.Disconnect:
				switch (State)
				{
				case ChatState.ConnectWithFallbackProtocol:
					AppSettings.EnableProtocolFallback = false;
					NameServerPortOverride = 0;
					Peer.TransportProtocol = ((Peer.TransportProtocol != ConnectionProtocol.Tcp) ? ConnectionProtocol.Tcp : ConnectionProtocol.Udp);
					ConnectIntern();
					return;
				case ChatState.Authenticated:
					ConnectToFrontEnd();
					return;
				case ChatState.Disconnecting:
					if (stateTimer != null)
					{
						stateTimer.Dispose();
						stateTimer = null;
					}
					break;
				default:
				{
					string empty = string.Empty;
					listener.DebugReturn(LogLevel.Warning, $"Got an unexpected Disconnect in ChatState: {State}. DisconnectedCause: {DisconnectedCause}. Server: {Peer.ServerAddress} Trace: {empty}");
					if (stateTimer != null)
					{
						stateTimer.Dispose();
						stateTimer = null;
					}
					break;
				}
				}
				if (AuthValues != null)
				{
					AuthValues.Token = null;
				}
				State = ChatState.Disconnected;
				listener.OnChatStateChange(ChatState.Disconnected);
				listener.OnDisconnected();
				break;
			case StatusCode.DisconnectByServerUserLimit:
				listener.DebugReturn(LogLevel.Error, "This connection was rejected due to the apps CCU limit.");
				Disconnect(ChatDisconnectCause.MaxCcuReached);
				break;
			case StatusCode.DnsExceptionOnConnect:
				Disconnect(ChatDisconnectCause.DnsExceptionOnConnect);
				break;
			case StatusCode.ServerAddressInvalid:
				Disconnect(ChatDisconnectCause.ServerAddressInvalid);
				break;
			case StatusCode.SecurityExceptionOnConnect:
			case StatusCode.ExceptionOnConnect:
			case StatusCode.EncryptionFailedToEstablish:
				DisconnectedCause = ChatDisconnectCause.ExceptionOnConnect;
				if (AppSettings.EnableProtocolFallback && State == ChatState.ConnectingToNameServer)
				{
					State = ChatState.ConnectWithFallbackProtocol;
				}
				else
				{
					Disconnect(ChatDisconnectCause.ExceptionOnConnect);
				}
				break;
			case StatusCode.Exception:
			case StatusCode.ExceptionOnReceive:
				Disconnect(ChatDisconnectCause.Exception);
				break;
			case StatusCode.DisconnectByServerTimeout:
				Disconnect(ChatDisconnectCause.ServerTimeout);
				break;
			case StatusCode.DisconnectByServerLogic:
				Disconnect(ChatDisconnectCause.DisconnectByServerLogic);
				break;
			case StatusCode.DisconnectByServerReasonUnknown:
				Disconnect(ChatDisconnectCause.DisconnectByServerReasonUnknown);
				break;
			case StatusCode.TimeoutDisconnect:
				DisconnectedCause = ChatDisconnectCause.ClientTimeout;
				if (AppSettings.EnableProtocolFallback && State == ChatState.ConnectingToNameServer)
				{
					State = ChatState.ConnectWithFallbackProtocol;
				}
				else
				{
					Disconnect(ChatDisconnectCause.ClientTimeout);
				}
				break;
			}
		}

		void IPhotonPeerListener.OnMessage(bool isRawMessage, object msg)
		{
		}

		public void OnDisconnectMessage(DisconnectMessage obj)
		{
			listener.DebugReturn(LogLevel.Error, $"OnDisconnectMessage. Code: {obj.Code} Msg: \"{obj.DebugMessage}\".");
			Disconnect(ChatDisconnectCause.DisconnectByDisconnectMessage);
		}

		private void TryAuthenticateOnNameServer()
		{
			if (!didAuthenticate)
			{
				didAuthenticate = AuthenticateOnNameServer(AppSettings.AppIdChat, AppSettings.AppVersion, FixedRegionOrDefault, AuthValues);
				if (!didAuthenticate && (int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, $"Error calling OpAuthenticate! Did not work on NameServer. Check log output, AuthValues and if you're connected. State: {State}");
				}
			}
		}

		private bool SendChannelOperation(string[] channels, byte operation, int historyLength)
		{
			ParameterDictionary parameterDictionary = new ParameterDictionary { { 0, channels } };
			if (historyLength != 0)
			{
				parameterDictionary.Add(14, historyLength);
			}
			return Peer.SendOperation(operation, parameterDictionary, SendOptions.SendReliable);
		}

		private void HandlePrivateMessageEvent(EventData eventData)
		{
			object message = eventData.Parameters[3];
			string text = (string)eventData.Parameters[5];
			int msgId = (int)eventData.Parameters[8];
			string privateChannelNameByUser;
			if (UserId != null && UserId.Equals(text))
			{
				string userName = (string)eventData.Parameters[225];
				privateChannelNameByUser = GetPrivateChannelNameByUser(userName);
			}
			else
			{
				privateChannelNameByUser = GetPrivateChannelNameByUser(text);
			}
			if (!PrivateChannels.TryGetValue(privateChannelNameByUser, out var value))
			{
				value = new ChatChannel(privateChannelNameByUser);
				value.IsPrivate = true;
				value.MessageLimit = MessageLimit;
				PrivateChannels.Add(value.Name, value);
			}
			value.Add(text, message, msgId);
			listener.OnPrivateMessage(text, message, privateChannelNameByUser);
		}

		private void HandleChatMessagesEvent(EventData eventData)
		{
			object[] messages = (object[])eventData.Parameters[2];
			string[] senders = (string[])eventData.Parameters[4];
			string text = (string)eventData.Parameters[1];
			int lastMsgId = (int)eventData.Parameters[8];
			if (!PublicChannels.TryGetValue(text, out var value))
			{
				if ((int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, "Channel " + text + " for incoming message event not found.");
				}
			}
			else
			{
				value.Add(senders, messages, lastMsgId);
				listener.OnGetMessages(text, senders, messages);
			}
		}

		private void HandleSubscribeEvent(EventData eventData)
		{
			string[] array = (string[])eventData.Parameters[0];
			bool[] array2 = (bool[])eventData.Parameters[15];
			for (int i = 0; i < array.Length; i++)
			{
				if (array2[i])
				{
					string text = array[i];
					if (!PublicChannels.TryGetValue(text, out var value))
					{
						value = new ChatChannel(text);
						value.MessageLimit = MessageLimit;
						PublicChannels.Add(value.Name, value);
					}
					if (eventData.Parameters.TryGetValue(22, out var value2))
					{
						Dictionary<object, object> newProperties = value2 as Dictionary<object, object>;
						value.ReadChannelProperties(newProperties);
					}
					if (value.PublishSubscribers)
					{
						value.AddSubscriber(UserId);
					}
					if (eventData.Parameters.TryGetValue(23, out value2))
					{
						string[] users = value2 as string[];
						value.AddSubscribers(users);
					}
				}
			}
			listener.OnSubscribed(array, array2);
		}

		private void HandleUnsubscribeEvent(EventData eventData)
		{
			string[] array = (string[])eventData[0];
			foreach (string text in array)
			{
				PublicChannels.Remove(text);
				PublicChannelsUnsubscribing.Remove(text);
			}
			listener.OnUnsubscribed(array);
		}

		private void HandleAuthResponse(OperationResponse operationResponse)
		{
			if ((int)LogLevelClient >= 3)
			{
				listener.DebugReturn(LogLevel.Info, operationResponse.ToStringFull() + " on: " + CurrentServerAddress);
			}
			if (operationResponse.ReturnCode == 0)
			{
				if (State == ChatState.ConnectedToNameServer)
				{
					State = ChatState.Authenticated;
					listener.OnChatStateChange(State);
					if (operationResponse.Parameters.ContainsKey(221))
					{
						if (AuthValues == null)
						{
							AuthValues = new AuthenticationValues();
						}
						AuthValues.Token = operationResponse[221];
						FrontendAddress = (string)operationResponse[230];
						Peer.Disconnect();
					}
					else if ((int)LogLevelClient >= 1)
					{
						listener.DebugReturn(LogLevel.Error, "No secret in authentication response.");
					}
					if (operationResponse.Parameters.ContainsKey(225))
					{
						string text = operationResponse.Parameters[225] as string;
						if (!string.IsNullOrEmpty(text))
						{
							UserId = text;
							listener.DebugReturn(LogLevel.Info, $"Received your UserID from server. Updating local value to: {UserId}");
						}
					}
				}
				else if (State == ChatState.ConnectingToFrontEnd)
				{
					State = ChatState.ConnectedToFrontEnd;
					listener.OnChatStateChange(State);
					listener.OnConnected();
				}
				Dictionary<string, object> dictionary = (Dictionary<string, object>)operationResponse[245];
				if (dictionary != null)
				{
					listener.OnCustomAuthenticationResponse(dictionary);
				}
			}
			else
			{
				switch (operationResponse.ReturnCode)
				{
				case short.MaxValue:
					DisconnectedCause = ChatDisconnectCause.InvalidAuthentication;
					break;
				case 32755:
					DisconnectedCause = ChatDisconnectCause.CustomAuthenticationFailed;
					listener.OnCustomAuthenticationFailed(operationResponse.DebugMessage);
					break;
				case 32756:
					DisconnectedCause = ChatDisconnectCause.InvalidRegion;
					break;
				case 32757:
					DisconnectedCause = ChatDisconnectCause.MaxCcuReached;
					break;
				case -3:
					DisconnectedCause = ChatDisconnectCause.OperationNotAllowedInCurrentState;
					break;
				case 32753:
					DisconnectedCause = ChatDisconnectCause.AuthenticationTicketExpired;
					break;
				}
				if ((int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, $"{operationResponse.ToStringFull()} ClientState: {State} ServerAddress: {Peer.ServerAddress}");
				}
				Disconnect(DisconnectedCause);
			}
		}

		private void HandleStatusUpdate(EventData eventData)
		{
			string user = (string)eventData.Parameters[5];
			int status = (int)eventData.Parameters[10];
			object message = null;
			bool flag = eventData.Parameters.ContainsKey(3);
			if (flag)
			{
				message = eventData.Parameters[3];
			}
			listener.OnStatusUpdate(user, status, flag, message);
		}

		private bool ConnectToFrontEnd()
		{
			State = ChatState.ConnectingToFrontEnd;
			if ((int)LogLevelClient >= 3)
			{
				listener.DebugReturn(LogLevel.Info, "Connecting to frontend " + FrontendAddress);
			}
			if (!Peer.Connect(FrontendAddress, AppSettings.AppIdChat, AuthValues.Token, null, AppSettings.ProxyServer))
			{
				if ((int)LogLevelClient >= 1)
				{
					listener.DebugReturn(LogLevel.Error, $"Connecting to frontend {FrontendAddress} failed.");
				}
				return false;
			}
			return true;
		}

		private bool AuthenticateOnFrontEnd()
		{
			if (AuthValues != null)
			{
				if (AuthValues.Token == null)
				{
					if ((int)LogLevelClient >= 1)
					{
						listener.DebugReturn(LogLevel.Error, "Can't authenticate on front end server. Secret (AuthValues.Token) is not set");
					}
					return false;
				}
				ParameterDictionary parameterDictionary = new ParameterDictionary { { 221, AuthValues.Token } };
				if (PrivateChatHistoryLength > -1)
				{
					parameterDictionary[14] = PrivateChatHistoryLength;
				}
				return Peer.SendOperation(230, parameterDictionary, SendOptions.SendReliable);
			}
			if ((int)LogLevelClient >= 1)
			{
				listener.DebugReturn(LogLevel.Error, "Can't authenticate on front end server. Authentication Values are not set");
			}
			return false;
		}

		private void HandleUserUnsubscribedEvent(EventData eventData)
		{
			string text = eventData.Parameters[1] as string;
			string text2 = eventData.Parameters[225] as string;
			if (PublicChannels.TryGetValue(text, out var value))
			{
				if (!value.PublishSubscribers && (int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, $"Channel \"{text}\" for incoming UserUnsubscribed (\"{text2}\") event does not have PublishSubscribers enabled.");
				}
				if (!value.RemoveSubscriber(text2) && (int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, $"Channel \"{text}\" does not contain unsubscribed user \"{text2}\".");
				}
			}
			else if ((int)LogLevelClient >= 2)
			{
				listener.DebugReturn(LogLevel.Warning, $"Channel \"{text}\" not found for incoming UserUnsubscribed (\"{text2}\") event.");
			}
			listener.OnUserUnsubscribed(text, text2);
		}

		private void HandleUserSubscribedEvent(EventData eventData)
		{
			string text = eventData.Parameters[1] as string;
			string text2 = eventData.Parameters[225] as string;
			if (PublicChannels.TryGetValue(text, out var value))
			{
				if (!value.PublishSubscribers && (int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, $"Channel \"{text}\" for incoming UserSubscribed (\"{text2}\") event does not have PublishSubscribers enabled.");
				}
				if (!value.AddSubscriber(text2))
				{
					if ((int)LogLevelClient >= 2)
					{
						listener.DebugReturn(LogLevel.Warning, $"Channel \"{text}\" already contains newly subscribed user \"{text2}\".");
					}
				}
				else if (value.MaxSubscribers > 0 && value.Subscribers.Count > value.MaxSubscribers && (int)LogLevelClient >= 2)
				{
					listener.DebugReturn(LogLevel.Warning, $"Channel \"{text}\"'s MaxSubscribers exceeded. count={value.Subscribers.Count} > MaxSubscribers={value.MaxSubscribers}.");
				}
			}
			else if ((int)LogLevelClient >= 2)
			{
				listener.DebugReturn(LogLevel.Warning, $"Channel \"{text}\" not found for incoming UserSubscribed (\"{text2}\") event.");
			}
			listener.OnUserSubscribed(text, text2);
		}

		[Conditional("SUPPORTED_UNITY")]
		private void ConfigUnitySockets()
		{
			Type type = null;
			type = Type.GetType("Photon.Client.SocketWebTcp, PhotonWebSocket", throwOnError: false);
			if (type == null)
			{
				type = Type.GetType("Photon.Client.SocketWebTcp, Assembly-CSharp-firstpass", throwOnError: false);
			}
			if (type == null)
			{
				type = Type.GetType("Photon.Client.SocketWebTcp, Assembly-CSharp", throwOnError: false);
			}
			if (type != null)
			{
				SocketImplementationConfig[ConnectionProtocol.WebSocket] = type;
				SocketImplementationConfig[ConnectionProtocol.WebSocketSecure] = type;
			}
		}

		private string GetNameServerAddress()
		{
			int value = 0;
			ProtocolToNameServerPort.TryGetValue(TransportProtocol, out value);
			if (NameServerPortOverride != 0)
			{
				listener.DebugReturn(LogLevel.Info, $"Using NameServerPortInAppSettings as port for Name Server: {NameServerPortOverride}");
				value = NameServerPortOverride;
			}
			switch (TransportProtocol)
			{
			case ConnectionProtocol.Udp:
			case ConnectionProtocol.Tcp:
				return $"{NameServerHost}:{value}";
			case ConnectionProtocol.WebSocket:
				return $"ws://{NameServerHost}:{value}";
			case ConnectionProtocol.WebSocketSecure:
				return $"wss://{NameServerHost}:{value}";
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		protected internal bool AuthenticateOnNameServer(string appId, string appVersion, string region, AuthenticationValues authValues)
		{
			if ((int)LogLevelClient >= 3)
			{
				listener.DebugReturn(LogLevel.Info, "OpAuthenticate()");
			}
			ParameterDictionary parameterDictionary = new ParameterDictionary();
			parameterDictionary[220] = appVersion;
			parameterDictionary[224] = appId;
			parameterDictionary[210] = region;
			if (authValues != null)
			{
				if (!string.IsNullOrEmpty(authValues.UserId))
				{
					parameterDictionary[225] = authValues.UserId;
				}
				if (authValues.AuthType != CustomAuthenticationType.None)
				{
					parameterDictionary[217] = (byte)authValues.AuthType;
					if (authValues.Token != null)
					{
						parameterDictionary[221] = authValues.Token;
					}
					else
					{
						if (!string.IsNullOrEmpty(authValues.AuthGetParameters))
						{
							parameterDictionary[216] = authValues.AuthGetParameters;
						}
						if (authValues.AuthPostData != null)
						{
							parameterDictionary[214] = authValues.AuthPostData;
						}
					}
				}
			}
			return Peer.SendOperation(230, parameterDictionary, new SendOptions
			{
				Reliability = true,
				Encrypt = true
			});
		}
	}
}
