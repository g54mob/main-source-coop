using System.Collections.Generic;
using Photon.Client;

namespace Photon.Realtime
{
	public class Room : RoomInfo
	{
		private bool isOffline;

		private Dictionary<int, Player> players = new Dictionary<int, Player>();

		public RealtimeClient RealtimeClient { get; set; }

		public new string Name
		{
			get
			{
				return name;
			}
			internal set
			{
				name = value;
			}
		}

		public bool IsOffline
		{
			get
			{
				return isOffline;
			}
			private set
			{
				isOffline = value;
			}
		}

		public new bool IsOpen
		{
			get
			{
				return isOpen;
			}
			set
			{
				if (value != isOpen && !isOffline)
				{
					RealtimeClient.OpSetPropertiesOfRoom(new PhotonHashtable { 
					{
						(byte)253,
						(object)value
					} });
				}
				isOpen = value;
			}
		}

		public new bool IsVisible
		{
			get
			{
				return isVisible;
			}
			set
			{
				if (value != isVisible && !isOffline)
				{
					RealtimeClient.OpSetPropertiesOfRoom(new PhotonHashtable { 
					{
						(byte)254,
						(object)value
					} });
				}
				isVisible = value;
			}
		}

		public new int MaxPlayers
		{
			get
			{
				return maxPlayers;
			}
			set
			{
				if (value >= 0 && value != maxPlayers)
				{
					maxPlayers = value;
					byte b = (byte)((value <= 255) ? ((byte)value) : 0);
					if (!isOffline)
					{
						RealtimeClient.OpSetPropertiesOfRoom(new PhotonHashtable
						{
							{
								byte.MaxValue,
								b
							},
							{
								(byte)243,
								(object)maxPlayers
							}
						});
					}
				}
			}
		}

		public new int PlayerCount
		{
			get
			{
				if (Players == null)
				{
					return 0;
				}
				return (byte)Players.Count;
			}
		}

		public Dictionary<int, Player> Players
		{
			get
			{
				return players;
			}
			private set
			{
				players = value;
			}
		}

		public string[] ExpectedUsers => expectedUsers;

		public int PlayerTtl
		{
			get
			{
				return playerTtl;
			}
			set
			{
				if (value != playerTtl && !isOffline)
				{
					RealtimeClient.OpSetPropertyOfRoom(246, value);
				}
				playerTtl = value;
			}
		}

		public int EmptyRoomTtl
		{
			get
			{
				return emptyRoomTtl;
			}
			set
			{
				if (value != emptyRoomTtl && !isOffline)
				{
					RealtimeClient.OpSetPropertyOfRoom(245, value);
				}
				emptyRoomTtl = value;
			}
		}

		public int MasterClientId
		{
			get
			{
				return masterClientId;
			}
			protected internal set
			{
				masterClientId = value;
			}
		}

		public object[] PropertiesListedInLobby
		{
			get
			{
				return propertiesListedInLobby;
			}
			private set
			{
				propertiesListedInLobby = value;
			}
		}

		public bool AutoCleanUp => autoCleanUp;

		public bool BroadcastPropertiesChangeToAll { get; private set; }

		public bool SuppressRoomEvents { get; private set; }

		public bool SuppressPlayerInfo { get; private set; }

		public bool PublishUserId { get; private set; }

		public bool DeleteNullProperties { get; private set; }

		public TypedLobby Lobby { get; internal set; }

		public Room(string roomName, RoomOptions options, bool isOffline = false)
			: base(roomName, options?.CustomRoomProperties)
		{
			if (options != null)
			{
				isVisible = options.IsVisible;
				isOpen = options.IsOpen;
				maxPlayers = options.MaxPlayers;
				propertiesListedInLobby = options.CustomRoomPropertiesForLobby;
			}
			this.isOffline = isOffline;
		}

		internal void InternalCacheRoomFlags(int roomFlags)
		{
			BroadcastPropertiesChangeToAll = (roomFlags & 0x20) != 0;
			SuppressRoomEvents = (roomFlags & 4) != 0;
			SuppressPlayerInfo = (roomFlags & 0x40) != 0;
			PublishUserId = (roomFlags & 8) != 0;
			DeleteNullProperties = (roomFlags & 0x10) != 0;
			autoCleanUp = (roomFlags & 2) != 0;
		}

		protected internal void InternalCacheProperties(PhotonHashtable propertiesToCache)
		{
			int num = masterClientId;
			InternalCachePropertiesRoomInfo(propertiesToCache);
			if (num != 0 && masterClientId != num)
			{
				RealtimeClient.InRoomCallbackTargets.OnMasterClientSwitched(GetPlayer(masterClientId));
			}
		}

