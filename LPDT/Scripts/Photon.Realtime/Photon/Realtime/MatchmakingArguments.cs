using System;
using Photon.Client;

namespace Photon.Realtime
{
	public struct MatchmakingArguments
	{
		public AppSettings PhotonSettings;

		public int PlayerTtlInSeconds;

		public int EmptyRoomTtlInSeconds;

		public string RoomName;

		public int MaxPlayers;

		public bool CanOnlyJoin;

		public AsyncConfig AsyncConfig;

		public RealtimeClient NetworkClient;

		public AuthenticationValues AuthValues;

		public string PluginName;

		public MatchmakingReconnectInformation ReconnectInformation;

		public PhotonHashtable CustomProperties;

		public string[] ExpectedUsers;

		public TypedLobby Lobby;

		public string[] CustomLobbyProperties;

		public string SqlLobbyFilter;

		public object Ticket;

		public MatchmakingMode RandomMatchingType;

		public RoomOptions CustomRoomOptions;

		public bool? IsRoomVisible;

		public bool? IsRoomOpen;

		public bool EnableCrc;

		public bool FastReconnectDisabled;

		public string UserId
		{
			get
			{
				return AuthValues?.UserId;
			}
			set
			{
				if (AuthValues == null)
				{
					AuthValues = new AuthenticationValues();
				}
				AuthValues.UserId = value;
			}
		}

		public string[] Plugins
		{
			get
			{
				if (!string.IsNullOrEmpty(PluginName))
				{
					return new string[1] { PluginName };
				}
				return new string[0];
			}
		}

		public bool CanRejoin => PlayerTtlInSeconds > 0;

		public override string ToString()
		{
			return "PhotonSettings " + PhotonSettings.ToStringFull() + "\n" + $"PlayerTtlInSeconds: {PlayerTtlInSeconds}\n" + $"EmptyRoomTtlInSeconds${EmptyRoomTtlInSeconds}\n" + "RoomName " + RoomName + "\n" + $"MaxPlayers {MaxPlayers}\n" + $"CanOnlyJoin {CanOnlyJoin}\n" + $"RealtimeClient {NetworkClient} \n" + $"AuthValues {AuthValues} \n" + $"ReconnectInformation {ReconnectInformation}";
		}

		public void Validate()
		{
			Assert(MaxPlayers >= 0, "MaxPlayer must be greater or equal than 0");
			Assert(MaxPlayers < 256, "MaxPlayer must be less than 256");
			Assert(PhotonSettings != null, "PhotonSettings must be set");
		}

		private static void Assert(bool expected, string message)
		{
			if (!expected)
			{
				throw new ArgumentException(message);
			}
		}
	}
}
