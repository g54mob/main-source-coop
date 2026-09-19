using Features.AIModuleStateMachine.Scripts.Core.Sensors;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class AnchorThrowDecisionSystem : MonoSystem
	{
		[SerializeField]
		private EnemyLineOfSightDetector _throwLineOfSight;

		private AnchorEnemyContext _context;

		private INavigationService _navigationService;

		private bool _isEnabled;

		public bool CanThrow { get; private set; }

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(AnchorEnemyContext context, INavigationService navigationService)
		{
			_context = context;
			_navigationService = navigationService;
		}

		public override void Enable()
		{
			_isEnabled = true;
		}

		public override void Disable()
		{
			_isEnabled = false;
			Clear();
		}

		public override void Clear()
		{
			CanThrow = false;
		}

		private void Update()
		{
			if (!base.Initialized || !_isEnabled || !base.HasStateAuthority)
			{
				CanThrow = false;
			}
			else
			{
				CanThrow = Evaluate();
			}
		}

		private bool Evaluate()
		{
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (priorityPlayer == null || priorityPlayer.NetworkObject == null)
			{
				return false;
			}
			if (_context.AttackCooldown > 0f)
			{
				return false;
			}
			Transform transform = priorityPlayer.NetworkObject.transform;
			float num = Vector3.Distance(_context.transform.position, transform.position);
			if (num < _context.MinHookDistance || num > _context.RecommendedHookDistance)
			{
				return false;
			}
			if (_throwLineOfSight == null)
			{
				return false;
			}
			PlayerRef inputAuthority = priorityPlayer.NetworkObject.InputAuthority;
			Vector3 targetPosition = ResolveAimPoint(inputAuthority, transform);
			return _throwLineOfSight.HasLineOfSight(targetPosition);
		}

		private Vector3 ResolveAimPoint(PlayerRef playerRef, Transform playerTransform)
		{
			if (_navigationService != null && _navigationService.TryGetPlayerTrackingPosition(playerRef, out var position))
			{
				return position;
			}
			return playerTransform.position;
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