		public virtual bool SetCustomProperties(PhotonHashtable propertiesToSet, PhotonHashtable expectedValues = null)
		{
			if (!propertiesToSet.CustomPropKeyTypesValid())
			{
				Log.Error("Room.SetCustomProperties() failed. Parameter propertiesToSet must be non-null, not empty and contain only int or string keys.", RealtimeClient.LogLevel);
				return false;
			}
			if (expectedValues != null && !expectedValues.CustomPropKeyTypesValid())
			{
				Log.Error("Room.SetCustomProperties() failed. Parameter expectedValues  must contain only int or string keys if it is not null.", RealtimeClient.LogLevel);
				return false;
			}
			if (isOffline)
			{
				base.CustomProperties.Merge(propertiesToSet);
				base.CustomProperties.StripKeysWithNullValues();
				RealtimeClient.InRoomCallbackTargets.OnRoomPropertiesUpdate(propertiesToSet);
				return true;
			}
			return RealtimeClient.OpSetPropertiesOfRoom(propertiesToSet, expectedValues);
		}

		public bool SetPropertiesListedInLobby(object[] lobbyProps)
		{
			if (isOffline)
			{
				return false;
			}
			if (!lobbyProps.CustomPropKeyTypesValid(NullOrZeroAccepted: true))
			{
				Log.Error("Room.SetPropertiesListedInLobby() failed. Parameter lobbyProps can be null, have zero items or all items must be int or string.", RealtimeClient.LogLevel);
				return false;
			}
			PhotonHashtable photonHashtable = new PhotonHashtable();
			photonHashtable[(byte)250] = lobbyProps;
			return RealtimeClient.OpSetPropertiesOfRoom(photonHashtable);
		}

		protected internal virtual void RemovePlayer(Player player)
		{
			Players.Remove(player.ActorNumber);
			player.RoomReference = null;
		}

		protected internal virtual void RemovePlayer(int id)
		{
			RemovePlayer(GetPlayer(id));
		}

		public bool SetMasterClient(Player masterClientPlayer)
		{
			if (isOffline)
			{
				return false;
			}
			PhotonHashtable gameProperties = new PhotonHashtable { 
			{
				(byte)248,
				(object)masterClientPlayer.ActorNumber
			} };
			PhotonHashtable expectedProperties = new PhotonHashtable { 
			{
				(byte)248,
				(object)MasterClientId
			} };
			return RealtimeClient.OpSetPropertiesOfRoom(gameProperties, expectedProperties);
		}

		public virtual bool AddPlayer(Player player)
		{
			if (!Players.ContainsKey(player.ActorNumber))
			{
				StorePlayer(player);
				return true;
			}
			return false;
		}

		public virtual Player StorePlayer(Player player)
		{
			Players[player.ActorNumber] = player;
			player.RoomReference = this;
			return player;
		}

		public virtual Player GetPlayer(int id, bool findMaster = false)
		{
			int key = ((findMaster && id == 0) ? MasterClientId : id);
			Player value = null;
			Players.TryGetValue(key, out value);
			return value;
		}

		public bool ClearExpectedUsers()
		{
			if (ExpectedUsers == null || ExpectedUsers.Length == 0)
			{
				return false;
			}
			return SetExpectedUsers(new string[0], ExpectedUsers);
		}

		public bool SetExpectedUsers(string[] newExpectedUsers)
		{
			if (newExpectedUsers == null || newExpectedUsers.Length == 0)
			{
				Log.Error("SetExpectedUsers() failed. Parameter newExpectedUsers array is null or empty. To set no expected users, call Room.ClearExpectedUsers() instead.", RealtimeClient.LogLevel);
				return false;
			}
			return SetExpectedUsers(newExpectedUsers, ExpectedUsers);
		}

		private bool SetExpectedUsers(string[] newExpectedUsers, string[] currentKnownExpectedUsers)
		{
			if (isOffline)
			{
				return false;
			}
			PhotonHashtable photonHashtable = new PhotonHashtable(1);
			photonHashtable.Add((byte)247, (object)newExpectedUsers);
			PhotonHashtable photonHashtable2 = new PhotonHashtable(1);
			photonHashtable2.Add((byte)247, (object)currentKnownExpectedUsers);
			return RealtimeClient.OpSetPropertiesOfRoom(photonHashtable, photonHashtable2);
		}

		public override string ToString()
		{
			return string.Format("Room: '{0}' {1},{2} {3}/{4} players {5}.", name, isVisible ? "visible" : "hidden", isOpen ? "open" : "closed", PlayerCount, maxPlayers, (Lobby == null) ? "DefaultLobby" : Lobby.ToString());
		}

		public new string ToStringFull()
		{
			return string.Format("Room: '{0}' {1},{2} {3}/{4} players {5}.\n  customProps: {6}", name, isVisible ? "visible" : "hidden", isOpen ? "open" : "closed", PlayerCount, maxPlayers, (Lobby == null) ? "DefaultLobby" : Lobby.ToString(), base.CustomProperties.ToStringFull());
		}
	}
}
