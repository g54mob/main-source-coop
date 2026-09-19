using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.SirenEnemy.Settings;
using Features.Movement.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy.Sensors
{
	public class SirenTargetSensor
	{
		private readonly Dictionary<PlayerRef, DateTime> _detectionTimers = new Dictionary<PlayerRef, DateTime>();

		private readonly PlayerState[] _ignoredStates;

		private readonly SirenEnemyContext _context;

		private readonly SirenIdleSettings _idleSettings;

		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		private readonly float _raycastThreshold;

		public SirenTargetSensor(SirenEnemyContext context, SirenEnemySettings enemySettings, SirenIdleSettings idleSettings, SpawnedPlayersModel spawnedPlayersModel)
		{
			_context = context;
			_idleSettings = idleSettings;
			_raycastThreshold = enemySettings.RaycastThreshold;
			_spawnedPlayersModel = spawnedPlayersModel;
			_ignoredStates = enemySettings.IgnorePlayerStates?.ToArray();
		}

		public void Reset()
		{
			_detectionTimers.Clear();
		}

		public bool TryAcquireTarget()
		{
			PruneStaleTimers();
			foreach (PlayerRef detectTarget in _context.TargetDetector.DetectTargets)
			{
				if (_context.IsPlayerEligibleForDetection(detectTarget.PlayerId, _ignoredStates))
				{
					DateTime value;
					if (!IsLookingAtSiren(detectTarget))
					{
						_detectionTimers.Remove(detectTarget);
					}
					else if (!_detectionTimers.TryGetValue(detectTarget, out value))
					{
						_detectionTimers[detectTarget] = DateTime.Now;
					}
					else if (!((DateTime.Now - value).TotalSeconds < (double)_idleSettings.MinLookTime))
					{
						_context.TargetPlayer = detectTarget;
						_detectionTimers.Clear();
						return true;
					}
				}
			}
			return false;
		}

		private void PruneStaleTimers()
		{
			List<PlayerRef> list = null;
			foreach (KeyValuePair<PlayerRef, DateTime> detectionTimer in _detectionTimers)
			{
				if (!_context.TargetDetector.DetectTargets.Contains(detectionTimer.Key))
				{
					if (list == null)
					{
						list = new List<PlayerRef>();
					}
					list.Add(detectionTimer.Key);
				}
			}
			if (list == null)
			{
				return;
			}
			foreach (PlayerRef item in list)
			{
				_detectionTimers.Remove(item);
			}
		}

		private bool IsLookingAtSiren(PlayerRef player)
		{
			if (!_spawnedPlayersModel.Players.TryGetValue(player, out var value))
			{
				return false;
			}
			PlayerLookDetection component = value.NetworkObject.GetComponent<PlayerLookDetection>();
			if (component != null)
			{
				return component.IsLookingAtObject(_context.LookAtTarget, angleCullEnabled: true, _context.GetStatValue(EntityStatType.HardDetectionDistance), _raycastThreshold);
			}
			return false;
		}
	}
}
