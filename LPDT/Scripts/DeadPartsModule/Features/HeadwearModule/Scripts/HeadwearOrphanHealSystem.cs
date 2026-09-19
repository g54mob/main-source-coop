using System.Collections.Generic;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.HeadwearModule.Scripts
{
	public class HeadwearOrphanHealSystem : ITickable
	{
		private const float RAGDOLL_DROP_DEBOUNCE_SECONDS = 0.75f;

		private readonly HeadwearModel _headwearModel;

		private readonly IPlayerStateService _playerStateService;

		private readonly List<int> _toDrop = new List<int>();

		private readonly List<int> _toShed = new List<int>();

		private readonly Dictionary<int, float> _ragdollingSince = new Dictionary<int, float>();

		public HeadwearOrphanHealSystem(HeadwearModel headwearModel, IPlayerStateService playerStateService)
		{
			_headwearModel = headwearModel;
			_playerStateService = playerStateService;
		}

		public void Tick()
		{
			_toDrop.Clear();
			_toShed.Clear();
			foreach (KeyValuePair<int, Headwear> item in _headwearModel.Worn)
			{
				Headwear value = item.Value;
				if (!(value == null) && !(value.Object == null) && value.Object.IsValid)
				{
					NetworkRunner runner = value.Runner;
					if (runner == null || !runner.IsRunning || !runner.IsSharedModeMasterClient)
					{
						return;
					}
					if (!_playerStateService.IsPlayerAlive(item.Key))
					{
						_toDrop.Add(item.Key);
					}
					else if (IsRagdollSustained(item.Key, value) && !value.IsWearerUnderCover)
					{
						_toShed.Add(item.Key);
					}
				}
			}
			foreach (int item2 in _toDrop)
			{
				if (_headwearModel.TryGetHeadwear(item2, out var headwear))
				{
					headwear.DropOnWearerLeft();
				}
			}
			foreach (int item3 in _toShed)
			{
				if (_headwearModel.TryGetHeadwear(item3, out var headwear2))
				{
					headwear2.DropOnWearerRagdolled();
				}
			}
		}

		private bool IsRagdollSustained(int playerId, Headwear headwear)
		{
			if (!headwear.IsWearerRagdolling)
			{
				_ragdollingSince.Remove(playerId);
				return false;
			}
			if (!_ragdollingSince.TryGetValue(playerId, out var value))
			{
				_ragdollingSince[playerId] = Time.time;
				return false;
			}
			return Time.time - value >= 0.75f;
		}
	}
}
