using System;
using Features.LineArmModule.Scripts;
using Features.Movement.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.Entities;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using Zenject;

namespace Features.PlayerFlyingModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerFlyingMovable : NetworkBehaviour
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private StatEntityNetworkedBase<EntityStatType> _statEntity;

		[SerializeField]
		private PlayerCharacterMovableBase _playerCharacterMovableBase;

		[SerializeField]
		private Transform _flyingCastTransform;

		[SerializeField]
		private LayerMask _flyingItemsLayerMask;

		[SerializeField]
		private float _itemsCheckRadius = 1f;

		[SerializeField]
		private float _minFlyingTimeToStartStaminaDrain = 0.5f;

		[SerializeField]
		private float _velocityMagnitudeThreshold = 0.1f;

		[SerializeField]
		private int _flyDetectMillisecondsThreshold = 100;

		private PlayerStatsConfiguration _playerStatsConfiguration;

		private IPlayerStaminaService _playerStaminaService;

		private PlayerMovableModel _playerMovableModel;

		private LineArmsModel _lineArmsModel;

		private IStat _flyingStaminaDrainRateStat;

		private DateTime _lastUnFlytime;

		private readonly Collider[] _raycastHits = new Collider[16];

		[Inject]
		private void InjectDependencies(PlayerStatsConfiguration playerStatsConfiguration, LineArmsModel lineArmsModel, PlayerMovableModel playerMovableModel, IPlayerStaminaService playerStaminaService)
		{
			_playerMovableModel = playerMovableModel;
			_playerStatsConfiguration = playerStatsConfiguration;
			_lineArmsModel = lineArmsModel;
			_playerStaminaService = playerStaminaService;
		}

		public override void Spawned()
		{
			InitializeStats();
		}

		private void InitializeStats()
		{
			_flyingStaminaDrainRateStat = _statEntity.GetStat(EntityStatType.FlyingStaminaDrainRate);
			_flyingStaminaDrainRateStat.OverrideValue(_playerStatsConfiguration.PlayerStats[EntityStatType.FlyingStaminaDrainRate]);
		}

		private void Update()
		{
			if (!(base.Object == null) && base.Object.HasStateAuthority)
			{
				UpdateStamina();
			}
		}

		private void UpdateStamina()
		{
			int num = Physics.OverlapSphereNonAlloc(_flyingCastTransform.position, _itemsCheckRadius, _raycastHits, _flyingItemsLayerMask, QueryTriggerInteraction.Ignore);
			LineArmControllerBase lineArm;
			bool flag = _lineArmsModel.TryGetLineArmForPlayer(base.Object.InputAuthority.PlayerId, out lineArm) && lineArm.CurrentGrabbables.Count > 0;
			if (!_playerCharacterMovableBase.IsGrounded && num > 0 && flag && _rigidbody.linearVelocity.magnitude > _velocityMagnitudeThreshold)
			{
				if ((DateTime.Now - _lastUnFlytime).TotalMilliseconds > (double)_flyDetectMillisecondsThreshold)
				{
					_playerStaminaService.SubtractStaminaLogic(Time.deltaTime, _flyingStaminaDrainRateStat.FullValue);
				}
				_playerMovableModel.SetIsFlying(isFlying: true);
			}
			else
			{
				_playerMovableModel.SetIsFlying(isFlying: false);
				_lastUnFlytime = DateTime.Now;
			}
		}

		private void OnDrawGizmos()
		{
			if (!(_flyingCastTransform == null))
			{
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(_flyingCastTransform.position, _itemsCheckRadius);
			}
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
