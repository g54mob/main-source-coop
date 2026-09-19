using System;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using UnityEngine;

namespace Features.SteamInviteModule.Scripts
{
	[Serializable]
	public class SteamLobbySyncModel : JsonSynchronizableBase<SteamLobbySyncModel>, ISessionCleanup
	{
		public override RPCType RPCType => RPCType.InAllWays;

		public override bool IsNeedToSynchronizeOnSpawn => true;

		[field: SerializeField]
		public ulong SteamLobbyId { get; private set; }

		public event Action<ulong> OnSteamLobbyIdChanged;

		public void SetSteamLobbyId(ulong steamLobbyId, bool sync = true)
		{
			SteamLobbyId = steamLobbyId;
			this.OnSteamLobbyIdChanged?.Invoke(steamLobbyId);
			if (sync)
			{
				Synchronize();
			}
		}

		protected override void SetNewValues(SteamLobbySyncModel synchronizable, bool isSynchronizedOnStart)
		{
			SetSteamLobbyId(synchronizable.SteamLobbyId, sync: false);
		}

		public void Cleanup()
		{
			SetSteamLobbyId(0uL, sync: false);
		}
	}
}
