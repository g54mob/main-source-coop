using System;
using System.Collections.Generic;
using System.Text;
using Photon.Client;

namespace Photon.Realtime
{
	[Serializable]
	public class AppSettings
	{
		public string AppIdRealtime;

		public string AppIdFusion;

		public string AppIdQuantum;

		public string AppIdChat;

		public string AppIdVoice;

		public string AppVersion;

		public bool UseNameServer = true;

		public string FixedRegion;

		[NonSerialized]
		public string BestRegionSummaryFromStorage;

		public string Server;

		public ushort Port;

		public string ProxyServer;

		public ConnectionProtocol Protocol;

		public bool EnableProtocolFallback = true;

		public AuthModeOption AuthMode = AuthModeOption.AuthOnceWss;

		public bool EnableLobbyStatistics;

		public LogLevel NetworkLogging = LogLevel.Error;

		public LogLevel ClientLogging = LogLevel.Warning;

		public bool IsMasterServerAddress => !UseNameServer;

		public bool IsBestRegion
		{
			get
			{
				if (UseNameServer)
				{
					return string.IsNullOrEmpty(FixedRegion);
				}
				return false;
			}
		}

		public bool IsDefaultNameServer
		{
			get
			{
				if (UseNameServer)
				{
					return string.IsNullOrEmpty(Server);
				}
				return false;
			}
		}

		public bool IsDefaultPort => Port <= 0;

		public AppSettings()
		{
		}

		public AppSettings(AppSettings original = null)
		{
			original?.CopyTo(this);
		}

		public string GetAppId(ClientAppType ct)
		{
			return ct switch
			{
				ClientAppType.Realtime => AppIdRealtime, 
				ClientAppType.Fusion => AppIdFusion, 
				ClientAppType.Quantum => AppIdQuantum, 
				ClientAppType.Voice => AppIdVoice, 
				ClientAppType.Chat => AppIdChat, 
				_ => null, 
			};
		}

		public ClientAppType ClientTypeDetect()
		{
			bool flag = !string.IsNullOrEmpty(AppIdRealtime);
			bool flag2 = !string.IsNullOrEmpty(AppIdFusion);
			bool flag3 = !string.IsNullOrEmpty(AppIdQuantum);
			if (flag && !flag2 && !flag3)
			{
				return ClientAppType.Realtime;
			}
			if (flag2 && !flag && !flag3)
			{
				return ClientAppType.Fusion;
			}
			if (flag3 && !flag && !flag2)
			{
				return ClientAppType.Quantum;
			}
			Log.Error("ConnectUsingSettings requires that the AppSettings contain exactly one value set out of AppIdRealtime, AppIdFusion or AppIdQuantum.");
			return ClientAppType.Detect;
		}

		public string ToStringFull()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("AppId ");
			List<string> list = new List<string>();
			AppendAppIdIfNotEmpty(list, "Realtime/PUN", AppIdRealtime);
			AppendAppIdIfNotEmpty(list, "Fusion", AppIdFusion);
			AppendAppIdIfNotEmpty(list, "Quantum", AppIdQuantum);
			AppendAppIdIfNotEmpty(list, "Chat", AppIdChat);
			AppendAppIdIfNotEmpty(list, "Voice", AppIdVoice);
			stringBuilder.Append(string.Join(", ", list));
			stringBuilder.Append($", NameServer: {UseNameServer}");
			stringBuilder.Append(", Region: " + FixedRegion);
			stringBuilder.Append(", AppVersion: " + AppVersion);
			stringBuilder.Append(", Server: " + Server);
			stringBuilder.Append($", Port: {Port}");
			stringBuilder.Append(", Proxy: " + ProxyServer);
			stringBuilder.Append($", AuthMode: {AuthMode}");
			stringBuilder.Append($", Protocol: {Protocol}");
			stringBuilder.Append($", Enable Protocol Fallback: {EnableProtocolFallback}");
			stringBuilder.Append($", Lobby Statistics: {EnableLobbyStatistics}");
			stringBuilder.Append($", Network Logging: {NetworkLogging}");
			stringBuilder.Append($", Client Logging: {ClientLogging}");
			return stringBuilder.ToString();
			void AppendAppIdIfNotEmpty(List<string> list2, string label, string value)
			{
				if (!string.IsNullOrEmpty(value))
				{
					list2.Add(label + ": " + HideAppId(value));
				}
			}
		}

		public static bool IsAppId(string val)
		{
			try
			{
				new Guid(val);
			}
			catch
			{
				return false;
			}
			return true;
		}

		private string HideAppId(string appId)
		{
			if (!string.IsNullOrEmpty(appId) && appId.Length >= 8)
			{
				return appId.Substring(0, 8) + "***";
			}
			return appId;
		}

		public AppSettings CopyTo(AppSettings target)
		{
			target.AppIdRealtime = AppIdRealtime;
			target.AppIdFusion = AppIdFusion;
			target.AppIdQuantum = AppIdQuantum;
			target.AppIdChat = AppIdChat;
			target.AppIdVoice = AppIdVoice;
			target.AppVersion = AppVersion;
			target.UseNameServer = UseNameServer;
			target.FixedRegion = FixedRegion;
			target.BestRegionSummaryFromStorage = BestRegionSummaryFromStorage;
			target.Server = Server;
			target.Port = Port;
			target.ProxyServer = ProxyServer;
			target.Protocol = Protocol;
			target.AuthMode = AuthMode;
			target.EnableLobbyStatistics = EnableLobbyStatistics;
			target.ClientLogging = ClientLogging;
			target.NetworkLogging = NetworkLogging;
			target.EnableProtocolFallback = EnableProtocolFallback;
			return target;
		}
	}
}
