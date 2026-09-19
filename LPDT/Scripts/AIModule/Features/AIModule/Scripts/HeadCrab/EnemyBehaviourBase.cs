using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Sensors;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModule.Scripts.HeadCrab
{
	[NetworkBehaviourWeaved(0)]
	public abstract class EnemyBehaviourBase<TStateEnum> : NetworkBehaviour, IEnemyBehaviour, IEnemyTypeProvider, IStateAuthorityChanged, IPublicFacingInterface where TStateEnum : Enum
	{
		[SerializeField]
		protected EnemyTargetDetector _enemyTargetDetector;

		private EnemyTargetPositionsModel _targetPositionsModel;

		protected PlayersStatesSynchronizer PlayersStatesSynchronizer;

		private readonly EnemySpawnPointOccupancyBinder _spawnPointOccupancyBinder = new EnemySpawnPointOccupancyBinder();

		protected Vector3 _areaPosition;

		public bool IsOccupySpawnPoint { get; private set; }

		public TStateEnum CurrentEnemyState { get; private set; }

		public NetworkObject NetworkObject => base.Object;

		public abstract EnemyType EnemyType { get; }

		protected abstract TStateEnum ReactivationState { get; }

		public event Action<IEnemyBehaviour> OnDeath;

		[Inject]
		private void InjectDependencies(EnemyTargetPositionsModel targetPositionsModel, PlayersStatesSynchronizer playersStatesSynchronizer)
		{
			_targetPositionsModel = targetPositionsModel;
			PlayersStatesSynchronizer = playersStatesSynchronizer;
		}

		public override void Spawned()
		{
			base.Spawned();
			_enemyTargetDetector.OnTargetDetectedInRange += OnPlayerDetected;
			PlayersStatesSynchronizer.OnSomePlayerStateChanged += OnSomePlayerPlayerStateChanged;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			ReleaseSpawnPointOccupancy();
			base.Despawned(runner, hasState);
			_enemyTargetDetector.OnTargetDetectedInRange -= OnPlayerDetected;
			PlayersStatesSynchronizer.OnSomePlayerStateChanged -= OnSomePlayerPlayerStateChanged;
			this.OnDeath?.Invoke(this);
		}

		public void BindSpawnPointOccupancy(IEnemySpawnPointOccupancy occupancy)
		{
			_spawnPointOccupancyBinder.Bind(occupancy, base.Object);
		}

		public void ReleaseSpawnPointOccupancy()
		{
			_spawnPointOccupancyBinder.ReleaseBound();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				HandleCurrentEnemyState(CurrentEnemyState);
			}
		}

		public virtual void StateAuthorityChanged()
		{
			if (base.HasStateAuthority)
			{
				OnAuthorityGained();
				if (EqualityComparer<TStateEnum>.Default.Equals(CurrentEnemyState, default(TStateEnum)))
				{
					ChangeEnemyState(ReactivationState);
				}
			}
		}

		protected virtual void OnAuthorityGained()
		{
		}

		public void ChangeEnemyState(TStateEnum newState)
		{
			CurrentEnemyState = newState;
			if (base.HasStateAuthority)
			{
				ReactOnEnemyStateChanged(newState);
			}
		}

		private void OnPlayerDetected(PlayerRef player)
		{
			if (base.HasStateAuthority)
			{
				ReactOnPlayerDetected(player, CurrentEnemyState);
			}
		}

		private void OnSomePlayerPlayerStateChanged(PlayerStateData playerStateData)
		{
			if (base.HasStateAuthority)
			{
				ReactOnPlayerStateChanged(playerStateData, CurrentEnemyState);
			}
		}

		protected abstract void HandleCurrentEnemyState(TStateEnum currentEnemyState);

		protected abstract void ReactOnEnemyStateChanged(TStateEnum newState);

		protected abstract void ReactOnPlayerDetected(PlayerRef player, TStateEnum currentEnemyState);

		protected abstract void ReactOnPlayerStateChanged(PlayerStateData playerStateData, TStateEnum currentEnemyState);

		public void SetAreaPosition(Vector3 areaPosition)
		{
			_areaPosition = areaPosition;
			_targetPositionsModel.UpdateEnemyAreaPosition(EnemyType, base.gameObject.GetHashCode(), _areaPosition);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
