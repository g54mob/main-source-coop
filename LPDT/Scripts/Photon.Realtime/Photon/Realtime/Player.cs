using System.Collections.Generic;
using Photon.Client;

namespace Photon.Realtime
{
	public class Player
	{
		private int actorNumber = -1;

		public readonly bool IsLocal;

		private string nickName = string.Empty;

		public object TagObject;

		protected internal Room RoomReference { get; set; }

		public int ActorNumber => actorNumber;

		public bool HasRejoined { get; internal set; }

		public string NickName
		{
			get
			{
				return nickName;
			}
			set
			{
				if (string.IsNullOrEmpty(nickName) || !nickName.Equals(value))
				{
					nickName = value;
					if (IsLocal)
					{
						SetNickNameProperty();
					}
				}
			}
		}

		public string UserId { get; internal set; }

		public bool IsMasterClient
		{
			get
			{
				if (RoomReference == null)
				{
					return false;
				}
				return ActorNumber == RoomReference.MasterClientId;
			}
		}

		public bool IsInactive { get; protected internal set; }

		public PhotonHashtable CustomProperties { get; set; }

		protected internal Player(string nickName, int actorNumber, bool isLocal, PhotonHashtable playerProperties = null)
		{
			IsLocal = isLocal;
			this.actorNumber = actorNumber;
			NickName = nickName;
			CustomProperties = new PhotonHashtable();
			InternalCacheProperties(playerProperties);
		}

		public Player Get(int id)
		{
			if (RoomReference == null)
			{
				return null;
			}
			return RoomReference.GetPlayer(id);
		}

		public Player GetNext()
		{
			return GetNextFor(ActorNumber);
		}

		public Player GetNextFor(Player currentPlayer)
		{
			if (currentPlayer == null)
			{
				return null;
			}
			return GetNextFor(currentPlayer.ActorNumber);
		}

		public Player GetNextFor(int currentPlayerId)
		{
			if (RoomReference == null || RoomReference.Players == null || RoomReference.Players.Count < 2)
			{
				return null;
			}
			Dictionary<int, Player> players = RoomReference.Players;
			int num = int.MaxValue;
			int num2 = currentPlayerId;
			foreach (int key in players.Keys)
			{
				if (key < num2)
				{
					num2 = key;
				}
				else if (key > currentPlayerId && key < num)
				{
					num = key;
				}
			}
			if (num == int.MaxValue)
			{
				return players[num2];
			}
			return players[num];
		}

		protected internal void InternalCacheProperties(PhotonHashtable properties)
		{
			if (properties != null && properties.Count != 0 && !CustomProperties.Equals(properties))
			{
				if (!IsLocal && properties.ContainsKey(byte.MaxValue))
				{
					string text = (string)properties[byte.MaxValue];
					NickName = text;
				}
				if (properties.ContainsKey(253))
				{
					UserId = (string)properties[(byte)253];
				}
				if (properties.ContainsKey(254))
				{
					IsInactive = (bool)properties[(byte)254];
				}
				CustomProperties.Merge(properties);
				CustomProperties.StripKeysWithNullValues();
			}
		}

		public override string ToString()
		{
			return $"#{ActorNumber:00} '{NickName}'";
		}

		public string ToStringFull()
		{
			return string.Format("#{0:00} '{1}'{2} {3}", ActorNumber, NickName, IsInactive ? " (inactive)" : "", CustomProperties.ToStringFull());
		}

		public override bool Equals(object p)
		{
			if (p is Player player)
			{
				return GetHashCode() == player.GetHashCode();
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ActorNumber;
		}

		protected internal void ChangeLocalID(int newID)
		{
			if (IsLocal)
			{
				actorNumber = newID;
			}
		}

		public bool SetCustomProperties(PhotonHashtable propertiesToSet, PhotonHashtable expectedValues = null)
		{
			if (!propertiesToSet.CustomPropKeyTypesValid())
			{
				Log.Error("Player.SetCustomProperties() failed. Parameter propertiesToSet must be non-null, not empty and contain only int or string keys.");
				return false;
			}
			if (expectedValues != null && !expectedValues.CustomPropKeyTypesValid())
			{
				Log.Error("Player.SetCustomProperties() failed. Parameter expectedValues must contain only int or string keys if it is not null.");
				return false;
			}
			if (RoomReference != null)
			{
				if (RoomReference.IsOffline)
				{
					CustomProperties.Merge(propertiesToSet);
					CustomProperties.StripKeysWithNullValues();
					RoomReference.RealtimeClient.InRoomCallbackTargets.OnPlayerPropertiesUpdate(this, propertiesToSet);
					return true;
				}
				return RoomReference.RealtimeClient.OpSetPropertiesOfActor(actorNumber, propertiesToSet, expectedValues);
			}
			if (IsLocal && expectedValues == null)
			{
				CustomProperties.Merge(propertiesToSet);
				CustomProperties.StripKeysWithNullValues();
				return true;
			}
			return false;
		}

		internal bool UpdateNickNameOnJoined()
		{
			if (RoomReference == null || RoomReference.CustomProperties == null || !IsLocal)
			{
				return false;
			}
			string b = CustomProperties[byte.MaxValue] as string;
			if (!string.Equals(NickName, b))
			{
				return SetNickNameProperty();
			}
			return true;
		}

		private bool SetNickNameProperty()
		{
			if (RoomReference != null && !RoomReference.IsOffline)
			{
				PhotonHashtable photonHashtable = new PhotonHashtable();
				photonHashtable[byte.MaxValue] = NickName;
				return RoomReference.RealtimeClient.OpSetPropertiesOfActor(ActorNumber, photonHashtable);
			}
			return false;
		}
	}
}
