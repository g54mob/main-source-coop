using System;
using Features.BeachInteractableCommonModule.Scripts;
using Features.NetworkedModelCodegen.Scripts;

namespace Features.PlayersStatisticsModule.Scripts
{
	[NetworkedModel(ModelScope.Session, ModelOwnership.Shared)]
	public sealed class LevelPlayersGameStatisticsModel : NetworkedModelBase
	{
		private const int MAX_PLAYER_SLOTS = 4;

		private Networked<int>[] _ids;

		private Networked<int>[] _deaths;

		private Networked<int>[] _kills;

		private Networked<int>[] _revives;

		private Networked<int>[] _cents;

		public Networked<int> Slot0PlayerIdPlusOne { get; } = new Networked<int>();

		public Networked<int> Slot0Deaths { get; } = new Networked<int>();

		public Networked<int> Slot0Kills { get; } = new Networked<int>();

		public Networked<int> Slot0Revives { get; } = new Networked<int>();

		public Networked<int> Slot0Cents { get; } = new Networked<int>();

		public Networked<int> Slot1PlayerIdPlusOne { get; } = new Networked<int>();

		public Networked<int> Slot1Deaths { get; } = new Networked<int>();

		public Networked<int> Slot1Kills { get; } = new Networked<int>();

		public Networked<int> Slot1Revives { get; } = new Networked<int>();

		public Networked<int> Slot1Cents { get; } = new Networked<int>();

		public Networked<int> Slot2PlayerIdPlusOne { get; } = new Networked<int>();

		public Networked<int> Slot2Deaths { get; } = new Networked<int>();

		public Networked<int> Slot2Kills { get; } = new Networked<int>();

		public Networked<int> Slot2Revives { get; } = new Networked<int>();

		public Networked<int> Slot2Cents { get; } = new Networked<int>();

		public Networked<int> Slot3PlayerIdPlusOne { get; } = new Networked<int>();

		public Networked<int> Slot3Deaths { get; } = new Networked<int>();

		public Networked<int> Slot3Kills { get; } = new Networked<int>();

		public Networked<int> Slot3Revives { get; } = new Networked<int>();

		public Networked<int> Slot3Cents { get; } = new Networked<int>();

		public Networked<int> CouldroneCount { get; } = new Networked<int>();

		public Networked<int> CartCount { get; } = new Networked<int>();

		public Networked<int> DeadPartCount { get; } = new Networked<int>();

		public Networked<int> SpawnedNewItemId { get; } = new Networked<int>();

		public NetworkedSignal<int> ReviveSignal { get; } = new NetworkedSignal<int>();

		public NetworkedSignal ResetSignal { get; } = new NetworkedSignal();

		public BeachInteractableType SpawnedNewItem => (BeachInteractableType)SpawnedNewItemId.Value;

		public event Action OnSpawnedItemsChanged;

		public LevelPlayersGameStatisticsModel()
		{
			_ids = new Networked<int>[4] { Slot0PlayerIdPlusOne, Slot1PlayerIdPlusOne, Slot2PlayerIdPlusOne, Slot3PlayerIdPlusOne };
			_deaths = new Networked<int>[4] { Slot0Deaths, Slot1Deaths, Slot2Deaths, Slot3Deaths };
			_kills = new Networked<int>[4] { Slot0Kills, Slot1Kills, Slot2Kills, Slot3Kills };
			_revives = new Networked<int>[4] { Slot0Revives, Slot1Revives, Slot2Revives, Slot3Revives };
			_cents = new Networked<int>[4] { Slot0Cents, Slot1Cents, Slot2Cents, Slot3Cents };
			CouldroneCount.Changed += RaiseSpawnedItemsChanged;
			CartCount.Changed += RaiseSpawnedItemsChanged;
			DeadPartCount.Changed += RaiseSpawnedItemsChanged;
			ReviveSignal.Received += ApplyRevive;
			ResetSignal.Received += ApplyReset;
		}

		public int GetDeaths(int playerId)
		{
			return ReadSlot(_deaths, playerId);
		}

