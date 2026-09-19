using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using Features.NavigationModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class HeadmanLowAttackInterestPointSystem : MonoSystem
	{
		private const float ReachDistance = 0.4f;

		private readonly List<Transform> _sortedPoints = new List<Transform>();

		private HeadmanEnemyContext _context;

		private HeadmanChasingSettings _chasingSettings;

		private INavigationService _navigationService;

		private bool _isEnabled;

		public bool HasReachableInterestPoint { get; private set; }

		public Vector3 ApproachPoint { get; private set; }

		public Transform InterestPoint { get; private set; }

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(HeadmanEnemyContext context, HeadmanChasingSettings chasingSettings, INavigationService navigationService)
		{
			_context = context;
			_chasingSettings = chasingSettings;
			_navigationService = navigationService;
		}

		public override void Enable()
		{
			_isEnabled = true;
		}

		public override void Disable()
		{
			_isEnabled = false;
		}

		public override void Clear()
		{
			HasReachableInterestPoint = false;
			ApproachPoint = default(Vector3);
			InterestPoint = null;
			_sortedPoints.Clear();
		}

		public bool TryResolveReachableInterestPoint()
		{
			ClearResult();
			if (!_context.Agent.enabled)
			{
				return false;
			}
			if (!_context.TryGetTargetTrackingWorldPosition(out var worldPosition))
			{
				return false;
			}
			if (!TryResolveSafeZone(worldPosition, out var safeZone))
			{
				return false;
			}
			if (safeZone.InteractionPoints.Count == 0)
			{
				return false;
			}
			CollectPointsSortedByDistance(safeZone);
			for (int i = 0; i < _sortedPoints.Count; i++)
			{
				Transform transform = _sortedPoints[i];
				if (!(transform == null) && _navigationService.IsPointOnNavMeshProjected(transform.position, _context.Agent, out var hit) && _navigationService.TryGetCompletePath(_context.Agent, transform.position, out var _))
				{
					InterestPoint = transform;
					ApproachPoint = hit.position;
					HasReachableInterestPoint = true;
					return true;
				}
			}
			return false;
		}

		public bool HasReachedInterestPoint()
		{
			if (!HasReachableInterestPoint || !_context.Agent.enabled)
			{
				return false;
			}
			if (_context.Agent.pathPending)
			{
				return false;
			}
			float num = Mathf.Max(_context.Agent.stoppingDistance, 0.4f);
			if (_context.Agent.hasPath && _context.Agent.remainingDistance <= num)
			{
				return true;
			}
			return Vector3.Distance(_context.transform.position, ApproachPoint) <= 0.4f;
		}

		public void MoveToInterestPoint()
		{
			if (HasReachableInterestPoint)
			{
				_context.MoveToPosition(ApproachPoint);
			}
		}

		public bool CanStartLowAttackAtInterestPoint()
		{
			if (!_context.CanEnemyInteractWithChaseTarget())
			{
				return false;
			}
			if (!_context.HasChaseTarget)
			{
				return false;
			}
			if (!_context.Agent.enabled || _context.Agent.isOnOffMeshLink)
			{
				return false;
			}
			if (_context.AttackTimer < _chasingSettings.AttackTime)
			{
				return false;
			}
			if (!_context.ShouldUseLowAttack())
			{
				return false;
			}
			if (!HasReachableInterestPoint)
			{
				return false;
			}
			if (!_context.CheckDistanceEnoughToAttack())
			{
				return false;
			}
			return HasReachedInterestPoint();
		}

		private bool TryResolveSafeZone(Vector3 playerPos, out PlayerSafeZone safeZone)
		{
			if (_context.SafeZoneBlocker.TryFindBlockingSafeZoneUnderPlayer(playerPos, out safeZone) && safeZone != null)
			{
				return true;
			}
			Vector3 enemyPosition = ((_context.SafeZoneRayCastPoint != null) ? _context.SafeZoneRayCastPoint.position : _context.transform.position);
			if (_context.SafeZoneBlocker.TryFindBlockingSafeZone(enemyPosition, playerPos, out safeZone))
			{
				return safeZone != null;
			}
			return false;
		}

		private void CollectPointsSortedByDistance(PlayerSafeZone safeZone)
		{
			_sortedPoints.Clear();
			Vector3 origin = _context.transform.position;
			for (int i = 0; i < safeZone.InteractionPoints.Count; i++)
			{
				Transform transform = safeZone.InteractionPoints[i];
				if (!(transform == null))
				{
					_sortedPoints.Add(transform);
				}
			}
			_sortedPoints.Sort(delegate(Transform left, Transform right)
			{
				float sqrMagnitude = (left.position - origin).sqrMagnitude;
				float sqrMagnitude2 = (right.position - origin).sqrMagnitude;
				return sqrMagnitude.CompareTo(sqrMagnitude2);
			});
		}

		private void ClearResult()
		{
			HasReachableInterestPoint = false;
			ApproachPoint = default(Vector3);
			InterestPoint = null;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
