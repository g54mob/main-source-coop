using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Fusion;

namespace Features.PlayerPresenceModule.Networked
{
	public static class SessionPlayerObjectRegistry
	{
		private static readonly ConditionalWeakTable<NetworkRunner, List<SessionPlayerNetworkObject>> _byRunner = new ConditionalWeakTable<NetworkRunner, List<SessionPlayerNetworkObject>>();

		public static void Register(SessionPlayerNetworkObject sessionPlayer)
		{
			if (!(sessionPlayer == null) && !(sessionPlayer.Runner == null))
			{
				List<SessionPlayerNetworkObject> orCreateValue = _byRunner.GetOrCreateValue(sessionPlayer.Runner);
				if (!orCreateValue.Contains(sessionPlayer))
				{
					orCreateValue.Add(sessionPlayer);
				}
			}
		}

		public static void Unregister(NetworkRunner runner, SessionPlayerNetworkObject sessionPlayer)
		{
			if (!(runner == null) && !(sessionPlayer == null) && _byRunner.TryGetValue(runner, out var value))
			{
				value.Remove(sessionPlayer);
			}
		}

		public static IReadOnlyList<SessionPlayerNetworkObject> GetAll(NetworkRunner runner)
		{
			if (runner == null || !_byRunner.TryGetValue(runner, out var value) || value.Count == 0)
			{
				return Array.Empty<SessionPlayerNetworkObject>();
			}
			List<SessionPlayerNetworkObject> list = new List<SessionPlayerNetworkObject>(value.Count);
			for (int i = 0; i < value.Count; i++)
			{
				if (value[i] != null)
				{
					list.Add(value[i]);
				}
			}
			return list;
		}

		public static bool TryGetByStateAuthority(NetworkRunner runner, int playerId, out SessionPlayerNetworkObject result)
		{
			result = null;
			if (runner == null || !_byRunner.TryGetValue(runner, out var value))
			{
				return false;
			}
			for (int i = 0; i < value.Count; i++)
			{
				SessionPlayerNetworkObject sessionPlayerNetworkObject = value[i];
				if (sessionPlayerNetworkObject != null && sessionPlayerNetworkObject.Object != null && sessionPlayerNetworkObject.Object.StateAuthority.PlayerId == playerId)
				{
					result = sessionPlayerNetworkObject;
					return true;
				}
			}
			return false;
		}

		public static bool TryGetLocalAuthority(NetworkRunner runner, int localPlayerId, out SessionPlayerNetworkObject result)
		{
			result = null;
			if (runner == null || !_byRunner.TryGetValue(runner, out var value))
			{
				return false;
			}
			for (int i = 0; i < value.Count; i++)
			{
				SessionPlayerNetworkObject sessionPlayerNetworkObject = value[i];
				if (sessionPlayerNetworkObject != null && sessionPlayerNetworkObject.Object != null && sessionPlayerNetworkObject.HasStateAuthority && sessionPlayerNetworkObject.Object.StateAuthority.PlayerId == localPlayerId)
				{
					result = sessionPlayerNetworkObject;
					return true;
				}
			}
			return false;
		}
	}
}