		public int GetKills(int playerId)
		{
			return ReadSlot(_kills, playerId);
		}

		public int GetRevives(int playerId)
		{
			return ReadSlot(_revives, playerId);
		}

		public int GetCents(int playerId)
		{
			return ReadSlot(_cents, playerId);
		}

		public int GetSpawnedItemCount(StatisticsSpawnedItemType type)
		{
			return type switch
			{
				StatisticsSpawnedItemType.Couldrone => CouldroneCount.Value, 
				StatisticsSpawnedItemType.Cart => CartCount.Value, 
				StatisticsSpawnedItemType.DeadPart => DeadPartCount.Value, 
				_ => 0, 
			};
		}

		public void AddDeath(int playerId)
		{
			if (base.IsAuthority)
			{
				int num = FindOrClaimSlot(playerId);
				if (num >= 0)
				{
					_deaths[num].Value = _deaths[num].Value + 1;
				}
			}
		}

		public void AddKill(int playerId)
		{
			if (base.IsAuthority)
			{
				int num = FindOrClaimSlot(playerId);
				if (num >= 0)
				{
					_kills[num].Value = _kills[num].Value + 1;
				}
			}
		}

		public void RaiseRevive(int byPlayerId)
		{
			ReviveSignal.Raise(byPlayerId);
		}

		public void SetCents(int playerId, int cents)
		{
			if (base.IsAuthority)
			{
				int num = FindOrClaimSlot(playerId);
				if (num >= 0)
				{
					_cents[num].Value = cents;
				}
			}
		}

		public void RegisterSpawnedItem(StatisticsSpawnedItemType type)
		{
			if (base.IsAuthority)
			{
				switch (type)
				{
				case StatisticsSpawnedItemType.Couldrone:
					CouldroneCount.Value += 1;
					break;
				case StatisticsSpawnedItemType.Cart:
					CartCount.Value += 1;
					break;
				case StatisticsSpawnedItemType.DeadPart:
					DeadPartCount.Value += 1;
					break;
				}
			}
		}

		public void SetSpawnedNewItem(BeachInteractableType type)
		{
			if (base.IsAuthority)
			{
				SpawnedNewItemId.Value = (int)type;
			}
		}

		public void RaiseReset()
		{
			ResetSignal.Raise();
		}

		private void ApplyRevive(int byPlayerId)
		{
			int num = FindOrClaimSlot(byPlayerId);
			if (num >= 0)
			{
				_revives[num].Value = _revives[num].Value + 1;
			}
		}

		private void ApplyReset()
		{
			for (int i = 0; i < 4; i++)
			{
				_ids[i].Value = 0;
				_deaths[i].Value = 0;
				_kills[i].Value = 0;
				_revives[i].Value = 0;
				_cents[i].Value = 0;
			}
			CouldroneCount.Value = 0;
			CartCount.Value = 0;
			DeadPartCount.Value = 0;
			SpawnedNewItemId.Value = 0;
		}

		private int ReadSlot(Networked<int>[] bank, int playerId)
		{
			int num = FindSlot(playerId);
			if (num >= 0)
			{
				return bank[num].Value;
			}
			return 0;
		}

		private int FindSlot(int playerId)
		{
			if (playerId < 0)
			{
				return -1;
			}
			for (int i = 0; i < 4; i++)
			{
				if (_ids[i].Value == playerId + 1)
				{
					return i;
				}
			}
			return -1;
		}

		private int FindOrClaimSlot(int playerId)
		{
			if (playerId < 0)
			{
				return -1;
			}
			int num = FindSlot(playerId);
			if (num >= 0)
			{
				return num;
			}
			for (int i = 0; i < 4; i++)
			{
				if (_ids[i].Value == 0)
				{
					_ids[i].Value = playerId + 1;
					return i;
				}
			}
			return -1;
		}

		private void RaiseSpawnedItemsChanged(int value)
		{
			this.OnSpawnedItemsChanged?.Invoke();
		}
	}
}
