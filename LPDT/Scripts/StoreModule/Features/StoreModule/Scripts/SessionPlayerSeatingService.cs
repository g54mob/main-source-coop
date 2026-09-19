using System.Collections.Generic;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerPresenceModule.Networked;
using Fusion;

namespace Features.StoreModule.Scripts
{
	public class SessionPlayerSeatingService : IStoreSeatingService
	{
		private readonly MultiplayerModel _multiplayerModel;

		public SessionPlayerSeatingService(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public int GetSeatIndex(int playerId)
		{
			if (playerId < 0)
			{
				return -1;
			}
			List<SessionPlayerNetworkObject> list = OrderedRoster();
			for (int i = 0; i < list.Count; i++)
			{
				if (LivePlayerId(list[i]) == playerId)
				{
					return i;
				}
			}
			return -1;
		}

		public int GetPlayerIdBySeat(int seatIndex)
		{
			List<SessionPlayerNetworkObject> list = OrderedRoster();
			if (seatIndex < 0 || seatIndex >= list.Count)
			{
				return -1;
			}
			return LivePlayerId(list[seatIndex]);
		}

		public string GetHandle(int playerId)
		{
			foreach (SessionPlayerNetworkObject item in OrderedRoster())
			{
				if (LivePlayerId(item) == playerId)
				{
					return item.Nickname.ToString();
				}
			}
			return null;
		}

		private List<SessionPlayerNetworkObject> OrderedRoster()
		{
			IReadOnlyList<SessionPlayerNetworkObject> all = SessionPlayerObjectRegistry.GetAll(_multiplayerModel.NetworkRunner);
			List<SessionPlayerNetworkObject> list = new List<SessionPlayerNetworkObject>(all.Count);
			HashSet<string> hashSet = new HashSet<string>();
			foreach (SessionPlayerNetworkObject item in all)
			{
				string text = item.OwnerId.ToString();
				if (!string.IsNullOrEmpty(text) && hashSet.Add(text))
				{
					list.Add(item);
				}
			}
			list.Sort((SessionPlayerNetworkObject first, SessionPlayerNetworkObject second) => string.CompareOrdinal(first.OwnerId.ToString(), second.OwnerId.ToString()));
			return list;
		}

		private static int LivePlayerId(SessionPlayerNetworkObject sessionPlayer)
		{
			if (sessionPlayer.Object == null || !sessionPlayer.Object.IsValid)
			{
				return -1;
			}
			if (!sessionPlayer.IsHeldByOwner)
			{
				return -1;
			}
			PlayerRef stateAuthority = sessionPlayer.Object.StateAuthority;
			if (!(stateAuthority == PlayerRef.None))
			{
				return stateAuthority.PlayerId;
			}
			return -1;
		}
	}
}
