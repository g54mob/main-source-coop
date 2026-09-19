using System;
using Photon.Client;

namespace Photon.Chat
{
	[Serializable]
	public class ChatAppSettings
	{
		public string AppIdChat;

		public string AppVersion;

		public string FixedRegion;

		public string Server;

		public ushort Port;

		public string ProxyServer;

		public ConnectionProtocol Protocol;

		public bool EnableProtocolFallback = true;

		public LogLevel NetworkLogging = LogLevel.Error;

		public LogLevel ClientLogging = LogLevel.Warning;

		public bool IsDefaultNameServer => string.IsNullOrEmpty(Server);

		public bool IsDefaultPort => Port <= 0;

		public ChatAppSettings()
		{
		}

		public ChatAppSettings(ChatAppSettings original)
		{
			original?.CopyTo(this);
		}

		public ChatAppSettings CopyTo(ChatAppSettings target)
		{
			target.AppIdChat = AppIdChat;
			target.AppVersion = AppVersion;
			target.FixedRegion = FixedRegion;
			target.Server = Server;
			target.Port = Port;
			target.ProxyServer = ProxyServer;
			target.Protocol = Protocol;
			target.ClientLogging = ClientLogging;
			target.NetworkLogging = NetworkLogging;
			target.EnableProtocolFallback = EnableProtocolFallback;
			return target;
		}
	}
}
