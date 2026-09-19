using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Sensors
{
	public abstract class EnemyTargetDetectorBase : MonoBehaviour, IEnemyTargetDetector
	{
		[SerializeField]
		protected float _detectionRadius;

		[SerializeField]
		protected bool _soloDetecting = true;

		protected SpawnedPlayersModel SpawnedPlayersModel;

		protected MultiplayerModel MultiplayerModel;

		protected IEnemyPlayerAttackabilityService EnemyPlayerAttackabilityService;

		private bool _isDetectingDisabled;

		public float DetectionRadius => _detectionRadius;

		public List<PlayerRef> DetectTargets { get; protected set; } = new List<PlayerRef>();

		public Dictionary<PlayerRef, DetectionType> DetectTargetsType { get; protected set; } = new Dictionary<PlayerRef, DetectionType>();

		public abstract event Action<PlayerRef> OnTargetDetectedInRange;

		public abstract event Action OnTargetDetecting;

		[Inject]
		public void InjectDependencies(SpawnedPlayersModel spawnedPlayersModel, MultiplayerModel multiplayerModel, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService)
		{
			SpawnedPlayersModel = spawnedPlayersModel;
			MultiplayerModel = multiplayerModel;
			EnemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
		}

		protected bool CanSelectPlayerAsTarget(PlayerRef player)
		{
			if (player != PlayerRef.None)
			{
				return EnemyPlayerAttackabilityService.CanEnemyTargetPlayer(player.PlayerId);
			}
			return false;
		}

		private void Update()
		{
			if (!_isDetectingDisabled)
			{
				DetectPlayers();
			}
		}

		protected abstract void DetectPlayers();

		public abstract bool IsPlayerDetected(PlayerRef player, out DetectionType detectionType);

		public virtual float GetDistanceToPlayer(PlayerRef player)
		{
			if (!SpawnedPlayersModel.Players.TryGetValue(player, out var value))
			{
				return float.PositiveInfinity;
			}
			return Vector3.Distance(value.NetworkObject.transform.position, new Vector3(base.transform.position.x, value.NetworkObject.transform.position.y, base.transform.position.z));
		}

		public void EnableDetecting()
		{
			_isDetectingDisabled = false;
		}

		public void DisableDetecting()
		{
			_isDetectingDisabled = true;
		}
	}
}
