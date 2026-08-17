using System.Collections.Generic;
using EvilCore;
using Mirror;

namespace NomadDrive.Features.Player.Downed
{
	internal static class DownedPlayerRegistry
	{
		private static readonly HashSet<uint> AllPlayers = new HashSet<uint>();

		private static readonly HashSet<uint> DownedPlayers = new HashSet<uint>();

		private static uint _lastDownedNetId;

		public static IGameOverService GameOver;

		public static bool GameOverEnabled = true;

		public static void ServerRegisterPlayer(uint netId)
		{
			if (NetworkServer.active)
			{
				AllPlayers.Add(netId);
				Recompute();
			}
		}

		public static void ServerUnregisterPlayer(uint netId)
		{
			if (NetworkServer.active)
			{
				AllPlayers.Remove(netId);
				DownedPlayers.Remove(netId);
				Recompute();
			}
		}

		public static void ServerMarkDowned(uint netId)
		{
			if (NetworkServer.active)
			{
				DownedPlayers.Add(netId);
				_lastDownedNetId = netId;
				Recompute();
			}
		}

		public static void ServerMarkRevived(uint netId)
		{
			if (NetworkServer.active)
			{
				DownedPlayers.Remove(netId);
			}
		}

		public static bool ServerHasOtherAlivePlayer(uint excludingNetId)
		{
			if (!NetworkServer.active)
			{
				return false;
			}
			foreach (uint allPlayer in AllPlayers)
			{
				if (allPlayer != excludingNetId && !DownedPlayers.Contains(allPlayer))
				{
					return true;
				}
			}
			return false;
		}

		private static void Recompute()
		{
			if (GameOverEnabled && AllPlayers.Count != 0 && DownedPlayers.Count >= AllPlayers.Count)
			{
				TriggerLastStandRespawn();
			}
		}

		private static void TriggerLastStandRespawn()
		{
			uint num = (DownedPlayers.Contains(_lastDownedNetId) ? _lastDownedNetId : FirstDowned());
			if (num != 0 && NetworkServer.spawned.TryGetValue(num, out var value) && value != null && value.TryGetComponent<PlayerDeathController>(out var component))
			{
				component.ServerScheduleLastStandRespawn();
			}
		}

		private static uint FirstDowned()
		{
			using (HashSet<uint>.Enumerator enumerator = DownedPlayers.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return 0u;
		}
	}
}
