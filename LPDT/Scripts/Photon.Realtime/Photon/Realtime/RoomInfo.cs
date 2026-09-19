using System;
using Photon.Client;

namespace Photon.Realtime
{
	public class RoomInfo
	{
		public bool RemovedFromList;

		private PhotonHashtable customProperties = new PhotonHashtable();

		protected int maxPlayers;

		protected int emptyRoomTtl;

		protected int playerTtl;

		protected string[] expectedUsers;

		protected bool isOpen = true;

		protected bool isVisible = true;

		protected bool autoCleanUp = true;

		protected string name;

		protected int masterClientId;

		protected object[] propertiesListedInLobby;

		public PhotonHashtable CustomProperties => customProperties;

		public string Name => name;

		public int PlayerCount { get; private set; }

		public int MaxPlayers => maxPlayers;

		public bool IsOpen => isOpen;

		public bool IsVisible => isVisible;

		protected internal RoomInfo(string roomName, PhotonHashtable roomProperties)
		{
			InternalCachePropertiesRoomInfo(roomProperties);
			name = roomName;
		}

		public override bool Equals(object other)
		{
			if (other is RoomInfo roomInfo)
			{
				return Name.Equals(roomInfo.name);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("Room: '{0}' {1},{2} {4}/{3} players.{5}", name, isVisible ? "visible" : "hidden", isOpen ? "open" : "closed", maxPlayers, PlayerCount, RemovedFromList ? " removed!" : "");
		}

		public string ToStringFull()
		{
			return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", name, isVisible ? "visible" : "hidden", isOpen ? "open" : "closed", maxPlayers, PlayerCount, customProperties.ToStringFull());
		}

		protected internal void InternalCachePropertiesRoomInfo(PhotonHashtable propertiesToCache)
		{
			if (propertiesToCache == null || propertiesToCache.Count == 0 || customProperties.Equals(propertiesToCache))
			{
				return;
			}
			if (propertiesToCache.ContainsKey(251))
			{
				RemovedFromList = (bool)propertiesToCache[(byte)251];
				if (RemovedFromList)
				{
					return;
				}
			}
			if (propertiesToCache.ContainsKey(243))
			{
				maxPlayers = Convert.ToInt32(propertiesToCache[(byte)243]);
			}
			else if (propertiesToCache.ContainsKey(byte.MaxValue))
			{
				maxPlayers = Convert.ToInt32(propertiesToCache[byte.MaxValue]);
			}
			if (propertiesToCache.ContainsKey(253))
			{
				isOpen = (bool)propertiesToCache[(byte)253];
			}
			if (propertiesToCache.ContainsKey(254))
			{
				isVisible = (bool)propertiesToCache[(byte)254];
			}
			if (propertiesToCache.ContainsKey(252))
			{
				PlayerCount = Convert.ToInt32(propertiesToCache[(byte)252]);
			}
			if (propertiesToCache.ContainsKey(249))
			{
				autoCleanUp = (bool)propertiesToCache[(byte)249];
			}
			if (propertiesToCache.ContainsKey(248))
			{
				masterClientId = (int)propertiesToCache[(byte)248];
			}
			if (propertiesToCache.ContainsKey(250))
			{
				propertiesListedInLobby = propertiesToCache[(byte)250] as object[];
			}
			if (propertiesToCache.ContainsKey(247))
			{
				expectedUsers = (string[])propertiesToCache[(byte)247];
			}
			if (propertiesToCache.ContainsKey(245))
			{
				emptyRoomTtl = (int)propertiesToCache[(byte)245];
			}
			if (propertiesToCache.ContainsKey(246))
			{
				playerTtl = (int)propertiesToCache[(byte)246];
			}
			customProperties.Merge(propertiesToCache);
			customProperties.StripKeysWithNullValues();
		}
	}
}
